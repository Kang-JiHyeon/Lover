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

        print(rotZ);

        if (h == 0 && v == 0) rotDir = 0f;
        else
        {
            // 1��и�
            if (rotZ <= up && rotZ > right)
            {
                if (h > 0 || v < 0)
                    rotDir = 1f;
                else if (h < 0 || v > 0)
                    rotDir = -1f;
            }

            // 2��и�
            else if (rotZ <= right && rotZ > -down)
            {
                if (h < 0 || v < 0)
                    rotDir = 1f;
                else if (h > 0 || v > 0)
                    rotDir = -1f;
            }

            // 3��и�
            else if (rotZ <= -down && rotZ > left)
            {
                if (h < 0 || v > 0)
                    rotDir = 1f;
                else if (h > 0 || v < 0)
                    rotDir = -1f;
            }
            // 4��и�
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

// rotation �����δ� ����
// ������ ����ؼ� ��и��� ��������!

