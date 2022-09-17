using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 엔진 방향의 반대방향으로 이동하고 싶다.
// Charater Controller 를 이용해 움직이고 싶다.

public class KANG_ShipMove : MonoBehaviour
{
    CharacterController cc;

    // 엔진
    public KANG_InputRotate engine;
    Transform machine;
    // 이동 방향
    Vector3 moveDir;
    // 이동속도
    public float moveSpeed = 0.01f;

    public bool isMove = false;

    // Start is called before the first frame update
    void Start()
    {
        cc = GetComponent<CharacterController>();
        machine = engine.transform.GetChild(0);

    }

    // Update is called once per frame
    void Update()
    {
        // 엔진이 없을 경우 반환
        if (engine.isControl == false)
            return;

        if (engine.isControl)
        {
            if (Input.GetKeyDown(KeyCode.M))
            {
                isMove = true;
            }

            if (Input.GetKeyUp(KeyCode.M))
            {
                isMove = false;
            }
        }
        else
        {
            isMove = false;
        }

        if (isMove)
        {
            // 엔진에서 우주선 중심을 향하는 벡터
            moveDir = transform.forward - machine.up;

            cc.Move(moveDir.normalized * moveSpeed);

        }
    }
}
