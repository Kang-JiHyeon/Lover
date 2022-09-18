using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 우주선을 따라 움직이고 싶다.

public class KANG_CameraMove : MonoBehaviour
{
    public GameObject spaceship;
    public Vector3 offset;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(spaceship)
            transform.position = spaceship.transform.position + offset;
    }
}
