using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// yamato를 조작할 수 있고, 공격 키를 누르면 공격을 발사하고 싶다.

public class KANG_Yamato : MonoBehaviour
{
    KIM_PlayerController pc;

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

        for (int i = 0; i < transform.childCount; i++)
        {
            Transform child = transform.GetChild(i);
            for(int j = 0; j< child.childCount; j++)
            {
                GameObject texture = child.GetChild(j).gameObject;

                if (texture)
                {
                    if (i == 0)
                        machines.Add(texture);
                    else if(i==1)
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

    // 활성화 상태
    // - 조작이 가능하고, 공격키 입력이 들어오면 공격 상태로 전환한다.
    private void Enable()
    {
        if (isYamatoControl)
        {
            // 1P / 2P
            if((!pc.is2P && Input.GetKeyDown(KeyCode.M)) || (pc.is2P && Input.GetKeyDown(KeyCode.X)))
            {
                    yState = YamatoState.Attack;
            }
        }
    }

    // 공격 상태
    // - 공격 시간동안 일정 간격으로 총알을 발사한다.
    // - 비활성화 상태로 넘어갈 때 비활성화 텍스쳐로 변경한다.
    private void Attack()
    {
        Fire();

        curAttackTime += Time.deltaTime;

        if (curAttackTime > attackTime)
        {
            curAttackTime = 0f;
            curCreateTime = createTime;

            SetEnableTexture(false);

            yState = YamatoState.Disable;
        }
    }

    // 일정 시간 간격으로 총알을 발사하고 싶다.
    void Fire()
    {
        curCreateTime += Time.deltaTime;

        if (curCreateTime > createTime)
        {
            curCreateTime = 0f;
            GameObject yamatoBullet = Instantiate(yamatoBulletFactory);
            yamatoBullet.transform.position = firePos.position;
            yamatoBullet.transform.up = firePos.transform.up;
        }
    }

    // 비활성화 상태
    // - 쿨 타임이 다 차면 활성화 상태로 전이하고 싶다.
    // - 쿨 타임이 다 차면 활성화 텍스쳐로 바꾼다.
    private void Disable()
    {
        curCoolTime += Time.deltaTime;

        if(curCoolTime > coolTime)
        {
            curCoolTime = 0f;

            SetEnableTexture(true);

            yState = YamatoState.Enable;
        }
    }

    // 텍스쳐 변경 함수
    void SetEnableTexture(bool isEnable)
    {
        machines[0].SetActive(isEnable);
        machines[1].SetActive(!isEnable);

        controls[0].SetActive(isEnable);
        controls[1].SetActive(!isEnable);
    }


    private void OnTriggerStay(Collider other)
    {
        // Player가 감지되는 동안 Yamato를 조작할 수 있다.
        if (other.gameObject.TryGetComponent<KIM_PlayerController>(out pc) && pc.IsModule)
        {
            isYamatoControl = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        // Player가 감지 범위를 벗어나면 Yamato를 조작할 수 있다.
        if (other.gameObject.TryGetComponent<KIM_PlayerController>(out pc) && !pc.IsModule)
        {
            isYamatoControl = false;
        }
    }

}
