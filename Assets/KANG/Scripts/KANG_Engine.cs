using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ������ ȸ����Ű��, �����ϰ� �ʹ�.
public class KANG_Engine : KANG_Machine
{
    CharacterController cc;
    Transform ship;
    public Vector3 moveDir;
    public float moveSpeed = 2f;
    public float curMoveSpeed = 0f;
    public bool isBounce;
    public float bounceTime = 0.2f;
    float curTime = 0f;
    public Vector3 bounceDir;


    // Start is called before the first frame update
    void Start()
    {
        ship = transform.parent;
        cc = ship.GetComponent<CharacterController>();
        localAngle = rotAxis.localEulerAngles;
    }

    // Update is called once per frame
    void Update()
    {
        // ���� �ε����� ��
        if (isBounce)
        {
            LerpMoveSpeed(0f);
            // �����ð����� ƨ��� �������� �̵��ϰ� �ʹ�.
            curTime += Time.deltaTime;


            if (curTime > bounceTime)
            {
                curTime = 0f;
                isBounce = false;
            }

            moveDir = bounceDir;
        }
    }

    // ���� ȸ��
    public override void ArrowKey()
    {
        base.Rotate();
    }

    // ���ּ� �̵�
    public override void ActionKey()
    {

        // ������ ����
        if (moveSpeed > 0 && !isBounce)
        {
            StopCoroutine(IeActionKeyUp(moveSpeed));
            LerpMoveSpeed(moveSpeed);
            // �������� ���ּ� �߽��� ���ϴ� ����
            moveDir = ship.forward - rotAxis.up;
            moveDir.z = 0;
            moveDir.Normalize();
        }

        cc.Move(moveDir * curMoveSpeed * Time.deltaTime);
    }

    // ���ּ� ����
    public override void ActionKeyUp()
    {
        StartCoroutine(IeActionKeyUp(0f));
    }

    IEnumerator IeActionKeyUp(float targetSpeed, float changeSpeed = 1f)
    {
        
        while(Mathf.Abs(targetSpeed - curMoveSpeed) > 0.1f)
        {
            curMoveSpeed = Mathf.Lerp(curMoveSpeed, targetSpeed, Time.deltaTime * changeSpeed);
            cc.Move(moveDir * curMoveSpeed * Time.deltaTime);
            yield return null;
        }
        curMoveSpeed = targetSpeed;

    }

    void LerpMoveSpeed(float targetSpeed, float changeSpeed = 1f)
    {
        curMoveSpeed = Mathf.Lerp(curMoveSpeed, targetSpeed, Time.deltaTime * changeSpeed);
        if (Mathf.Abs(targetSpeed - curMoveSpeed) < 0.1f)
        {
            curMoveSpeed = targetSpeed;
        }
    }
}
