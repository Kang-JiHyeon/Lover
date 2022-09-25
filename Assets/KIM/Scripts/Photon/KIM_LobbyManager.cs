using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class KIM_LobbyManager : MonoBehaviourPunCallbacks
{
    AudioSource source;
    public AudioClip swipe;
    public AudioClip ready;

    public GameObject text;
    public GameObject readyText1;
    public GameObject readyText2;
    public GameObject readyText3;
    public GameObject readyText4;
    public GameObject countText;

    public Sprite charactor1;
    public Sprite charactor2;
    public Sprite charactor3;
    public Sprite charactor4;

    public Sprite[] arr = new Sprite[4];

    public GameObject player1;
    public GameObject player2;
    public GameObject player3;
    public GameObject player4;

    bool player1Ready = false;
    bool player2Ready = false;
    bool player3Ready = false;
    bool player4Ready = false;


    public int playerIndex = 1;

    public int idx1 = 0;
    public int Idx1
    {
        get { return idx1; }
        set
        {
            if (idx1 != value)
            {
                if (playerIndex == 1)
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
                if (playerIndex == 2)
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
                if (playerIndex == 3)
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
                if (playerIndex == 4)
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
        source = GetComponent<AudioSource>();
        CreateRoom();
        arr[0] = charactor1;
        arr[1] = charactor2;
        arr[2] = charactor3;
        arr[3] = charactor4;
    }

    private void Update()
    {
        if (PhotonNetwork.PlayerList.Length > 0 && PhotonNetwork.LocalPlayer == PhotonNetwork.PlayerList[0])
            playerIndex = 1;
        if (PhotonNetwork.PlayerList.Length > 1 && PhotonNetwork.LocalPlayer == PhotonNetwork.PlayerList[1])
            playerIndex = 2;
        if (PhotonNetwork.PlayerList.Length > 2 && PhotonNetwork.LocalPlayer == PhotonNetwork.PlayerList[2])
            playerIndex = 3;
        if (PhotonNetwork.PlayerList.Length > 3 && PhotonNetwork.LocalPlayer == PhotonNetwork.PlayerList[3])
            playerIndex = 4;

        KIM_PlayerTransit.Instance.playerIndex = playerIndex;   

        if (PhotonNetwork.PlayerList.Length == 2)
        {
            text.SetActive(true);
            photonView.RPC("RPCReady", RpcTarget.AllBuffered, false, 3);
            photonView.RPC("RPCReady", RpcTarget.AllBuffered, false, 4);
            if (playerIndex == 1 && player1Ready && player2Ready)
            {
                text.GetComponent<Text>().text = "Press Enter To Start!";
            }
            else if (playerIndex == 1 && !player1Ready)
            {
                text.GetComponent<Text>().text = "Press Enter To Ready!";
            }
            else if (playerIndex == 1 && !player2Ready)
            {
                text.GetComponent<Text>().text = "Wating Player2 Ready...";
            }
            else if (playerIndex == 2 && player1Ready && player2Ready)
            {
                text.GetComponent<Text>().text = "Wating For Start...";
            }
            else if (playerIndex == 2 && !player2Ready)
            {
                text.GetComponent<Text>().text = "Press Enter To Ready!";
            }
            else if (playerIndex == 2 && !player1Ready)
            {
                text.GetComponent<Text>().text = "Wating Player1 Ready...";
            }
        }
        else if (PhotonNetwork.PlayerList.Length == 3)
        {
            text.SetActive(true);
            photonView.RPC("RPCReady", RpcTarget.AllBuffered, false, 4);
            if (playerIndex == 1 && player1Ready && player2Ready && player3Ready)
            {
                text.GetComponent<Text>().text = "Press Enter To Start!";
            }
            else if (playerIndex == 1 && !player1Ready)
            {
                text.GetComponent<Text>().text = "Press Enter To Ready!";
            }
            else if (playerIndex == 1 && (!player2Ready || !player3Ready))
            {
                text.GetComponent<Text>().text = "Wating Other Player Ready...";
            }
            else if (playerIndex == 2 && player1Ready && player2Ready && player3Ready)
            {
                text.GetComponent<Text>().text = "Wating For Start...";
            }
            else if (playerIndex == 2 && !player2Ready)
            {
                text.GetComponent<Text>().text = "Press Enter To Ready!";
            }
            else if (playerIndex == 2 && (!player1Ready || !player3Ready))
            {
                text.GetComponent<Text>().text = "Wating Other Player Ready...";
            }
            else if (playerIndex == 3 && player1Ready && player2Ready && player3Ready)
            {
                text.GetComponent<Text>().text = "Wating For Start...";
            }
            else if (playerIndex == 3 && !player3Ready)
            {
                text.GetComponent<Text>().text = "Press Enter To Ready!";
            }
            else if (playerIndex == 3 && (!player1Ready || !player2Ready))
            {
                text.GetComponent<Text>().text = "Wating Other Player Ready...";
            }
        }
        else if (PhotonNetwork.PlayerList.Length == 4)
        {
            text.SetActive(true);

            if (playerIndex == 1 && player1Ready && player2Ready && player3Ready && player4Ready)
            {
                text.GetComponent<Text>().text = "Press Enter To Start!";
            }
            else if (playerIndex == 1 && !player1Ready)
            {
                text.GetComponent<Text>().text = "Press Enter To Ready!";
            }
            else if (playerIndex == 1 && (!player2Ready || !player3Ready || !player4Ready))
            {
                text.GetComponent<Text>().text = "Wating Other Player Ready...";
            }
            else if (playerIndex == 2 && player1Ready && player2Ready && player3Ready && player4Ready)
            {
                text.GetComponent<Text>().text = "Wating For Start...";
            }
            else if (playerIndex == 2 && !player2Ready)
            {
                text.GetComponent<Text>().text = "Press Enter To Ready!";
            }
            else if (playerIndex == 2 && (!player1Ready || !player3Ready || !player4Ready))
            {
                text.GetComponent<Text>().text = "Wating Other Player Ready...";
            }
            else if (playerIndex == 3 && player1Ready && player2Ready && player3Ready && player4Ready)
            {
                text.GetComponent<Text>().text = "Wating For Start...";
            }
            else if (playerIndex == 3 && !player3Ready)
            {
                text.GetComponent<Text>().text = "Press Enter To Ready!";
            }
            else if (playerIndex == 3 && (!player1Ready || !player2Ready || !player4Ready))
            {
                text.GetComponent<Text>().text = "Wating Other Player Ready...";
            }
            else if (playerIndex == 4 && player1Ready && player2Ready && player3Ready && player4Ready)
            {
                text.GetComponent<Text>().text = "Wating For Start...";
            }
            else if (playerIndex == 4 && !player4Ready)
            {
                text.GetComponent<Text>().text = "Press Enter To Ready!";
            }
            else if (playerIndex == 4 && (!player1Ready || !player2Ready || !player3Ready))
            {
                text.GetComponent<Text>().text = "Wating Other Player Ready...";
            }
        }
        else if (PhotonNetwork.PlayerList.Length < 2)
        {
            text.SetActive(false);
            photonView.RPC("RPCReady", RpcTarget.AllBuffered, false, 1);
            photonView.RPC("RPCReady", RpcTarget.AllBuffered, false, 2);
            photonView.RPC("RPCReady", RpcTarget.AllBuffered, false, 3);
            photonView.RPC("RPCReady", RpcTarget.AllBuffered, false, 4);
        }

        // 내가 플레이어 1일 때, 엔터키 누르면 레디
        if (text.activeSelf == true && Input.GetKeyDown(KeyCode.Return) && playerIndex == 1)
        {
            photonView.RPC("RPCSound", RpcTarget.All);
            photonView.RPC("RPCReady", RpcTarget.AllBuffered, true, 1);
        }
        // 내가 플레이어 1일 때, ESC를 누르면 레디 해제
        else if (text.activeSelf == true && Input.GetKeyDown(KeyCode.Escape) && playerIndex == 1)
        {
            photonView.RPC("RPCReady", RpcTarget.AllBuffered, false, 1);
        }

        // 내가 플레이어 2일 때, 엔터키 누르면 레디
        if (text.activeSelf == true && Input.GetKeyDown(KeyCode.Return) && playerIndex == 2)
        {
            photonView.RPC("RPCSound", RpcTarget.All);
            photonView.RPC("RPCReady", RpcTarget.AllBuffered, true, 2);
        }
        // 내가 플레이어 2일 때, ESC를 누르면 레디 해제
        else if (text.activeSelf == true && Input.GetKeyDown(KeyCode.Escape) && playerIndex == 2)
        {
            photonView.RPC("RPCReady", RpcTarget.AllBuffered, false, 2);
        }

        // 내가 플레이어 3일 때, 엔터키 누르면 레디
        if (text.activeSelf == true && Input.GetKeyDown(KeyCode.Return) && playerIndex == 3)
        {
            photonView.RPC("RPCSound", RpcTarget.All);
            photonView.RPC("RPCReady", RpcTarget.AllBuffered, true, 3);
        }
        // 내가 플레이어 3일 때, ESC를 누르면 레디 해제
        else if (text.activeSelf == true && Input.GetKeyDown(KeyCode.Escape) && playerIndex == 3)
        {
            photonView.RPC("RPCReady", RpcTarget.AllBuffered, false, 3);
        }

        // 내가 플레이어 4일 때, 엔터키 누르면 레디
        if (text.activeSelf == true && Input.GetKeyDown(KeyCode.Return) && playerIndex == 4)
        {
            photonView.RPC("RPCSound", RpcTarget.All);
            photonView.RPC("RPCReady", RpcTarget.AllBuffered, true, 4);
        }
        // 내가 플레이어 4일 때, ESC를 누르면 레디 해제
        else if (text.activeSelf == true && Input.GetKeyDown(KeyCode.Escape) && playerIndex == 4)
        {
            photonView.RPC("RPCReady", RpcTarget.AllBuffered, false, 4);
        }

        // 내가 플레이어 1일 때, 모두가 레디중일 때 엔터키 누르면 게임 시작
        if (PhotonNetwork.PlayerList.Length == 2 && playerIndex == 1 && player1Ready && player2Ready)
        {
            if (text.activeSelf == true && Input.GetKeyDown(KeyCode.Return))
            {
                photonView.RPC("RPCLoadScene", RpcTarget.All);
            }
        }
        else if (PhotonNetwork.PlayerList.Length == 3 && playerIndex == 1 && player1Ready && player2Ready && player3Ready)
        {
            if (text.activeSelf == true && Input.GetKeyDown(KeyCode.Return))
            {
                photonView.RPC("RPCLoadScene", RpcTarget.All);
            }
        }
        else if (PhotonNetwork.PlayerList.Length == 4 && playerIndex == 1 && player1Ready && player2Ready && player3Ready && player4Ready)
        {
            if (text.activeSelf == true && Input.GetKeyDown(KeyCode.Return))
            {
                photonView.RPC("RPCLoadScene", RpcTarget.All);
            }
        }

        countText.GetComponent<Text>().text = "Player Number: " + playerIndex.ToString();

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            source.PlayOneShot(swipe);
            if (playerIndex == 1 && !player1Ready)
            {
                if (idx1 == 3)
                    Idx1 = 0;
                else
                    Idx1++;
            }
            else if (playerIndex == 2 && !player2Ready)
            {
                if (idx2 == 3)
                    Idx2 = 0;
                else
                    Idx2++;
            }
            else if (playerIndex == 3 && !player3Ready)
            {
                if (idx3 == 3)
                    Idx3 = 0;
                else
                    Idx3++;
            }
            else if (playerIndex == 4 && !player4Ready)
            {
                if (idx4 == 3)
                    Idx4 = 0;
                else
                    Idx4++;
            }
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            source.PlayOneShot(swipe);
            if (playerIndex == 1 && !player1Ready)
            {
                if (idx1 == 0)
                    Idx1 = 3;
                else
                    Idx1--;
            }
            else if (playerIndex == 2 && !player2Ready)
            {
                if (idx2 == 0)
                    Idx2 = 3;
                else
                    Idx2--;
            }
            else if (playerIndex == 3 && !player3Ready)
            {
                if (idx3 == 0)
                    Idx3 = 3;
                else
                    Idx3--;
            }
            else if (playerIndex == 4 && !player4Ready)
            {
                if (idx4 == 0)
                    Idx4 = 3;
                else
                    Idx4--;
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
        else if (player == 3)
        {
            player3Ready = ready;
            readyText3.SetActive(ready);
        }
        else if (player == 4)
        {
            player4Ready = ready;
            readyText4.SetActive(ready);
        }
    }

    [PunRPC]
    void RPCSound()
    {
        source.PlayOneShot(ready);
    }

    [PunRPC]
    void RPCSprite(int idx, int player)
    {
        if (player == 1)
        {
            player1.GetComponent<SpriteRenderer>().sprite = arr[idx];
        }
        else if (player == 2)
        {
            player2.GetComponent<SpriteRenderer>().sprite = arr[idx];
        }
        else if (player == 3)
        {
            player3.GetComponent<SpriteRenderer>().sprite = arr[idx];
        }
        else if (player == 4)
        {
            player4.GetComponent<SpriteRenderer>().sprite = arr[idx];
        }
    }

    [PunRPC]
    void RPCSetInt(int idx, int player)
    {
        if (player == 1)
        {
            KIM_PlayerTransit.Instance.idx1 = idx;
        }
        else if (player == 2)
        {
            KIM_PlayerTransit.Instance.idx2 = idx;
        }
        else if (player == 3)
        {
            KIM_PlayerTransit.Instance.idx3 = idx;
        }
        else if (player == 4)
        {
            KIM_PlayerTransit.Instance.idx4 = idx;
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
        roomOptions.MaxPlayers = 4;
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
