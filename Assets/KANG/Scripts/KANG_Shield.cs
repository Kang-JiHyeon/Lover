
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

    public Transform shieldWave;

    Vector3 downPos;
    Vector3 upPos;

    //public float rotSpeed = 30f;

    // Start is called before the first frame update
    void Start()
    {
        //rotAxis = transform.GetChild(0);
        //engine = GameObject.Find("Engine").GetComponent<KANG_Engine>();
        //shieldWave = rotAxis.Find("ShieldWave_Tex");

        upPos = shieldWave.localPosition;
        downPos = shieldWave.localPosition;
        downPos.y = shieldWave.localPosition.y - upDownValue;
        localAngle = rotAxis.localEulerAngles;
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

    // ���� ȸ��
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

    // ����Ű�� ���� shieldWave�� upPos���� �ö������ �ϰ� �ʹ�.
    // ArrowKey�� �ԷµǸ� stopCorutine�� �ϰ� �ʹ�.
    IEnumerator IeArrowKeyUp()
    {
        while (shieldWave.localPosition.y < upPos.y)
        {
            //print(shieldWave.localPosition.y);
            LerpUpDownPos(upPos);
            yield return null;
        }
    }

    void LerpUpDownPos(Vector3 pos)
    {
        photonView.RPC("RPCLerpUpDownPos", RpcTarget.All, Vector3.Lerp(shieldWave.localPosition, pos, Time.deltaTime * upDownSpeed));
        
        if (Mathf.Abs((shieldWave.localPosition - pos).magnitude) < 0.1f)
        {
            photonView.RPC("RPCLerpUpDownPos", RpcTarget.All, pos);
        }
    }

    [PunRPC]
    void RPCLerpUpDownPos(Vector3 position)
    {
        shieldWave.localPosition = position;
    }
}
