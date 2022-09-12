using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 엔진 방향의 반대방향으로 이동하고 싶다.
public class KANG_Move : MonoBehaviour
{
    Rigidbody rigid;
    // 엔진
    GameObject engine;
    // 이동 방향
    Vector3 moveDir;
    // 이동속도
    public float moveSpeed = 5f;

    // Start is called before the first frame update
    void Start()
    {
        rigid = GetComponent<Rigidbody>();
        engine = GameObject.Find("Engine");
    }

    // Update is called once per frame
    void Update()
    {
        // 엔진이 없을 경우 반환
        if (!engine) return;

        // 엔진에서 우주선 중심을 향하는 벡터
        moveDir = transform.position - engine.transform.position;

        // v = v0 + at
        rigid.velocity = moveDir * moveSpeed;

    }
}
