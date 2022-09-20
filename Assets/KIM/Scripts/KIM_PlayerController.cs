using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class KIM_PlayerController : MonoBehaviourPun, IPunObservable
{
    CharacterController cc;
    GameObject ship;

    float yVelocity;
    public float gravity = -9.81f;
    public float jumpPower = 5f;
    public float speed = 5f;
    public bool is2P = false;

    bool isLadder = false;
    bool isModule = false;

    //���� ��ġ
    Vector3 receivePos;
    //ȸ���Ǿ� �ϴ� ��
    Quaternion receiveRot;

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
        if (photonView.IsMine)
        {
            // ��ٸ��� ���� �� ������
            if (isLadder)
            {
                // ���÷� �����̱� ���� ���ּ��� �̵��ϴ� �������� �켱 �̵�
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
                // ���÷� �����̱� ���� ���ּ��� �̵��ϴ� �������� �켱 �̵�
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
            transform.position = Vector3.Lerp(transform.position, receivePos, 5 * Time.deltaTime);
            transform.rotation = Quaternion.Lerp(transform.rotation, receiveRot, 5 * Time.deltaTime);
        }
    }

    public void LocalMove(Vector3 dir)
    {
        cc.Move(dir * Time.deltaTime);

    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        //������ ������
        if (stream.IsWriting) // isMine == true
        {
            //position, rotation
            stream.SendNext(transform.rotation);
            stream.SendNext(transform.position);
        }
        //������ �ޱ�
        else if (stream.IsReading) // ismMine == false
        {
            receiveRot = (Quaternion)stream.ReceiveNext();
            receivePos = (Vector3)stream.ReceiveNext();
        }
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
