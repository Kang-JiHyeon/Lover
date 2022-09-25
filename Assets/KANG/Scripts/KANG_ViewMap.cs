using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class KANG_ViewMap : KANG_Machine
{
    public GameObject mapTV;
    
    // Start is called before the first frame update
    void Start()
    {
        mapTV.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // M키를 누르고 있으면 맵 지도를 띄우고 싶다.
    public override void ActionKey()
    {
        photonView.RPC("RpcMapSetActive", RpcTarget.All, true);
    }

    // M키를 떼면 맵 지도를 끄고 싶다.
    public override void ActionKeyUp()
    {
        photonView.RPC("RpcMapSetActive", RpcTarget.All, false);
    }

    [PunRPC]
    void RpcMapSetActive(bool valse)
    {
        mapTV.SetActive(valse);
    }

}
