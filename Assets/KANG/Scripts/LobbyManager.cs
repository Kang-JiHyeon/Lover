//using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using ExitGames.Client.Photon;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    // ���̸�
    public InputField inputRoomName;
    // ��й�ȣ
    public InputField inputPassword;
    //// �ִ��ο�
    public InputField inputMaxPlayer;

    public Button btnJoin;
    public Button btnCreate;

    // �� ������ ��� �ִ� ��ųʸ�
    Dictionary<string, RoomInfo> roomCache = new Dictionary<string, RoomInfo>();
    // ����Ʈ Content
    public Transform trListContent;

    //// mapThumbnail
    //public GameObject[] mapThumbs;


    // Start is called before the first frame update
    void Start()
    {
        // ���̸�(InputField)�� ����� �� ȣ��Ǵ� �Լ� ���
        inputRoomName.onValueChanged.AddListener(onRoomNameValueChanged);
        //// �ִ� �ο�(InputField)�� ����� �� ȣ��Ǵ� �Լ� ���
        //inputMaxPlayer.onValueChanged.AddListener(onMaxPlayerValueChanged);
        inputMaxPlayer.text = "4";
    }

    public void onRoomNameValueChanged(string s)
    {
        // �� ����
        btnJoin.interactable = s.Length > 0;

        // �� ���� --> ���̸�, �ִ��ο� �ΰ��� ���� ����ؾ� ��
        //btnCreate.interactable = s.Length > 0 && inputMaxPlayer.text.Length > 0;
        btnCreate.interactable = s.Length > 0;


        //if (s.Length > 0)
        //{
        //    btnJoin.interactable = true;
        //}
        //else
        //{
        //    btnJoin.interactable = false;
        //    btnCreate.interactable = false;
        //}
    }

    public void onMaxPlayerValueChanged(string s)
    {
        // �� ���� --> ���̸�, �ִ��ο� �ΰ��� ���� ����ؾ� ��
        btnCreate.interactable = s.Length > 0 && inputRoomName.text.Length > 0;


        //if (btnJoin.interactable && s.Length > 0)
        //    btnCreate.interactable = true;
        //else
        //    btnCreate.interactable = false;
    }


    // �� ����
    public void CreateRoom()
    {
        // �� �ɼ� ���� (�� ����, �ο� ��)
        RoomOptions roomOptions = new RoomOptions();
        // �ִ� �ο�(0�̸� �⺻ �ִ��ο�)
        roomOptions.MaxPlayers = byte.Parse(inputMaxPlayer.text);
        // �� ��Ͽ� ���̴��� ���� (�⺻�� = true)
        roomOptions.IsVisible = true;

        // * custom ������ ����
        // Hashtable : Dictionary<object, object>;
        // ExitGames.Client.Photon.Hashtable hash = new ExitGames.Client.Photon.Hashtable;
        Hashtable hash = new Hashtable();
        hash["desc"] = "#" + Random.Range(1, 1000);
        hash["map_id"] = Random.Range(0, 10);    // �ӽ�
        hash["room_name"] = inputRoomName.text;
        hash["password"] = inputPassword.text;

        roomOptions.CustomRoomProperties = hash;

        // * custom ������ �����ϴ� ����
        // �����ϰ��� �ϴ� key������ ����
        roomOptions.CustomRoomPropertiesForLobby = new string[] { "desc", "map_id", "room_name", "password" };

        // �� ���� ��û
        PhotonNetwork.CreateRoom(inputRoomName.text + inputPassword.text, roomOptions, TypedLobby.Default);
    }

    // �� ���� �Ϸ�
    public override void OnCreatedRoom()
    {
        base.OnCreatedRoom();
        print(System.Reflection.MethodBase.GetCurrentMethod().Name);
    }

    // �� ���� ����
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        base.OnCreateRoomFailed(returnCode, message);
        print("OnCreateRoomFailed, " + returnCode + ", " + message);
    }

    // �� ���� (������ڴ� �ڵ����� ������ ��)
    public void JoinRoom()
    {
        PhotonNetwork.JoinRoom(inputRoomName.text+inputPassword.text);
    }

    // �� ���忡 �������� �� �Ҹ��� �Լ�
    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        print("OnJoinedRoom");
        // �� ��ȯ �� ������ ������ ���� ���� ��� --> ���ο��� SceneManager.LoadScene �� ��
        //PhotonNetwork.LoadLevel("GameScene");
        PhotonNetwork.LoadLevel("KIM_Lobby");
    }

    // �� ���� ���� �� ȣ��Ǵ� �Լ� (������ ���� �̸��� ���� ���)
    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        base.OnJoinRoomFailed(returnCode, message);
        print("OnJoinRoomFailed, " + returnCode + ", " + message);
    }

    // �濡 ���� ������ ����Ǹ� ȣ��Ǵ� �Լ�(�߰�/����/����)
    // roomList : ������ ����� �� ����Ʈ
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        //base.OnRoomListUpdate

        // �븮��Ʈ UI�� ��ü ����
        DeleteRoomListUI();
        // �븮��Ʈ ������ ������Ʈ
        UpdateRoomChach(roomList);
        // �븮���� UI ��ü ����
        CreateRoomListUI();
    }

    
    void DeleteRoomListUI()
    {
        foreach(Transform tr in trListContent)
        {
            Destroy(tr.gameObject);
        }
    }

    void UpdateRoomChach(List<RoomInfo> roomList)
    {
        for(int i = 0; i<roomList.Count; i++)
        {
            #region ����
            //// roomList�� rooCache�� ������ �߰�, ������ ���� �� ����
            //// ����, ����
            //if (roomCache.ContainsKey(roomList[i].Name))
            //{
            //    // ����, �ش� ���� ���� �� ���̶��
            //    if (roomList[i].RemovedFromList)
            //    {
            //        // roomCache���� �ش� ������ ����
            //        roomCache.Remove(roomList[i].Name);
            //        continue;
            //    }
            //}
            //    // �׷��� �ʴٸ�
            //    else
            //    {
            //        // ���� ����
            //        roomCache[roomList[i].Name] = roomList[i];
            //    }
            //}
            //// �߰�
            //else
            //{
            //    // ���� �߰�
            //    roomCache[roomList[i].Name] = roomList[i];
            //}
            #endregion

            // roomList�� rooCache�� ������ �߰�, ������ ���� �� ����
            // ����, ����
            if (roomCache.ContainsKey(roomList[i].Name))
            {
                // ����, �ش� ���� ���� �� ���̶��
                if (roomList[i].RemovedFromList)
                {
                    // roomCache���� �ش� ������ ����
                    roomCache.Remove(roomList[i].Name);
                    continue;
                }
            }
            // ���� ���� �� �߰�
            roomCache[roomList[i].Name] = roomList[i];
        }
    }

    public GameObject itemFactory;

    void CreateRoomListUI()
    {
        foreach(RoomInfo info in roomCache.Values)
        {
            // �θ�� ������ ������Ʈ�� �ڽ����� ���ӿ�����Ʈ ������ (�����Ǵ� ������Ʈ, �θ� ������Ʈ)
            GameObject go = Instantiate(itemFactory, trListContent);
            RoomItem item = go.GetComponent<RoomItem>();
            item.SetInfo(info);

            // roomItem ��ư�� Ŭ���Ǹ� ȣ��Ǵ� �Լ� ���
            // 1) �Լ��� ����
            item.onClickAction = SetRoomName;

            //// 2) ���ٽ����� ����
            //item.onClickAction = (string room) => {
            //    inputRoomName.text = room;
            //};

            // object ��ȯ���� ����� �ڷ������� ����ȯ
            string desc = (string)info.CustomProperties["desc"];
            int map_id = (int)info.CustomProperties["map_id"];
            //print(desc + ", " + map_id);
        }
    }
    // ���� Thumbnail�� id
    int preMapId = -1;

    void SetRoomName(string room, int map_id)
    {
        inputRoomName.text = room;

        // ���� ���� �� Thumbnail�� Ȱ��ȭ �Ǿ��ִٸ�
        if(preMapId > -1)
        {
            // ���� �� Thumbnail�� ��Ȱ��ȭ�Ѵ�.
            //mapThumbs[preMapId].SetActive(false);
        }
        
        // �� Thumbnail ����
        //mapThumbs[map_id].SetActive(true);

        // ���� �� id ����
        preMapId = map_id;

    }
}
