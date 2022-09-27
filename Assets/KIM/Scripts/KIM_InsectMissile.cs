using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KIM_InsectMissile : MonoBehaviour
{
    public GameObject effect;
    public float speed = 5;
    public Vector3 dir;
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, 10);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += dir.normalized * speed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        // ship.hp--;
        //if (other.gameObject.CompareTag("Map"))
        //{
        //    Destroy(gameObject);
        //}

        //if(other.gameObject.layer == LayerMask.NameToLayer(""))
        //{
        //    Destroy(other.gameObject);
        //}

        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        GameObject explo = Instantiate(effect);
        explo.transform.position = transform.position;
    }
}
