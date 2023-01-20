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
    float shortDistance;

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

        // ���� �Ÿ� ���� enemy �߿��� �ִ� �Ÿ��� �ִ� ���ʹ̸� ã�� �ʹ�. 
        Collider[] enemyCols = Physics.OverlapSphere(transform.position, 50, 1 << 29);

        // �����̻��ϰ� ���� ������ �ִ� ���ʹ̸� ã��
        for(int i = 0; i < enemyCols.Length; i++)
        {
            shortDistance = float.MaxValue;

            float distance = Vector3.Distance(transform.position, enemyCols[i].transform.position);

            if(distance < shortDistance)
            {
                nearstEnemy = enemyCols[i].transform;
            }
        }

        if (nearstEnemy != null)
        {
            Vector3 enemyDir = nearstEnemy.position - transform.position;

            if (enemyDir.magnitude < 5f)
            {
                targetDir = enemyDir;
            }
            else
            {
                Vector3 dir = originDir - transform.up;

                if (dir.magnitude < 0.2f)
                {
                    targetDir = originDir;
                }
            }
        }

        transform.up = Vector3.Lerp(transform.up, targetDir, Time.deltaTime * rotSpeed);
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
