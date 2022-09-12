using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ���� ������ �ݴ�������� �̵��ϰ� �ʹ�.
public class KANG_Move : MonoBehaviour
{
    Rigidbody rigid;
    // ����
    GameObject engine;
    // �̵� ����
    Vector3 moveDir;
    // �̵��ӵ�
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
        // ������ ���� ��� ��ȯ
        if (!engine) return;

        // �������� ���ּ� �߽��� ���ϴ� ����
        moveDir = transform.position - engine.transform.position;

        // v = v0 + at
        rigid.velocity = moveDir * moveSpeed;

    }
}
