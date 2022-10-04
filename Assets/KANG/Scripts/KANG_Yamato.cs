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
    public AudioClip beamLoop;
    public AudioClip metalLoop;
    public AudioClip metalSound;
    public AudioClip missile;
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

    public KANG_YamatoMetal metal;


    public enum YamatoState
    {
        Enable,
        Attack,
        Disable
    }

    public YamatoState yState = YamatoState.Disable;
    public MachineState mState = MachineState.Idle;


    public List<Transform> powerFirePoss;

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

        // �Ŀ� �ѱ� ��ġ ����Ʈ
        Transform yamatoPower = liveTextures[2].transform;
        for (int i=0; i< yamatoPower.childCount; i++)
        {
            powerFirePoss.Add(yamatoPower.GetChild(i));
        }
    }

    // Update is called once per frame
    void Update()
    {
        // ġƮŰ
        if (PhotonNetwork.IsMasterClient)
        {
            // idle
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                photonView.RPC("RpcChangeMState", RpcTarget.All, MachineState.Idle);
                SetTexture(yState != YamatoState.Disable);
            }

            // Beam
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                photonView.RPC("RpcChangeMState", RpcTarget.All, MachineState.Beam);
                SetTexture(yState != YamatoState.Disable);
            }
            // power
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                photonView.RPC("RpcChangeMState", RpcTarget.All, MachineState.Power);
                SetTexture(yState != YamatoState.Disable);
            }
            // metal
            if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                photonView.RPC("RpcChangeMState", RpcTarget.All, MachineState.Metal);
                SetTexture(yState != YamatoState.Disable);
            }
        }

        


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
    public void ChangeYState(YamatoState state)
    {
        photonView.RPC("RpcChangeYState", RpcTarget.All, state);
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
                IdleAttack();
                Fire();
                break;
            case MachineState.Beam:
                BeamAttack();
                break;
            case MachineState.Power:
                IdleAttack();
                PowerAttack();
                break;
            case MachineState.Metal:
                MetalAttack();
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

            SetTexture(false);

            if (photonView.IsMine)
            {
                photonView.RPC("RpcChangeYState", RpcTarget.All, YamatoState.Disable);
            }
        }
        //Fire();
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

    [PunRPC]
    void RPCMissileSound()
    {
        source.PlayOneShot(missile);
    }

    // �� ���� ����
    void BeamAttack()
    {
        if (Laser.activeSelf == false)
        {
            Laser.SetActive(true);
        }

        photonView.RPC("RPCYBeamSound", RpcTarget.All, true);

        curAttackTime += Time.deltaTime;

        if (curAttackTime > attackTime)
        {
            photonView.RPC("RPCYBeamSound", RpcTarget.All, false);
            curAttackTime = 0f;
            SetTexture(false);

            if (photonView.IsMine)
            {
                photonView.RPC("RpcChangeYState", RpcTarget.All, YamatoState.Disable);
            }
        }
    }

    [PunRPC]
    void RPCYBeamSound(bool value)
    {
        if(value)
        {
            if (!source.isPlaying)
            {
                source.clip = beamLoop;
                source.Play();
            }
        }
        else
            source.Stop();
    }

    // ���� �ð����� �ѱ��鿡�� ����ź�� ������ �ϰ� �ʹ�.
    void PowerAttack()
    {
        curCreateTime += Time.deltaTime;

        if (curCreateTime > 0.25f)
        {
            for (int i = 0; i < powerFirePoss.Count; i++)
            {
                PhotonNetwork.Instantiate("YamatoPowerMissile", powerFirePoss[i].position, powerFirePoss[i].rotation);
            }
            photonView.RPC("RPCMissileSound", RpcTarget.All);
            curCreateTime = 0f;
        }
    }

    void MetalAttack()
    {
        if (metal.state == KANG_YamatoMetal.BladeState.Idle)
        {
            metal.state = KANG_YamatoMetal.BladeState.UpRotate;
            photonView.RPC("RPCMetalSound", RpcTarget.All);
        }
    }

    [PunRPC]
    void RPCMetalSound()
    {
        source.PlayOneShot(metalSound);
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

            SetTexture(true);

            if (photonView.IsMine)
            {
                photonView.RPC("RpcChangeYState", RpcTarget.All, YamatoState.Enable);
            }
        }
    }

    // �ؽ��� ���� �Լ�
    public void SetTexture(bool isEnable)
    {
        if (photonView.IsMine)
        {
            photonView.RPC("RpcSetTexture", RpcTarget.All, isEnable);
        }
    }

    [PunRPC]
    void RpcSetTexture(bool isEnable)
    {
        for (int i = 0; i < liveTextures.Count; i++)
        {
            liveTextures[i].SetActive(false);
            deadTextures[i].SetActive(false);
        }

        liveTextures[(int)mState].SetActive(isEnable);
        deadTextures[(int)mState].SetActive(!isEnable);

        controls[0].SetActive(isEnable);
        controls[1].SetActive(!isEnable);
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name.Contains("Beam"))
        {
            photonView.RPC("RpcChangeMState", RpcTarget.All, MachineState.Beam);
        }
        else if (other.gameObject.name.Contains("Power"))
        {
            photonView.RPC("RpcChangeMState", RpcTarget.All, MachineState.Power);
        }
        else if (other.gameObject.name.Contains("Metal"))
        {
            photonView.RPC("RpcChangeMState", RpcTarget.All, MachineState.Metal);
        }

        SetTexture(yState != YamatoState.Disable);
    }

    private void OnTriggerExit(Collider other)
    {
        // ���� ���� �������� mState�� �⺻ ���·� ��ȯ�ϰ� �ʹ�.
        if (other.gameObject.name.Contains("Crystal"))
        {
            if (photonView.IsMine)
            {
                photonView.RPC("RpcChangeMState", RpcTarget.All, MachineState.Idle);
                textureIndex = 0;
                SetTexture(yState != YamatoState.Disable);
            }
        } 
       
    }
}
