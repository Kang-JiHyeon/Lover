using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ÃÑ¾ËÀ» ½î°í ½Í´Ù.

public class KANG_Turret : MonoBehaviour
{
    public GameObject bulletFactory;
    public float createTime = 0.5f;
    float currentTime = 0f;

    public bool isShoot = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isShoot)
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
}
