using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KANG_TestPlayer : MonoBehaviour
{
    GameObject target;
    KANG_Machine machine;
    bool isArrowDown = false;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        if (target)
        {
            if (Input.GetKey(KeyCode.M))
            {
                
                    machine = target.GetComponent<KANG_Machine>();
                    machine.ActionKey();
                
            }
            else if (Input.GetKeyUp(KeyCode.M))
            {
                
                    machine = target.GetComponent<KANG_Machine>();
                    machine.ActionKeyUp();
                
            }


            if (Input.GetKey(KeyCode.UpArrow))
            {
                
                    machine = target.GetComponent<KANG_Machine>();
                    machine.UpKey();
                
            }


            if (Input.GetKey(KeyCode.DownArrow))
            {
                
                    machine = target.GetComponent<KANG_Machine>();
                    machine.DownKey();
                
            }


            if (Input.GetKey(KeyCode.LeftArrow))
            {
               
                    machine = target.GetComponent<KANG_Machine>();
                    machine.LeftKey();
                
            }


            if (Input.GetKey(KeyCode.RightArrow))
            {
                
                    machine = target.GetComponent<KANG_Machine>();
                    machine.RightKey();
                
            }


            if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow))
            {
                
                    machine = target.GetComponent<KANG_Machine>();
                    machine.ArrowKey();
                    print("machine ArrowKeyUp: " + target.name);
                
            }

            if ((Input.GetKeyUp(KeyCode.UpArrow) && !(Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow)))
                || (Input.GetKeyUp(KeyCode.DownArrow) && !(Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow)))
                || (Input.GetKeyUp(KeyCode.LeftArrow) && !(Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.RightArrow)))
                || (Input.GetKeyUp(KeyCode.RightArrow) && !(Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.LeftArrow))))
            {
                
                    machine = target.GetComponent<KANG_Machine>();
                    machine.ArrowKeyUp();
                    print("machine ArrowKeyUp: " + target.name);
                
            }
        }
        //if ((Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow)) == false)
        //{
        //    if (target)
        //    {
        //        machine = target.GetComponent<KANG_Machine>();
        //        machine.ArrowKeyUp();
        //        isArrowDown = false;
        //    }
        //}

        // if ((Input.GetKey(KeyCode.UpArrow) || Input.GetKeyUp(KeyCode.DownArrow) || Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.RightArrow)) == false)
        //{

        //}

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
