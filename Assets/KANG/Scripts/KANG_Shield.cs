using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 쉴드에 적, 적공격이 닿으면 없애고 싶다.
// 쉴드가 운석, 맵 요소에 닿으면 튕겨나고 싶다.

// 쉴드가 움직이는 중일 때는 y값을 일정 간격 감소시키고 싶다.

public class KANG_Shield : MonoBehaviour
{
    public float upDownSpeed = 5f;
    public float upDownValue = 0.4f;

    Vector3 targetPos;
    Vector3 originPos;

    public KANG_InputRotate shield;
    
    // Start is called before the first frame update
    void Start()
    {
        originPos = transform.localPosition;
        targetPos = transform.localPosition;
        targetPos.y = transform.localPosition.y - upDownValue;

        
    }

    // Update is called once per frame
    void Update()
    {
        // 쉴드가 움직이는 중이라면
        if (shield.isShieldMove)
        {
            LerpMovePos(targetPos);
        }

        else
        {
            LerpMovePos(originPos);
        }
    }

    void LerpMovePos(Vector3 pos)
    {
        transform.localPosition = Vector3.Lerp(transform.localPosition, pos, Time.deltaTime * upDownSpeed);

        if(Mathf.Abs((transform.localPosition - pos).magnitude) < 0.1f)
        {
            transform.localPosition = pos;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("EnemyBullet"))
        {
            Destroy(other.gameObject);
        }

        // 에너미라면 밀쳐내고 싶다.

        if (other.gameObject.CompareTag("Map"))
        {
            KANG_ShipMove.instance.bounceDir = transform.position - other.transform.position;
            KANG_ShipMove.instance.bounceDir.z = 0f;
            KANG_ShipMove.instance.bounceDir.Normalize();
            KANG_ShipMove.instance.isBounce = true;

            print("Shield Wave Bounce");

        }
    }
}
