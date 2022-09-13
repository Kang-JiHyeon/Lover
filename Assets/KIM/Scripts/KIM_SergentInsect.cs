using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KIM_SergentInsect : KIM_InsectController
{
    public GameObject stomach;
    public GameObject bulletFactory;
    public GameObject bulletDispen;
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }

    float currentTime = 0;
    protected override void Attack()
    {
        base.Attack();

        // ���� �� �ִϸ��̼� ����
        currentTime += Time.deltaTime;
        if (currentTime > 4.0f)
        {
            iTween.ScaleTo(stomach, iTween.Hash("x", 0.5f, "y", 1.1f, "time", 0.3f, "easetype", iTween.EaseType.easeInOutBack));
            iTween.MoveTo(stomach, iTween.Hash("x", 0.63f, "time", 0.3f, "easetype", iTween.EaseType.easeInOutBack, "islocal", true));
            iTween.ScaleTo(stomach, iTween.Hash("x", 1, "y", 1, "time", 0.3f, "easetype", iTween.EaseType.easeInOutBack, "delay", 0.31f));
            iTween.MoveTo(stomach, iTween.Hash("x", 0.95f, "time", 0.3f, "easetype", iTween.EaseType.easeInOutBack, "delay", 0.31f, "islocal", true));
            StartCoroutine("Fire");
            currentTime = 0;
        }
    }

    IEnumerator Fire()
    {
        yield return new WaitForSeconds(0.5f);
        GameObject bullet = Instantiate(bulletFactory);
        bullet.transform.position = bulletDispen.transform.position + transform.forward;
        bullet.GetComponent<KIM_InsectMissile>().dir = -transform.right;
        bullet.transform.up = -transform.right;
        GameObject bullet2 = Instantiate(bulletFactory);
        bullet2.transform.position = bulletDispen.transform.position + transform.forward;
        bullet2.GetComponent<KIM_InsectMissile>().dir = -transform.right;
        bullet2.GetComponent<KIM_InsectMissile>().dir = Quaternion.AngleAxis(-30, Vector3.forward) * dir;
        bullet2.transform.up = bullet2.GetComponent<KIM_InsectMissile>().dir;
        GameObject bullet3 = Instantiate(bulletFactory);
        bullet3.transform.position = bulletDispen.transform.position + transform.forward;
        bullet3.GetComponent<KIM_InsectMissile>().dir = -transform.right;
        bullet3.GetComponent<KIM_InsectMissile>().dir = Quaternion.AngleAxis(-15, Vector3.forward) * dir;
        bullet3.transform.up = bullet3.GetComponent<KIM_InsectMissile>().dir;
        GameObject bullet4 = Instantiate(bulletFactory);
        bullet4.transform.position = bulletDispen.transform.position + transform.forward;
        bullet4.GetComponent<KIM_InsectMissile>().dir = -transform.right;
        bullet4.GetComponent<KIM_InsectMissile>().dir = Quaternion.AngleAxis(15, Vector3.forward) * dir;
        bullet4.transform.up = bullet4.GetComponent<KIM_InsectMissile>().dir;
        GameObject bullet5 = Instantiate(bulletFactory);
        bullet5.transform.position = bulletDispen.transform.position + transform.forward;
        bullet5.GetComponent<KIM_InsectMissile>().dir = -transform.right;
        bullet5.GetComponent<KIM_InsectMissile>().dir = Quaternion.AngleAxis(30, Vector3.forward) * dir;
        bullet5.transform.up = bullet5.GetComponent<KIM_InsectMissile>().dir;
    }

    private void OnTriggerEnter(Collider other)
    {
        // �ε��� �� ƨ�ܳ����� ����
        if (other.gameObject.layer == LayerMask.NameToLayer("Ship"))
        {
            // ship.hp--;
            StartCoroutine("Collide");
        }
    }

    IEnumerator Collide()
    {
        float collideTime = 0;

        while (collideTime < 0.5f)
        {
            collideTime += Time.deltaTime;
            transform.position = Vector3.Lerp(transform.position, transform.position - dir.normalized, Time.deltaTime * 10);
            yield return null;
        }
    }
}