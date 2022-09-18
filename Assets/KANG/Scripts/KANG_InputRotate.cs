using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 입력값 
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
        // 키입력
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");


        Vector3 dir = Vector3.right * h + Vector3.up * v;
        dir.Normalize();


        //print(target.eulerAngles.z);

        float worldZ = target.eulerAngles.z;
        // 각도에 따라 움직임을 제한하고 싶다.

        if (v > 0)
        {
            // 왼쪽에 있으면 시계
            if (worldZ > 0f && worldZ < 180f)
            {
                rotDir = -1;
            }
            // 오른쪽에 있으면 반시계
            else
            {
                rotDir = 1;
            }
        }

        // Down
        if (v < 0)
        {
            // 왼쪽에 있으면 반시계
            if (worldZ >= 0f && worldZ < 180f)
            {
                rotDir = 1;
            }
            // 오른쪽에 있으면 시계
            else
            {
                rotDir = -1;
            }
        }

        // Right
        if (h > 0)
        {
            // 위쪽에 있으면 시계
            if ((worldZ >= 0f && worldZ < 90f) || (worldZ >= 270f && worldZ < 360f))
            {
                rotDir = -1;
            }
            // 아래쪽에 있으면 반시계
            else
            {
                rotDir = 1;
            }
        }

        // Left
        if (h < 0)
        {
            // 위쪽에 있으면 반시계
            if ((worldZ >= 0f && worldZ < 90f) || (worldZ >= 270f && worldZ < 360f))
            {
                rotDir = 1;
            }
            // 아래쪽에 있으면 시계
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