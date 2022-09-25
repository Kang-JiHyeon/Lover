using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �� �������� ũ�Ⱑ Ŀ���� �۾����ٸ� �ݺ��ϰ� �ʹ�.
// �� �������� scale�� �����ϰ� �ʹ�.
public class KANG_MapIcon : MonoBehaviour
{
    float maxSize = 1.7f;
    float minSize = 0.5f;

    public float sizeChangeTime = 0.5f;
    float curTime = 0f;
    Vector3 originSize;

    // Start is called before the first frame update
    void Start()
    {
        originSize = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        curTime += Time.deltaTime;

        if (curTime > sizeChangeTime * 2)
        {
            curTime = 0;
        }
        else if (curTime > sizeChangeTime)
        {
            LerpSize(minSize);
        }
        else
        {
            LerpSize(maxSize);
        }

    }

    void LerpSize(float size)
    {
        transform.localScale =
            Vector3.Lerp(transform.localScale, originSize * size, Time.deltaTime);
    }
}
