using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 키 입력을 받아 회전하는 오브젝트를 관리하는 매니저

public class KANG_RotationManager : MonoBehaviour
{
    public static KANG_RotationManager instance;

    public bool isShieldControll = false;
    public bool isEngineControll = false;
    public bool isTurretControll = false;

    public GameObject Shield;
    public GameObject Engine;
    //public GameObject Turret;


    private void Awake()
    {
        if (instance == false)
            instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // 방어막 오브젝트가 있고 조종할 수 있을 때
        if(Shield && isShieldControll)
        {
            Shield.GetComponent<KANG_InputRotate>().enabled = true;
        }
        else
        {
            Shield.GetComponent<KANG_InputRotate>().enabled = false;
        }

        // 엔진 오브젝트가 있고 조종할 수 있을 때
        if (Engine && isEngineControll)
        {
            Engine.GetComponent<KANG_InputRotate>().enabled = true;
        }
        else
        {
            Engine.GetComponent<KANG_InputRotate>().enabled = false;
        }

        //// 공격 오브젝트가 있고 조종할 수 있을 때
        //if (Turret && isTurretControll)
        //{
        //    Turret.GetComponent<KANG_TurretRotate>().enabled = true;
        //}
        //else
        //{
        //    Turret.GetComponent<KANG_TurretRotate>().enabled = false;
        //}

    }
}
