using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KIM_PlayerController : MonoBehaviour
{
    CharacterController cc;
    GameObject ship;

    float yVelocity;
    public float gravity = -9.81f;
    public float jumpPower = 5f;
    public float speed = 5f;

    bool isLadder = false;
    bool isModule = false;

    public bool is2P = false;
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
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (!is2P)
        {
            // 사다리에 탔을 때 움직임
            if (isLadder)
            {
                // 로컬로 움직이기 위해 우주선이 이동하는 방향으로 우선 이동
                LocalMove(KANG_ShipMove.instance.moveDir * KANG_ShipMove.instance.curMoveSpeed);
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
            // 모듈에 착석했을 때 움직임
            else if (isModule)
            {
                if (Input.GetKeyDown(KeyCode.B))
                {
                    isModule = false;
                }
            }
            // 그외 상황에서의 움직임
            else
            {
                // 로컬로 움직이기 위해 우주선이 이동하는 방향으로 우선 이동
                LocalMove(KANG_ShipMove.instance.moveDir * KANG_ShipMove.instance.curMoveSpeed);
                Debug.DrawRay(transform.position, Vector3.down * 0.3f, Color.red);
                if (!Physics.Raycast(transform.position, Vector3.down, 0.37f, LayerMask.GetMask("ShipFloor")))
                {
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
        }
        else
        {
            // 사다리에 탔을 때 움직임
            if (isLadder)
            {
                // 로컬로 움직이기 위해 우주선이 이동하는 방향으로 우선 이동
                LocalMove(KANG_ShipMove.instance.moveDir * KANG_ShipMove.instance.curMoveSpeed);
                yVelocity = 0f;

                if (Input.GetKey(KeyCode.A))
                {
                    cc.Move(-Vector3.right * speed * Time.deltaTime);
                }
                else if (Input.GetKey(KeyCode.D))
                {
                    cc.Move(Vector3.right * speed * Time.deltaTime);
                }
                else if (Input.GetKey(KeyCode.W))
                {
                    cc.Move(Vector3.up * speed * Time.deltaTime);
                }
                else if (Input.GetKey(KeyCode.S))
                {
                    cc.Move(Vector3.down * speed * Time.deltaTime);
                }
            }
            // 모듈에 착석했을 때 움직임
            else if (isModule)
            {
                if (Input.GetKeyDown(KeyCode.Z))
                {
                    isModule = false;
                }
            }
            // 그외 상황에서의 움직임
            else
            {
                // 로컬로 움직이기 위해 우주선이 이동하는 방향으로 우선 이동
                LocalMove(KANG_ShipMove.instance.moveDir * KANG_ShipMove.instance.curMoveSpeed);
                Debug.DrawRay(transform.position, Vector3.down * 0.3f, Color.red);
                if (!Physics.Raycast(transform.position, Vector3.down, 0.37f, LayerMask.GetMask("ShipFloor")))
                {
                    yVelocity += gravity * Time.deltaTime;
                }
                else
                {
                    yVelocity = 0f;
                    if (Input.GetKeyDown(KeyCode.X))
                        yVelocity = jumpPower;
                }

                if (Input.GetKey(KeyCode.A))
                    cc.Move(-Vector3.right * speed * Time.deltaTime);
                else if (Input.GetKey(KeyCode.D))
                    cc.Move(Vector3.right * speed * Time.deltaTime);

                cc.Move(yVelocity * Vector3.up * Time.deltaTime);
            }

            if (!isModule && canModule && (Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.A) ||
                Input.GetKeyUp(KeyCode.S) || Input.GetKeyUp(KeyCode.D) || Input.GetKeyDown(KeyCode.C)))
            {
                isModule = true;
            }
        }
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

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Module") && isModule)
        {
            transform.position = Vector3.Lerp(transform.position, other.transform.position, Time.deltaTime * 10);
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
