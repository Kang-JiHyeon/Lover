using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ������ �̵��ϰ� �ʹ�.
// �ε����� �������� �ʹ�.

public class KANG_Bullet : MonoBehaviour
{
    public float bulletSpeed = 10f;
    public float destroyTime = 10f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // P = P0 + vt
        transform.position += transform.up * bulletSpeed * Time.deltaTime;

        Destroy(gameObject, destroyTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        Destroy(gameObject);
    }
}
