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
    // 방이름
    public InputField inputRoomName;
    // 비밀번호
    public InputField inputPassword;
    //// 최대인원
    public InputField inputMaxPlayer;

    public Button btnJoin;
    public Button btnCreate;

    // 방 정보를 담고 있는 딕셔너리
    Dictionary<string, RoomInfo> roomCache = new Dictionary<string, RoomInfo>();
    // 리스트 Content
    public Transform trListContent;

    //// mapThumbnail
    //public GameObject[] mapThumbs;


    // Start is called before the first frame update
    void Start()
    {
        // 방이름(InputField)이 변경될 때 호출되는 함수 등록
        inputRoomName.onValueChanged.AddListener(onRoomNameValueChanged);
        //// 최대 인원(InputField)이 변경될 때 호출되는 함수 등록
        //inputMaxPlayer.onValueChanged.AddListener(onMaxPlayerValueChanged);
        inputMaxPlayer.text = "4";
    }

    public void onRoomNameValueChanged(string s)
    {
        // 방 참가
        btnJoin.interactable = s.Length > 0;

        // 방 생성 --> 방이름, 최대인원 두가지 조건 고려해야 함
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
        // 방 생성 --> 방이름, 최대인원 두가지 조건 고려해야 함
        btnCreate.interactable = s.Length > 0 && inputRoomName.text.Length > 0;


        //if (btnJoin.interactable && s.Length > 0)
        //    btnCreate.interactable = true;
        //else
        //    btnCreate.interactable = false;
    }


    // 방 생성
    public void CreateRoom()
    {
        // 방 옵션 설정 (방 제목, 인원 수)
        RoomOptions roomOptions = new RoomOptions();
        // 최대 인원(0이면 기본 최대인원)
        roomOptions.MaxPlayers = byte.Parse(inputMaxPlayer.text);
        // 룸 목록에 보이는지 여부 (기본값 = true)
        roomOptions.IsVisible = true;

        // * custom 정보를 설정
        // Hashtable : Dictionary<object, object>;
        // ExitGames.Client.Photon.Hashtable hash = new ExitGames.Client.Photon.Hashtable;
        Hashtable hash = new Hashtable();
        hash["desc"] = "#" + Random.Range(1, 1000);
        hash["map_id"] = Random.Range(0, 10);    // 임시
        hash["room_name"] = inputRoomName.text;
        hash["password"] = inputPassword.text;

        roomOptions.CustomRoomProperties = hash;

        // * custom 정보를 공개하는 설정
        // 공개하고자 하는 key값들을 나열
        roomOptions.CustomRoomPropertiesForLobby = new string[] { "desc", "map_id", "room_name", "password" };

        // 방 생성 요청
        PhotonNetwork.CreateRoom(inputRoomName.text + inputPassword.text, roomOptions, TypedLobby.Default);
    }

    // 방 생성 완료
    public override void OnCreatedRoom()
    {
        base.OnCreatedRoom();
        print(System.Reflection.MethodBase.GetCurrentMethod().Name);
    }

    // 방 생성 실패
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        base.OnCreateRoomFailed(returnCode, message);
        print("OnCreateRoomFailed, " + returnCode + ", " + message);
    }

    // 방 입장 (방생성자는 자동으로 입장이 됨)
    public void JoinRoom()
    {
        PhotonNetwork.JoinRoom(inputRoomName.text+inputPassword.text);
    }

    // 방 입장에 성공했을 때 불리는 함수
    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        print("OnJoinedRoom");
        // 씬 전환 시 데이터 유실을 막기 위해 사용 --> 내부에서 SceneManager.LoadScene 을 함
        //PhotonNetwork.LoadLevel("GameScene");
        PhotonNetwork.LoadLevel("KIM_Lobby");
    }

    // 방 입장 실패 시 호출되는 함수 (들어가려는 방의 이름이 없는 경우)
    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        base.OnJoinRoomFailed(returnCode, message);
        print("OnJoinRoomFailed, " + returnCode + ", " + message);
    }

    // 방에 대한 정보가 변경되면 호출되는 함수(추가/삭제/수정)
    // roomList : 정보가 변경된 방 리스트
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        //base.OnRoomListUpdate

        // 룸리스트 UI를 전체 삭제
        DeleteRoomListUI();
        // 룸리스트 정보를 업데이트
        UpdateRoomChach(roomList);
        // 룸리스터 UI 전체 생성
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
            #region 정석
            //// roomList에 rooCache가 없으면 추가, 있으면 수정 및 삭제
            //// 수정, 삭제
            //if (roomCache.ContainsKey(roomList[i].Name))
            //{
            //    // 만약, 해당 룸이 삭제 된 것이라면
            //    if (roomList[i].RemovedFromList)
            //    {
            //        // roomCache에서 해당 정보를 삭제
            //        roomCache.Remove(roomList[i].Name);
            //        continue;
            //    }
            //}
            //    // 그렇지 않다면
            //    else
            //    {
            //        // 정보 수정
            //        roomCache[roomList[i].Name] = roomList[i];
            //    }
            //}
            //// 추가
            //else
            //{
            //    // 정보 추가
            //    roomCache[roomList[i].Name] = roomList[i];
            //}
            #endregion

            // roomList에 rooCache가 없으면 추가, 있으면 수정 및 삭제
            // 수정, 삭제
            if (roomCache.ContainsKey(roomList[i].Name))
            {
                // 만약, 해당 룸이 삭제 된 것이라면
                if (roomList[i].RemovedFromList)
                {
                    // roomCache에서 해당 정보를 삭제
                    roomCache.Remove(roomList[i].Name);
                    continue;
                }
            }
            // 정보 수정 및 추가
            roomCache[roomList[i].Name] = roomList[i];
        }
    }

    public GameObject itemFactory;

    void CreateRoomListUI()
    {
        foreach(RoomInfo info in roomCache.Values)
        {
            // 부모로 지정한 오브젝트의 자식으로 게임오브젝트 생성됨 (생성되는 오브젝트, 부모 오브젝트)
            GameObject go = Instantiate(itemFactory, trListContent);
            RoomItem item = go.GetComponent<RoomItem>();
            item.SetInfo(info);

            // roomItem 버튼이 클릭되면 호출되는 함수 등록
            // 1) 함수로 구현
            item.onClickAction = SetRoomName;

            //// 2) 람다식으로 구현
            //item.onClickAction = (string room) => {
            //    inputRoomName.text = room;
            //};

            // object 반환형을 사용할 자료형으로 형변환
            string desc = (string)info.CustomProperties["desc"];
            int map_id = (int)info.CustomProperties["map_id"];
            //print(desc + ", " + map_id);
        }
    }
    // 이전 Thumbnail의 id
    int preMapId = -1;

    void SetRoomName(string room, int map_id)
    {
        inputRoomName.text = room;

        // 만약 이전 맵 Thumbnail이 활성화 되어있다면
        if(preMapId > -1)
        {
            // 이전 맵 Thumbnail을 비활성화한다.
            //mapThumbs[preMapId].SetActive(false);
        }
        
        // 맵 Thumbnail 설정
        //mapThumbs[map_id].SetActive(true);

        // 이전 맵 id 저장
        preMapId = map_id;

    }
}
