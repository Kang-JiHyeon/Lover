using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KIM_InsectRotate : MonoBehaviour
{
    public float rotSpeed = 5f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(rotSpeed, 0, 0);
    }
}
