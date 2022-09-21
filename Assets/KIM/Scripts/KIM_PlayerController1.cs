using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class KIM_PlayerController1 : MonoBehaviourPun, IPunObservable
{
    GameObject target;
    KANG_Machine machine;

    Rigidbody rb;
    bool isLadder;
    bool isModule;

    //도착 위치
    Vector3 receivePos;
    //회전되야 하는 값
    Quaternion receiveRot;

    [SerializeField]
    float speed = 5;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>(); 
    }

    // Update is called once per frame
    void Update()
    {
        if (photonView.IsMine)
        {
            // 사다리 탔을 때는 중력 영향 안받음
            if (isLadder || isModule)
                photonView.RPC("RPCGravity", RpcTarget.All, false);
            else
                photonView.RPC("RPCGravity", RpcTarget.All, true);

            if (Input.GetKey(KeyCode.M))
            {
                if (isModule)
                {
                    machine = target.GetComponent<KANG_Machine>();
                    machine.ActionKey();
                }
            }

            if (Input.GetKey(KeyCode.UpArrow))
            {
                if (isModule)
                {
                    machine = target.GetComponent<KANG_Machine>();
                    machine.UpKey();
                }
                else if (isLadder)
                {
                    transform.localPosition += Vector3.up * Time.deltaTime * speed;
                }
            }
            if (Input.GetKey(KeyCode.DownArrow))
            {
                if (isModule)
                {
                    machine = target.GetComponent<KANG_Machine>();
                    machine.DownKey();
                }
                else if (isLadder)
                {
                    transform.localPosition += Vector3.down * Time.deltaTime * speed;
                }
            }
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                if (isModule)
                {
                    machine = target.GetComponent<KANG_Machine>();
                    machine.LeftKey();
                }
                else if (isLadder)
                {
                    transform.localPosition += Vector3.left * Time.deltaTime * speed;
                }
                else
                {
                    transform.localPosition += Vector3.left * Time.deltaTime * speed;
                }
            }
            if (Input.GetKey(KeyCode.RightArrow))
            {
                if (isModule)
                {
                    machine = target.GetComponent<KANG_Machine>();
                    machine.RightKey();
                }
                else if (isLadder)
                {
                    transform.localPosition += Vector3.right * Time.deltaTime * speed;
                }
                else
                {
                    transform.localPosition += Vector3.right * Time.deltaTime * speed;
                }
            }

            if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow))
            {
                if (isModule)
                {
                    machine = target.GetComponent<KANG_Machine>();
                    machine.ArrowKey();
                }
            }

            Debug.DrawRay(transform.position, Vector3.down * 0.37f, Color.red);
            if (Physics.Raycast(transform.position, Vector3.down, 0.37f, LayerMask.GetMask("ShipFloor")))
            {
                if (Input.GetKeyDown(KeyCode.Space))
                    rb.AddForce(Vector3.up * 150f);
            }

            // 모듈에 타고 있을 때 B키 누르면 모듈 내림
            if (isModule)
            {
                if (Input.GetKeyDown(KeyCode.B))
                {
                    isModule = false;
                }
            }
            // 모듈에 타고 있지 않을 때 모듈 위에 있고, 키를 누르면 모듈에 탐
            else if (target && (Input.GetKeyUp(KeyCode.UpArrow) || Input.GetKeyUp(KeyCode.LeftArrow) ||
                Input.GetKeyUp(KeyCode.DownArrow) || Input.GetKeyUp(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.M)))
            {
                isModule = true;
            }
        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, receivePos, 15 * Time.deltaTime);
            transform.rotation = Quaternion.Lerp(transform.rotation, receiveRot, 15 * Time.deltaTime);
        }
    }

    [PunRPC]
    void RPCGravity(bool use)
    {
        rb.useGravity = use;
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        //데이터 보내기
        if (stream.IsWriting) // isMine == true
        {
            //position, rotation
            stream.SendNext(transform.rotation);
            stream.SendNext(transform.position);
        }
        //데이터 받기
        else if (stream.IsReading) // ismMine == false
        {
            receiveRot = (Quaternion)stream.ReceiveNext();
            receivePos = (Vector3)stream.ReceiveNext();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Module"))
        {
            print(other.gameObject.name);
            target = other.gameObject;
        }

        if (other.gameObject.layer == LayerMask.NameToLayer("Ladder"))
        {
            rb.velocity = Vector3.zero;
            isLadder = true;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Ladder"))
        {
            isLadder = true;
        }

        if (other.gameObject.layer == LayerMask.NameToLayer("Module") && isModule)
        {
            transform.position = Vector3.Lerp(transform.position, other.transform.position, Time.deltaTime * 10);
            //transform.position = Vector3.Lerp(transform.position, other.transform.Find("SeatPos").position, Time.deltaTime * 10);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Module"))
        {
            target = null;
        }

        if (other.gameObject.layer == LayerMask.NameToLayer("Ladder"))
        {
            isLadder = false;
        }
    }
}
