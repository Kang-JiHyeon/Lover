using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

// ���ּ� �߽��� �������� ȸ���ϰ� �ʹ�.
// �ʿ�Ӽ�: ���ּ� �߽�, ȸ�� �ӵ�, ȸ�� ����

public class KANG_YamatoRotate : MonoBehaviourPun, IPunObservable
{
    // ȸ���ӵ�
    public float rotSpeed = 30f;

    // Rpc ȸ�� �ӵ�
    public float lerpSpeed = 35f;
    
    // rpc ȸ��
    Quaternion receiveRot;

    // Start is called before the first frame update
    void Start()
    {
        //// ȸ�� �߽� = ���ּ��� �߽�
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
        //������ ������
        if (stream.IsWriting) // isMine == true
        {
            stream.SendNext(transform.rotation);
        }
        //������ �ޱ�
        else if (stream.IsReading) // ismMine == false
        {
            receiveRot = (Quaternion)stream.ReceiveNext();
        }
    }
}
