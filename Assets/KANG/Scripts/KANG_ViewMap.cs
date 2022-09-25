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

    // MŰ�� ������ ������ �� ������ ���� �ʹ�.
    public override void ActionKey()
    {
        photonView.RPC("RpcMapSetActive", RpcTarget.All, true);
    }

    // MŰ�� ���� �� ������ ���� �ʹ�.
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
