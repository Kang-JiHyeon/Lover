using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Realtime;


// 버튼의 Text를 변경하고 싶다.
public class RoomItem : MonoBehaviour
{
    // 내용 (방이름 (0/0))
    public Text roomInfo;

    // 설명
    public Text roomDesc;

    // 맵 id
    int map_id;

    // 클릭이 되었을 때 호출되는 함수를 가지고 있는 변수 --> 액션 사용!
    // Action<string>에는 string, int을 매개변수로 가지는 함수들만 담을 수 있다.
    public System.Action<string, int> onClickAction;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetInfo(string roomName, int curPlayer, byte maxPlayer)
    {
        // 게임 오브젝트의 이름을 roomName으로 한다.
        name = roomName;
        roomInfo.text = roomName + "(" + curPlayer + " / " + maxPlayer + ")";
    }

    public void SetInfo(RoomInfo info)
    {
        SetInfo((string)info.CustomProperties["room_name"], info.PlayerCount, info.MaxPlayers);

        // desc 설정
        roomDesc.text = (string)info.CustomProperties["desc"];

        // 맵id 설정
        map_id = (int)info.CustomProperties["map_id"];
    }

    public void OnClick()
    {
        // 만약 onClickAction이 null 이 아니라면
        if(onClickAction != null)
        {
            // onClickAction을 실행한다.
            // 액션에 담긴 함수에 name을 전달한다.
            onClickAction(name, map_id);
        }

        //// 1. InputRoomName 게임 오브젝트 찾는다.
        //GameObject go = GameObject.Find("InputRoomName");
        //// 2. InputField 컴포넌트 가져온다.
        //InputField inputField = go.GetComponent<InputField>();
        //// 3. text에 roomName을 넣는다.
        //inputField.text = name;
    }
}
