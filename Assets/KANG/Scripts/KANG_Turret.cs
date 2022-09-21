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
    public float rotSpeed = 30f;
    public float rotDir = 0f;
    Vector3 localAngle;
    float worldZ;


    // Start is called before the first frame update
    void Start()
    {
        // 총구 입구
        for (int i = 0; i < transform.childCount; i++)
        {
            turretCannons.Add(transform.GetChild(i));
        }

        // 현재 회전값
        localAngle = transform.localEulerAngles;

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void UpKey()
    {
        worldZ = transform.eulerAngles.z;

        rotDir = (worldZ > 0f && worldZ < 180f) ? -1 : 1;
    }

    public override void DownKey()
    {
        worldZ = transform.eulerAngles.z;

        rotDir = (worldZ >= 0f && worldZ <= 180f) ? 1 : -1;
    }

    public override void LeftKey()
    {
        worldZ = transform.eulerAngles.z;

        rotDir = (worldZ >= 0f && worldZ < 90f) || (worldZ >= 270f && worldZ < 360f) ? 1 : -1;
    }

    public override void RightKey()
    {
        worldZ = transform.eulerAngles.z;

        rotDir = (worldZ >= 0f && worldZ < 90f) || (worldZ >= 270f && worldZ < 360f) ? -1 : 1;
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
            bullet.transform.up = transform.up;
            currentTime = 0f;
            index++;
            index %= transform.childCount;
        }
    }

    void TurretRotate()
    {
        localAngle.z += rotDir * rotSpeed * Time.deltaTime;
        localAngle.z = localAngle.z > 180 ? localAngle.z - 360 : localAngle.z;
        localAngle.z = Mathf.Clamp(localAngle.z, -100, 100);
        transform.localRotation = Quaternion.Euler(0, 0, localAngle.z);
    }
}
