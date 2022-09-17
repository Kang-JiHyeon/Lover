using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ������ Ʈ���Ű� üũ�Ǹ� ����� Turret�� isControl�� �����Ű�� �ʹ�.
public class KANG_TurretControl : MonoBehaviour
{
    public GameObject target;

    // Start is called before the first frame update
    void Start()
    {
            
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            target.GetComponent<KANG_TurretRotate2>().isControl = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            target.GetComponent<KANG_TurretRotate2>().isControl = false;
        }
    }
}
