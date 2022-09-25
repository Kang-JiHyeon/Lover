using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// yamato를 조작할 수 있고, 공격 키를 누르면 공격을 발사하고 싶다.

public class KANG_Yamato : KANG_Machine
{
    public GameObject yamatoBulletFactory;
    public Transform firePos;
    public bool isYamatoControl = false;

    // 공격시간
    public float attackTime = 3f;
    float curAttackTime = 0f;

    // 생성시간
    public float createTime = 1f;
    float curCreateTime = 0f;

    // texture 변경 시간
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

    // 활성화 상태
    // - 조작이 가능하고, 공격키 입력이 들어오면 공격 상태로 전환한다.
    private void Enable()
    {

    }

    // 공격 상태
    // - 공격 시간동안 일정 간격으로 총알을 발사한다.
    // - 비활성화 상태로 넘어갈 때 비활성화 텍스쳐로 변경한다.
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

    // 일정 시간 간격으로 총알을 발사하고 싶다.
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

    // 비활성화 상태
    // - 쿨 타임이 다 차면 활성화 상태로 전이하고 싶다.
    // - 쿨 타임이 다 차면 활성화 텍스쳐로 바꾼다.
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

    // 텍스쳐 변경 함수
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

    // * 빔 상태 공격
    // up 방향으로 Ray를 쏜다.
    // 닿인 대상이 enemy라면
    // 없앤다.
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
