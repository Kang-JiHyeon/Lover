using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 우주선 중심을 기준으로 회전하고 싶다.
// 필요속성: 우주선 중심, 회전 속도, 회전 방향

public class KANG_YamatoRotate : MonoBehaviour
{
    // 회전중심
    public Transform spaceship;
    // 회전속도
    public float rotSpeed = 20f;
    // 회전방향
    public float rotDir = 1f;

    // yamato texture
    public List<Transform> rotObjects;


    // Start is called before the first frame update
    void Start()
    {
        // 회전 중심 = 우주선의 중심
        spaceship = GameObject.Find("Spaceship").transform;

        for(int i = 0; i<transform.childCount; i++)
        {
            rotObjects.Add(transform.GetChild(i));
        }
    }

    // Update is called once per frame
    public virtual void Update()
    {
        if (!spaceship) return;

        Rotate();
    }

    public virtual void Rotate()
    {
        // yamato texture들을 우주선 중심을 기준으로 회전하고 싶다.
        for (int i = 0; i< rotObjects.Count; i++)
        {
            rotObjects[i].RotateAround(spaceship.position, -spaceship.forward, rotDir * rotSpeed * Time.deltaTime);
        }
    }
}
