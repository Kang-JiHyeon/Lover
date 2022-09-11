using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 입력값 
public class KANG_InputRotate : KANG_AutoRotate
{
    // 방향키마다 지정된 각도까지만 회전하고 싶다.
    float targetRot = 0f;
    // Start is called before the first frame update


    public float up = 0f;
    public float right = -0.7f;
    public float down = 1f;
    public float left = 0.7f;
    void Start()
    {
        
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Rotate();
        Move();
        //InputDir();
    }

    void InputDir()
    {
        rotDir = Input.GetAxisRaw("Horizontal");

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            targetRot = 0f;
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            targetRot = -90f;
        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            targetRot = 180f;
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            targetRot = 90f;
        }


    }
    void Move()
    {
        // 좌우
        float h = Input.GetAxisRaw("Horizontal");
        // 수직
        float v = Input.GetAxisRaw("Vertical");

        // 회전 구간을 나누자!
        float rotZ = transform.localRotation.z;

        print(rotZ);

        if (h == 0 && v == 0) rotDir = 0f;
        else
        {
            // 1사분면
            if (rotZ <= up && rotZ > right)
            {
                if (h > 0 || v < 0)
                    rotDir = 1f;
                else if (h < 0 || v > 0)
                    rotDir = -1f;
            }

            // 2사분면
            else if (rotZ <= right && rotZ > -down)
            {
                if (h < 0 || v < 0)
                    rotDir = 1f;
                else if (h > 0 || v > 0)
                    rotDir = -1f;
            }

            // 3사분면
            else if (rotZ <= -down && rotZ > left)
            {
                if (h < 0 || v > 0)
                    rotDir = 1f;
                else if (h > 0 || v < 0)
                    rotDir = -1f;
            }
            // 4사분면
            else if (rotZ <= left && rotZ > up)
            {
                if (h > 0 || v > 0)
                    rotDir = 1f;
                else if (h < 0 || v < 0)
                    rotDir = -1f;
            }
        }
    }
}

// rotation 값으로는 실패
// 각도로 계산해서 사분면을 나눠보자!

