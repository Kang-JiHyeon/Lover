using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

// �θ� Ŭ������ Ư��Ű�� �ԷµǾ��� �� �����ϴ� �Լ��� �������Ѵ�.

public class KANG_Turret : KANG_Machine
{
    // Fire
    public List<Transform> turretCannons;
    public GameObject bulletFactory;
    public GameObject turretEffect;
    public float createTime = 0.5f;
    float currentTime = 0f;
    int index = 0;

    // Rotate
    //public float rotSpeed = 30f;

    // Start is called before the first frame update
    void Start()
    {
        // �ѱ� �Ա�
        for (int i = 0; i < rotAxis.childCount; i++)
        {
            turretCannons.Add(rotAxis.GetChild(i));
        }

        // ���� ȸ����
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

    // ���� ȸ��
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
            photonView.RPC("RPCAnim", RpcTarget.All, index);
            photonView.RPC("RPCTurretEffect", RpcTarget.All, index);

            //GameObject bullet = Instantiate(bulletFactory);
            //bullet.transform.position = turretCannons[index].position;
            //bullet.transform.up = rotAxis.up;
            currentTime = 0f;
            index++;
            index %= rotAxis.childCount;
        }
    }

    [PunRPC]
    void RPCTurretEffect(int idx)
    {
        GameObject effect = Instantiate(turretEffect);
        effect.transform.position = turretCannons[idx].position + turretCannons[idx].up * 0.5f;
        effect.transform.up = turretCannons[idx].up;
        Destroy(effect, 1.0f);
    }

    [PunRPC]
    void RPCAnim(int idx)
    {
        iTween.ScaleTo(turretCannons[idx].gameObject, iTween.Hash("y", 0.7f, "time", 0.05f, "easetype", iTween.EaseType.easeInOutBack));
        iTween.ScaleTo(turretCannons[idx].gameObject, iTween.Hash("y", 1f, "time", 0.05f, "delay", 0.06f, "easetype", iTween.EaseType.easeInOutBack));
        iTween.MoveTo(turretCannons[idx].gameObject, iTween.Hash("islocal", true, "y", 0.55f, "time", 0.05f, "easetype", iTween.EaseType.easeInOutBack));
        iTween.MoveTo(turretCannons[idx].gameObject, iTween.Hash("islocal", true, "y", 0.7f, "time", 0.05f, "delay", 0.06f, "easetype", iTween.EaseType.easeInOutBack));
    }

    void TurretRotate(float deltaTime)
    {
        localAngle.z += rotDir * rotSpeed * deltaTime;
        localAngle.z = localAngle.z > 180 ? localAngle.z - 360 : localAngle.z;
        localAngle.z = Mathf.Clamp(localAngle.z, -100, 100);
        rotAxis.localRotation = Quaternion.Euler(0, 0, localAngle.z);
    }
}
