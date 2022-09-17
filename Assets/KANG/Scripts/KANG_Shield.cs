using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 쉴드에 적, 적공격이 닿으면 없애고 싶다.
// 쉴드가 운석, 맵 요소에 닿으면 튕겨나고 싶다.

public class KANG_Shield : MonoBehaviour
{
    public bool isBounce = false;
    Vector3 bounceDir;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        


        // 부딪힌 방향의 반대 방향으로 이동시키고 싶다.
        // 부딪혔을 때 진행 방향을 알고 싶다.

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            Destroy(other.gameObject);
        }

        if (other.gameObject.CompareTag("Map"))
        {
            bounceDir = transform.position - other.transform.position;

            isBounce = true;

        }
    }
}
