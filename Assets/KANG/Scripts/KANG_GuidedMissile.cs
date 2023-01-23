using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 1. �⺻ ���� ����
// up���� -> yamato.transform.up�������� ���� ȸ��

// 2. ���� �Ÿ� �ȿ� ���� ���� ���
// �� �������� ���� ���� �̵�

public class KANG_GuidedMissile : MonoBehaviour
{
    public Transform yamato;
    public float moveSpeed = 8f;
    public float rotSpeed = 2f;
    Vector3 targetDir;
    Vector3 originDir;


    public List<GameObject> enemys;
    Transform nearstEnemy;
    float shortDistance = float.MaxValue;

    public GameObject smokeFactory;

    
    // Start is called before the first frame update
    void Start()
    {
        if (GameObject.Find("YamatoPower").transform != null)
        {
            yamato = GameObject.Find("YamatoPower").transform;
            originDir = yamato.up;
            targetDir = yamato.up;
            Destroy(gameObject, 3f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (yamato == null) return;

        // ���� �Ÿ� �ȿ� �ִ� ���� �ݶ��̴��� ��� �迭
        Collider[] enemyCols = Physics.OverlapSphere(transform.position, 50, 1 << 29);

        shortDistance = float.MaxValue;

        // �ݶ��̴� �迭���� �̻��ϰ� ���� ������ �ִ� ���� ��ġ�� ã�´�.
        for (int i = 0; i < enemyCols.Length; i++)
        {
            float distance = Vector3.Distance(transform.position, enemyCols[i].transform.position);

            if(distance < shortDistance)
            {
                shortDistance = distance;
                nearstEnemy = enemyCols[i].transform;
            }
        }

        // ����� ���� ���� �� 
        if (nearstEnemy != null)
        {
            Vector3 enemyDir = nearstEnemy.position - transform.position;

            // ������ �Ÿ��� ���� �Ÿ� ���̸� �� ������ �̵� �������� ����
            if (enemyDir.magnitude < 5f)
                targetDir = enemyDir;

            // ���� ���� �Ÿ� ���� �ִٸ� �̻��� ó�� ���� �������� �̵� ������ ����
            else
            {
                Vector3 dir = originDir - transform.up;

                if (dir.magnitude < 0.2f)
                    targetDir = originDir;
            }
        }
        // ����ź�� ���� ���� �� �̵�
        transform.up = Vector3.Lerp(transform.up, targetDir, rotSpeed * Time.deltaTime);
        transform.position += transform.up * moveSpeed * Time.deltaTime;
    }

    void Smoke()
    {
        GameObject smoke = Instantiate(smokeFactory);
    }


    private void OnTriggerEnter(Collider other)
    {
        Destroy(gameObject);
    }
}
