using System.Collections;
using System.Collections.Generic;
using UnityEngine; 


// 포탑의 총구를 변경시키고 싶다.
// 회전 방법
// 1. Rotate() (v)
// 2. Euler
// 3. localeulerAngles


public class KANG_TurretRotate2 : MonoBehaviour
{
    public float rotSpeed = 30f;
    public float rotDir = 0f;
    public bool isControl = false;
    Vector3 localAngle;

    // Start is called before the first frame update
    void Start()
    {
        localAngle = transform.localEulerAngles;
    }


    // Update is called once per frame
    void Update()
    {
        // 키입력
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");


        Vector3 dir = Vector3.right * h + Vector3.up * v;
        dir.Normalize();


        //print(transform.eulerAngles.z);

        float worldZ = transform.eulerAngles.z;
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
            if ((worldZ >= 0f && worldZ < 90f) || (worldZ >= 270f &&  worldZ < 360f))
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

        if((h != 0 || v != 0) && isControl)
            TurretRotate();

    }
    
    void TurretRotate()
    {
        localAngle.z += rotDir * rotSpeed * Time.deltaTime;
        localAngle.z = localAngle.z > 180 ? localAngle.z - 360 : localAngle.z;
        localAngle.z = Mathf.Clamp(localAngle.z, -90, 90);
        transform.localRotation = Quaternion.Euler(0, 0, localAngle.z);
    }
}
