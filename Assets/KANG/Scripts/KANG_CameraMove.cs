using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 우주선을 따라 움직이고 싶다.

public class KANG_CameraMove : MonoBehaviour
{
    GameObject warp;

    public GameObject spaceship;
    public Vector3 offset;
    bool unLock = false;
    public bool UnLock
    {
        get { return unLock; }
        set
        {
            if (unLock != value && value == true)
            {
                StartCoroutine("OnUnLock");
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        warp = GameObject.Find("Warp");
    }

    // Update is called once per frame
    void Update()
    {
        if(spaceship && !unLock)
            transform.position = spaceship.transform.position + offset;
    }

    IEnumerator OnUnLock()
    {
        float currentTime = 0f;
        while (currentTime < 5f)
        {
            currentTime += Time.deltaTime;
            transform.position = Vector3.Lerp(transform.position, warp.transform.position + offset, Time.deltaTime);
            yield return null;
        }
        while (currentTime < 10f)
        {
            currentTime += Time.deltaTime;
            transform.position = Vector3.Lerp(transform.position, spaceship.transform.position + offset, Time.deltaTime);
            yield return null;
        }
        unLock = false;
    }
}
