using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// targetPos를 따라 이동하고 싶다.
// 맵에 부딪히면 바운스
// 적에 부딪히면 아주 살짝 바운스

public class KANG_MetalBlade : MonoBehaviour
{
    public Transform targetPos;
    public float moveSpeed = 10f;
    private float curMoveSpeed;
    Vector3 bounceDir;
    bool isBounce = false;

    // Start is called before the first frame update
    void Start()
    {
        //targetPos = transform.parent.GetChild(3).GetChild(1);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, targetPos.position, Time.deltaTime * moveSpeed);

        if (isBounce)
        {
            transform.position = Vector3.Lerp(transform.position, bounceDir.normalized * 10f, Time.deltaTime * moveSpeed);

            if ((transform.position - bounceDir.normalized * 5f).magnitude < 0.1)
            {
                isBounce = false;
                print("metal blade 바운스 끝");
            }
        }
    }

    //void LerpMoveSpeed(float targetSpeed)
    //{
    //    curMoveSpeed = Mathf.Lerp(curMoveSpeed, targetSpeed, Time.deltaTime * lerpSpeed);
    //    if (Mathf.Abs(targetSpeed - curMoveSpeed) < 0.1f)
    //    {
    //        curMoveSpeed = targetSpeed;
    //    }
    //}

 
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Map"))
        {
            bounceDir = transform.position - other.transform.position;

            isBounce = true;
        }
    }
}
