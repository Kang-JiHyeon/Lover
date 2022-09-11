using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KIM_InsectMissile : MonoBehaviour
{
    GameObject ship;
    public float speed = 5;
    // Start is called before the first frame update
    void Start()
    {
        ship = GameObject.Find("Ship");
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 dir = ship.transform.position - transform.position;
        dir.Normalize();

        transform.position += dir * speed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        // ship.hp--;
        Destroy(gameObject);
    }
}
