using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

// ���ּ� �߽��� �������� ȸ���ϰ� �ʹ�.
// �ʿ�Ӽ�: ���ּ� �߽�, ȸ�� �ӵ�, ȸ�� ����

public class KANG_YamatoRotate : MonoBehaviourPun, IPunObservable
{
    // ȸ���߽�
    public Transform spaceship;
    // ȸ���ӵ�
     float rotSpeed = 100;
    // ȸ������
    public float rotDir = 1f;

    // yamato texture
    public List<Transform> rotObjects;

    float rotZ;

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
            transform.Rotate(-transform.forward * Time.deltaTime * 20);
        }
        else
        {
            //transform.Rotate(-transform.forward);
            transform.rotation = Quaternion.Lerp(transform.rotation, receiveRot, 25 * Time.deltaTime);
        }
    }

    Quaternion receiveRot;


    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        //������ ������
        if (stream.IsWriting) // isMine == true
        {
            //position, rotation
            stream.SendNext(transform.rotation);
        }
        //������ �ޱ�
        else if (stream.IsReading) // ismMine == false
        {
            receiveRot = (Quaternion)stream.ReceiveNext();
        }
    }
}
