using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KANG_ShieldMachine : KANG_Machine
{
    public float upDownSpeed = 5f;
    public float upDownValue = 0.4f;

    //public Transform shieldAxis;
    public Transform shieldWave;

    Vector3 downPos;
    Vector3 upPos;

    //public float rotSpeed = 30f;

    // Start is called before the first frame update
    void Start()
    {
        upPos = shieldWave.localPosition;
        downPos = shieldWave.localPosition;
        downPos.y = shieldWave.localPosition.y - upDownValue;
        localAngle = rotAxis.localEulerAngles;
    }

    // Update is called once per frame
    void Update()
    {

    }

    // ����Ű�� ������ �ִ� ���� shieldWave�� ������ �ʹ�.
    public override void ArrowKey()
    {
        StopCoroutine(IeArrowKeyUp());
        base.Rotate();
        LerpMovePos(downPos);
    }

    public override void ArrowKeyUp()
    {
        StartCoroutine(IeArrowKeyUp());
    }

    // ����Ű�� ���� shieldWave�� upPos���� �ö������ �ϰ� �ʹ�.
    // ArrowKey�� �ԷµǸ� stopCorutine�� �ϰ� �ʹ�.
    IEnumerator IeArrowKeyUp()
    {
        while (shieldWave.localPosition.y < upPos.y)
        {
            LerpMovePos(upPos);
            yield return null;
        }
    }

    void LerpMovePos(Vector3 pos)
    {
        shieldWave.localPosition = Vector3.Lerp(shieldWave.localPosition, pos, Time.deltaTime * upDownSpeed);

        if (Mathf.Abs((shieldWave.localPosition - pos).magnitude) < 0.1f)
        {
            shieldWave.localPosition = pos;
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("EnemyBullet"))
        {
            Destroy(other.gameObject);
        }


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
