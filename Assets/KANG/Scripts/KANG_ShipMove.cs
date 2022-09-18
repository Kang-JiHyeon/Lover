using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ���� ������ �ݴ�������� �̵��ϰ� �ʹ�.
// Charater Controller �� �̿��� �����̰� �ʹ�.

// ��, ��� ���� ���
// �ε��� ������ �ݴ�������� ���� ���ϰ� �ʹ�.

public class KANG_ShipMove : MonoBehaviour
{
    CharacterController cc;

    // ����
    public KANG_InputRotate engine;
    Transform machine;
    // �̵� ����
    public Vector3 moveDir;
    // �̵��ӵ�
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
        // ������ ���� ��� ��ȯ
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

        // ������ �ӵ� 
        if (isMove)
        {
            LerpMoveSpeed(moveSpeed);
        }
        // ���� �ӵ�
        else
        {
            LerpMoveSpeed(0f);
        }

        // ������ ����
        if (isMove && moveSpeed > 0 && !isBounce)
        {
            // �������� ���ּ� �߽��� ���ϴ� ����
            moveDir = transform.forward - machine.up;
            moveDir.z = 0;
            moveDir.Normalize();
        }

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
