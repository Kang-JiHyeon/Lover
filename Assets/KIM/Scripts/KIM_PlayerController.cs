using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KIM_PlayerController : MonoBehaviour
{
    CharacterController cc;
    GameObject ship;
    KANG_Move mv;

    float yVelocity;
    public float gravity = -9.81f;
    public float jumpPower = 5f;
    public float speed = 5f;

    bool isLadder = false;
    bool isModule = false;
    public bool IsModule
    {
        get { return isModule; }
    }
    bool canModule = false;
    // Start is called before the first frame update
    void Start()
    {
        ship = GameObject.Find("Spaceship");
        cc = GetComponent<CharacterController>();   
        mv = ship.GetComponent<KANG_Move>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        // ���÷� �����̱� ���� ���ּ��� �̵��ϴ� �������� �켱 �̵�
        LocalMove(KANG_ShipMove.instance.moveDir * KANG_ShipMove.instance.curMoveSpeed);
        // ��ٸ��� ���� �� ������
        if (isLadder)
        {
            yVelocity = 0f;

            if (Input.GetKey(KeyCode.LeftArrow))
            {
                cc.Move(-Vector3.right * speed * Time.deltaTime);
            }
            else if (Input.GetKey(KeyCode.RightArrow))
            {
                cc.Move(Vector3.right * speed * Time.deltaTime);
            }
            else if (Input.GetKey(KeyCode.UpArrow))
            {
                cc.Move(Vector3.up * speed * Time.deltaTime);
            }
            else if (Input.GetKey(KeyCode.DownArrow))
            {
                cc.Move(Vector3.down * speed * Time.deltaTime);
            }
        }
        // ��⿡ �������� �� ������
        else if (isModule)
        {
            if (Input.GetKeyDown(KeyCode.B))
            {
                isModule = false;
            }
        }
        // �׿� ��Ȳ������ ������
        else
        {
            RaycastHit hit;
            Debug.DrawRay(transform.position, Vector3.down * 0.4f, Color.red);
            if (!Physics.Raycast(transform.position, Vector3.down, out hit, 0.4f, LayerMask.GetMask("Ship")))
            {
                Debug.Log(hit);
                yVelocity += gravity * Time.deltaTime;
            }
            else
            {
                yVelocity = 0f;
                if (Input.GetKeyDown(KeyCode.Space))
                    yVelocity = jumpPower;
            }

            if (Input.GetKey(KeyCode.LeftArrow))
                cc.Move(-Vector3.right * speed * Time.deltaTime);
            else if (Input.GetKey(KeyCode.RightArrow))
                cc.Move(Vector3.right * speed * Time.deltaTime);

            cc.Move(yVelocity * Vector3.up * Time.deltaTime);
        }

        if (!isModule && canModule && (Input.GetKeyUp(KeyCode.UpArrow) || Input.GetKeyUp(KeyCode.LeftArrow) ||
            Input.GetKeyUp(KeyCode.DownArrow) || Input.GetKeyUp(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.M)))
        {
            isModule = true;
        }

        //Debug.Log(isModule);
    }

    public void LocalMove(Vector3 dir)
    {
        cc.Move(dir * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Ladder"))
        {
            isLadder = true;
        }

        if (other.gameObject.layer == LayerMask.NameToLayer("Module"))
        {
            canModule = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Ladder"))
        {
            isLadder = false;
        }

        if (other.gameObject.layer == LayerMask.NameToLayer("Module"))
        {
            canModule = false;
        }
    }
}
