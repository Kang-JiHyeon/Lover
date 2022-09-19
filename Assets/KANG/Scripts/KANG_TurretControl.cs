using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ������ Ʈ���Ű� üũ�Ǹ� ����� Turret�� isControl�� �����Ű�� �ʹ�.
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
