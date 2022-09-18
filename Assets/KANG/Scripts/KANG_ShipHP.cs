using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// ���ּ��� ü���� �����ϰ� �ʹ�.
// ���ּ��� ü���� ���� ������ �Ž� �����ϰ� �ʹ�.
public class KANG_ShipHP : MonoBehaviour
{
    public GameObject DeathSprite;


    public static KANG_ShipHP instance;
    public int maxHP = 10;
    int hp;
    public float warningHP = 5f;

    public float intervalTime = 0.5f;
    float curTime = 0f;
    bool isDie = false;
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
                isDie = true;
            }
            else if(hp > warningHP)
            {
                if(isDie == false)
                {
                    StopAllCoroutines();
                    StartCoroutine(IeDeath());

                }
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
        curTime = intervalTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (hp <= warningHP)
        {
            Warning();
        }

    }

    // ���� �ð� �������� ���ӿ�����Ʈ�� ���� �״� �ϰ� �ʹ�.


    void Warning()
    {
        curTime += Time.deltaTime;

        if(curTime > intervalTime * 2f)
        {
            DeathSprite.SetActive(true);
            curTime = 0f;
        }
        else if(curTime > intervalTime)
        {
            DeathSprite.SetActive(false);
        }
    }


    IEnumerator IeDeath()
    {
        DeathSprite.SetActive(true);
        yield return new WaitForSeconds(0.2f);

        DeathSprite.SetActive(false);
        yield return new WaitForSeconds(0.2f);
    }
}
