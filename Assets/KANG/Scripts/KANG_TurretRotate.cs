using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KANG_TurretRotate : MonoBehaviour
{
    Vector3 pos;
    public float rotSpeed = 30f;
    public float rotDir = 0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // 좌우
        float h = Input.GetAxisRaw("Horizontal");
        // 수직
        float v = Input.GetAxisRaw("Vertical");


        float angle = KANG_InputRotate.instance.GetAngle(transform.parent.position, transform.GetChild(0).position);

        print(angle);
         
        if(angle < 0 || (h == 0 && v == 0))
        {
            angle = 0f;
            return;
        }

        // Right 키를 눌렀을 때
        if (h > 0) rotDir = 1;
        else if(h < 0) rotDir = -1;

        // UP키를 눌렀을 때
        if (v > 0)
        {
            if (Mathf.Abs(angle - 90f) < 0.1f)
            {
                rotDir = 0;
                return;
            }

            float theta = Mathf.Abs(angle);
            rotDir = theta > 0f && theta <= 90f ? -1 : 1;
        }


        transform.RotateAround(transform.parent.position, -transform.forward, rotDir * rotSpeed * Time.deltaTime);


    }
}
