using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �Է°� 
public class KANG_InputRotate : MonoBehaviour
{
    public Transform target;
    public float rotSpeed = 30f;
    public float rotDir = 0f;
    Vector3 localAngle;

    public bool isControl = false;

    // Start is called before the first frame update
    void Start()
    {
        localAngle = target.localEulerAngles;
    }


    // Update is called once per frame
    void Update()
    {
        // Ű�Է�
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");


        Vector3 dir = Vector3.right * h + Vector3.up * v;
        dir.Normalize();


        //print(target.eulerAngles.z);

        float worldZ = target.eulerAngles.z;
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

    }

    void Rotate()
    {
        localAngle.z += rotDir * rotSpeed * Time.deltaTime;
        localAngle.z = localAngle.z > 180 ? localAngle.z - 360 : localAngle.z;
        //localAngle.z = Mathf.Clamp(localAngle.z, -90, 90);
        target.localRotation = Quaternion.Euler(0, 0, localAngle.z);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            isControl = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            isControl = false;
        }
    }

}