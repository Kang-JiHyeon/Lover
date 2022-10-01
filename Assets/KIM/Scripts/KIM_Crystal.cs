using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KIM_Crystal : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
         
    }

    // Update is called once per frame
    void Update()
    {
        transform.localPosition = new Vector3(0, 1f, 0);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Module"))
        {
            //맵에 부딪힐때
            if (other.gameObject.name.Contains("Map"))
                return;
            //모듈에 부딪힐 때 스프라이트 변경시키고 파괴됨
            if (!gameObject.name.Contains("CrowBar"))
            {
                other.transform.Find("CrystalPot").GetComponent<SpriteRenderer>().sprite = GetComponentInChildren<SpriteRenderer>().sprite;
                Destroy(gameObject);
            }
            //내가 크로우바라면 모듈에 부딪힐 때 스프라이트 변경시키고 자신의 스프라이트도 변경, 동시에 이름도 변경
            else
            {
                GetComponentInChildren<SpriteRenderer>().sprite = other.transform.Find("CrystalPot").GetComponent<SpriteRenderer>().sprite;
                gameObject.name = GetComponentInChildren<SpriteRenderer>().sprite.name;
                other.transform.Find("CrystalPot").GetComponent<SpriteRenderer>().sprite = null;
            }
        }
    }
}
