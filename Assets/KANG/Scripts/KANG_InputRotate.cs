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

        float angle = GetAngle(spaceShip.position, transform.position);

        //print(angle);

        if (h == 0 && v == 0) rotDir = 0f;
        #region localRotation을 이용한 방법

        //else
        //{
        //    // 1사분면
        //    if (rotZ <= up && rotZ > right)
        //    {
        //        if (h > 0 || v < 0)
        //            rotDir = 1f;
        //        else if (h < 0 || v > 0)
        //            rotDir = -1f;
        //    }

        //    // 2사분면
        //    else if (rotZ <= right && rotZ > -down)
        //    {
        //        if (h < 0 || v < 0)
        //            rotDir = 1f;
        //        else if (h > 0 || v > 0)
        //            rotDir = -1f;
        //    }

        //    // 3사분면
        //    else if (rotZ <= -down && rotZ > left)
        //    {
        //        if (h < 0 || v > 0)
        //            rotDir = 1f;
        //        else if (h > 0 || v < 0)
        //            rotDir = -1f;
        //    }
        //    // 4사분면
        //    else if (rotZ <= left && rotZ > up)
        //    {
        //        if (h > 0 || v > 0)
        //            rotDir = 1f;
        //        else if (h < 0 || v < 0)
        //            rotDir = -1f;
        //    }
        #endregion


        #region angle 방법 1
        //    // 만약 오른쪽 방향키를 누르f고 있는데 angle이 0도 이하이면 rotDir = 0
        //    if (h > 0 && angle <= 0)
        //        rotDir = 0;


        //    // 1사분면
        //    if (angle > 0f && angle <= 90f)
        //    {
        //        print("1사분면");
        //        if (h > 0 || v < 0)
        //            rotDir = 1f;
        //        else if (h < 0 || v > 0)
        //            rotDir = -1f;
        //    }

        //    // 2사분면
        //    if (angle > 90f && angle <= 180f)
        //    {
        //        print("2사분면");
        //        if (h < 0 || v < 0)
        //            rotDir = 1f;
        //        else if (h > 0 || v > 0)
        //            rotDir = -1f;
        //    }

        //    // 3사분면
        //    if (angle <= -90f && angle > -180f)
        //    {
        //        print("3사분면");
        //        if (h < 0 || v > 0)
        //            rotDir = 1f;
        //        else if (h > 0 || v < 0)
        //            rotDir = -1f;
        //    }

        //    // 4사분면
        //    if (angle <= 0f && angle > -90f)
        //    {
        //        print("4사분면");
        //        if (h > 0 && angle <= 0)
        //            rotDir = 0;
        //        if (v < 0 && angle <= 0)
        //            rotDir = 1f;

        //        if (v > 0)
        //            rotDir = 1f;
        //        else if (h < 0 || v < 0)
        //            rotDir = -1f;
        //    }

        //}
        #endregion

        // UP키를 눌렀을 때
        if(v > 0)
        {
            if (Mathf.Abs(angle - 90f) < 0.5f)
            {
                rotDir = 0;
                return;
            }

            float theta = Mathf.Abs(angle);

            if(theta > 0f && theta <= 90f)
            {
                rotDir = -1;
            }
            else if(theta > 90f && theta <= 180)
            {
                print("2사분면 UP");
                rotDir = 1;
            }
        }

        // Right 키를 눌렀을 때
        if (h > 0)
        {
            if (Mathf.Abs(angle) < 0.1f)
            {
                rotDir = 0;
                return;
            }

            if (angle > 0f)
            {
                rotDir = 1;
            }
            else {
                rotDir = -1;
            }
        }
        // Down 키를 눌렀을 때
        if (v < 0)
        {
            if (Mathf.Abs(angle - (-90f)) < 0.1f)
            {
                rotDir = 0;
                return;
            }

            float theta = Mathf.Abs(angle);

            if (theta > 0f && theta <= 90f)
            {
                rotDir = 1;
            }
            else if (theta > 90f && theta <= 180)
            {
                rotDir = -1;
            }
        }

        // Left 키를 눌렀을 때
        if (h < 0)
        {
            if (Mathf.Abs(angle-180f) < 0.1f || Mathf.Abs(angle + 180f) < 0.1f)
            {
                rotDir = 0;
                return;
            }

            if (angle > 0f)
            {
                rotDir = -1;
            }
            else
            {
                rotDir = 1;
            }
        }



    }
        public static float GetAngle(Vector3 vStart, Vector3 vEnd)
    {
        Vector3 v = vEnd - vStart;

        return Mathf.Atan2(v.y, v.x) * Mathf.Rad2Deg;
    }
}

// rotation 값으로는 실패
// 각도로 계산해서 사분면을 나눠보자!

