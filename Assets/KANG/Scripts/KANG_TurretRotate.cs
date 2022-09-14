using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KANG_TurretRotate : MonoBehaviour
{
    public float rotSpeed = 30f;
    public float rotDir = 0f;
    public bool isControl = false;
    bool isMove = false;
    float originAngle = 0f;

    // Start is called before the first frame update
    void Start()
    {
        originAngle = KANG_InputRotate.instance.GetAngle(transform.parent.position, transform.GetChild(0).position);

        //if (originAngle < 0) originAngle *= -1;

        print(transform.parent.gameObject.name + " : " + originAngle);
    }

    // Update is called once per frame
    void Update()
    {
        if (isControl == false) return;

        float angle = KANG_InputRotate.instance.GetAngle(transform.parent.position, transform.GetChild(0).position) - originAngle;
        print(angle);
        // �¿�
        float h = Input.GetAxisRaw("Horizontal");
        // ����
        float v = Input.GetAxisRaw("Vertical");

        // �Է��� ���� ���� ȸ��
        if (h != 0 || v != 0)
            isMove = true;
        else
            isMove = false;

        // ������ ����Ű�� ������ ��
        if (h > 0)
        {
            if (angle < -90) isMove = false;
            rotDir = 1;
        }
        
        // ���� ����Ű�� ������ ��
        if(h < 0)
        {
            if (angle > 90) isMove = false;
            rotDir = -1;
        }
        
        // ���� ����Ű�� ������ ��
        if(v > 0)
        {
            if (angle > 0) rotDir = 1;
            else rotDir = -1;

            if (Mathf.Abs(angle) < 0.1f) isMove = false;
        }

        // �Ʒ� ����Ű�� ������ ��
        if(v < 0)
        {
            if (angle < 0) rotDir = 1;
            else rotDir = -1;


            if (Mathf.Abs(angle) > 90) isMove = false;
        }
        
        if (isMove)
            transform.RotateAround(transform.parent.position, -transform.forward, rotDir * rotSpeed * Time.deltaTime);
    }
}
