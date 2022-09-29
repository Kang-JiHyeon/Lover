using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

// 부모 클래스의 특정키가 입력되었을 때 동작하는 함수를 재정의한다.

public class KANG_Turret : KANG_Machine
{
    AudioSource source;
    public AudioClip attackSound;
    // Fire
    public List<Transform> idleFirePostions;
    public GameObject bulletFactory;
    public GameObject turretEffect;
    public float createTime = 0.5f;
    float currentTime = 0f;
    int index = 0;

    public List<GameObject> turretStateTexs;
    public Transform beamFirePosition;

    LineRenderer Line;

    public MachineState mState = MachineState.Idle;

    // Start is called before the first frame update
    void Start()
    {
        source = GetComponent<AudioSource>();

        // 현재 회전값
        localAngle = rotAxis.localEulerAngles;

        beamFirePosition = rotAxis.GetChild(1);

        for (int i = 0; i < rotAxis.childCount; i++)
        {
            turretStateTexs.Add(rotAxis.GetChild(i).gameObject);
            turretStateTexs[i].SetActive(false);
        }
        turretStateTexs[0].SetActive(true);

        // idle 총구 입구
        for (int i = 0; i < turretStateTexs[0].transform.childCount; i++)
        {
            idleFirePostions.Add(turretStateTexs[0].transform.GetChild(i));
        }

        Line = GetComponent<LineRenderer>();

    }

    private void Update()
    {
        //if (isLine)
        //{
        //    RaycastHit hit;
        //    Line.SetPosition(0, beamFirePosition.position);
        //    if (Physics.Raycast(beamFirePosition.position, beamFirePosition.up, out hit, 100))
        //    {
        //        Debug.DrawRay(beamFirePosition.position, beamFirePosition.up, Color.red);
        //        Line.SetPosition(1, beamFirePosition.position + new Vector3(0, hit.distance, 0));
        //        print("조준선 그림");
        //    }
        //}
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

    // 엔진 회전
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
                IdleFire();
                break;
            case MachineState.Beam:
                BeamFire();
                break;
        }
    }

    void IdleFire()
    {
        currentTime += Time.deltaTime;

        if (currentTime > createTime)
        {
            PhotonNetwork.Instantiate("Bullet", idleFirePostions[index].position, idleFirePostions[index].rotation);
            photonView.RPC("RPCAnim", RpcTarget.All, index);
            photonView.RPC("RPCTurretEffect", RpcTarget.All, index);
            currentTime = 0f;
            index++;
            index %= rotAxis.childCount;
        }
    }
    bool isBeamFire = false;

    void BeamFire()
    {
        if(isBeamFire == false)
        {
            PhotonNetwork.Instantiate("TurretBeam", beamFirePosition.position, beamFirePosition.rotation);
            isBeamFire = true;
        }
    }


    public override void ActionKeyUp()
    {
        isBeamFire = false;
    }

    [PunRPC]
    void RPCTurretEffect(int idx)
    {
        source.PlayOneShot(attackSound);
        GameObject effect = Instantiate(turretEffect);
        effect.transform.position = idleFirePostions[idx].position + idleFirePostions[idx].up * 0.5f;
        effect.transform.up = idleFirePostions[idx].up;
        Destroy(effect, 1.0f);
    }

    [PunRPC]
    void RPCAnim(int idx)
    {
        iTween.ScaleTo(idleFirePostions[idx].gameObject, iTween.Hash("y", 0.7f, "time", 0.05f, "easetype", iTween.EaseType.easeInOutBack));
        iTween.ScaleTo(idleFirePostions[idx].gameObject, iTween.Hash("y", 1f, "time", 0.05f, "delay", 0.06f, "easetype", iTween.EaseType.easeInOutBack));
        iTween.MoveTo(idleFirePostions[idx].gameObject, iTween.Hash("islocal", true, "y", 0.55f, "time", 0.05f, "easetype", iTween.EaseType.easeInOutBack));
        iTween.MoveTo(idleFirePostions[idx].gameObject, iTween.Hash("islocal", true, "y", 0.7f, "time", 0.05f, "delay", 0.06f, "easetype", iTween.EaseType.easeInOutBack));
    }

    void TurretRotate(float deltaTime)
    {
        localAngle.z += rotDir * rotSpeed * deltaTime;
        localAngle.z = localAngle.z > 180 ? localAngle.z - 360 : localAngle.z;
        localAngle.z = Mathf.Clamp(localAngle.z, -100, 100);
        rotAxis.localRotation = Quaternion.Euler(0, 0, localAngle.z);
    }

    bool isLine = false;

    [PunRPC]
    void RpcChangeMState(MachineState state)
    {
        mState = state;
    }
    [PunRPC]
    void RpcChangeTurretTex()
    {
        for(int i =0; i<turretStateTexs.Count; i++)
        {
            turretStateTexs[i].SetActive(false);
        }
        turretStateTexs[(int)mState].SetActive(true);
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


    // 플레이어가 감지되면 조종선 활성화
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            isLine = true;
        }

        if (other.gameObject.name.Contains("Beam"))
        {
            
            photonView.RPC("RpcChangeMState", RpcTarget.All, MachineState.Beam);
            photonView.RPC("RpcChangeTurretTex", RpcTarget.All);
            photonView.RPC("RpcChangeCannonTex", RpcTarget.All, true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        isLine = false;
    }
}
