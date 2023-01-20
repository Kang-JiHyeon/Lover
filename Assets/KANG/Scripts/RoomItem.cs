using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Realtime;


// ��ư�� Text�� �����ϰ� �ʹ�.
public class RoomItem : MonoBehaviour
{
    // ���� (���̸� (0/0))
    public Text roomInfo;

    // ����
    public Text roomDesc;

    // �� id
    int map_id;

    // Ŭ���� �Ǿ��� �� ȣ��Ǵ� �Լ��� ������ �ִ� ���� --> �׼� ���!
    // Action<string>���� string, int�� �Ű������� ������ �Լ��鸸 ���� �� �ִ�.
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
        // ���� ������Ʈ�� �̸��� roomName���� �Ѵ�.
        name = roomName;
        roomInfo.text = roomName + "(" + curPlayer + " / " + maxPlayer + ")";
    }

    public void SetInfo(RoomInfo info)
    {
        SetInfo((string)info.CustomProperties["room_name"], info.PlayerCount, info.MaxPlayers);

        // desc ����
        roomDesc.text = (string)info.CustomProperties["desc"];

        // ��id ����
        map_id = (int)info.CustomProperties["map_id"];
    }

    public void OnClick()
    {
        // ���� onClickAction�� null �� �ƴ϶��
        if(onClickAction != null)
        {
            // onClickAction�� �����Ѵ�.
            // �׼ǿ� ��� �Լ��� name�� �����Ѵ�.
            onClickAction(name, map_id);
        }

        //// 1. InputRoomName ���� ������Ʈ ã�´�.
        //GameObject go = GameObject.Find("InputRoomName");
        //// 2. InputField ������Ʈ �����´�.
        //InputField inputField = go.GetComponent<InputField>();
        //// 3. text�� roomName�� �ִ´�.
        //inputField.text = name;
    }
}
