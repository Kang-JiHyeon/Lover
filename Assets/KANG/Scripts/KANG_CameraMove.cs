using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ���ּ��� ���� �����̰� �ʹ�.

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
        transform.position = spaceship.transform.position + offset;
    }
}
