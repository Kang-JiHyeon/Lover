using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KIM_Planet : MonoBehaviour
{
    GameObject ship;
    float distance;
    Color col;
    SpriteRenderer orbit;
    CharacterController cc;
    KIM_PlayerController pc;
    
    // Start is called before the first frame update
    void Start()
    {
        ship = GameObject.Find("Spaceship");
        cc = ship.GetComponent<CharacterController>();
        orbit = transform.Find("Orbit").GetComponent<SpriteRenderer>();
        pc = GameObject.Find("Player").GetComponent<KIM_PlayerController>();    
    }

    // Update is called once per frame
    void Update()
    {
        FadeOrbit();

        if (distance <= 18)
        {
            Vector3 dir = ship.transform.position - transform.position;
            dir.Normalize();

            cc.Move(((transform.position + dir * 13.5f) - ship.transform.position) * Time.deltaTime /5);
            pc.LocalMove(((transform.position + dir * 13.5f) - ship.transform.position) / 5);
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
