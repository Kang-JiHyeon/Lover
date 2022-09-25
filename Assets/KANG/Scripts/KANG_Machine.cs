using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

// 머신들을 기능을 정의
// - Arrow Key Down
// - Arrow Key Up
// - M키 Down
// - M키 Up


public class KANG_Machine : MonoBehaviourPun
{
    public Transform rotAxis;
    public Vector3 localAngle;
    public float worldZ;
    public float rotDir = 0f;
    public float rotSpeed = 30f;


    public enum MachineState
    {
        Idle,
        Power,
        Beam,
        Steel
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public virtual void UpKey()
    {
        //worldZ = rotAxis.eulerAngles.z;

        //rotDir = (worldZ > 0f && worldZ < 180f) ? -1 : 1;
        ////photonView.RPC("RpcUpKey", RpcTarget.All, (worldZ > 0f && worldZ < 180f) ? -1 : 1);
    }

    public virtual void DownKey()
    {
        //worldZ = rotAxis.eulerAngles.z;

        //rotDir = (worldZ >= 0f && worldZ <= 180f) ? 1 : -1;
    }

    public virtual void LeftKey()
    {
        //worldZ = rotAxis.eulerAngles.z;

        //rotDir = (worldZ >= 0f && worldZ < 90f) || (worldZ >= 270f && worldZ < 360f) ? 1 : -1;
    }

    public virtual void RightKey()
    {
        //worldZ = rotAxis.eulerAngles.z;

        //rotDir = (worldZ >= 0f && worldZ < 90f) || (worldZ >= 270f && worldZ < 360f) ? -1 : 1;
    }

    public virtual void ArrowKey()
    {

    }

    public virtual void ArrowKeyUp()
    {

    }

    public virtual void ActionKey()
    {

    }

    public virtual void ActionKeyUp()
    {

    }

    public virtual void Rotate()
    {
        //localAngle.z += rotDir * rotSpeed * deltaTime;
        //localAngle.z = localAngle.z > 180 ? localAngle.z - 360 : localAngle.z;
        //rotAxis.localRotation = Quaternion.Euler(0, 0, localAngle.z);

    }
}
