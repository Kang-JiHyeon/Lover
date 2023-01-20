using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ������Ʈ Ǯ�� -> ���� ����ȭ!!
// - ������Ʈ�� Pool�� �����ΰ�, �� �ȿ��� �ʿ��� ������ ��ü�� ������ ����ϴ� ��
// - �޸𸮸� �Ҵ� �صα� ������ �޸𸮸� ����Ͽ� ������ ���̴� ��

// ������Ʈ Ǯ�� ������� ���� ���?
// - ������Ʈ ���� �� �ı��� �� �޸� �Ҵ� �� ���� �ݺ� -> CPU ���
// - ����Ƽ���� �޸� ������ �ϸ� ������ �÷��Ͱ� �߻�
// - ���� ������Ʈ�� �ı��Ҽ��� ���� ������ �÷��Ͱ� �߻�
// - CPU �δ� Ŀ��

public class KANG_ObjectPooling : MonoBehaviour
{
    // ������Ʈ Ǯ�� ���� ���� ������Ʈ
    public GameObject goFactory;
    // ������Ʈ Ǯ ����
    public List<GameObject> goPool = new List<GameObject>();
    // ������Ʈ Ǯ ũ��
    public static int poolSize = 30;

    void Start()
    {
        // ������Ʈ�� �̸� �����Ͽ� pool�� �߰��� �� ��Ȱ��ȭ
        for(int i=0; i<poolSize; i++)
        {
            GameObject go = Instantiate(goFactory);
            go.transform.parent = transform;
            goPool.Add(go);
            go.SetActive(false);
        }
    }

    /// <summary>
    /// ������Ʈ Ǯ�� �ִ� ��ü�� ����� �� ȣ��Ǵ� �Լ�
    /// </summary>
    /// <param name="pos">������Ʈ�� ��ġ</param>
    public void UseObject(Vector3 position, Quaternion rotation)
    {
        // ������Ʈ Ǯ�� ������Ʈ�� �ִٸ�
        if(goPool.Count > 0)
        {
            // ������Ʈ�� Ȱ��ȭ�� �� Pool���� ����
            GameObject go = goPool[0];
            go.SetActive(true);
            go.transform.position = position;
            go.transform.rotation = rotation;
            goPool.RemoveAt(0);
        }
    }
}
