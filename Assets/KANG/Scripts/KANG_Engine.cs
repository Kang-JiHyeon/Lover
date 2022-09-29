using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

// 엔진을 회전시키고, 가동하고 싶다.
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

    public List<GameObject> engineTextures;



    // Start is called before the first frame update
    void Start()
    {
        ship = transform.parent;
        cc = ship.GetComponent<CharacterController>();
        localAngle = rotAxis.localEulerAngles;

        ke = GetComponent<KIM_Engine>();

        Transform engine = transform.GetChild(0);
        for (int i=1; i< engine.childCount; i++)
        {
            engineTextures.Add(engine.GetChild(i).gameObject);
            engineTextures[i - 1].SetActive(false);
        }
        engineTextures[0].SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        // 벽에 부딪혔을 때
        if (isBounce)
        {
            LerpMoveSpeed(0f);
            // 일정시간동안 튕기는 방향으로 이동하고 싶다.
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

    // 엔진 회전
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
    

    // 우주선 이동
    public override void ActionKey()
    {
        switch (mState)
        {
            case MachineState.Idle:
                break;
            case MachineState.Beam:
                Beam();
                break;
        }
        Move();
    }

    public GameObject engineBeamFactory;
    GameObject engineBeam;
    void Beam()
    {
        curActionKeyDownTime += Time.deltaTime;

        // 키를 누른지 일정시간이 지나면 엔진beam 효과를 생성하고 싶다.
        if(curActionKeyDownTime > beamShootTime)
        {
            if(engineBeam == null)
            {
                print("엔진 빔 가열");

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

    void Move()
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

    // 우주선 멈춤
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
        }
        StartCoroutine(IeActionKeyUp(0f));
    }

    // 키를 누른지 일정 시간 이상이 되었을 때 키를 떼면 빔을 발사하고 싶다.
    void BeamShoot()
    {
        if(curActionKeyDownTime > beamShootTime)
        {
            if(engineBeam!= null)
            {
                photonView.RPC("RpcBeamDestroy", RpcTarget.All);
            }
            PhotonNetwork.Instantiate("EngineLaser", firePos.position, firePos.rotation);
            print("Beam 발사");
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
    }

    [PunRPC]
    void RpcSetEngine(int index, bool isEnable)
    {
        engineTextures[index].SetActive(isEnable);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name.Contains("Beam"))
        {
            photonView.RPC("RpcSetEngine", RpcTarget.All, (int)mState, false);

            photonView.RPC("RpcChangeMState", RpcTarget.All, MachineState.Beam);

            photonView.RPC("RpcSetEngine", RpcTarget.All, (int)mState, true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // 보석 장착 없어지면 mState를 기본 상태로 전환하고 싶다.
        if (other.gameObject.name.Contains("Jewel"))
        {
            photonView.RPC("RpcSetEngine", RpcTarget.All, (int)mState, false);

            photonView.RPC("RpcChangeMState", RpcTarget.All, MachineState.Idle);

            photonView.RPC("RpcSetEngine", RpcTarget.All, (int)mState, true);
        }
    }
}
