using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 우주선을 따라 움직이고 싶다.

public class KANG_CameraMove : MonoBehaviour
{
    GameObject warp;
    Camera postCam;

    public float shakeTime = 0.5f;
    public float shakeSpeed = 100f;
    public float shakePos = 1.0f;

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
            unLock = value;
        }
    }
    bool shake = false;

    bool captured = false;
    public bool Captured
    {
        get { return captured; }
        set { captured = value; }
    }
    public Vector3 capPos;
    public float capSize;

    // Start is called before the first frame update
    void Start()
    {
        postCam = transform.Find("Post Camera").GetComponent<Camera>();
        warp = GameObject.Find("Warp");
    }

    // Update is called once per frame
    void Update()
    {
        if (spaceship && !unLock && !shake && !Captured)
        {
            transform.position = Vector3.Lerp(transform.position, spaceship.transform.position + offset, Time.deltaTime * 3f);
            Camera.main.orthographicSize = Mathf.Lerp(Camera.main.orthographicSize, 9, Time.deltaTime * 0.5f);
            postCam.orthographicSize = Mathf.Lerp(postCam.orthographicSize, 9, Time.deltaTime * 0.5f);
        }
        else if (Captured && !unLock)
        {
            transform.position = Vector3.Lerp(transform.position, capPos + offset, Time.deltaTime * 0.5f);
            Camera.main.orthographicSize = Mathf.Lerp(Camera.main.orthographicSize, capSize, Time.deltaTime * 0.5f);
            postCam.orthographicSize = Mathf.Lerp(postCam.orthographicSize, capSize, Time.deltaTime * 0.5f);
        }
    }

    public void CamShake()
    {
        if (!unLock)
        {
            StopCoroutine("OnCamShake");
            StartCoroutine("OnCamShake");
        }
    }

    IEnumerator OnCamShake()
    {
        shake = true;
        float currentTime = 0f;
        while (currentTime < shakeTime)
        {
            currentTime += Time.deltaTime;
            if (!Captured)
                transform.position = Vector3.Lerp(transform.position, spaceship.transform.position + offset, Time.deltaTime * 3f);
            else
                transform.position = Vector3.Lerp(transform.position, capPos + offset, Time.deltaTime * 0.5f);
            transform.position += Vector3.right * Mathf.Sin(shakeSpeed * currentTime) * shakePos + Vector3.up * Mathf.Sin(shakeSpeed * currentTime) * shakePos; 
            yield return null;
        }
        shake = false;
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
