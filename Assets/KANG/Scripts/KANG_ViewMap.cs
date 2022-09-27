using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KANG_ViewMap : KANG_Machine
{
    public GameObject mapTV;
    
    // Start is called before the first frame update
    void Start()
    {
        mapTV.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // MŰ�� ������ ������ �� ������ ���� �ʹ�.
    public override void ActionKey()
    {
        mapTV.SetActive(true);
    }

    // MŰ�� ���� �� ������ ���� �ʹ�.
    public override void ActionKeyUp()
    {
        mapTV.SetActive(false);
    }
}
