using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ���ּ� �߽��� �������� ȸ���ϰ� �ʹ�.
// �ʿ�Ӽ�: ���ּ� �߽�, ȸ�� �ӵ�, ȸ�� ����

public class KANG_AutoRotate : MonoBehaviour
{
    // ȸ���߽�
    public Transform spaceship;
    // ȸ���ӵ�
    public float rotSpeed = 20f;
    // ȸ������
    public float rotDir = 1f;

    // Start is called before the first frame update
    void Start()
    {
        // ȸ�� �߽� = ���ּ��� �߽�
        spaceship = GameObject.Find("Spaceship").transform;
    }

    // Update is called once per frame
    public virtual void Update()
    {
        Rotate();
    }

    public virtual void Rotate()
    {
        if (!spaceship) return;

        // ���ּ� �߽��� �������� ȸ���ϰ� �ʹ�.
        transform.RotateAround(spaceship.position, -spaceship.forward, rotDir * rotSpeed * Time.deltaTime);
    }
}
