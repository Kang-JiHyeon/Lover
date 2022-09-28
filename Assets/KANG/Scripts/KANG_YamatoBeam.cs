using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters;
using System;
using UnityEngine;

public class KANG_YamatoBeam : MonoBehaviour
{

    private LineRenderer Laser;
    public float MaxLength;

    private Vector4 Length = new Vector4(1, 1, 1, 1);
    //private Vector4 LaserSpeed = new Vector4(0, 0, 0, 0); {DISABLED AFTER UPDATE}
    //private Vector4 LaserStartSpeed; {DISABLED AFTER UPDATE}
    //One activation per shoot
    private bool UpdateSaver = false;


    void Start()
    {
        Laser = GetComponent<LineRenderer>();
    }

    float laserLength = 0f;
    float maxLength = 7f;
    void Update()
    {

        Laser.material.SetTextureScale("_MainTex", new Vector2(Length[0], Length[1]));
        Laser.material.SetTextureScale("_Noise", new Vector2(Length[2], Length[3]));
        //To set LineRender position
        if (Laser != null && UpdateSaver == false)
        {
            Laser.SetPosition(0, transform.localPosition);
            RaycastHit hit; //DELETE THIS IF YOU WANT USE LASERS IN 2D
            if (Physics.Raycast(transform.position, transform.up, out hit, 100))
            {
                if(hit.transform.gameObject.layer == LayerMask.NameToLayer("Enemy") || hit.transform.gameObject.layer == LayerMask.NameToLayer("EnemyBullet"))
                {
                    Destroy(hit.transform.gameObject);
                    //print("Destroy Enemy");
                }

                laserLength = Math.Min(hit.distance, maxLength);
                Laser.SetPosition(1, transform.localPosition + new Vector3(0, laserLength, 0));
            }
        }
    }
}
