using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

// 부모 클래스의 특정키가 입력되었을 때 동작하는 함수를 재정의한다.

public class KANG_Turret : KANG_Machine
{
    // Fire
    public List<Transform> turretCannons;
    public GameObject bulletFactory;
    public float createTime = 0.5f;
    float currentTime = 0f;
    int index = 0;

    // Rotate
    //public float rotSpeed = 30f;

    // Start is called before the first frame update
    void Start()
    {
        // 총구 입구
        for (int i = 0; i < rotAxis.childCount; i++)
        {
            turretCannons.Add(rotAxis.GetChild(i));
        }

        // 현재 회전값
        localAngle = rotAxis.localEulerAngles;

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

    //public override void ArrowKey()
    //{
    //    TurretRotate();
    //}

    public override void ActionKey()
    {
        //if (photonView.IsMine == false) return;

        currentTime += Time.deltaTime;

        if (currentTime > createTime)
        {
            PhotonNetwork.Instantiate("Bullet", turretCannons[index].position, turretCannons[index].rotation);


            //GameObject bullet = Instantiate(bulletFactory);
            //bullet.transform.position = turretCannons[index].position;
            //bullet.transform.up = rotAxis.up;
            currentTime = 0f;
            index++;
            index %= rotAxis.childCount;
        }
    }

    void TurretRotate(float deltaTime)
    {
        localAngle.z += rotDir * rotSpeed * deltaTime;
        localAngle.z = localAngle.z > 180 ? localAngle.z - 360 : localAngle.z;
        localAngle.z = Mathf.Clamp(localAngle.z, -100, 100);
        rotAxis.localRotation = Quaternion.Euler(0, 0, localAngle.z);
    }
}
