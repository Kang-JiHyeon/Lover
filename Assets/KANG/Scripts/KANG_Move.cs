using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 엔진 방향의 반대방향으로 이동하고 싶다.
// Charater Controller 를 이용해 움직이고 싶다.

public class KANG_Move : MonoBehaviour
{
    CharacterController cc;

    // 엔진
    GameObject engine;
    // 이동 방향
    public Vector3 moveDir;
    // 이동속도
    public float moveSpeed = 1f;

    // Start is called before the first frame update
    void Start()
    {
        cc = GetComponent<CharacterController>();
        engine = GameObject.Find("Engine");
    }

    // Update is called once per frame
    void Update()
    {
        // 엔진이 없을 경우 반환
        if (!engine) return;

        // 엔진에서 우주선 중심을 향하는 벡터
        moveDir = transform.position - engine.transform.position;

        cc.Move(moveDir.normalized * moveSpeed * Time.deltaTime);
    }
}
