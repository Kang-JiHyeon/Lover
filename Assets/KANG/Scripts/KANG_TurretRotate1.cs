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

        // �¿�
         h = Input.GetAxisRaw("Horizontal");
        // ����
        v = Input.GetAxisRaw("Vertical");



        //// ������ ����Ű�� ������ ��
        //// - �ð� ���� ȸ��
        //if (h > 0)
        //{
        //    if (angle < rightLimitAngle) isMove = false;
        //    rotDir = 1;
        //}

        //// ���� ����Ű�� ������ ��
        //// - �ݽð� ���� ȸ��
        //if (h < 0)
        //{
        //    if (angle > leftLimitAngle) isMove = false;
        //    rotDir = -1;
        //}

        //// ���� ����Ű�� ������ ��
        //// - ���� ������ ���� ���� �ݽð�
        //// - ������ ������ ���� ���� �ð�
        //if(v > 0)
        //{
        //    if (angle > 0) rotDir = 1;
        //    else rotDir = -1;

        //    if (Mathf.Abs(angle) < 0.1f) isMove = false;
        //}

        //// �Ʒ� ����Ű�� ������ ��
        //// - ���� ������ ���� �� �ð�
        //// - ������ ������ ���� �� �ݽð�
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

        //// �Է��� ���� ���� ȸ��
        //if (h != 0 || v != 0)
        //    isMove = true;
        //else
        //    isMove = false;

        if ((angle > minLimitAngle && angle <= middleLimitAngle) || (angle >= middleLimitAngle && angle < maxLimitAngle))
        {
            // �Է��� ���� ���� ȸ��
            if (h != 0 || v != 0)
                isMove = true;
            else
                isMove = false;
            //isMove = true;
        }
        //else
        //    isMove = false;

        // UPŰ�� ������ ��
        if (v > 0)
        {
            // ���� ����

            if (Mathf.Abs(angle - upLimitAngle) < 0.1f)
            {
                //rotDir = 0;
                //return;
                isMove = false;
            }

            float theta = Mathf.Abs(angle);

            rotDir = theta > 0f && theta <= 90f ? -1 : 1;
        }

        // Right Ű�� ������ ��
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

        // Down Ű�� ������ ��
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

        // Left Ű�� ������ ��
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
