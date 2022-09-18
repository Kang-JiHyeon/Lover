using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �Է°� 
public class KANG_InputRotate : MonoBehaviour
{
    KIM_PlayerController pc;

    public Transform target;
    public float rotSpeed = 30f;
    public float rotDir = 0f;
    Vector3 localAngle;

    public bool isControl = false;
    public bool isShieldMove = false;
    public bool is2P = false;

    // Start is called before the first frame update
    void Start()
    {
        localAngle = target.localEulerAngles;
    }


    // Update is called once per frame
    void Update()
    {

        // Ű�Է�
        //float h = Input.GetAxisRaw("Horizontal");
        //float v = Input.GetAxisRaw("Vertical");
        float worldZ = target.eulerAngles.z;

        // 1P
        if (!is2P)
        {
            // Up
            if (Input.GetKey(KeyCode.UpArrow))
            {
                // ���ʿ� ������ �ð�
                if (worldZ > 0f && worldZ < 180f)
                {
                    rotDir = -1;
                }
                // �����ʿ� ������ �ݽð�
                else
                {
                    rotDir = 1;
                }
            }

            // Down
            if (Input.GetKey(KeyCode.DownArrow))
            {
                // ���ʿ� ������ �ݽð�
                if (worldZ >= 0f && worldZ < 180f)
                {
                    rotDir = 1;
                }
                // �����ʿ� ������ �ð�
                else
                {
                    rotDir = -1;
                }
            }

            // Right
            if (Input.GetKey(KeyCode.RightArrow))
            {
                // ���ʿ� ������ �ð�
                if ((worldZ >= 0f && worldZ < 90f) || (worldZ >= 270f && worldZ < 360f))
                {
                    rotDir = -1;
                }
                // �Ʒ��ʿ� ������ �ݽð�
                else
                {
                    rotDir = 1;
                }
            }

            // Left
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                // ���ʿ� ������ �ݽð�
                if ((worldZ >= 0f && worldZ < 90f) || (worldZ >= 270f && worldZ < 360f))
                {
                    rotDir = 1;
                }
                // �Ʒ��ʿ� ������ �ð�
                else
                {
                    rotDir = -1;
                }
            }

            if ((Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow)) && isControl)
            { 
                Rotate();
                isShieldMove = true;
            }
            else
            {
                isShieldMove = false;
            }
        }
        // 2P
        else
        {
            // Up
            if (Input.GetKey(KeyCode.W))
            {
                // ���ʿ� ������ �ð�
                if (worldZ > 0f && worldZ < 180f)
                {
                    rotDir = -1;
                }
                // �����ʿ� ������ �ݽð�
                else
                {
                    rotDir = 1;
                }
            }

            // Down
            if (Input.GetKey(KeyCode.S))
            {
                // ���ʿ� ������ �ݽð�
                if (worldZ >= 0f && worldZ < 180f)
                {
                    rotDir = 1;
                }
                // �����ʿ� ������ �ð�
                else
                {
                    rotDir = -1;
                }
            }

            // Right
            if (Input.GetKey(KeyCode.D))
            {
                // ���ʿ� ������ �ð�
                if ((worldZ >= 0f && worldZ < 90f) || (worldZ >= 270f && worldZ < 360f))
                {
                    rotDir = -1;
                }
                // �Ʒ��ʿ� ������ �ݽð�
                else
                {
                    rotDir = 1;
                }
            }

            // Left
            if (Input.GetKey(KeyCode.A))
            {
                // ���ʿ� ������ �ݽð�
                if ((worldZ >= 0f && worldZ < 90f) || (worldZ >= 270f && worldZ < 360f))
                {
                    rotDir = 1;
                }
                // �Ʒ��ʿ� ������ �ð�
                else
                {
                    rotDir = -1;
                }
            }

            if ((Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D)) && isControl)
            {
                Rotate();
                isShieldMove = true;
            }
            else
            {
                isShieldMove = false;
            }
        }
        #region GetAxis �Է�
        /*
        // ������ ���� �������� �����ϰ� �ʹ�.

        if (v > 0)
        {
            // ���ʿ� ������ �ð�
            if (worldZ > 0f && worldZ < 180f)
            {
                rotDir = -1;
            }
            // �����ʿ� ������ �ݽð�
            else
            {
                rotDir = 1;
            }
        }

        // Down
        if (v < 0)
        {
            // ���ʿ� ������ �ݽð�
            if (worldZ >= 0f && worldZ < 180f)
            {
                rotDir = 1;
            }
            // �����ʿ� ������ �ð�
            else
            {
                rotDir = -1;
            }
        }

        // Right
        if (h > 0)
        {
            // ���ʿ� ������ �ð�
            if ((worldZ >= 0f && worldZ < 90f) || (worldZ >= 270f && worldZ < 360f))
            {
                rotDir = -1;
            }
            // �Ʒ��ʿ� ������ �ݽð�
            else
            {
                rotDir = 1;
            }
        }

        // Left
        if (h < 0)
        {
            // ���ʿ� ������ �ݽð�
            if ((worldZ >= 0f && worldZ < 90f) || (worldZ >= 270f && worldZ < 360f))
            {
                rotDir = 1;
            }
            // �Ʒ��ʿ� ������ �ð�
            else
            {
                rotDir = -1;
            }
        }

        if ((h != 0 || v != 0) && isControl)
            Rotate();
        */

        #endregion

    }

    void Rotate()
    {
        localAngle.z += rotDir * rotSpeed * Time.deltaTime;
        localAngle.z = localAngle.z > 180 ? localAngle.z - 360 : localAngle.z;
        target.localRotation = Quaternion.Euler(0, 0, localAngle.z);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.TryGetComponent<KIM_PlayerController>(out pc) && pc.IsModule)
        {
            isControl = true;
            is2P = pc.is2P;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.TryGetComponent<KIM_PlayerController>(out pc) && !pc.IsModule)
        {
            isControl = false;
            is2P = false;
        }
    }
}