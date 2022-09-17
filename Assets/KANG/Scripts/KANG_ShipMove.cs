using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ���� ������ �ݴ�������� �̵��ϰ� �ʹ�.
// Charater Controller �� �̿��� �����̰� �ʹ�.

public class KANG_ShipMove : MonoBehaviour
{
    CharacterController cc;

    // ����
    public KANG_InputRotate engine;
    Transform machine;
    // �̵� ����
    Vector3 moveDir;
    // �̵��ӵ�
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
        // ������ ���� ��� ��ȯ
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
            // �������� ���ּ� �߽��� ���ϴ� ����
            moveDir = transform.forward - machine.up;

            cc.Move(moveDir.normalized * moveSpeed);

        }
    }
}
