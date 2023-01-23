using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 1. 기본 진행 방향
// up방향 -> yamato.transform.up방향으로 점차 회전

// 2. 일정 거리 안에 적이 있을 경우
// 적 방향으로 점차 방향 이동

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

        // 일정 거리 안에 있는 적의 콜라이더를 담는 배열
        Collider[] enemyCols = Physics.OverlapSphere(transform.position, 50, 1 << 29);

        shortDistance = float.MaxValue;

        // 콜라이더 배열에서 미사일과 가장 가까이 있는 적의 위치를 찾는다.
        for (int i = 0; i < enemyCols.Length; i++)
        {
            float distance = Vector3.Distance(transform.position, enemyCols[i].transform.position);

            if(distance < shortDistance)
            {
                shortDistance = distance;
                nearstEnemy = enemyCols[i].transform;
            }
        }

        // 가까운 적이 있을 때 
        if (nearstEnemy != null)
        {
            Vector3 enemyDir = nearstEnemy.position - transform.position;

            // 적과의 거리가 일정 거리 안이면 적 방향을 이동 방향으로 지정
            if (enemyDir.magnitude < 5f)
                targetDir = enemyDir;

            // 적이 일정 거리 보다 멀다면 미사일 처음 생성 방향으로 이동 방향을 지정
            else
            {
                Vector3 dir = originDir - transform.up;

                if (dir.magnitude < 0.2f)
                    targetDir = originDir;
            }
        }
        // 유도탄의 방향 변경 및 이동
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
