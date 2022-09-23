using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class KANG_ShipHit : MonoBehaviourPun
{
    KANG_Engine engine;

    // Start is called before the first frame update
    void Start()
    {
        engine = transform.Find("Engine").GetComponent<KANG_Engine>();    
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    void OnHit()
    {
        if (photonView.IsMine)
        {
            photonView.RPC("RpcOnHit", RpcTarget.All);
        }
    }

    [PunRPC]
    void RpcOnHit()
    {
        KANG_ShipHP.instance.HP--;

        print(KANG_ShipHP.instance.HP);
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Map"))
        {
            engine.bounceDir = transform.position - other.transform.position;
            engine.bounceDir.z = 0f;
            engine.bounceDir.Normalize();
            engine.isBounce = true;

            OnHit();

            print("Map Bounce, HP--");
        }

        // Enemy
        if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            OnHit();
            print("Enemy, HP--" + other.gameObject.name);
        }

        // EnemyBullet
        if (other.gameObject.layer == LayerMask.NameToLayer("EnemyBullet"))
        {
            OnHit();
            print("trigger Object : " + other.gameObject.name);
        }
    }
}
