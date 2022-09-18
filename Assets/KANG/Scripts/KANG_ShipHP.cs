using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 우주선의 체력을 관리하고 싶다.
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
