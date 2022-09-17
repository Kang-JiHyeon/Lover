using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TurretCannon의 스크립트를 제어하고 싶다.
public class KANG_TurretFireControl : MonoBehaviour
{
    KANG_TurretRotate2 turret;

    // 자식 TurretCannon 리스트
    public List<KANG_TurretFire> turretCannons;
    public bool isFire = false;

    // Start is called before the first frame update
    void Start()
    {
        turret = GetComponent<KANG_TurretRotate2>();

        for (int i=0; i<transform.childCount; i++)
        {
            turretCannons.Add(transform.GetChild(i).GetComponent<KANG_TurretFire>());
            turretCannons[i].enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (turret.isControl)
        {
            if (Input.GetKeyDown(KeyCode.M))
            {
                isFire = true;
            }

            if (Input.GetKeyUp(KeyCode.M))
            {
                isFire = false;
            }
        }
        else
        {
            isFire = false;
        }


        if (isFire)
        {
            for (int i = 0; i < turretCannons.Count; i++)
            {
                turretCannons[i].enabled = true;
            }
        }
        else
        {
            for (int i = 0; i < turretCannons.Count; i++)
            {
                turretCannons[i].enabled = false;
            }
        }
    }
}
