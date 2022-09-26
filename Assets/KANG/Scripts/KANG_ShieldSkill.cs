using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// * �Ϲ� ��� *
// ���忡 �������� ������ ���ְ� �ʹ�.
// ���尡 �, �� ��ҿ� ������ ƨ�ܳ��� �ʹ�.

// * �� ��� *
// ���忡 ��, �������� ��

// ���尡 �����̴� ���� ���� y���� ���� ���� ���ҽ�Ű�� �ʹ�.



public class KANG_ShieldSkill : MonoBehaviour
{
    public KANG_Engine engine;
    public KANG_Shield shield;

    KIM_InsectMissile missile;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    void Idle(GameObject go)
    {
        Destroy(go);
    }

    // �� ������ ƨ�ܳ��� �ʹ�.
    void Beam(Vector3 dir)
    {
        if (missile)
        {
            missile.dir = dir.normalized;
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("EnemyBullet"))
        {
            switch (shield.mState)
            {
                case KANG_Machine.MachineState.Idle:
                    Idle(other.gameObject);
                    break;
                case KANG_Machine.MachineState.Beam:
                    missile = other.gameObject.GetComponent<KIM_InsectMissile>();
                    Beam(other.transform.position - transform.position);
                    break;
            }
        }


        if (other.gameObject.CompareTag("Map"))
        {
            engine.bounceDir = transform.position - other.transform.position;
            engine.bounceDir.z = 0f;
            engine.bounceDir.Normalize();
            engine.isBounce = true;

            print("Shield Wave Bounce");

        }
    }
}
