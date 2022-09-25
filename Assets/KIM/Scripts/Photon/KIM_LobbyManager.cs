using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class KIM_LobbyManager : MonoBehaviourPunCallbacks
{
    public GameObject text;
    public GameObject readyText1;
    public GameObject readyText2;
    public GameObject countText;

    public Sprite charactor1;
    public Sprite charactor2;
    public Sprite charactor3;
    public Sprite charactor4;

    public Sprite[] arr = new Sprite[4];

    public GameObject player1;
    public GameObject player2;

    bool player1Ready = false;
    bool player2Ready = false;

    int playerIndex = 1;

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
                    photonView.RPC("RPCSprite", RpcTarget.AllBuffered, value, 1);
                    photonView.RPC("RPCSetInt", RpcTarget.AllBuffered, value, 1);
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
                    photonView.RPC("RPCSprite", RpcTarget.AllBuffered, value, 2);
                    photonView.RPC("RPCSetInt", RpcTarget.AllBuffered, value, 2);
                }
            }
            idx2 = value;
        }
    }
    public int idx3 = 0;
    public int Idx3
    {
        get { return idx3; }
        set
        {
            if (idx3 != value)
            {
                if (!PhotonNetwork.IsMasterClient)
                {
                    photonView.RPC("RPCSprite", RpcTarget.AllBuffered, value, 3);
                    photonView.RPC("RPCSetInt", RpcTarget.AllBuffered, value, 3);
                }
            }
            idx3 = value;
        }
    }
    public int idx4 = 0;
    public int Idx4
    {
        get { return idx4; }
        set
        {
            if (idx4 != value)
            {
                if (!PhotonNetwork.IsMasterClient)
                {
                    photonView.RPC("RPCSprite", RpcTarget.AllBuffered, value, 4);
                    photonView.RPC("RPCSetInt", RpcTarget.AllBuffered, value, 4);
                }
            }
            idx4 = value;
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
        playerIndex = PhotonNetwork.LocalPlayer.ActorNumber;

        if (PhotonNetwork.PlayerList.Length >= 2)
        {
            text.SetActive(true);

            if (PhotonNetwork.IsMasterClient && player1Ready && player2Ready)
            {
                text.GetComponent<Text>().text = "Press Enter To Start!";
            }
            else if (PhotonNetwork.IsMasterClient && !player1Ready)
            {
                text.GetComponent<Text>().text = "Press Enter To Ready!";
            }
            else if (PhotonNetwork.IsMasterClient && !player2Ready)
            {
                text.GetComponent<Text>().text = "Wating Player2 Ready...";
            }
            else if (!PhotonNetwork.IsMasterClient && player1Ready && player2Ready)
            {
                text.GetComponent<Text>().text = "Wating For Start...";
            }
            else if (!PhotonNetwork.IsMasterClient && !player2Ready)
            {
                text.GetComponent<Text>().text = "Press Enter To Ready!";
            }
            else if (!PhotonNetwork.IsMasterClient && !player1Ready)
            {
                text.GetComponent<Text>().text = "Wating Player1 Ready...";
            }
        }
        else if (PhotonNetwork.PlayerList.Length < 2)
        {
            text.SetActive(false);
            photonView.RPC("RPCReady", RpcTarget.AllBuffered, false, 1);
            photonView.RPC("RPCReady", RpcTarget.AllBuffered, false, 2);
        }

        if (text.activeSelf == true && Input.GetKeyDown(KeyCode.Return) && PhotonNetwork.IsMasterClient && (!player1Ready || !player2Ready))
        {
            //player1Ready = true;
            photonView.RPC("RPCReady", RpcTarget.AllBuffered, true, 1);
        }
        else if (text.activeSelf == true && Input.GetKeyDown(KeyCode.Return) && PhotonNetwork.IsMasterClient && player1Ready && player2Ready)
        {
            photonView.RPC("RPCLoadScene", RpcTarget.All);
        }
        else if (text.activeSelf == true && Input.GetKeyDown(KeyCode.Escape) && PhotonNetwork.IsMasterClient)
        {
            //player1Ready = false;
            photonView.RPC("RPCReady", RpcTarget.AllBuffered, false, 1);
        }

        if (text.activeSelf == true && Input.GetKeyDown(KeyCode.Return) && !PhotonNetwork.IsMasterClient && (!player1Ready || !player2Ready))
        {
            //player2Ready = true;
            photonView.RPC("RPCReady", RpcTarget.AllBuffered, true, 2);
        }
        else if (text.activeSelf == true && Input.GetKeyDown(KeyCode.Escape) && !PhotonNetwork.IsMasterClient)
        {
            //player2Ready = false;
            photonView.RPC("RPCReady", RpcTarget.AllBuffered, false, 2);
        }

        countText.GetComponent<Text>().text = "PlayerCount: " + PhotonNetwork.PlayerList.Length.ToString();

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (PhotonNetwork.IsMasterClient && !player1Ready)
            {
                if (idx1 == 3)
                    Idx1 = 0;
                else
                    Idx1++;
            }
            else if (!PhotonNetwork.IsMasterClient && !player2Ready)
            {
                if (idx2 == 3)
                    Idx2 = 0;
                else
                    Idx2++;
            }
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (PhotonNetwork.IsMasterClient && !player1Ready)
            {
                if (idx1 == 0)
                    Idx1 = 3;
                else
                    Idx1--;
            }
            else if (!PhotonNetwork.IsMasterClient && !player2Ready)
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
    void RPCReady(bool ready, int player)
    {
        if (player == 1)
        {
            player1Ready = ready;
            readyText1.SetActive(ready);
        }
        else if (player == 2)
        {
            player2Ready = ready;
            readyText2.SetActive(ready);
        }
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
    void RPCLoadScene()
    {
        PhotonNetwork.LoadLevel("KIM_Scene_Game 2");
        //PhotonNetwork.LoadLevel("KANG_Scene_Game 2");
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
