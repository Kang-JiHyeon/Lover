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
        FadeOrbit();

        orbit.transform.Rotate(0, 0, -5f * Time.deltaTime);

        if (distance <= 25 && distance >= 0.1f)
        {
            Vector3 dir = ship.transform.position - transform.position;
            dir.Normalize();

            cc.Move(((transform.position + dir * 13.5f) - ship.transform.position).normalized  * Time.deltaTime);
        }
    }

    void FadeOrbit()
    {
        distance = Vector3.Distance(transform.position, ship.transform.position);

        orbit.transform.localScale = distance / 7.1f * Vector3.one;

        col = orbit.color;
        col.a = 30 / distance - 1;
        orbit.color = col;
    }
}
