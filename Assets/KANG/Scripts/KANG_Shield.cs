using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ���忡 ��, �������� ������ ���ְ� �ʹ�.
// ���尡 �, �� ��ҿ� ������ ƨ�ܳ��� �ʹ�.

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
        


        // �ε��� ������ �ݴ� �������� �̵���Ű�� �ʹ�.
        // �ε����� �� ���� ������ �˰� �ʹ�.

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
