using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// ���ּ��� ü���� �����ϰ� �ʹ�.
// ���ּ��� ü���� ���� ������ �Ž� �����ϰ� �ʹ�.
// ���ּ� ü���� ���� ������ Slider�� ���� �����ϰ� �ʹ�.
public class KANG_ShipHP : MonoBehaviour
{
    public GameObject DeathSprite;
    Slider HPBar;
    GameObject fillArea;

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

            HPBar.value--;

            if(hp <= 0)
            {
                print("Ship Die");
                hp = 0;
                DeathSprite.SetActive(true);
                isDie = true;

                HPBar.value = 0f;
                fillArea.SetActive(false);


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
        HPBar = GameObject.Find("Slider").GetComponent<Slider>();
        fillArea = HPBar.gameObject.transform.GetChild(1).gameObject;

        hp = maxHP;
        HPBar.maxValue = maxHP;
        HPBar.value = maxHP;

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

        if(curTime > intervalTime + 0.1f)
        {
            DeathSprite.SetActive(false);
            curTime = 0f;
        }
        else if(curTime > intervalTime)
        {
            DeathSprite.SetActive(true);
        }
    }


    IEnumerator IeDeath()   
    {
        DeathSprite.SetActive(false);
        yield return null;

        DeathSprite.SetActive(true);
        yield return new WaitForSeconds(0.1f);

        DeathSprite.SetActive(false);
        yield return null;
    }
}
