using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KANG_TestPlayer : MonoBehaviour
{
    GameObject target;
    KANG_Machine machine;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.M))
        {
            if (target)
            {
                machine = target.GetComponent<KANG_Machine>();
                machine.ActionKey();
            }
        }

        if (Input.GetKey(KeyCode.UpArrow))
        {
            if (target)
            {
                machine = target.GetComponent<KANG_Machine>();
                machine.UpKey();
            }
        }


        if (Input.GetKey(KeyCode.DownArrow))
        {
            if (target)
            {
                machine = target.GetComponent<KANG_Machine>();
                machine.DownKey();
            }
        }


        if (Input.GetKey(KeyCode.LeftArrow))
        {
            if (target)
            {
                machine = target.GetComponent<KANG_Machine>();
                machine.LeftKey();
            }
        }


        if (Input.GetKey(KeyCode.RightArrow))
        {
            if (target)
            {
                machine = target.GetComponent<KANG_Machine>();
                machine.RightKey();
            }
        }

        if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow))
        {
            if (target)
            {
                machine = target.GetComponent<KANG_Machine>();
                machine.ArrowKey();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Module"))
        {
            print(other.gameObject.name);
            target = other.gameObject;
        }
    }
}
