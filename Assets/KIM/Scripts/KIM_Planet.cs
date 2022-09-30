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
    KANG_CameraMove cm;
    bool cap;

    // Start is called before the first frame update
    void Start()
    {
        radius = transform.localScale.x / 1.92f * 10;
        ship = GameObject.Find("Spaceship");
        cm = GameObject.Find("Main Camera").GetComponent<KANG_CameraMove>();
        cc = ship.GetComponent<CharacterController>();
        orbit = transform.Find("Orbit").GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        radius = transform.localScale.x * 4.63f;
        FadeOrbit();

        orbit.transform.Rotate(0, 0, -5f * Time.deltaTime);

        if (distance <= transform.localScale.x * 14 && distance >= transform.localScale.x * 9.14f)
        {
            cap = true;
            Vector3 dir = ship.transform.position - transform.position;
            dir.Normalize();
            cm.Captured = true;
            cm.capSize = ((transform.position + dir * transform.localScale.x * 9.14f) - ship.transform.position).magnitude + 9f;
            cm.capPos = transform.position + dir * transform.localScale.x * 7;
            cc.Move(((transform.position + dir * transform.localScale.x * 9.14f) - ship.transform.position).normalized * 0.7f * Time.deltaTime);
        }
        else if (distance >= transform.localScale.x * 14f && cap)
        {
            cm.Captured = false;
            cap = false;
        }
    }

    void FadeOrbit()
    {
        distance = Vector3.Distance(transform.position, ship.transform.position);

        orbit.transform.localScale = (distance / transform.localScale.x / 4.9f) * Vector3.one;

        col = orbit.color;
        col.a = transform.localScale.x * 18f / distance - 1;
        orbit.color = col;
    }
}
