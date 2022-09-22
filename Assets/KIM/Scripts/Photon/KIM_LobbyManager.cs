using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class KIM_LobbyManager : MonoBehaviourPunCallbacks
{
    public GameObject text;
    public GameObject countText;

    void Awake()
    {
        CreateRoom();
    }

    private void Update()
    {
        if (PhotonNetwork.PlayerList.Length >= 2 && !text.activeSelf)
            photonView.RPC("RPCSetActive", RpcTarget.All, true);
        else if (PhotonNetwork.PlayerList.Length < 2 && text.activeSelf)
            photonView.RPC("RPCSetActive", RpcTarget.All, false);

        if (text.activeSelf == true && Input.GetKeyDown(KeyCode.Return) && PhotonNetwork.IsMasterClient)
        {
            photonView.RPC("RPCLoadScene", RpcTarget.All);
        }

        countText.GetComponent<Text>().text = "PlayerCount: " + PhotonNetwork.PlayerList.Length.ToString();
    }

    [PunRPC]
    void RPCSetActive(bool boolean)
    {
        text.SetActive(boolean);


        if (PhotonNetwork.IsMasterClient)
        {
            text.GetComponent<Text>().text = "Press Enter To Start";
        }
        else
        {
            text.GetComponent<Text>().text = "Waiting For Start...";
        }
        
    }

    [PunRPC]
    void RPCLoadScene()
    {
        PhotonNetwork.LoadLevel("KIM_Scene_Game");
    }

    //�� ����
    public void CreateRoom()
    {
        // �� �ɼ��� ����
        RoomOptions roomOptions = new RoomOptions();
        // �ִ� �ο� (0�̸� �ִ��ο�)
        roomOptions.MaxPlayers = 2;
        // �� ����Ʈ�� ������ �ʰ�? ���̰�?
        roomOptions.IsVisible = true;

        // �� ���� ��û (�ش� �ɼ��� �̿��ؼ�)
        PhotonNetwork.CreateRoom("Room", roomOptions);
    }

    //���� �����Ǹ� ȣ�� �Ǵ� �Լ�
    public override void OnCreatedRoom()
    {
        base.OnCreatedRoom();
        print("OnCreatedRoom");
    }

    //�� ������ ���� �ɶ� ȣ�� �Ǵ� �Լ�
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        base.OnCreateRoomFailed(returnCode, message);
        print("OnCreateRoomFailed , " + returnCode + ", " + message);
        JoinRoom();
    }

    //�� ���� ��û
    public void JoinRoom()
    {
        PhotonNetwork.JoinRoom("Room");
    }

    //�� ������ �Ϸ� �Ǿ��� �� ȣ�� �Ǵ� �Լ�
    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        print("OnJoinedRoom");
    }

    //�� ������ ���� �Ǿ��� �� ȣ�� �Ǵ� �Լ�
    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        base.OnJoinRoomFailed(returnCode, message);
        print("OnJoinRoomFailed, " + returnCode + ", " + message);
    }
}
