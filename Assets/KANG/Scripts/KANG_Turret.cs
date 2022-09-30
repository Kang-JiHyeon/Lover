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
    public float createTime = 0.1f;
    float currentTime = 0f;
    int index = 0;

    public List<GameObject> cannonStates;
    public Transform beamFirePosition;

    LineRenderer Line;

    SpriteRenderer spriteRender;
    public List<Sprite> turretTexs;

    public MachineState mState = MachineState.Idle;

    // Start is called before the first frame update
    void Start()
    {
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
                BulletFire(createTime);
                break;
            case MachineState.Beam:
                BeamFire();
                break;
            case MachineState.Power:
                BulletFire(createTime / 2);
                break;

        }
    }
    int addValue = 1;
    void BulletFire(float fireTime)
    {
        currentTime += Time.deltaTime;

        if (currentTime > fireTime)
        {
            //cannonStates[(int)mState].transform.GetChild(index).position
            Transform cannon = cannonStates[(int)mState].transform.GetChild(index);

            PhotonNetwork.Instantiate("Bullet", cannon.position, cannon.rotation);
            photonView.RPC("RPCAnim", RpcTarget.All, index);
            photonView.RPC("RPCTurretEffect", RpcTarget.All, index);
            currentTime = 0f;

            if (index <= 0) addValue = 1;
            else if (index >= cannon.parent.childCount - 1) addValue = -1;
            index += addValue;

        }
    }
    bool isBeamFire = false;

    void BeamFire()
    {
        if (isBeamFire == false)
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
        Transform cannon = cannonStates[(int)mState].transform;

        //source.PlayOneShot(attackSound);
        //GameObject effect = Instantiate(turretEffect);
        //effect.transform.position = idleFirePostions[idx].position + idleFirePostions[idx].up * 0.5f;
        //effect.transform.up = idleFirePostions[idx].up;
        //Destroy(effect, 1.0f);

        source.PlayOneShot(attackSound);
        GameObject effect = Instantiate(turretEffect);
        effect.transform.position = cannon.GetChild(idx).position + cannon.GetChild(idx).up * 0.5f;
        effect.transform.up = cannon.GetChild(idx).up;
        Destroy(effect, 1.0f);
    }

    [PunRPC]
    void RPCAnim(int idx)
    {
        //iTween.ScaleTo(idleFirePostions[idx].gameObject, iTween.Hash("y", 0.7f, "time", 0.05f, "easetype", iTween.EaseType.easeInOutBack));
        //iTween.ScaleTo(idleFirePostions[idx].gameObject, iTween.Hash("y", 1f, "time", 0.05f, "delay", 0.06f, "easetype", iTween.EaseType.easeInOutBack));
        //iTween.MoveTo(idleFirePostions[idx].gameObject, iTween.Hash("islocal", true, "y", 0.55f, "time", 0.05f, "easetype", iTween.EaseType.easeInOutBack));
        //iTween.MoveTo(idleFirePostions[idx].gameObject, iTween.Hash("islocal", true, "y", 0.7f, "time", 0.05f, "delay", 0.06f, "easetype", iTween.EaseType.easeInOutBack));

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

    bool isLine = false;

    [PunRPC]
    void RpcChangeMState(MachineState state)
    {
        mState = state;
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
        index = 0;
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
        else if (other.gameObject.name.Contains("Power"))
        {
            photonView.RPC("RpcChangeMState", RpcTarget.All, MachineState.Power);
            photonView.RPC("RpcChangeTurretTex", RpcTarget.All);

        }

    }

    private void OnTriggerExit(Collider other)
    {
        isLine = false;
    }
}
