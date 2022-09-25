using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// * 우주선의 체력을 관리하고 싶다.
// 우주선의 체력이 깎일 때마다 매쉬 변경하고 싶다.
// 우주선 체력이 깎일 때마다 Slider의 값을 변경하고 싶다.
// 피격당했을 때 Red Image의 alpha 값을 변경하고 싶다.


public class KANG_ShipHP : MonoBehaviour
{
    public GameObject DeathSprite;
    Slider HPBar;
    GameObject fillArea;
    public Image hitImage;
    Color hitColor;


    public static KANG_ShipHP instance;
    public int maxHP = 10;
    int hp;
    public float warningHP = 5f;

    public float intervalTime = 0.5f;
    float curTime = 0f;

    //public float hitTime = 0.2f;
    //float curHitTime = 0f;

    bool isDamage = false;
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
            hitColor.a = 0.1f;
            isDamage = true;


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

        hitColor = hitImage.color;
    }

    // Update is called once per frame
    void Update()
    {
        if (hp <= warningHP)
        {
            Warning();
        }

        if (isDamage)
        {
            Damage();
        }

    }

    // 일정 시간 간격으로 게임오브젝트를 껐다 켰다 하고 싶다.
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

    

    void Damage()
    {
        hitColor.a = Mathf.Lerp(hitColor.a, 0, Time.deltaTime);

        if (hitColor.a < 0.001f)
        {
            hitColor.a = 0f;
            isDamage = false;
        }

        hitImage.color = hitColor;
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
