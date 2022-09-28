using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// yamato�� ������ �� �ְ�, ���� Ű�� ������ ������ �߻��ϰ� �ʹ�.

public class KANG_Yamato : KANG_Machine
{
    AudioSource source;

    public AudioClip attackSound;
    public GameObject yamatoEffect;

    public GameObject yamatoBulletFactory;
    public Transform firePos;
    public bool isYamatoControl = false;

    // ���ݽð�
    public float attackTime = 3f;
    float curAttackTime = 0f;

    // �����ð�
    public float createTime = 1f;
    float curCreateTime = 0f;

    // texture ���� �ð�
    public float coolTime = 5f;
    float curCoolTime = 0f;

    // Texture
    public List<GameObject> liveTextures;
    public List<GameObject> deadTextures;
    public List<GameObject> controls;
    int textureIndex = 0;
    // Laser
    public GameObject Laser;


    public enum YamatoState
    {
        Enable,
        Attack,
        Disable
    }

    public YamatoState yState = YamatoState.Disable;
    public MachineState mState = MachineState.Idle;


    // Start is called before the first frame update
    void Start()
    {
        source = GetComponent<AudioSource>();

        curCreateTime = createTime;

        for (int i = 0; i < liveTextures.Count; i++)
        {
            liveTextures[i].SetActive(false);
            deadTextures[i].SetActive(false);
        }
        deadTextures[0].SetActive(true);

    }

    // Update is called once per frame
    void Update()
    {
        switch (yState)
        {
            case YamatoState.Enable:
                Enable();
                break;
            case YamatoState.Attack:
                Attack();
                break;
            case YamatoState.Disable:
                Disable();
                break;
        }
    }

    public override void ActionKey()
    {
        if (yState == YamatoState.Enable)
        {
            photonView.RPC("RpcChangeYState", RpcTarget.All, YamatoState.Attack);
        }

    }

    [PunRPC]
    void RpcChangeYState(YamatoState state)
    {
        yState = state;
        print("RPC : " + yState);
    }
    [PunRPC]
    void RpcChangeMState(MachineState state)
    {
        mState = state;
        print("RPC mState : " + mState);
    }

    // Ȱ��ȭ ����
    // - ������ �����ϰ�, ����Ű �Է��� ������ ���� ���·� ��ȯ�Ѵ�.
    private void Enable()
    {

    }

    // ���� ����
    // - ���� �ð����� ���� �������� �Ѿ��� �߻��Ѵ�.
    // - ��Ȱ��ȭ ���·� �Ѿ �� ��Ȱ��ȭ �ؽ��ķ� �����Ѵ�.
    private void Attack()
    {
        switch (mState)
        {
            case MachineState.Idle:
                textureIndex = 0;
                IdleAttack();
                break;
            case MachineState.Beam:
                textureIndex = 1;
                BeamAttack();
                break;
        }
    }
    void IdleAttack()
    {
        curAttackTime += Time.deltaTime;

        if (curAttackTime > attackTime)
        {
            curAttackTime = 0f;
            curCreateTime = createTime;

            SetTexture(textureIndex, false);

            if (photonView.IsMine)
            {
                photonView.RPC("RpcChangeYState", RpcTarget.All, YamatoState.Disable);
            }
        }
        Fire();
    }

    // ���� �ð� �������� �Ѿ��� �߻��ϰ� �ʹ�.
    void Fire()
    {
        if (photonView.IsMine == false) return;

        curCreateTime += Time.deltaTime;

        if (curCreateTime > createTime)
        {
            curCreateTime = 0f;
            PhotonNetwork.Instantiate("YamatoMissile", firePos.position, firePos.rotation);
            photonView.RPC("RPCYamatoEffect", RpcTarget.All);
        }
    }

    [PunRPC]
    void RPCYamatoEffect()
    {
        source.PlayOneShot(attackSound);
        GameObject effect = Instantiate(yamatoEffect);
        effect.transform.position = firePos.position + firePos.up * 0.5f;
        effect.transform.up = firePos.up;
        Destroy(effect, 1.0f);
    }

    // �� ���� ����
    void BeamAttack()
    {
        if (Laser.activeSelf == false)
        {
            Laser.SetActive(true);
        }

        curAttackTime += Time.deltaTime;

        if (curAttackTime > attackTime)
        {
            curAttackTime = 0f;

            SetTexture(textureIndex, false);

            if (photonView.IsMine)
            {
                photonView.RPC("RpcChangeYState", RpcTarget.All, YamatoState.Disable);
            }
        }
    }

    // ��Ȱ��ȭ ����
    // - �� Ÿ���� �� ���� Ȱ��ȭ ���·� �����ϰ� �ʹ�.
    // - �� Ÿ���� �� ���� Ȱ��ȭ �ؽ��ķ� �ٲ۴�.
    private void Disable()
    {
        if (Laser.activeSelf == true)
        {
            Laser.SetActive(false);
        }

        curCoolTime += Time.deltaTime;

        if (curCoolTime > coolTime)
        {
            curCoolTime = 0f;

            SetTexture(textureIndex, true);

            if (photonView.IsMine)
            {
                photonView.RPC("RpcChangeYState", RpcTarget.All, YamatoState.Enable);
            }
        }
    }

    // �ؽ��� ���� �Լ�
    void SetTexture(int index, bool isEnable)
    {
        if (photonView.IsMine)
        {
            photonView.RPC("RpcSetTexture", RpcTarget.All, index, isEnable);
        }
    }

    [PunRPC]
    void RpcSetTexture(int index, bool isEnable)
    {
        for (int i = 0; i < liveTextures.Count; i++)
        {
            liveTextures[i].SetActive(false);
            deadTextures[i].SetActive(false);
        }

        liveTextures[index].SetActive(isEnable);
        deadTextures[index].SetActive(!isEnable);

        controls[0].SetActive(isEnable);
        controls[1].SetActive(!isEnable);
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name.Contains("Beam"))
        {
            if (photonView.IsMine)
            {
                photonView.RPC("RpcChangeMState", RpcTarget.All, MachineState.Beam);
                textureIndex = 1;
                SetTexture(textureIndex, yState != YamatoState.Disable);
            }

        }
    }

    private void OnTriggerExit(Collider other)
    {
        // ���� ���� �������� mState�� �⺻ ���·� ��ȯ�ϰ� �ʹ�.
        if (other.gameObject.name.Contains("Jewel"))
        {
            if (photonView.IsMine)
            {
                photonView.RPC("RpcChangeMState", RpcTarget.All, MachineState.Idle);
                textureIndex = 0;
                SetTexture(textureIndex, yState != YamatoState.Disable);
            }
        } 
       
    }
}
