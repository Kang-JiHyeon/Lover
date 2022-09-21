using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 엔진을 회전시키고, 가동하고 싶다.
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
        // 벽에 부딪혔을 때
        if (isBounce)
        {
            LerpMoveSpeed(0f);
            // 일정시간동안 튕기는 방향으로 이동하고 싶다.
            curTime += Time.deltaTime;


            if (curTime > bounceTime)
            {
                curTime = 0f;
                isBounce = false;
            }

            moveDir = bounceDir;
        }
    }

    // 엔진 회전
    public override void ArrowKey()
    {
        base.Rotate();
    }

    // 우주선 이동
    public override void ActionKey()
    {

        // 움직임 방향
        if (moveSpeed > 0 && !isBounce)
        {
            StopCoroutine(IeActionKeyUp(moveSpeed));
            LerpMoveSpeed(moveSpeed);
            // 엔진에서 우주선 중심을 향하는 벡터
            moveDir = ship.forward - rotAxis.up;
            moveDir.z = 0;
            moveDir.Normalize();
        }

        cc.Move(moveDir * curMoveSpeed * Time.deltaTime);
    }

    // 우주선 멈춤
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
