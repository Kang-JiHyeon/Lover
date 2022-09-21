using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 부모 클래스의 특정키가 입력되었을 때 동작하는 함수를 재정의한다.

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
        // 총구 입구
        for (int i = 0; i < rotAxis.childCount; i++)
        {
            turretCannons.Add(rotAxis.GetChild(i));
        }

        // 현재 회전값
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
