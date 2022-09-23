using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

// ������ ȸ����Ű��, �����ϰ� �ʹ�.
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

    // ���
    // - ������ġ
    Vector3 receivePos;
    // -ȸ����
    Quaternion receiveRot;
    // - �����ӷ�
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
            // ���� �ε����� ��
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
        //}
    }

    // ���� ȸ��
    public override void ArrowKey()
    {
        //if (photonView.IsMine)
        //{
            base.Rotate();
        //}
    }

    // ���ּ� �̵�
    public override void ActionKey()
    {
        //if (photonView.IsMine)
        //{
                // ������ ����
            if (moveSpeed > 0 && !isBounce)
            {
                StopCoroutine(IeActionKeyUp(moveSpeed));
                LerpMoveSpeed(moveSpeed);
                // �������� ���ּ� �߽��� ���ϴ� ����
                moveDir = ship.forward - rotAxis.up;
                moveDir.z = 0;
                moveDir.Normalize();
            }

            cc.Move(moveDir * curMoveSpeed * Time.deltaTime);
        //}
        //else
        //{
        //    //Lerp�� �̿��ؼ� ������, ����������� �̵� �� ȸ��
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


    // ���ּ� ����
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
        // ������ ������
        // �����͸� ���� �� �ִٸ�? (isMine == true)
        if (stream.IsWriting)
        {
            // ��ġ, ȸ���� �޾ƿ��� �ʹ�.
            // value Ÿ�Ը� ���� �� �ִ�.
            // �迭, ����Ʈ�� ��
            // Ŭ������ ���� �� ����.
            stream.SendNext(ship.position);
            stream.SendNext(rotAxis.localRotation);
        }

        // ������ �ޱ� (isMine == false)
        // if (stream.IsReading)
        else if (stream.IsReading)
        {
            // ���� ������� �޾ƾ� ��.
            // ��ȯ���� ������Ʈ���̹Ƿ� ���� ���İ� ������ ���� �� ��ȯ ����� ��.
            receivePos = (Vector3)stream.ReceiveNext();
            receiveRot = (Quaternion)stream.ReceiveNext();
        }
    }
}
