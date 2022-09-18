using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ���ּ��� ü���� �����ϰ� �ʹ�.
// ���ּ��� ü���� ���� ������ �Ž� �����ϰ� �ʹ�.
public class KANG_ShipHP : MonoBehaviour
{
    public GameObject DeathSprite;


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
                hp = 0;
                DeathSprite.SetActive(true);
            }
            else
            {
                StopAllCoroutines();
                //StartCoroutine(IeDeath());
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

    IEnumerator IeDeath()
    {
        DeathSprite.SetActive(true);
        yield return new WaitForSeconds(0.2f);

        DeathSprite.SetActive(false);
        yield return new WaitForSeconds(0.2f);
    }
}
