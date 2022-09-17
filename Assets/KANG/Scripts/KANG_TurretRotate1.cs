using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KANG_TurretRotate1 : MonoBehaviour
{
    public float rotSpeed = 30f;
    public float rotDir = 0f;
    public bool isControl = false;
    bool isMove = false;
    float originAngle = 0f;

    public float upLimitAngle;
    public float downLimitAngle;
    public float leftLimitAngle;
    public float rightLimitAngle;

    float up = 90f;
    float right = 0f;
    float down = -90f;
    float left = -180f;

    public float minLimitAngle;
    public float maxLimitAngle;
    public float middleLimitAngle;

    // Start is called before the first frame update
    void Start()
    {
        //originAngle = KANG_InputRotate.instance.GetAngle(transform.parent.position, transform.GetChild(0).position);

        ////if (originAngle < 0) originAngle *= -1;

        //print(transform.parent.gameObject.name + " : " + originAngle);
    }

    private void FixedUpdate()
    {
        
    }
    float angle, h, v;

    // Update is called once per frame
    void Update()
    {
        if (isControl == false) return;

        //angle = KANG_InputRotate.instance.GetAngle(transform.parent.position, transform.GetChild(0).position);
        //print(angle);

        // 좌우
         h = Input.GetAxisRaw("Horizontal");
        // 수직
        v = Input.GetAxisRaw("Vertical");



        //// 오른쪽 방향키를 눌렀을 때
        //// - 시계 방향 회전
        //if (h > 0)
        //{
        //    if (angle < rightLimitAngle) isMove = false;
        //    rotDir = 1;
        //}

        //// 왼쪽 방향키를 눌렀을 때
        //// - 반시계 방향 회전
        //if (h < 0)
        //{
        //    if (angle > leftLimitAngle) isMove = false;
        //    rotDir = -1;
        //}

        //// 윗쪽 방향키를 눌렀을 때
        //// - 왼쪽 범위에 있을 때는 반시계
        //// - 오른쪽 범위에 있을 때는 시계
        //if(v > 0)
        //{
        //    if (angle > 0) rotDir = 1;
        //    else rotDir = -1;

        //    if (Mathf.Abs(angle) < 0.1f) isMove = false;
        //}

        //// 아래 방향키를 눌렀을 때
        //// - 왼쪽 범위에 있을 때 시계
        //// - 오른쪽 범위에 있을 때 반시계
        //if(v < 0)
        //{
        //    if (angle < 0) rotDir = 1;
        //    else rotDir = -1;


        //    if (Mathf.Abs(angle) > 90) isMove = false;
        //}

        //if (isMove)
        //    transform.RotateAround(transform.parent.position, -transform.forward, rotDir * rotSpeed * Time.deltaTime);

        //if (h == 0 && v == 0)
        //{
        //    rotDir = 0f;
        //    return;
        //}

        //// 입력이 있을 때만 회전
        //if (h != 0 || v != 0)
        //    isMove = true;
        //else
        //    isMove = false;

        if ((angle > minLimitAngle && angle <= middleLimitAngle) || (angle >= middleLimitAngle && angle < maxLimitAngle))
        {
            // 입력이 있을 때만 회전
            if (h != 0 || v != 0)
                isMove = true;
            else
                isMove = false;
            //isMove = true;
        }
        //else
        //    isMove = false;

        // UP키를 눌렀을 때
        if (v > 0)
        {
            // 제한 각도

            if (Mathf.Abs(angle - upLimitAngle) < 0.1f)
            {
                //rotDir = 0;
                //return;
                isMove = false;
            }

            float theta = Mathf.Abs(angle);

            rotDir = theta > 0f && theta <= 90f ? -1 : 1;
        }

        // Right 키를 눌렀을 때
        if (h > 0)
        {
            if (Mathf.Abs(angle-rightLimitAngle) < 0.1f)
            {
                //rotDir = 0;
                //return;
                isMove = false;
            }

            rotDir = angle > 0f ? 1 : -1;
        }

        // Down 키를 눌렀을 때
        if (v < 0)
        {
            if (Mathf.Abs(angle - downLimitAngle) < 0.1f)
            {
                //rotDir = 0;
                //return;
                isMove = false;
            }

            float theta = Mathf.Abs(angle);

            rotDir = theta > 0f && theta <= 90f ? 1 : -1;
        }

        // Left 키를 눌렀을 때
        if (h < 0)
        {
            if (Mathf.Abs(angle - leftLimitAngle) < 0.1f || Mathf.Abs(angle + leftLimitAngle) < 0.1f)
            {
                //rotDir = 0;
                //return;
                isMove = false;
            }

            rotDir = angle > 0f ? -1 : 1;
        }

        if (isMove)
            transform.RotateAround(transform.parent.position, -transform.forward, rotDir * rotSpeed * Time.deltaTime);

    }
}
