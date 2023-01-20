using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ������ �̵��ϰ� �ʹ�.
// �ε����� �������� �ʹ�.

public class KANG_Bullet : MonoBehaviourPun
{
    public GameObject turret;
    public float bulletSpeed = 10f;
    public float destroyTime = 5f;

    // Start is called before the first frame update
    void Start()
    {
        turret = transform.parent.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        // P = P0 + vt
        transform.position += transform.up * bulletSpeed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(turret != null)
        {
            KANG_ObjectPooling pool = turret.GetComponent<KANG_ObjectPooling>();
            pool.goPool.Add(gameObject);
            gameObject.SetActive(false);
        }
    }
}
