using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ���忡 ��, �������� ������ ���ְ� �ʹ�.
// ���尡 �, �� ��ҿ� ������ ƨ�ܳ��� �ʹ�.

// ���尡 �����̴� ���� ���� y���� ���� ���� ���ҽ�Ű�� �ʹ�.

public class KANG_Shield : MonoBehaviour
{
    public float upDownSpeed = 5f;
    public float upDownValue = 0.4f;

    Vector3 targetPos;
    Vector3 originPos;

    public KANG_InputRotate shield;
    
    // Start is called before the first frame update
    void Start()
    {
        originPos = transform.localPosition;
        targetPos = transform.localPosition;
        targetPos.y = transform.localPosition.y - upDownValue;

        
    }

    // Update is called once per frame
    void Update()
    {
        // ���尡 �����̴� ���̶��
        if (shield.isShieldMove)
        {
            LerpMovePos(targetPos);
        }

        else
        {
            LerpMovePos(originPos);
        }
    }

    void LerpMovePos(Vector3 pos)
    {
        transform.localPosition = Vector3.Lerp(transform.localPosition, pos, Time.deltaTime * upDownSpeed);

        if(Mathf.Abs((transform.localPosition - pos).magnitude) < 0.1f)
        {
            transform.localPosition = pos;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("EnemyBullet"))
        {
            Destroy(other.gameObject);
        }

        // ���ʹ̶�� ���ĳ��� �ʹ�.

        if (other.gameObject.CompareTag("Map"))
        {
            KANG_ShipMove.instance.bounceDir = transform.position - other.transform.position;
            KANG_ShipMove.instance.bounceDir.z = 0f;
            KANG_ShipMove.instance.bounceDir.Normalize();
            KANG_ShipMove.instance.isBounce = true;

            print("Shield Wave Bounce");

        }
    }
}
