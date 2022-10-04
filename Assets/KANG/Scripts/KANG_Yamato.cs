using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// yamato를 조작할 수 있고, 공격 키를 누르면 공격을 발사하고 싶다.

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

        // 파워 총구 위치 리스트
        Transform yamatoPower = liveTextures[2].transform;
        for (int i=0; i< yamatoPower.childCount; i++)
        {
            powerFirePoss.Add(yamatoPower.GetChild(i));
        }
    }

    // Update is called once per frame
    void Update()
    {
        // 치트키
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

    // 일정 시간 간격으로 총알을 발사하고 싶다.
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

    // 빔 상태 공격
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

    // 일정 시간마다 총구들에서 유도탄이 나가게 하고 싶다.
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

            SetTexture(true);

            if (photonView.IsMine)
            {
                photonView.RPC("RpcChangeYState", RpcTarget.All, YamatoState.Enable);
            }
        }
    }

    // 텍스쳐 변경 함수
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
        // 보석 장착 없어지면 mState를 기본 상태로 전환하고 싶다.
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
