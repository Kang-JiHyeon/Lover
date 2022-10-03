using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ȸ���ϰ� �ʹ�.
// �����ð��� ������ ���� �ð����� �����̴ٰ� ������� �ʹ�.
public class KANG_Shuriken : MonoBehaviour
{
    public float delayTime = 5f;
    public float twinklingTime = 3f;
    public float intervalTime = 0.1f;
    float curTime = 0f;
    float curIntervalTime = 0f;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation *= Quaternion.Euler(1, 2, 0);
        curTime += Time.deltaTime;


        if (curIntervalTime > intervalTime)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).GetComponent<MeshRenderer>().enabled = false;
            }
        }

        if (curTime > delayTime + twinklingTime)
        {
            Destroy(gameObject);
            curTime = 0f;
        }
        else if (curTime > delayTime)
        {
            // ������ ����
            print("����");
            curIntervalTime += Time.deltaTime;

            if (curIntervalTime > intervalTime * 2)
            {
                for (int i = 0; i < transform.childCount; i++)
                {
                    transform.GetChild(i).GetComponent<MeshRenderer>().enabled = true;
                }

                curIntervalTime = 0f;
            }
            else if (curIntervalTime > intervalTime)
            {
                for (int i = 0; i < transform.childCount; i++)
                {
                    transform.GetChild(i).GetComponent<MeshRenderer>().enabled = false;
                }
            }
        }


    }
}
