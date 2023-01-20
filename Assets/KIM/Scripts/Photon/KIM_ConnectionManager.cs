using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class KIM_ConnectionManager : MonoBehaviourPunCallbacks
{
    void Start()
    {
        
    }
    public void OnClickConnect()
    {
        //���� ���� ��û
        PhotonNetwork.ConnectUsingSettings();
    }

    //������ ���� ���Ӽ����� ȣ��(Lobby�� ������ �� ���� ����)
    public override void OnConnected()
    {
        base.OnConnected();
    }

    //������ ���� ���Ӽ����� ȣ��(Lobby�� ������ �� �ִ� ����)
    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        PhotonNetwork.NickName = "Player_" + Random.Range(1, 1000);
        PhotonNetwork.JoinLobby();
    }

    //�κ� ���� ������ ȣ��
    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();
        PhotonNetwork.LoadLevel("KANG_LobbyScene");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKeyDown)
            OnClickConnect();
    }
}
