using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ���ּ��� ü���� �����ϰ� �ʹ�.
// 
public class KANG_ShipHP : MonoBehaviour
{
    public static KANG_ShipHP instance;

    public int maxHP = 10;
    int hp;

    public int HP
    {
        get
        {
            return hp;
        }

        set
        {
            hp = value;

            print("ShipHP : "+hp);
            
            if(hp <= 0)
            {
                print("Ship Die");

                //Destroy(gameObject);
            }
        }


    }

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        hp = maxHP;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
