using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ���忡 ��, �������� ������ ���ְ� �ʹ�.
// ���尡 �, �� ��ҿ� ������ ƨ�ܳ��� �ʹ�.

public class KANG_Shield : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // �ε��� ������ �ݴ� �������� �̵���Ű�� �ʹ�.
        // �ε����� �� ���� ������ �˰� �ʹ�.

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("EnemyBullet"))
        {
            Destroy(other.gameObject);
        }

        // ���ʹ̶�� ���ĳ��� �ʹ�.

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
