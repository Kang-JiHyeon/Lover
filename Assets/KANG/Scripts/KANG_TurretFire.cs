using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �Ѿ��� ��� �ʹ�.
// TurretCannon�� �ѱ� ��ġ���� �Ѿ��� �߻��ϰ� �ʹ�.
public class KANG_TurretFire : MonoBehaviour
{
    public GameObject bulletFactory;
    public float createTime = 0.5f;
    float currentTime = 0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame

    void Update()
    {
        currentTime += Time.deltaTime;

        if(currentTime > createTime)
        {
            GameObject bullet = Instantiate(bulletFactory);
            bullet.transform.position = transform.position;
            bullet.transform.up = transform.up;
            currentTime = 0f;
        }
    }
}
