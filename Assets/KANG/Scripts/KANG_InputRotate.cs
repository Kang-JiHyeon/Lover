using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �Է°� 
public class KANG_InputRotate : KANG_AutoRotate
{
    // ����Ű���� ������ ���������� ȸ���ϰ� �ʹ�.
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
        // �¿�
        float h = Input.GetAxisRaw("Horizontal");
        // ����
        float v = Input.GetAxisRaw("Vertical");

        // ȸ�� ������ ������!
        float rotZ = transform.localRotation.z;

        float angle = GetAngle(spaceShip.position, transform.position);

        //print(angle);

        if (h == 0 && v == 0) rotDir = 0f;
        #region localRotation�� �̿��� ���

        //else
        //{
        //    // 1��и�
        //    if (rotZ <= up && rotZ > right)
        //    {
        //        if (h > 0 || v < 0)
        //            rotDir = 1f;
        //        else if (h < 0 || v > 0)
        //            rotDir = -1f;
        //    }

        //    // 2��и�
        //    else if (rotZ <= right && rotZ > -down)
        //    {
        //        if (h < 0 || v < 0)
        //            rotDir = 1f;
        //        else if (h > 0 || v > 0)
        //            rotDir = -1f;
        //    }

        //    // 3��и�
        //    else if (rotZ <= -down && rotZ > left)
        //    {
        //        if (h < 0 || v > 0)
        //            rotDir = 1f;
        //        else if (h > 0 || v < 0)
        //            rotDir = -1f;
        //    }
        //    // 4��и�
        //    else if (rotZ <= left && rotZ > up)
        //    {
        //        if (h > 0 || v > 0)
        //            rotDir = 1f;
        //        else if (h < 0 || v < 0)
        //            rotDir = -1f;
        //    }
        #endregion


        #region angle ��� 1
        //    // ���� ������ ����Ű�� ����f�� �ִµ� angle�� 0�� �����̸� rotDir = 0
        //    if (h > 0 && angle <= 0)
        //        rotDir = 0;


        //    // 1��и�
        //    if (angle > 0f && angle <= 90f)
        //    {
        //        print("1��и�");
        //        if (h > 0 || v < 0)
        //            rotDir = 1f;
        //        else if (h < 0 || v > 0)
        //            rotDir = -1f;
        //    }

        //    // 2��и�
        //    if (angle > 90f && angle <= 180f)
        //    {
        //        print("2��и�");
        //        if (h < 0 || v < 0)
        //            rotDir = 1f;
        //        else if (h > 0 || v > 0)
        //            rotDir = -1f;
        //    }

        //    // 3��и�
        //    if (angle <= -90f && angle > -180f)
        //    {
        //        print("3��и�");
        //        if (h < 0 || v > 0)
        //            rotDir = 1f;
        //        else if (h > 0 || v < 0)
        //            rotDir = -1f;
        //    }

        //    // 4��и�
        //    if (angle <= 0f && angle > -90f)
        //    {
        //        print("4��и�");
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

        // UPŰ�� ������ ��
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
                print("2��и� UP");
                rotDir = 1;
            }
        }

        // Right Ű�� ������ ��
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
        // Down Ű�� ������ ��
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

        // Left Ű�� ������ ��
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

// rotation �����δ� ����
// ������ ����ؼ� ��и��� ��������!

