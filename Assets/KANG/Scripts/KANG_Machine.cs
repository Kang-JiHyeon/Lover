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
    public Vector3 localAngle;
    public Transform rotAxis;
    public float rotDir = 0f;
    public float rotSpeed = 30f;
    public float worldZ;
    protected bool isControl = false;

    public virtual bool IsControl
    {
        get { return isControl; }
        set { isControl = value; }
    }

    public enum MachineState
    {
        Idle,
        Beam,
        Power,
        Metal
    }

    public virtual void UpKey()
    {

    }
    public virtual void DownKey()
    {

    }
    public virtual void LeftKey()
    {
        
    }
    public virtual void RightKey()
    {
 
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

    }
}
