using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System;

// 부모 클래스의 특정키가 입력되었을 때 동작하는 함수를 재정의한다.

public class KANG_Turret : KANG_Machine
{
    // Object Pooling
    KANG_ObjectPooling objectPooling;

    AudioSource source;
    public AudioClip attackSound;
    public AudioClip beamSound;
    public AudioClip metalsound;

    // Turret
    SpriteRenderer spriteRender;
    public List<Sprite> turretTexs;
    public List<GameObject> cannonStates;
    
    // Idle, Power
    public List<Transform> idleFirePostions;
    public GameObject bulletFactory;
    public GameObject turretEffect;
    public float createTime = 0.1f;
    float currentTime = 0f;
    int firePosIndex = 0;

    // Beam
    public Transform beamFirePosition;
    LineRenderer Line;

    // Metal
    public GameObject turretMetal;
    public Transform metalTargetPos;
    public float metalMaxDis = 10f;
    Vector3 originMetalTargetPos;
    float metalTargetMoveSpeed = 5f;
    float originRotSpeed;
    bool isMetalDown = false;

    // 포탑 상태
    public MachineState mState = MachineState.Idle;
    
    // Start is called before the first frame update
    void Start()
    {
        objectPooling = GetComponent<KANG_ObjectPooling>();

        originRotSpeed = rotSpeed;
        spriteRender = GetComponent<SpriteRenderer>();

        source = GetComponent<AudioSource>();

        // 현재 회전값
        localAngle = rotAxis.localEulerAngles;

        beamFirePosition = rotAxis.GetChild(1);

        for (int i = 0; i < rotAxis.childCount; i++)
        {
            cannonStates.Add(rotAxis.GetChild(i).gameObject);
            cannonStates[i].SetActive(false);
        }
        cannonStates[0].SetActive(true);

        // idle 총구 입구
        for (int i = 0; i < cannonStates[0].transform.childCount; i++)
        {
            idleFirePostions.Add(cannonStates[0].transform.GetChild(i));
        }

        Line = GetComponent<LineRenderer>();

        originMetalTargetPos = metalTargetPos.localPosition;

    }

    private void Update()
    {
        // Test
        if (Input.GetKey(KeyCode.Q))
        {
            print("총알 발사 중");
            ActionKey();
        }

        // 치트키
        if (PhotonNetwork.IsMasterClient)
        {
            // idle
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                photonView.RPC("RpcChangeMState", RpcTarget.All, MachineState.Idle);
                photonView.RPC("RpcChangeTurretTex", RpcTarget.All);
            }

            // Beam
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                photonView.RPC("RpcChangeMState", RpcTarget.All, MachineState.Beam);
                photonView.RPC("RpcChangeCannonTex", RpcTarget.All, true);
                photonView.RPC("RpcChangeTurretTex", RpcTarget.All);
            }
            // power
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                photonView.RPC("RpcChangeMState", RpcTarget.All, MachineState.Power);
                photonView.RPC("RpcChangeTurretTex", RpcTarget.All);
            }
            // metal
            if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                photonView.RPC("RpcChangeMState", RpcTarget.All, MachineState.Metal);
                photonView.RPC("RpcChangeTurretTex", RpcTarget.All);
            }

        }
        if (isMetalDown)
        {
            MetalDown();
        }
    }


    public override void UpKey()
    {
        photonView.RPC("RpcUpKey", RpcTarget.All);
    }

    [PunRPC]
    void RpcUpKey()
    {
        worldZ = rotAxis.eulerAngles.z;
        rotDir = (worldZ > 0f && worldZ < 180f) ? -1 : 1;
    }

    public override void DownKey()
    {
        photonView.RPC("RpcDownKey", RpcTarget.All);
    }

    [PunRPC]
    void RpcDownKey()
    {
        worldZ = rotAxis.eulerAngles.z;
        rotDir = (worldZ >= 0f && worldZ <= 180f) ? 1 : -1;
    }

    public override void LeftKey()
    {
        photonView.RPC("RpcLeftKey", RpcTarget.All);
    }

    [PunRPC]
    void RpcLeftKey()
    {
        worldZ = rotAxis.eulerAngles.z;
        rotDir = (worldZ >= 0f && worldZ < 90f) || (worldZ >= 270f && worldZ < 360f) ? 1 : -1;
    }

    public override void RightKey()
    {
        photonView.RPC("RpcRightKey", RpcTarget.All);
    }

    [PunRPC]
    void RpcRightKey()
    {
        worldZ = rotAxis.eulerAngles.z;
        rotDir = (worldZ >= 0f && worldZ < 90f) || (worldZ >= 270f && worldZ < 360f) ? -1 : 1;
    }

    public override void ArrowKey()
    {
        photonView.RPC("RpcArrowKey", RpcTarget.All, Time.deltaTime);
    }

    [PunRPC]
    void RpcArrowKey(float deltaTime)
    {
        TurretRotate(deltaTime);
    }

    public override void ActionKey()
    {
        switch (mState)
        {
            case MachineState.Idle:
                BulletFire(createTime);
                break;
            case MachineState.Beam:
                BeamFire();
                break;
            case MachineState.Power:
                BulletFire(createTime / 2);
                break;
            case MachineState.Metal:
                MetalUp();
                break;
        }

        isMetalDown = false;
    }

    // M키를 누르는동안 최대 거리까지 targetPos의 y값을 증가시키고 싶다.
    [PunRPC]
    void RpcSetMetalTargetPos(Vector3 position)
    {
        metalTargetPos.localPosition = position;

    }

    private void MetalUp()
    {
        Vector3 dir = metalTargetPos.position - originMetalTargetPos;
        photonView.RPC("RPCMetalTurret", RpcTarget.All, true);
        Vector3 pos = originMetalTargetPos + new Vector3(0, metalMaxDis, 0);
        
        photonView.RPC("RpcSetMetalTargetPos", RpcTarget.All, Vector3.Lerp(metalTargetPos.localPosition, pos, Time.deltaTime * metalTargetMoveSpeed));

        if (Mathf.Abs(dir.magnitude - metalMaxDis) < 0.1f)
        {
            photonView.RPC("RpcSetMetalTargetPos", RpcTarget.All, pos);
        }
    }

    [PunRPC]
    void RPCMetalTurret(bool value)
    {
        if (value)
        {
            if (!source.isPlaying)
            {
                source.clip = metalsound;
                source.Play();
            }
        }
        else
            source.Stop();
    }

    private void MetalDown()
    {
        Vector3 dir = metalTargetPos.position - originMetalTargetPos;
        photonView.RPC("RPCMetalTurret", RpcTarget.All, false);
        
        photonView.RPC("RpcSetMetalTargetPos", RpcTarget.All, Vector3.Lerp(metalTargetPos.localPosition, originMetalTargetPos, Time.deltaTime * metalTargetMoveSpeed));

        if ((dir - originMetalTargetPos).magnitude < 0.1f)
        {
            photonView.RPC("RpcSetMetalTargetPos", RpcTarget.All, originMetalTargetPos);
        }
    }


    int addValue = 1;
    void BulletFire(float fireTime)
    {
        currentTime += Time.deltaTime;

        if (currentTime > fireTime)
        {
            currentTime = 0f;

            // 총구 트랜스폼
            Transform cannon = cannonStates[(int)mState].transform.GetChild(firePosIndex);

            // 총알 발사
            photonView.RPC("RPCFire", RpcTarget.All, cannon.position, cannon.rotation);
            photonView.RPC("RPCAnim", RpcTarget.All, firePosIndex);
            photonView.RPC("RPCTurretEffect", RpcTarget.All, firePosIndex);

            // 총구 인덱스 지정
            if (firePosIndex <= 0) addValue = 1;
            else if (firePosIndex >= cannon.parent.childCount - 1) addValue = -1;
            firePosIndex += addValue;

        }
    }

    bool isBeamFire = false;
    void BeamFire()
    {
        if (isBeamFire == false)
        {
            PhotonNetwork.Instantiate("TurretBeam", beamFirePosition.position, beamFirePosition.rotation);
            photonView.RPC("RPCBeamSound", RpcTarget.All);
            isBeamFire = true;
        }
    }

    [PunRPC]
    void RPCFire(Vector3 position, Quaternion rotation)
    {
        objectPooling.UseObject(position, rotation);
    }


    [PunRPC]
    void RPCBeamSound()
    {
        source.PlayOneShot(beamSound);
    }

    public override void ActionKeyUp()
    {
        isBeamFire = false;
        isMetalDown = true;
    }

    [PunRPC]
    void RPCTurretEffect(int idx)
    {
        Transform cannon = cannonStates[(int)mState].transform;

        source.PlayOneShot(attackSound);
        GameObject effect = Instantiate(turretEffect);
        effect.transform.position = cannon.GetChild(idx).position + cannon.GetChild(idx).up * 0.5f;
        effect.transform.up = cannon.GetChild(idx).up;
        Destroy(effect, 1.0f);
    }

    [PunRPC]
    void RPCAnim(int idx)
    {
        Transform cannon = cannonStates[(int)mState].transform;

        iTween.ScaleTo(cannon.GetChild(idx).gameObject, iTween.Hash("y", 0.7f, "time", 0.05f, "easetype", iTween.EaseType.easeInOutBack));
        iTween.ScaleTo(cannon.GetChild(idx).gameObject, iTween.Hash("y", 1f, "time", 0.05f, "delay", 0.06f, "easetype", iTween.EaseType.easeInOutBack));
        iTween.MoveTo(cannon.GetChild(idx).gameObject, iTween.Hash("islocal", true, "y", 0.55f, "time", 0.05f, "easetype", iTween.EaseType.easeInOutBack));
        iTween.MoveTo(cannon.GetChild(idx).gameObject, iTween.Hash("islocal", true, "y", 0.7f, "time", 0.05f, "delay", 0.06f, "easetype", iTween.EaseType.easeInOutBack));

    }

    void TurretRotate(float deltaTime)
    {
        localAngle.z += rotDir * rotSpeed * deltaTime;
        localAngle.z = localAngle.z > 180 ? localAngle.z - 360 : localAngle.z;
        localAngle.z = Mathf.Clamp(localAngle.z, -100, 100);
        rotAxis.localRotation = Quaternion.Euler(0, 0, localAngle.z);
    }

    [PunRPC]
    void RpcChangeMState(MachineState state)
    {
        mState = state;
        if(mState == MachineState.Metal)
        {
            rotSpeed = originRotSpeed * 6f;
        }
        else
        {
            rotSpeed = originRotSpeed;
        }
    }
    [PunRPC]
    void RpcChangeTurretTex()
    {
        for (int i = 0; i < cannonStates.Count; i++)
        {
            cannonStates[i].SetActive(false);
        }
        cannonStates[(int)mState].SetActive(true);
        spriteRender.sprite = turretTexs[(int)mState];
        firePosIndex = 0;

        if(mState == MachineState.Metal)
        {
            turretMetal.SetActive(true);
        }
        else
        {
            turretMetal.SetActive(false);
        }
    }
    

    [PunRPC]
    void RpcChangeCannonTex(bool isEnable)
    {
        switch (mState)
        {
            case MachineState.Beam:
                // 활성화 상태
                beamFirePosition.GetChild(0).gameObject.SetActive(isEnable);
                // 비활성화 상태
                beamFirePosition.GetChild(1).gameObject.SetActive(!isEnable);
                break;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // 보석에 따른 Turret 모드 변경
        if (other.gameObject.name.Contains("Beam"))
        {
            photonView.RPC("RpcChangeMState", RpcTarget.All, MachineState.Beam);
            photonView.RPC("RpcChangeCannonTex", RpcTarget.All, true);
        }
        if (other.gameObject.name.Contains("Power"))
        {
            photonView.RPC("RpcChangeMState", RpcTarget.All, MachineState.Power);
        }
        if (other.gameObject.name.Contains("Metal"))
        {
            photonView.RPC("RpcChangeMState", RpcTarget.All, MachineState.Metal);
        }

        photonView.RPC("RpcChangeTurretTex", RpcTarget.All);

    }

    private void OnTriggerExit(Collider other)
    {
        // 보석 제거할 경우 기본 모드로 변경
        if (other.gameObject.name.Contains("Crystal"))
        {
            photonView.RPC("RpcChangeMState", RpcTarget.All, MachineState.Idle);
            photonView.RPC("RpcChangeTurretTex", RpcTarget.All);
        }
    }
}
