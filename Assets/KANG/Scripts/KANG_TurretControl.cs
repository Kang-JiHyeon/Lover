using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 조정석 트리거가 체크되면 연결된 Turret의 isControl을 변경시키고 싶다.
public class KANG_TurretControl : MonoBehaviour
{
    KIM_PlayerController pc;
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
        if (other.gameObject.TryGetComponent<KIM_PlayerController>(out pc) && pc.IsModule)
        {
            target.GetComponent<KANG_TurretRotate2>().isControl = true;
            target.GetComponent<KANG_TurretRotate2>().is2P = pc.is2P;

        }
        else if (other.gameObject.TryGetComponent<KIM_PlayerController>(out pc) && !pc.IsModule)
        {
            target.GetComponent<KANG_TurretRotate2>().isControl = true;
            target.GetComponent<KANG_TurretRotate2>().is2P = pc.is2P;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.TryGetComponent<KIM_PlayerController>(out pc) && !pc.IsModule)
        {
            target.GetComponent<KANG_TurretRotate2>().isControl = false;
        }
    }
}
