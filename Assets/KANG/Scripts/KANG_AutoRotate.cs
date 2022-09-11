using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 우주선 중심을 기준으로 회전하고 싶다.
// 필요속성: 우주선 중심, 회전 속도, 회전 방향

public class KANG_AutoRotate : MonoBehaviour
{
    // 회전중심
    public Transform spaceShip;
    // 회전속도
    public float rotSpeed = 20f;
    // 회전방향
    public float rotDir = 1f;

    // Start is called before the first frame update
    void Start()
    {
        // 회전 중심 = 우주선의 중심
        spaceShip = transform.parent;
    }

    // Update is called once per frame
    public virtual void Update()
    {
        Rotate();
    }

    public virtual void Rotate()
    {
        // 우주선 중심을 기준으로 회전하고 싶다.
        transform.RotateAround(spaceShip.position, -spaceShip.forward, rotDir * rotSpeed * Time.deltaTime);
    }
}
