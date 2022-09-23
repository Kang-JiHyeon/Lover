using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class KIM_LobbyManager : MonoBehaviourPunCallbacks
{
    public GameObject text;
    public GameObject countText;

    public Sprite charactor1;
    public Sprite charactor2;
    public Sprite charactor3;
    public Sprite charactor4;

    public Sprite[] arr = new Sprite[4];

    public GameObject player1;
    public GameObject player2;

    public int idx1 = 0;
    public int Idx1
    {
        get { return idx1; }
        set
        {
            if (idx1 != value)
            {
                if (PhotonNetwork.IsMasterClient)
                {
                    photonView.RPC("RPCSprite", RpcTarget.All, value, 1);
                    photonView.RPC("RPCSetInt", RpcTarget.All, value, 1);
                }
            }
            idx1 = value;
        }
    }
    public int idx2 = 0;
    public int Idx2
    {
        get { return idx2; }
        set
        {
            if (idx2 != value)
            {
                if (!PhotonNetwork.IsMasterClient)
                {
                    photonView.RPC("RPCSprite", RpcTarget.All, value, 2);
                    photonView.RPC("RPCSetInt", RpcTarget.All, value, 2);
                }
            }
            idx2 = value;
        }
    }

    void Start()
    {
        CreateRoom();
        arr[0] = charactor1;
        arr[1] = charactor2;
        arr[2] = charactor3;
        arr[3] = charactor4;
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

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (PhotonNetwork.IsMasterClient)
            {
                if (idx1 == 3)
                    Idx1 = 0;
                else
                    Idx1++;
            }
            else
            {
                if (idx2 == 3)
                    Idx2 = 0;
                else
                    Idx2++;
            }
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (PhotonNetwork.IsMasterClient)
            {
                if (idx1 == 0)
                    Idx1 = 3;
                else
                    Idx1--;
            }
            else
            {
                if (idx2 == 0)
                    Idx2 = 3;
                else
                    Idx2--;
            }
        }

        //player1.GetComponent<SpriteRenderer>().sprite = arr[idx1];
        //player2.GetComponent<SpriteRenderer>().sprite = arr[idx2];
    }

    [PunRPC]
    void RPCSprite(int idx, int player)
    {
        if (player == 1)
        {
            player1.GetComponent<SpriteRenderer>().sprite = arr[idx];
        }
        else
        {
            player2.GetComponent<SpriteRenderer>().sprite = arr[idx];
        }
    }

    [PunRPC]
    void RPCSetInt(int idx, int player)
    {
        if (player == 1)
        {
            KIM_PlayerTransit.Instance.idx1 = idx;
        }
        else
        {
            KIM_PlayerTransit.Instance.idx2 = idx;
        }
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
        //PhotonNetwork.LoadLevel("KIM_Scene_Game");
        PhotonNetwork.LoadLevel("KANG_Scene_Game 1");
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
