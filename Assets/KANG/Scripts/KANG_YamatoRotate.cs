using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ���ּ� �߽��� �������� ȸ���ϰ� �ʹ�.
// �ʿ�Ӽ�: ���ּ� �߽�, ȸ�� �ӵ�, ȸ�� ����

public class KANG_YamatoRotate : MonoBehaviour
{
    // ȸ���߽�
    public Transform spaceship;
    // ȸ���ӵ�
    public float rotSpeed = 20f;
    // ȸ������
    public float rotDir = 1f;

    // yamato texture
    public List<Transform> rotObjects;


    // Start is called before the first frame update
    void Start()
    {
        // ȸ�� �߽� = ���ּ��� �߽�
        spaceship = GameObject.Find("Spaceship").transform;

        for(int i = 0; i<transform.childCount; i++)
        {
            rotObjects.Add(transform.GetChild(i));
        }
    }

    // Update is called once per frame
    public virtual void Update()
    {
        if (!spaceship) return;

        Rotate();
    }

    public virtual void Rotate()
    {
        // yamato texture���� ���ּ� �߽��� �������� ȸ���ϰ� �ʹ�.
        for (int i = 0; i< rotObjects.Count; i++)
        {
            rotObjects[i].RotateAround(spaceship.position, -spaceship.forward, rotDir * rotSpeed * Time.deltaTime);
        }
    }
}
