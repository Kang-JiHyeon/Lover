using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KIM_PlayerController_2 : MonoBehaviour
{
    public float speed = 5f;

    bool isLadder = false;
    bool isModule = false;
    public bool IsModule
    {
        get { return isModule; }
    }
    bool canModule = false;

    Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isLadder)
        {
            // ���÷� �����̱� ���� ���ּ��� �̵��ϴ� �������� �켱 �̵�

            if (Input.GetKey(KeyCode.LeftArrow))
            {
                transform.localPosition += Vector3.left * speed * Time.deltaTime;
            }
            else if (Input.GetKey(KeyCode.RightArrow))
            {
                transform.localPosition += Vector3.right * speed * Time.deltaTime;
            }
            else if (Input.GetKey(KeyCode.UpArrow))
            {
                transform.localPosition += Vector3.up * speed * Time.deltaTime;
            }
            else if (Input.GetKey(KeyCode.DownArrow))
            {
                transform.localPosition += Vector3.down * speed * Time.deltaTime;
            }
        }
        // ��⿡ �������� �� ������
        else if (isModule)
        {
            if (Input.GetKeyDown(KeyCode.B))
            {
                isModule = false;
            }
        }
        else
        {
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                transform.localPosition += Vector3.left * speed * Time.deltaTime;
            }
            else if (Input.GetKey(KeyCode.RightArrow))
            {
                transform.localPosition += Vector3.right * speed * Time.deltaTime;
            }

            if (Input.GetKey(KeyCode.Space))
            {
                rb.AddForce(Vector2.up * speed);
            }

            if (!isModule && canModule && (Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.A) ||
                Input.GetKeyUp(KeyCode.S) || Input.GetKeyUp(KeyCode.D) || Input.GetKeyDown(KeyCode.C)))
            {
                isModule = true;
            }
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

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Module") && isModule)
        {
            transform.position = Vector3.Lerp(transform.position, other.transform.position, Time.deltaTime * 10);
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
