using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KIM_InsectMissile : MonoBehaviour
{
    public float speed = 5;
    public Vector3 dir;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.position += dir.normalized * speed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        // ship.hp--;
        Debug.Log(other.gameObject.name);
        Destroy(gameObject);
    }
}
