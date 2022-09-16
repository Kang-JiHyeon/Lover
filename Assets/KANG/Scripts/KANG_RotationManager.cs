using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Ű �Է��� �޾� ȸ���ϴ� ������Ʈ�� �����ϴ� �Ŵ���

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
        // �� ������Ʈ�� �ְ� ������ �� ���� ��
        if(Shield && isShieldControll)
        {
            Shield.GetComponent<KANG_InputRotate>().enabled = true;
        }
        else
        {
            Shield.GetComponent<KANG_InputRotate>().enabled = false;
        }

        // ���� ������Ʈ�� �ְ� ������ �� ���� ��
        if (Engine && isEngineControll)
        {
            Engine.GetComponent<KANG_InputRotate>().enabled = true;
        }
        else
        {
            Engine.GetComponent<KANG_InputRotate>().enabled = false;
        }

        //// ���� ������Ʈ�� �ְ� ������ �� ���� ��
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
