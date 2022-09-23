using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ������ �̵��ϰ� �ʹ�.
// �ε����� �������� �ʹ�.

public class KANG_Bullet : MonoBehaviourPun
{
    public float bulletSpeed = 10f;
    public float destroyTime = 5f;

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, destroyTime);
    }

    // Update is called once per frame
    void Update()
    {
        // P = P0 + vt
        transform.position += transform.up * bulletSpeed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        Destroy(gameObject);
    }
}
