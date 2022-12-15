using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 앞으로 이동하고 싶다.
// 부딪히면 없어지고 싶다.

public class KANG_EngineBeam : MonoBehaviourPun
{
    public float bulletSpeed = 60f;
    public float destroyTime = 1f;
    public GameObject effectFactory;
    public float createTime = 0.02f;
    float curTime = 0f;

    // Start is called before the first frame update
    void Start()
    {
        curTime = createTime;
        Destroy(gameObject, destroyTime);
    }

    // Update is called once per frame
    void Update()
    {
        // P = P0 + vt
        transform.position += transform.up * bulletSpeed * Time.deltaTime;

        curTime += Time.deltaTime;
        if (curTime > createTime)
        {
            GameObject effect = Instantiate(effectFactory);
            effect.transform.position = transform.position;
            curTime = 0f;
        }
    }

    private void OnTriggerEnter(Collider other) 
    {
        Destroy(gameObject);
    }

    void CreateEffect()
    {
        //photonView.RPC("RpcCreateEffect", RpcTarget.All, Time.deltaTime);
    }

    void RpcCreateEffect(float deltaTime) 
    {
        


    }
}
