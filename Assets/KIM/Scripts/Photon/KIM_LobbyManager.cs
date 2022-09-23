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

    //방 생성
    public void CreateRoom()
    {
        // 방 옵션을 설정
        RoomOptions roomOptions = new RoomOptions();
        // 최대 인원 (0이면 최대인원)
        roomOptions.MaxPlayers = 2;
        // 룸 리스트에 보이지 않게? 보이게?
        roomOptions.IsVisible = true;

        // 방 생성 요청 (해당 옵션을 이용해서)
        PhotonNetwork.CreateRoom("Room", roomOptions);
    }

    //방이 생성되면 호출 되는 함수
    public override void OnCreatedRoom()
    {
        base.OnCreatedRoom();
        print("OnCreatedRoom");
    }

    //방 생성이 실패 될때 호출 되는 함수
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        base.OnCreateRoomFailed(returnCode, message);
        print("OnCreateRoomFailed , " + returnCode + ", " + message);
        JoinRoom();
    }

    //방 참가 요청
    public void JoinRoom()
    {
        PhotonNetwork.JoinRoom("Room");
    }

    //방 참가가 완료 되었을 때 호출 되는 함수
    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        print("OnJoinedRoom");
    }

    //방 참가가 실패 되었을 때 호출 되는 함수
    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        base.OnJoinRoomFailed(returnCode, message);
        print("OnJoinRoomFailed, " + returnCode + ", " + message);
    }
}
