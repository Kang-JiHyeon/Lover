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


    public enum YamatoState
    {
        Enable,
        Attack,
        Disable
    }

    public YamatoState yState = YamatoState.Disable;


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
            
            photonView.RPC("RpcChangeState", RpcTarget.All, YamatoState.Attack);
        }
    }

    [PunRPC]
    void RpcChangeState(YamatoState state)
    {
        yState = state;
        print("RPC : " + yState);
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
        curAttackTime += Time.deltaTime;

        if (curAttackTime > attackTime)
        {
            curAttackTime = 0f;
            curCreateTime = createTime;

            SetEnableTexture(false);

            //yState = YamatoState.Disable;
            if (photonView.IsMine)
            {
                photonView.RPC("RpcChangeState", RpcTarget.All, YamatoState.Disable);
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


            //GameObject yamatoBullet = Instantiate(yamatoBulletFactory);
            //yamatoBullet.transform.position = firePos.position;
            //yamatoBullet.transform.up = firePos.transform.up;
        }
    }

    // ��Ȱ��ȭ ����
    // - �� Ÿ���� �� ���� Ȱ��ȭ ���·� �����ϰ� �ʹ�.
    // - �� Ÿ���� �� ���� Ȱ��ȭ �ؽ��ķ� �ٲ۴�.
    private void Disable()
    {
        curCoolTime += Time.deltaTime;

        if (curCoolTime > coolTime)
        {
            curCoolTime = 0f;

            SetEnableTexture(true);

            //yState = YamatoState.Enable;

            if (photonView.IsMine)
            {
                photonView.RPC("RpcChangeState", RpcTarget.All, YamatoState.Enable);

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
}
