using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class KIM_Engine : MonoBehaviourPun
{
    float currentTime = 0;
    public GameObject engineEffect;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CreateEffect()
    {
        photonView.RPC("RPCEffect", RpcTarget.All, Time.deltaTime);
    }

    [PunRPC]
    void RPCEffect(float deltaTime)
    {
        currentTime += deltaTime;

        if (currentTime > 0.4f)
        {
            GameObject effect = Instantiate(engineEffect);
            effect.transform.position = transform.Find("EngineMachine").transform.Find("EngineOrangeBalls_Tex").position +
                transform.Find("EngineMachine").transform.Find("EngineOrangeBalls_Tex").up * 0.5f;
            effect.transform.up = transform.Find("EngineMachine").up;
            iTween.ScaleTo(effect, iTween.Hash("x", 3f, "y", 3f, "z", 3f, "time", 1.5f, "easetype", iTween.EaseType.easeOutQuart));
            StartCoroutine(OnColor(effect));
            Destroy(effect, 1.5f);
            currentTime = 0;
        }
    }

    IEnumerator OnColor(GameObject effect)
    {
        float coCurrentTime = 0;
        Color col = effect.GetComponent<SpriteRenderer>().color;
        
        while (coCurrentTime < 1.4f)
        {
            coCurrentTime += Time.deltaTime;
            col.a = Mathf.Lerp(col.a, 0, Time.deltaTime * 1.5f);
            effect.GetComponent<SpriteRenderer>().color = col;
            yield return null;
        }
    }
}
