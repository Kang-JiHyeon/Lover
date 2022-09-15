using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ���� ������ �ݴ�������� �̵��ϰ� �ʹ�.
// Charater Controller �� �̿��� �����̰� �ʹ�.

public class KANG_Move : MonoBehaviour
{
    CharacterController cc;

    // ����
    GameObject engine;
    // �̵� ����
    public Vector3 moveDir;
    // �̵��ӵ�
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
        // ������ ���� ��� ��ȯ
        if (!engine) return;

        // �������� ���ּ� �߽��� ���ϴ� ����
        moveDir = transform.position - engine.transform.position;

        cc.Move(moveDir.normalized * moveSpeed * Time.deltaTime);
    }
}
