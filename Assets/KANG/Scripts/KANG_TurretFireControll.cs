using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TurretCannon의 스크립트를 제어하고 싶다.

public class KANG_TurretFireControll : MonoBehaviour
{
    // 자식 TurretCannon 리스트
    public List<KANG_TurretFire> turretCannons;
    public bool isFire = false;

    // Start is called before the first frame update
    void Start()
    {
        for(int i=0; i<transform.childCount; i++)
        {
            turretCannons.Add(transform.GetChild(i).GetComponent<KANG_TurretFire>());
            turretCannons[i].enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
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
