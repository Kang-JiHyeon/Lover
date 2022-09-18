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
    KIM_PlayerController pc1;
    KIM_PlayerController pc2;

    // Start is called before the first frame update
    void Start()
    {
        radius = transform.localScale.x / 1.92f * 10;
        ship = GameObject.Find("Spaceship");
        cc = ship.GetComponent<CharacterController>();
        orbit = transform.Find("Orbit").GetComponent<SpriteRenderer>();
        pc1 = GameObject.Find("Player").GetComponent<KIM_PlayerController>();
        pc2 = GameObject.Find("Player (1)").GetComponent<KIM_PlayerController>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        FadeOrbit();

        if (distance <= 18)
        {
            Vector3 dir = ship.transform.position - transform.position;
            dir.Normalize();

            cc.Move(((transform.position + dir * 13.5f) - ship.transform.position) * Time.deltaTime / 3);
            if (!pc1.IsModule)
            {
                pc1.LocalMove(((transform.position + dir * 13.5f) - ship.transform.position) / 3);
            }
            if (!pc2.IsModule)
            {
                pc2.LocalMove(((transform.position + dir * 13.5f) - ship.transform.position) / 3);
            }
        }
    }

    void FadeOrbit()
    {
        distance = Vector3.Distance(transform.position, ship.transform.position);

        col = orbit.color;
        col.a = 30 / distance - 1;
        orbit.color = col;
    }
}
