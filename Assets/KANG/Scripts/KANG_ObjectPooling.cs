using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 오브젝트 풀링 -> 성능 최적화!!
// - 오브젝트의 Pool을 만들어두고, 그 안에서 필요할 때마다 객체를 꺼내서 사용하는 것
// - 메모리를 할당 해두기 때문에 메모리를 희생하여 성능을 높이는 것

// 오브젝트 풀링 사용하지 않을 경우?
// - 오브젝트 생성 및 파괴할 때 메모리 할당 및 해제 반복 -> CPU 담당
// - 유니티에서 메모리 해제를 하며 가비지 컬렉터가 발생
// - 많은 오브젝트를 파괴할수록 많은 가비지 컬렉터가 발생
// - CPU 부담 커짐

public class KANG_ObjectPooling : MonoBehaviour
{
    // 오브젝트 풀에 담을 게임 오브젝트
    public GameObject goFactory;
    // 오브젝트 풀 생성
    public List<GameObject> goPool = new List<GameObject>();
    // 오브젝트 풀 크기
    public static int poolSize = 30;

    void Start()
    {
        // 오브젝트를 미리 생성하여 pool에 추가한 뒤 비활성화
        for(int i=0; i<poolSize; i++)
        {
            GameObject go = Instantiate(goFactory);
            go.transform.parent = transform;
            goPool.Add(go);
            go.SetActive(false);
        }
    }

    /// <summary>
    /// 오브젝트 풀에 있는 객체를 사용할 때 호출되는 함수
    /// </summary>
    /// <param name="pos">오브젝트의 위치</param>
    public void UseObject(Vector3 position, Quaternion rotation)
    {
        // 오프젝트 풀에 오브젝트가 있다면
        if(goPool.Count > 0)
        {
            // 오브젝트를 활성화한 후 Pool에서 제거
            GameObject go = goPool[0];
            go.SetActive(true);
            go.transform.position = position;
            go.transform.rotation = rotation;
            goPool.RemoveAt(0);
        }
    }
}
