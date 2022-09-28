using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class KIM_PresentOmni : MonoBehaviourPun
{
    public Sprite jewel;
    SpriteRenderer render;
    // Start is called before the first frame update
    void Awake()
    {
        render = GetComponentInChildren<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            photonView.RPC("RPCSetJewelSpr", RpcTarget.All);
            //transform.SetParent(other.transform);
        }
    }

    [PunRPC]
    void RPCSetJewelSpr()
    {
        render.sprite = jewel;
    }
}
