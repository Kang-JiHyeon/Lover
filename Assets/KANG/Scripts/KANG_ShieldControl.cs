using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KANG_ShieldControl : MonoBehaviour
{
    public KANG_InputRotate shield;

    // Start is called before the first frame update
    void Start()
    {
        //shield = GetComponent<KANG_InputRotate>();
    }

    // Update is called once per frame
    void Update()
    {

        
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            shield.isControl = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            shield.isControl = false;
        }
    }
}
