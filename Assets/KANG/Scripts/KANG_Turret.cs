using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �θ� Ŭ������ Ư��Ű�� �ԷµǾ��� �� �����ϴ� �Լ��� �������Ѵ�.

public class KANG_Turret : KANG_Machine
{
    // Fire
    public List<Transform> turretCannons;
    public GameObject bulletFactory;
    public float createTime = 0.5f;
    float currentTime = 0f;
    int index = 0;

    // Rotate
    //public float rotSpeed = 30f;

    // Start is called before the first frame update
    void Start()
    {
        // �ѱ� �Ա�
        for (int i = 0; i < rotAxis.childCount; i++)
        {
            turretCannons.Add(rotAxis.GetChild(i));
        }

        // ���� ȸ����
        localAngle = rotAxis.localEulerAngles;

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void ArrowKey()
    {
        TurretRotate();
    }

    override public void ActionKey()
    {
        currentTime += Time.deltaTime;

        if (currentTime > createTime)
        {
            GameObject bullet = Instantiate(bulletFactory);
            bullet.transform.position = turretCannons[index].position;
            bullet.transform.up = rotAxis.up;
            currentTime = 0f;
            index++;
            index %= rotAxis.childCount;
        }
    }

    void TurretRotate()
    {
        localAngle.z += rotDir * rotSpeed * Time.deltaTime;
        localAngle.z = localAngle.z > 180 ? localAngle.z - 360 : localAngle.z;
        localAngle.z = Mathf.Clamp(localAngle.z, -100, 100);
        rotAxis.localRotation = Quaternion.Euler(0, 0, localAngle.z);
    }
}
