using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KIM_Planet : MonoBehaviour
{
    float radius;
    public float Radius { get { return radius; } }

    GameObject ship;
    float distance;
    Color col;
    SpriteRenderer orbit;
    CharacterController cc;

    // Start is called before the first frame update
    void Start()
    {
        radius = transform.localScale.x / 1.92f * 10;
        ship = GameObject.Find("Spaceship");
        cc = ship.GetComponent<CharacterController>();
        orbit = transform.Find("Orbit").GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        radius = transform.localScale.x * 4.63f;
        Debug.Log("Radius: " + radius);
        FadeOrbit();

        orbit.transform.Rotate(0, 0, -5f * Time.deltaTime);

        if (distance <= transform.localScale.x * 18 && distance >= transform.localScale.x * 9.14f)
        {
            Vector3 dir = ship.transform.position - transform.position;
            dir.Normalize();

            cc.Move(((transform.position + dir * transform.localScale.x * 9.14f) - ship.transform.position).normalized * 0.7f * Time.deltaTime);
        }
    }

    void FadeOrbit()
    {
        distance = Vector3.Distance(transform.position, ship.transform.position);

        orbit.transform.localScale = (distance / transform.localScale.x / 4.9f) * Vector3.one;

        col = orbit.color;
        col.a = transform.localScale.x * 21f / distance - 1;
        orbit.color = col;
    }
}
