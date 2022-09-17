using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 조정석 트리거가 체크되면 연결된 Turret의 isControl을 변경시키고 싶다.
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
