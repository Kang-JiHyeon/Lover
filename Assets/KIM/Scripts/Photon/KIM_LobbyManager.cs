using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class KIM_LobbyManager : MonoBehaviourPunCallbacks
{
    void Start()
    {
        CreateRoom();
    }
    //�� ����
    public void CreateRoom()
    {
        // �� �ɼ��� ����
        RoomOptions roomOptions = new RoomOptions();
        // �ִ� �ο� (0�̸� �ִ��ο�)
        roomOptions.MaxPlayers = 4;
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
        PhotonNetwork.LoadLevel("KIM_Scene_Game");
    }

    //�� ������ ���� �Ǿ��� �� ȣ�� �Ǵ� �Լ�
    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        base.OnJoinRoomFailed(returnCode, message);
        print("OnJoinRoomFailed, " + returnCode + ", " + message);
    }
}
