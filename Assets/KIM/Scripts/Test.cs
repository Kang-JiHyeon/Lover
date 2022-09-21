using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Test : MonoBehaviourPun
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        photonView.RPC("move", RpcTarget.All, h * Vector3.right + v * Vector3.up);
    }

    [PunRPC]
    void move(Vector3 dir)
    {
        transform.position += dir.normalized * Time.deltaTime;
    }
}
