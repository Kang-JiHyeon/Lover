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
            effect.transform.position = transform.Find("EngineMachine").transform.Find("EngineOrangeBalls_Tex").position;
            effect.transform.up = transform.Find("EngineMachine").up;
            iTween.ScaleTo(effect, iTween.Hash("x", 3f, "y", 3f, "z", 3f, "time", 1.5f, "easetype", iTween.EaseType.easeOutQuart));
            Destroy(effect, 1.5f);
            currentTime = 0;
        }
    }
}
