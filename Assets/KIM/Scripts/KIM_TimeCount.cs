using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class KIM_TimeCount : MonoBehaviourPun
{
    Text timeText;

    // Start is called before the first frame update
    void Start()
    {
        timeText = GetComponent<Text>();

        if (PhotonNetwork.IsMasterClient)
            photonView.RPC("RPCSetText", RpcTarget.All, ((int)KIM_GameManager.Instance.GameTime / 60).ToString() + ":" + ((int)KIM_GameManager.Instance.GameTime % 60).ToString());
    }

    [PunRPC]
    void RPCSetText(string s)
    {
        timeText.text = s;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
            Application.Quit();
    }
}
