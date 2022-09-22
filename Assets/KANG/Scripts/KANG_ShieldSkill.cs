using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 쉴드에 적, 적공격이 닿으면 없애고 싶다.
// 쉴드가 운석, 맵 요소에 닿으면 튕겨나고 싶다.

// 쉴드가 움직이는 중일 때는 y값을 일정 간격 감소시키고 싶다.

public class KANG_ShieldSkill : MonoBehaviour
{

    public KANG_Engine engine;

    // Start is called before the first frame update
    void Start()
    {

        
    }

    // Update is called once per frame
    void Update()
    {
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("EnemyBullet"))
        {
            Destroy(other.gameObject);
        }


        if (other.gameObject.CompareTag("Map"))
        {
            engine.bounceDir = transform.position - other.transform.position;
            engine.bounceDir.z = 0f;
            engine.bounceDir.Normalize();
            engine.isBounce = true;

            print("Shield Wave Bounce");

        }
    }
}
