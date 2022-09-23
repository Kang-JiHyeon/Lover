using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

// 엔진을 회전시키고, 가동하고 싶다.
public class KANG_Engine : KANG_Machine, IPunObservable
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

    // 통신
    // - 도착위치
    Vector3 receivePos;
    // -회전값
    Quaternion receiveRot;
    // - 보간속력
    float lerpSpeed = 100f;


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
        //if (photonView.IsMine)
        //{
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
        //}
    }

    // 엔진 회전
    public override void ArrowKey()
    {
        //if (photonView.IsMine)
        //{
            base.Rotate();
        //}
    }

    // 우주선 이동
    public override void ActionKey()
    {
        //if (photonView.IsMine)
        //{
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
        //}
        //else
        //{
        //    //Lerp를 이용해서 목적지, 목적방향까지 이동 및 회전
        //    ship.position = Vector3.Lerp(ship.position, receivePos, lerpSpeed * Time.deltaTime);
        //    rotAxis.localRotation = Quaternion.Lerp(rotAxis.localRotation, receiveRot, lerpSpeed * Time.deltaTime);

        //}
        //    photonView.RPC("RpcActionKey", RpcTarget.All);
    }

 
    [PunRPC]
    void RpcActionKey()
    {


        //cc.Move(moveDir * curMoveSpeed * Time.deltaTime);
    }


    // 우주선 멈춤
    public override void ActionKeyUp()
    {
        if (photonView.IsMine)
        {
            StartCoroutine(IeActionKeyUp(0f));
        }
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



    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        // isWriting <-> isReading
        // 데이터 보내기
        // 데이터를 보낼 수 있다면? (isMine == true)
        if (stream.IsWriting)
        {
            // 위치, 회전값 받아오고 싶다.
            // value 타입만 보낼 수 있다.
            // 배열, 리스트는 들어감
            // 클래스는 보낼 수 없다.
            stream.SendNext(ship.position);
            stream.SendNext(rotAxis.localRotation);
        }

        // 데이터 받기 (isMine == false)
        // if (stream.IsReading)
        else if (stream.IsReading)
        {
            // 보낸 순서대로 받아야 함.
            // 반환형이 오브젝트형이므로 기존 형식과 같도록 강제 형 변환 해줘야 함.
            receivePos = (Vector3)stream.ReceiveNext();
            receiveRot = (Quaternion)stream.ReceiveNext();
        }
    }
}
