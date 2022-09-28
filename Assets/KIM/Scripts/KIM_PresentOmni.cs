using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class KIM_PresentOmni : MonoBehaviourPun
{
    AudioSource source;
    public AudioClip clip;
    GameObject ship;
    public Sprite jewel;
    SpriteRenderer render;
    bool unPacked;
    // Start is called before the first frame update
    void Awake()
    {
        source = GetComponent<AudioSource>();
        ship = GameObject.Find("Spaceship");
        render = GetComponentInChildren<SpriteRenderer>();
        transform.SetParent(ship.transform);
    }

    // Update is called once per frame
    void Update()
    {
        if (!unPacked)
            transform.localPosition = Vector3.zero + Vector3.left * 0.5f;
        else
            transform.localPosition = Vector3.zero + Vector3.up;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            photonView.RPC("RPCSetJewelSpr", RpcTarget.All);
            transform.SetParent(other.transform);
        }
    }

    [PunRPC]
    void RPCSetJewelSpr()
    {
        render.sprite = jewel;
        unPacked = true;
    }
}
