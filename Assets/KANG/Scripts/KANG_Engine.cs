using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

// ������ ȸ����Ű��, �����ϰ� �ʹ�.
public class KANG_Engine : KANG_Machine
{
    KIM_Engine ke;
    CharacterController cc;
    Transform ship;
    public Vector3 moveDir;
    public Vector3 bounceDir;
    public float moveSpeed = 2f;
    public float curMoveSpeed = 0f;
    public bool isBounce;
    public float bounceTime = 0.2f;
    float curBounceTime = 0f;

    public Transform firePos;

    public MachineState mState = MachineState.Idle;

    //public List<GameObject> engineTextures;
    public List<Sprite> engineTextures;
    SpriteRenderer engineSprite;



    // Start is called before the first frame update
    void Start()
    {
        engineSprite = firePos.GetComponent<SpriteRenderer>();
        engineSprite.sprite = engineTextures[0];

        ship = transform.parent;
        cc = ship.GetComponent<CharacterController>();
        localAngle = rotAxis.localEulerAngles;

        ke = GetComponent<KIM_Engine>();

        Transform engine = transform.GetChild(0);
        //for (int i=1; i< engine.childCount; i++)
        //{
        //    engineTextures.Add(engine.GetChild(i).gameObject);
        //    engineTextures[i - 1].SetActive(false);
        //}
        //engineTextures[0].SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        // ���� �ε����� ��
        if (isBounce)
        {
            LerpMoveSpeed(0f);
            // �����ð����� ƨ��� �������� �̵��ϰ� �ʹ�.
            curBounceTime += Time.deltaTime;


            if (curBounceTime > bounceTime)
            {
                curBounceTime = 0f;
                isBounce = false;
            }

            moveDir = bounceDir;
        }
    }

    public override void UpKey()
    {
        photonView.RPC("RpcUpKey", RpcTarget.All);
    }

    [PunRPC]
    void RpcUpKey()
    {
        worldZ = rotAxis.eulerAngles.z;
        rotDir = (worldZ > 0f && worldZ < 180f) ? -1 : 1;
    }

    public override void DownKey()
    {
        photonView.RPC("RpcDownKey", RpcTarget.All);
    }

    [PunRPC]
    void RpcDownKey()
    {
        worldZ = rotAxis.eulerAngles.z;
        rotDir = (worldZ >= 0f && worldZ <= 180f) ? 1 : -1;
    }

    public override void LeftKey()
    {
        photonView.RPC("RpcLeftKey", RpcTarget.All);
    }

    [PunRPC]
    void RpcLeftKey()
    {
        worldZ = rotAxis.eulerAngles.z;
        rotDir = (worldZ >= 0f && worldZ < 90f) || (worldZ >= 270f && worldZ < 360f) ? 1 : -1;
    }

    public override void RightKey()
    {
        photonView.RPC("RpcRightKey", RpcTarget.All);
    }

    [PunRPC]
    void RpcRightKey()
    {
        worldZ = rotAxis.eulerAngles.z;
        rotDir = (worldZ >= 0f && worldZ < 90f) || (worldZ >= 270f && worldZ < 360f) ? -1 : 1;
    }

    // ���� ȸ��
    public override void ArrowKey()
    {
        photonView.RPC("RpcArrowKey", RpcTarget.All, Time.deltaTime);
    }

    [PunRPC]
    void RpcArrowKey(float deltaTime)
    {
        localAngle.z += rotDir * rotSpeed * deltaTime;
        localAngle.z = localAngle.z > 180 ? localAngle.z - 360 : localAngle.z;
        rotAxis.localRotation = Quaternion.Euler(0, 0, localAngle.z);
    }
    

    // ���ּ� �̵�
    public override void ActionKey()
    {
        switch (mState)
        {
            case MachineState.Idle:
                Move(moveSpeed);
                break;
            case MachineState.Beam:
                Move(moveSpeed);
                Beam();
                break;
            case MachineState.Power:
                Move(moveSpeed * 2);
                break;
            case MachineState.Metal:
                Move(moveSpeed);
                Metal();
                break;
        }
        
    }

    public GameObject engineBeamFactory;
    GameObject engineBeam;
    void Beam()
    {
        curActionKeyDownTime += Time.deltaTime;

        // Ű�� ������ �����ð��� ������ ����beam ȿ���� �����ϰ� �ʹ�.
        if(curActionKeyDownTime > beamShootTime)
        {
            if(engineBeam == null)
            {
                print("���� �� ����");

                photonView.RPC("RpcCreateBeam", RpcTarget.All);
            }
        }
    }
    [PunRPC]
    void RpcCreateBeam()
    {
        engineBeam = Instantiate(engineBeamFactory);
        engineBeam.transform.parent = firePos;
        engineBeam.transform.position = firePos.transform.position;
        engineBeam.transform.localPosition = new Vector3(0, 0.6f, 0.1f);
        engineBeam.transform.forward = firePos.up;
    }

    float createTIme = 1f;
    float curTime = 1f;
    void Metal()
    {
        curTime += Time.deltaTime;
        
        if(curTime > createTIme)
        {
            Vector3 pos = firePos.position;
            pos.y += 0.2f;
            pos.z -= 0.3f;
            PhotonNetwork.Instantiate("Shuriken", pos, firePos.rotation);
            curTime = 0f;
        }
        
    }



    void Move(float speed)
    {
        // ������ ����
        if (speed > 0 && !isBounce)
        {
            StopCoroutine(IeActionKeyUp(speed));
            LerpMoveSpeed(speed);
            // �������� ���ּ� �߽��� ���ϴ� ����
            moveDir = ship.forward - rotAxis.up;
            moveDir.z = 0;
            moveDir.Normalize();
        }

        //cc.Move(moveDir * curMoveSpeed * Time.deltaTime);
        photonView.RPC("RPCMove", RpcTarget.All, moveDir * curMoveSpeed * Time.deltaTime);
        ke.CreateEffect();
    }


    float curActionKeyDownTime = 0f;
    float beamShootTime = 3f;

       

    [PunRPC]
    void RPCMove(Vector3 dir)
    {
        cc.Move(dir);
    }

    // ���ּ� ����
    public override void ActionKeyUp()
    {
        switch (mState)
        {
            case MachineState.Idle:
                break;
            case MachineState.Beam:
                BeamShoot();
                curActionKeyDownTime = 0f;
                break;
            case MachineState.Power:
                break;
        }
        StartCoroutine(IeActionKeyUp(0f));
        ke.EndSound();
    }

    // Ű�� ������ ���� �ð� �̻��� �Ǿ��� �� Ű�� ���� ���� �߻��ϰ� �ʹ�.
    void BeamShoot()
    {
        if(curActionKeyDownTime > beamShootTime)
        {
            if(engineBeam!= null)
            {
                photonView.RPC("RpcBeamDestroy", RpcTarget.All);
            }
            PhotonNetwork.Instantiate("EngineLaser", firePos.position, firePos.rotation);
        }
    }

    [PunRPC]
    void RpcBeamDestroy()
    {
        Destroy(engineBeam);
    }

    IEnumerator IeActionKeyUp(float targetSpeed, float changeSpeed = 1f)
    {
        
        while(Mathf.Abs(targetSpeed - curMoveSpeed) > 0.1f)
        {
            curMoveSpeed = Mathf.Lerp(curMoveSpeed, targetSpeed, Time.deltaTime * changeSpeed);
            //cc.Move(moveDir * curMoveSpeed * Time.deltaTime);
            photonView.RPC("RPCMove", RpcTarget.All, moveDir * curMoveSpeed * Time.deltaTime);
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

    [PunRPC]
    void RpcChangeMState(MachineState state)
    {
        mState = state;
        engineSprite.sprite = engineTextures[(int)mState];
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name.Contains("Beam"))
        {
            photonView.RPC("RpcChangeMState", RpcTarget.All, MachineState.Beam);
            
        }

        else if (other.gameObject.name.Contains("Power"))
        {
            photonView.RPC("RpcChangeMState", RpcTarget.All, MachineState.Power);
        }

        else if (other.gameObject.name.Contains("Metal"))
        {
            photonView.RPC("RpcChangeMState", RpcTarget.All, MachineState.Metal);
        }

    }

    private void OnTriggerExit(Collider other)
    {
        // ���� ���� �������� mState�� �⺻ ���·� ��ȯ�ϰ� �ʹ�.
        if (other.gameObject.name.Contains("Crystal"))
        {
            photonView.RPC("RpcChangeMState", RpcTarget.All, MachineState.Idle);
        }
    }
}
