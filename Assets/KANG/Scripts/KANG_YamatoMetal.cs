using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// 1. metal�� 180�� ȸ���ϰ� �ʹ�.
// 2. �������� y Scale�� �ø���, Blade�� y ��ġ�� �ø��� �ʹ�. 
// 3. Blade�� ũ�⸦ �ø��� �ʹ�.

public class KANG_YamatoMetal : MonoBehaviour
{
    public KANG_Yamato yamato;
    public Transform yamatoMetal;
    public Transform axis;
    public Transform blade;
    public Transform bladeBack;
    Vector3 currentRot;
    float curAxisScaleY;
    float curBladePosY;
    float curBladeScale;

    public float rotSpeed = 250f;
    public float axisYScale = 180;
    public float bladePos;
    public float bladeScale;

    public float attackTime = 3f;
    float curTime = 0f;

    public enum BladeState
    {
        Idle,
        UpRotate,
        Up,
        UpBlade,
        Expand,
        Attack,
        Contract,
        Down,
        DownRotate
    }

    public BladeState state = BladeState.UpRotate;

    // Start is called before the first frame update
    void Start()
    {
        yamatoMetal = transform.GetChild(0);
        axis = yamatoMetal.GetChild(0);
        blade = yamatoMetal.GetChild(1);
        bladeBack = blade.GetChild(1);

        currentRot = yamatoMetal.localEulerAngles;
        curAxisScaleY = axis.localScale.y;
        curBladePosY = blade.localPosition.y;
        curBladeScale = bladeBack.localScale.x;
    }

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case BladeState.Idle:
                Idle();
                break;
            case BladeState.UpRotate:
                Rotate(0, 1);
                break;
            case BladeState.Up:
                Move(3.7f, 0.9f, 1);
                break;
            case BladeState.Expand:
                SetBladeScale(1.1f, 1);
                break;
            case BladeState.Contract:
                SetBladeScale(0, -1);
                break;
            case BladeState.Down:
                Move(1.2f, 0.3f, -1);
                break;
            case BladeState.DownRotate:
                Rotate(-180, -1);
                break;
        }
    }

    private void Idle()
    {

    }

    // 1. metal�� 180�� ȸ���ϰ� �ʹ�.
    private void Rotate(float targetZ, float op)
    {
        currentRot.z += Time.deltaTime * rotSpeed * op;
        currentRot.z = currentRot.z > 180 ? currentRot.z - 360 : currentRot.z;
        currentRot.z = Mathf.Clamp(currentRot.z, -180, 0);
        yamatoMetal.localRotation = Quaternion.Euler(0, 0, currentRot.z);


        if(op > 0 && currentRot.z >= targetZ)
        {
            state = BladeState.Up;
        }
        else if(op < 0 && currentRot.z <= targetZ)
        {
            state = BladeState.Idle;
            yamato.ChangeYState(KANG_Yamato.YamatoState.Disable);
            yamato.SetTexture(false);
        }
    }

    private void Move(float targetScaleY, float targetPosY, float op)
    {
        // ������ ������ ����
        curAxisScaleY += Time.deltaTime * 10 * op;
        curAxisScaleY = Mathf.Clamp(curAxisScaleY, 1.2f, 3.7f);
        axis.localScale = new Vector3(axis.localScale.x, curAxisScaleY, axis.localScale.z);

        // ���̵� ��ġ ����
        curBladePosY += Time.deltaTime * 2f * op;
        curBladePosY = Mathf.Clamp(curBladePosY, 0.3f, 0.9f);
        blade.localPosition = new Vector3(blade.localPosition.x, curBladePosY, blade.localPosition.z);


        if(op > 0 && curAxisScaleY >= targetScaleY && curBladePosY >= targetPosY)
        {
            state = BladeState.Expand;
        }
        else if(op < 0 && curAxisScaleY <= targetScaleY && curBladePosY <= targetPosY)
        {
            state = BladeState.DownRotate;
        }

    }



    // 3. Blade�� ũ�⸦ �ø��ų� ���̰� �ʹ�.
    private void SetBladeScale(float targetScale, float op)
    {
        if(op > 0)
        {
            curTime += Time.deltaTime;

            if(curTime > attackTime)
            {
                state = BladeState.Contract;
                curTime = 0f;
            }
        }

        curBladeScale += Time.deltaTime * 7 * op;
        curBladeScale = Mathf.Clamp(curBladeScale, 0f, 1.1f);
        bladeBack.localScale = new Vector3(curBladeScale, curBladeScale, curBladeScale);

        if(op < 0 && curBladeScale <= targetScale)
        {
            state = BladeState.Down;
        }
    }
}
