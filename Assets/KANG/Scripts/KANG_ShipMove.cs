using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 엔진 방향의 반대방향으로 이동하고 싶다.
// Charater Controller 를 이용해 움직이고 싶다.

// 맵, 운석에 닿일 경우
// 부딪힌 방향의 반대방향으로 힘을 가하고 싶다.

public class KANG_ShipMove : MonoBehaviour
{
    CharacterController cc;

    // 엔진
    public KANG_InputRotate engine;
    Transform machine;
    // 이동 방향
    public Vector3 moveDir;
    // 이동속도
    public float moveSpeed = 3f;
    public float curMoveSpeed = 0f;
    public bool isMove = false;

    public Vector3 bounceDir;
    public bool isBounce = false;

    public float bounceTime = 0.2f;
    float curTime = 0f;

    public static KANG_ShipMove instance;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        cc = GetComponent<CharacterController>();
        machine = engine.transform.GetChild(0);
    }

    // Update is called once per frame
    void Update()
    {
        // 엔진이 없을 경우 반환
        //if (engine.isControl == false)
            //return;

        if (engine.isControl)
        {
            //1P
            if(engine.GetComponent<KANG_InputRotate>().is2P == false)
            {
                if (Input.GetKeyDown(KeyCode.M))
                    isMove = true;

                if (Input.GetKeyUp(KeyCode.M))
                    isMove = false;
            }
            // 2P
            else
            {
                if (Input.GetKeyDown(KeyCode.X))
                    isMove = true;

                if (Input.GetKeyUp(KeyCode.X))
                    isMove = false;
            }
        }
        else
        {
            isMove = false;
        }

        // 움직임 속도 
        if (isMove)
        {
            LerpMoveSpeed(moveSpeed);
        }
        // 멈춤 속도
        else
        {
            LerpMoveSpeed(0f);
        }

        // 움직임 방향
        if (isMove && moveSpeed > 0 && !isBounce)
        {
            // 엔진에서 우주선 중심을 향하는 벡터
            moveDir = transform.forward - machine.up;
            moveDir.z = 0;
            moveDir.Normalize();
        }

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

        cc.Move(moveDir * curMoveSpeed * Time.deltaTime);

    }

    void LerpMoveSpeed(float targetSpeed, float changeSpeed = 1f)
    {
        curMoveSpeed = Mathf.Lerp(curMoveSpeed, targetSpeed, Time.deltaTime * changeSpeed);
        if (Mathf.Abs(targetSpeed - curMoveSpeed) < 0.1f)
        {
            curMoveSpeed = targetSpeed;
        }
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.CompareTag("Map"))
        {
            bounceDir = transform.position - other.transform.position;
            bounceDir.z = 0f;
            bounceDir.Normalize();
            isBounce = true;

            KANG_ShipHP.instance.HP--;

            print("Map Bounce, HP--");
        }


        // Enemy
        if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            KANG_ShipHP.instance.HP--;
            print("Enemy, HP--");
        }

        // EnemyBullet
        if (other.gameObject.layer == LayerMask.NameToLayer("EnemyBullet"))
        {
            print("trigger Object : " + other.gameObject.name);
            KANG_ShipHP.instance.HP--;
        }
    }
}
