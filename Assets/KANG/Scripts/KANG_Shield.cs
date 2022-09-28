
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class KANG_Shield : KANG_Machine
{
    public float upDownSpeed = 5f;
    public float upDownValue = 0.4f;

    //public Transform shieldAxis;
    public KANG_Engine engine;

    public Transform shield;

    Vector3 downPos;
    Vector3 upPos;

    public MachineState mState = MachineState.Idle;

    public List<GameObject> shieldGenerators;
    public List<GameObject> shieldTextures;

    // Start is called before the first frame update
    void Start()
    {
        //rotAxis = transform.GetChild(0);
        //engine = GameObject.Find("Engine").GetComponent<KANG_Engine>();
        //shieldWave = rotAxis.Find("ShieldWave_Tex");

        upPos = shield.localPosition;
        downPos = shield.localPosition;
        downPos.y = shield.localPosition.y - upDownValue;
        localAngle = rotAxis.localEulerAngles;

        shieldGenerators[0].SetActive(true);
        shieldTextures[0].SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
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
        StopCoroutine(IeArrowKeyUp());
        LerpUpDownPos(downPos);
        photonView.RPC("RpcArrowKey", RpcTarget.All, Time.deltaTime);
    }

    [PunRPC]
    void RpcArrowKey(float deltaTime)
    {
        localAngle.z += rotDir * rotSpeed * deltaTime;
        localAngle.z = localAngle.z > 180 ? localAngle.z - 360 : localAngle.z;
        rotAxis.localRotation = Quaternion.Euler(0, 0, localAngle.z);
    }


    public override void ArrowKeyUp()
    {
        StartCoroutine(IeArrowKeyUp());
    }

    // 방향키를 떼면 shieldWave가 upPos까지 올라오도록 하고 싶다.
    // ArrowKey가 입력되면 stopCorutine을 하고 싶다.
    IEnumerator IeArrowKeyUp()
    {
        while (shield.localPosition.y < upPos.y)
        {
            //print(shieldWave.localPosition.y);
            LerpUpDownPos(upPos);
            yield return null;
        }
    }

    void LerpUpDownPos(Vector3 pos)
    {
        photonView.RPC("RPCLerpUpDownPos", RpcTarget.All, Vector3.Lerp(shield.localPosition, pos, Time.deltaTime * upDownSpeed));
        
        if (Mathf.Abs((shield.localPosition - pos).magnitude) < 0.1f)
        {
            photonView.RPC("RPCLerpUpDownPos", RpcTarget.All, pos);
        }
    }

    [PunRPC]
    void RPCLerpUpDownPos(Vector3 position)
    {
        shield.localPosition = position;
    }

    // 머신 상태에 따른 쉴드 변경
    void SetShield(int index, bool isEnable)
    {
        photonView.RPC("RpcSetShield", RpcTarget.All, index, isEnable);
    }

    [PunRPC]
    void RpcSetShield(int index, bool isEnable)
    {
        shieldGenerators[index].SetActive(isEnable);
        shieldTextures[index].SetActive(isEnable);
    }

    [PunRPC]
    void RpcChangeMState(MachineState state)
    {
        mState = state;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name.Contains("Beam"))
        {
            SetShield((int)mState, false);

            //mState = MachineState.Beam;
            photonView.RPC("RpcChangeMState", RpcTarget.All, MachineState.Beam);

            SetShield((int)mState, true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // 보석 장착 없어지면 mState를 기본 상태로 전환하고 싶다.
        if (other.gameObject.name.Contains("Jewel"))
        {
            SetShield((int)mState, false);

            //mState = MachineState.Idle;
            photonView.RPC("RpcChangeMState", RpcTarget.All, MachineState.Idle);

            SetShield((int)mState, true);
        }
    }
}
