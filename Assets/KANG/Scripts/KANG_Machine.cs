using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

// �ӽŵ��� ����� ����
// - Arrow Key Down
// - Arrow Key Up
// - MŰ Down
// - MŰ Up


public class KANG_Machine : MonoBehaviourPun
{
    #region ����
    public Transform rotAxis;
    public Vector3 localAngle;
    public float worldZ;
    public float rotDir = 0f;
    public float rotSpeed = 30f;

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
    #endregion
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
