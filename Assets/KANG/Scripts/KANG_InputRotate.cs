using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �Է°� 
public class KANG_InputRotate : KANG_AutoRotate
{
    float up = 90f;
    float right = 0f;
    float down = -90f;
    float left = -180f;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    public override void Update()
    {
        base.Rotate();
        Move();
    }

    void Move()
    {
        // �¿�
        float h = Input.GetAxisRaw("Horizontal");
        // ����
        float v = Input.GetAxisRaw("Vertical");
        // ����
        float angle = GetAngle(spaceShip.position, transform.position);


        if (h == 0 && v == 0) rotDir = 0f;

        // UPŰ�� ������ ��
        if(v > 0)
        {
            if (Mathf.Abs(angle - up) < 0.1f)
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
            if (Mathf.Abs(angle - down) < 0.1f)
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
            if (Mathf.Abs(angle - left) < 0.1f || Mathf.Abs(angle + left) < 0.1f)
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

