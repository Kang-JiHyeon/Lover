using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters;
using System;
using UnityEngine;

public class KANG_YamatoBeam : MonoBehaviour
{
    private LineRenderer laser;
    float laserLength = 0f;
    float maxLength = 7f;
    private Vector4 Length = new Vector4(1, 1, 1, 1);


    void Start()
    {
        laser = GetComponent<LineRenderer>();
    }

    void Update()
    {
        laser.material.SetTextureScale("_MainTex", new Vector2(Length[0], Length[1]));
        laser.material.SetTextureScale("_Noise", new Vector2(Length[2], Length[3]));
        
        //LineRender position ����
        if (laser != null)
        {
            // laser ���� ���� : ���� ��ġ
            laser.SetPosition(0, transform.localPosition);

            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.up, out hit, 100))
            {
                if(hit.transform.gameObject.layer == LayerMask.NameToLayer("Enemy")
                    || hit.transform.gameObject.layer == LayerMask.NameToLayer("EnemyBullet"))
                {
                    Destroy(hit.transform.gameObject);
                }
                
                // hit ������ �Ÿ��� �ִ� ���� �� ª�� ���� laser�� ���̷� ����
                laserLength = Math.Min(hit.distance, maxLength);
                // laser �� ���� : ���� ��ġ���� ���� y�� �������� laserLength��ŭ ����
                laser.SetPosition(1, transform.localPosition + new Vector3(0, laserLength, 0));
            }
        }
    }
}
