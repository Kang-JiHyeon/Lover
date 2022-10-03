using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class KIM_PlayerController1 : MonoBehaviourPun, IPunObservable
{
    AudioSource source;

    GameObject ship;
    GameObject target;
    KANG_Machine machine;

    public GameObject powerCrystal;
    public GameObject MetalCrystal;
    public GameObject BeamCrystal;
    public GameObject CrowBarCrystal;

    Rigidbody rb;
    bool isLadder;
    bool isModule;

    //���� ��ġ
    Vector3 receivePos;
    //ȸ���Ǿ� �ϴ� ��
    Quaternion receiveRot;

    [SerializeField]
    float speed = 5;
    // Start is called before the first frame update
    void Awake()
    {
        source = GetComponent<AudioSource>();
        ship = GameObject.Find("Spaceship");
        transform.SetParent(ship.transform);
        rb = GetComponent<Rigidbody>(); 
    }

    // Update is called once per frame
    void Update()
    {
        if (photonView.IsMine)
        {
            // ��ٸ� ���� ���� �߷� ���� �ȹ���
            if (isLadder || isModule)
                photonView.RPC("RPCGravity", RpcTarget.All, false);
            else
                photonView.RPC("RPCGravity", RpcTarget.All, true);

            if (Input.GetKey(KeyCode.M))
            {
                if (isModule && target)
                {
                    machine = target.GetComponent<KANG_Machine>();
                    machine.ActionKey();
                }
            }
            else if (Input.GetKeyUp(KeyCode.M))
            {

                if (isModule && target)
                {
                    machine = target.GetComponent<KANG_Machine>();
                    machine.ActionKeyUp();
                }
            }

            if (Input.GetKey(KeyCode.UpArrow))
            {
                if (isModule && target)
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
                if (isModule && target)
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
                if (isModule && target)
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
                if (isModule && target)
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
                if (isModule && target)
                {
                    machine = target.GetComponent<KANG_Machine>();
                    machine.ArrowKey();
                }
            }

            if ((Input.GetKeyUp(KeyCode.UpArrow) && !(Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow)))
            || (Input.GetKeyUp(KeyCode.DownArrow) && !(Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow)))
            || (Input.GetKeyUp(KeyCode.LeftArrow) && !(Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.RightArrow)))
            || (Input.GetKeyUp(KeyCode.RightArrow) && !(Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.LeftArrow))))
            {
                if (isModule && target)
                {
                    machine = target.GetComponent<KANG_Machine>();
                    machine.ArrowKeyUp();
                    print("machine ArrowKeyUp: " + target.name);
                }
            }

            Debug.DrawRay(transform.position, Vector3.down * 0.4f, Color.red);
            if (Physics.Raycast(transform.position, Vector3.down, 0.4f, LayerMask.GetMask("ShipFloor")))
            {
                if (Input.GetKeyDown(KeyCode.Space))
                    rb.AddForce(Vector3.up * 150f);
            }

            // ��⿡ Ÿ�� ���� �� BŰ ������ ��� ����
            if (isModule && target)
            {
                if (Input.GetKeyDown(KeyCode.B))
                {
                    isModule = false;
                    photonView.RPC("RPCControl", RpcTarget.AllBuffered, false, target.name);
                }
            }
            // ��⿡ Ÿ�� ���� ���� �� ��� ���� �ְ�, Ű�� ������ ��⿡ Ž
            else if (target && !target.GetComponent<KANG_Machine>().IsControl && 
                (Input.GetKeyUp(KeyCode.UpArrow) || Input.GetKeyUp(KeyCode.LeftArrow) ||
                Input.GetKeyUp(KeyCode.DownArrow) || Input.GetKeyUp(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.M)))
            {
                isModule = true;
                photonView.RPC("RPCControl", RpcTarget.AllBuffered, true, target.name);
            }
        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, receivePos, 15 * Time.deltaTime);
            transform.rotation = Quaternion.Lerp(transform.rotation, receiveRot, 15 * Time.deltaTime);
        }
    }

    [PunRPC] 
    void RPCControl(bool value, string s)
    {
        machine = GameObject.Find(s).GetComponent<KANG_Machine>();
        machine.IsControl = value;
    }

    [PunRPC]
    void RPCGravity(bool use)
    {
        rb.useGravity = use;
    }

    public void CrystalInit(int idx, AudioClip clip)
    {
        if (idx == 1)
        {
            GameObject crystal = Instantiate(powerCrystal);
            crystal.transform.SetParent(transform);
            crystal.transform.localPosition = new Vector3(0, 1f, 0);
            source.PlayOneShot(clip);
            Destroy(crystal, 15f);
        }
        else if (idx == 2)
        {
            GameObject crystal = Instantiate(MetalCrystal);
            crystal.transform.SetParent(transform);
            crystal.transform.localPosition = new Vector3(0, 1f, 0);
            source.PlayOneShot(clip);
            Destroy(crystal, 15f);
        }
        else if (idx == 3)
        {
            GameObject crystal = Instantiate(BeamCrystal);
            crystal.transform.SetParent(transform);
            crystal.transform.localPosition = new Vector3(0, 1f, 0);
            source.PlayOneShot(clip);
            Destroy(crystal, 15f);
        }
        else if (idx == 4)
        {
            GameObject crystal = Instantiate(CrowBarCrystal);
            crystal.transform.SetParent(transform);
            crystal.transform.localPosition = new Vector3(0, 1f, 0);
            source.PlayOneShot(clip);
            Destroy(crystal, 15f);
        }
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
            //transform.position = Vector3.Lerp(transform.position, other.transform.position, Time.deltaTime * 10);
            transform.position = Vector3.Lerp(transform.position, other.transform.Find("SeatPos").position, Time.deltaTime * 10);
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
