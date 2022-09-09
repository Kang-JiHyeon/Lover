using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KIM_PlayerController : MonoBehaviour
{
    CharacterController cc;

    float yVelocity;
    public float gravity = -9.81f;
    public float jumpPower = 5f;
    public float speed = 5f;

    bool isLadder = false;

    bool canModule = false;
    // Start is called before the first frame update
    void Start()
    {
        cc = GetComponent<CharacterController>();   
    }

    // Update is called once per frame
    void Update()
    {
        if (isLadder)
        {
            yVelocity = -1f;

            if (Input.GetKey(KeyCode.A))
                cc.Move(-Vector3.right * speed * Time.deltaTime);
            else if (Input.GetKey(KeyCode.D))
                cc.Move(Vector3.right * speed * Time.deltaTime);
            else if (Input.GetKey(KeyCode.W))
                cc.Move(Vector3.up * speed * Time.deltaTime);
            else if (Input.GetKey(KeyCode.S))
                cc.Move(Vector3.down * speed * Time.deltaTime);
        }
        else
        {
            if (!cc.isGrounded)
                yVelocity += gravity * Time.deltaTime;
            else
                yVelocity = -1f;

            if (Input.GetKeyDown(KeyCode.Space) && cc.isGrounded)
                yVelocity = jumpPower;


            if (Input.GetKey(KeyCode.A))
                cc.Move(-Vector3.right * speed * Time.deltaTime);
            else if (Input.GetKey(KeyCode.D))
                cc.Move(Vector3.right * speed * Time.deltaTime);
            
            cc.Move(yVelocity * Vector3.up * Time.deltaTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Ladder"))
        {
            isLadder = true;
        }

        if (other.gameObject.layer == LayerMask.NameToLayer("Module"))
        {
            canModule = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Ladder"))
        {
            isLadder = false;
        }

        if (other.gameObject.layer == LayerMask.NameToLayer("Module"))
        {
            canModule = false;
        }
    }
}
