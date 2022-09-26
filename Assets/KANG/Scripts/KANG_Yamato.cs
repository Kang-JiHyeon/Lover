using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// yamato�� ������ �� �ְ�, ���� Ű�� ������ ������ �߻��ϰ� �ʹ�.

public class KANG_Yamato : KANG_Machine
{
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
    public List<GameObject> machines;
    public List<GameObject> controls;

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
        curCreateTime = createTime;

        for (int i = 0; i < transform.childCount - 1; i++)
        {
            Transform child = transform.GetChild(i);
            for (int j = 0; j < child.childCount; j++)
            {
                GameObject texture = child.GetChild(j).gameObject;

                if (texture)
                {
                    if (i == 0)
                        machines.Add(texture);
                    else if (i == 1)
                        controls.Add(texture);
                }
            }

        }
        SetEnableTexture(false);
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
            //yState = YamatoState.Attack;
            
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
                IdleAttack();
                break;
            case MachineState.Beam:
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

            SetEnableTexture(false);

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

            SetEnableTexture(true);

            if (photonView.IsMine)
            {
                photonView.RPC("RpcChangeYState", RpcTarget.All, YamatoState.Enable);

            }
        }
    }

    // �ؽ��� ���� �Լ�
    void SetEnableTexture(bool isEnable)
    {
        //machines[0].SetActive(isEnable);
        //machines[1].SetActive(!isEnable);

        //controls[0].SetActive(isEnable);
        //controls[1].SetActive(!isEnable);

        if (photonView.IsMine)
        {
            photonView.RPC("RpcSetEnableTexture", RpcTarget.All, isEnable);
        }
    }

    [PunRPC]
    void RpcSetEnableTexture(bool isEnable)
    {
        machines[0].SetActive(isEnable);
        machines[1].SetActive(!isEnable);

        controls[0].SetActive(isEnable);
        controls[1].SetActive(!isEnable);
    }

    // * �� ���� ����
    // up �������� Ray�� ���.
    // ���� ����� enemy���
    // ���ش�.
    // 
    void BeamAttack()
    {
        if(Laser.activeSelf == false)
        {
            Laser.SetActive(true);
        }

        curAttackTime += Time.deltaTime;

        if (curAttackTime > attackTime)
        {
            curAttackTime = 0f;

            if (photonView.IsMine)
            {
                photonView.RPC("RpcChangeYState", RpcTarget.All, YamatoState.Disable);
            }
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name.Contains("Beam"))
        {
            if (photonView.IsMine)
            {
                photonView.RPC("RpcChangeMState", RpcTarget.All, MachineState.Beam);
            }
            
        }
    }
}
