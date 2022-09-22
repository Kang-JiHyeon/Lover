using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ���忡 ��, �������� ������ ���ְ� �ʹ�.
// ���尡 �, �� ��ҿ� ������ ƨ�ܳ��� �ʹ�.

// ���尡 �����̴� ���� ���� y���� ���� ���� ���ҽ�Ű�� �ʹ�.

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
