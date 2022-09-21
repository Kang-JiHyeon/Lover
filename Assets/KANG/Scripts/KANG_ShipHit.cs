using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KANG_ShipHit : MonoBehaviour
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


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Map"))
        {
            engine.bounceDir = transform.position - other.transform.position;
            engine.bounceDir.z = 0f;
            engine.bounceDir.Normalize();
            engine.isBounce = true;

            KANG_ShipHP.instance.HP--;

            print("Map Bounce, HP--");
        }

        // Enemy
        if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            KANG_ShipHP.instance.HP--;
            print("Enemy, HP--" + other.gameObject.name);
        }

        // EnemyBullet
        if (other.gameObject.layer == LayerMask.NameToLayer("EnemyBullet"))
        {
            print("trigger Object : " + other.gameObject.name);
            KANG_ShipHP.instance.HP--;
        }
    }
}
