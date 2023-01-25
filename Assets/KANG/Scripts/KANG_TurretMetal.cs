using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

// targetPos를 따라 이동하고 싶다.
// 맵에 부딪히면 바운스
// 적에 부딪히면 아주 살짝 바운스
// 총구에서 나까지 line을 그리고 싶다.

public class KANG_TurretMetal : MonoBehaviourPun
{
    // 메탈
    public Transform targetPos;
    public float moveSpeed = 2f;
    Vector3 bounceDir;
    float bounceTime = 0.05f;
    float curBounceTime = 0f;
    bool isBounce = false;

    // 줄
    LineRenderer line;
    public Transform cannonPos;
    public Vector3 offset = new Vector3(0, 0, -0.2f);


    // Start is called before the first frame update
    void Start()
    {
        line = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        DrawLine();

        if (isBounce)
        {
            curBounceTime += Time.deltaTime;

            transform.position = Vector3.Lerp(transform.position, bounceDir, Time.deltaTime);

            if (curBounceTime > bounceTime)
            {
                isBounce = false;
                curBounceTime = 0f;
            }
        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, targetPos.position, Time.deltaTime * moveSpeed);

            Vector3 dir = targetPos.position - transform.position;
            if(dir.magnitude < 0.3f)
            {
                transform.position = targetPos.position;
            }
        }

        if((cannonPos.position - transform.position).magnitude > 0.5f)
        {
            transform.localScale = new Vector3(2f, 2f, 2f);
            transform.Rotate(-transform.forward, 5f);
        }
        else
        {
            transform.localScale = new Vector3(1.3f, 1.3f, 1.3f);
        }
    }

    void DrawLine()
    {
        Vector3 cPos = cannonPos.position;
        Vector3 tPos = transform.position;
        cPos.z = 0;
        tPos.z = 0;

        line.SetPosition(0, cPos + offset);
        line.SetPosition(1, tPos + offset);
    }

 
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Map"))
        {
            bounceDir = transform.position - other.transform.position;
            bounceDir.z = 0f;
            bounceDir.Normalize();

            isBounce = true;
        }
    }
}
