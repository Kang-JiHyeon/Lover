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
    public bool isUpDown = false;
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
    }

    //public List<Vector3> linePositions;
    //public Vector3[] linePositions = new Vector3[5];
    
    void DrawLine()
    {
        //int index = 0;
        //for(int i= linePositions.Length; i > 0; i--)
        //{
        //    float x = cannonPos.position.x - transform.position.x;
        //    float y = cannonPos.position.y - transform.position.y;
        //    float z = cannonPos.position.z - transform.position.z;

        //    linePositions[index] = cannonPos.position - new Vector3((x / linePositions.Length) * i, (y / linePositions.Length) * i, z) + offset;
        //    index++;
        //}

        //line.SetPositions(linePositions);
        line.SetPosition(0, cannonPos.position + offset);
        line.SetPosition(1, transform.position + offset);
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
