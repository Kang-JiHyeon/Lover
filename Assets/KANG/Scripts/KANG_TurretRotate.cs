using System.Collections;
using System.Collections.Generic;
using UnityEngine; 


// 포탑의 총구를 입력받은 방향까지 이동시키고 싶다.


public class KANG_TurretRotate : MonoBehaviour
{
    public float rotSpeed = 30f;
    public float rotDir = 0f;

    Vector3 localAngle;

    
    Vector3 ship;

    Vector3 inputDir;

    // Start is called before the first frame update
    void Start()
    {
        localAngle = transform.localEulerAngles;
        ship = GameObject.Find("Spaceship").transform.position;
    }


    // Update is called once per frame
    void Update()
    {
        // 키입력
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");


        inputDir = Vector3.right * h + Vector3.up * v;
        inputDir.Normalize();


        //print(transform.eulerAngles.z);

        float worldZ = transform.eulerAngles.z;
        // 각도에 따라 움직임을 제한하고 싶다.

        Vector3 dir1 = transform.position - ship;

        float angle = GetAngle(transform.up, inputDir);
        //print(GetAngle(transform.up, inputDir));

        ////Up
        //if (worldZ >= 0f && worldZ < 180f)
        //{
        //    rotDir = v > 0 ? -1 : 1;
        //}
        //else
        //{
        //    rotDir = v < 0 ? -1 : 1;
        //}



        //// Right
        //if (h > 0)
        //{
        //    // 위쪽에 있으면 시계
        //    if ((worldZ >= 0f && worldZ < 90f) || (worldZ >= 270f &&  worldZ < 360f))
        //    {
        //        rotDir = -1;
        //    }
        //    // 아래쪽에 있으면 반시계
        //    else
        //    {
        //        rotDir = 1;
        //    }
        //}

        //// Left
        //if (h < 0)
        //{
        //    // 위쪽에 있으면 반시계
        //    if ((worldZ >= 0f && worldZ < 90f) || (worldZ >= 270f && worldZ < 360f))
        //    {
        //        rotDir = 1;
        //    }
        //    // 아래쪽에 있으면 시계
        //    else
        //    {
        //        rotDir = -1;
        //    }
        //}
        
        if ((h != 0 || v != 0))
        {
            //localAngle.z += angle * Time.deltaTime;
            //transform.localRotation = Quaternion.Euler(0, 0, 10 * Time.deltaTime);
            //print(transform.localRotation.z);
            transform.up = inputDir;

            //TurretRotate();
        }

    }

    void TurretRotate()
    {
        localAngle.z += rotDir * rotSpeed * Time.deltaTime;
        localAngle.z = localAngle.z > 180 ? localAngle.z - 360 : localAngle.z;
        localAngle.z = Mathf.Clamp(localAngle.z, -90, 90);
        transform.localRotation = Quaternion.Euler(0, 0, localAngle.z);
    }

    void TurretRotate1()
    {

    }

    public static float GetAngle(Vector3 from, Vector3 to)
    {
        Vector3 v = to - from;
        return Mathf.Atan2(v.y, v.x) * Mathf.Rad2Deg;
    }
}
