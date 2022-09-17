using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 입력값 
public class KANG_InputRotate : KANG_AutoRotate
{
    float up = 90f;
    float right = 0f;
    float down = -90f;
    float left = -180f;

    public static KANG_InputRotate instance;

    private void Awake()
    {
        if (instance == false)
            instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    public override void Update()
    {
        //Move(spaceship.position, transform.position);

    }

    public void Move(Vector3 vStart, Vector3 vEnd)
    {
        base.Rotate();

        //if (!spaceship) return;

        // 좌우
        float h = Input.GetAxisRaw("Horizontal");
        // 수직

        float v = Input.GetAxisRaw("Vertical");
        // 각도
        float angle = GetAngle(vStart, vEnd);


        if ((h == 0 && v == 0))
        {
            rotDir = 0f;
            return;
        }

        // UP키를 눌렀을 때
        if(v > 0)
        {
            if (Mathf.Abs(angle - up) < 0.1f)
            {
                rotDir = 0;
                return;
            }

            float theta = Mathf.Abs(angle);

            rotDir = theta > 0f && theta <= 90f ? -1 : 1;
        }

        // Right 키를 눌렀을 때
        if (h > 0)
        {
            if (Mathf.Abs(angle) < 0.1f)
            {
                rotDir = 0;
                return;
            }

            rotDir = angle > 0f ? 1 : -1;
        }

        // Down 키를 눌렀을 때
        if (v < 0)
        {
            if (Mathf.Abs(angle - down) < 0.1f)
            {
                rotDir = 0;
                return;
            }

            float theta = Mathf.Abs(angle);

            rotDir = theta > 0f && theta <= 90f ? 1 : -1;
        }

        // Left 키를 눌렀을 때
        if (h < 0)
        {
            if (Mathf.Abs(angle - left) < 0.1f || Mathf.Abs(angle + left) < 0.1f)
            {
                rotDir = 0;
                return;
            }

            rotDir = angle > 0f ? -1 : 1;
        }
    }
    // 각도 구하는 함수
    public float GetAngle(Vector3 vStart, Vector3 vEnd)
    {
        Vector3 v = vEnd - vStart;

        return Mathf.Atan2(v.y, v.x) * Mathf.Rad2Deg;
    }
}