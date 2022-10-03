using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

// 우주선 중심을 기준으로 회전하고 싶다.
// 필요속성: 우주선 중심, 회전 속도, 회전 방향

public class KANG_YamatoRotate : MonoBehaviourPun, IPunObservable
{
    // 회전속도
    public float rotSpeed = 30f;

    // Rpc 회전 속도
    public float lerpSpeed = 35f;
    
    // rpc 회전
    Quaternion receiveRot;

    // Start is called before the first frame update
    void Start()
    {
        //// 회전 중심 = 우주선의 중심
        //spaceship = transform.parent.parent;

        //for(int i = 0; i< transform.childCount; i++)
        //{
        //    rotObjects.Add(transform.GetChild(i));
        //}

    }

    // Update is called once per frame
    void Update()
    {
        if(photonView.IsMine)
        {
            transform.Rotate(-transform.forward * rotSpeed * Time.deltaTime);
        }
        else
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, receiveRot, lerpSpeed * Time.deltaTime);
        }
    }




    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        //데이터 보내기
        if (stream.IsWriting) // isMine == true
        {
            stream.SendNext(transform.rotation);
        }
        //데이터 받기
        else if (stream.IsReading) // ismMine == false
        {
            receiveRot = (Quaternion)stream.ReceiveNext();
        }
    }
}
