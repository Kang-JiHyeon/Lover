using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ���ּ� �߽��� �������� ȸ���ϰ� �ʹ�.
// �ʿ�Ӽ�: ���ּ� �߽�, ȸ�� �ӵ�, ȸ�� ����

public class KANG_AutoRotate : MonoBehaviour
{
    // ȸ���߽�
    public Transform spaceShip;
    // ȸ���ӵ�
    public float rotSpeed = 20f;
    // ȸ������
    public float rotDir = 1f;

    // Start is called before the first frame update
    void Start()
    {
        // ȸ�� �߽� = ���ּ��� �߽�
        spaceShip = transform.parent;
    }

    // Update is called once per frame
    public virtual void Update()
    {
        Rotate();
    }

    public virtual void Rotate()
    {
        // ���ּ� �߽��� �������� ȸ���ϰ� �ʹ�.
        transform.RotateAround(spaceShip.position, -spaceShip.forward, rotDir * rotSpeed * Time.deltaTime);
    }
}
