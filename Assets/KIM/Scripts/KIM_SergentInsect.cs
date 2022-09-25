using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class KIM_SergentInsect : KIM_InsectController
{
    public GameObject head;
    public GameObject stomach;
    public GameObject bulletFactory;
    public GameObject bulletDispen;
    public GameObject dieExplo;

    public Sprite damaged;
    public Sprite damaged2;
    public Sprite heavyDamaged;
    public Sprite heavyDamaged2;


    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        hp = 5;
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        if (hp <= 0)
            Destroy(gameObject);
        else if (hp <= 2)
        {
            head.GetComponent<SpriteRenderer>().sprite = heavyDamaged;
            stomach.GetComponent<SpriteRenderer>().sprite = heavyDamaged2;
        }
        else if (hp <= 4)
        {
            head.GetComponent<SpriteRenderer>().sprite = damaged;
            stomach.GetComponent<SpriteRenderer>().sprite = damaged2;
        }
    }

    float currentTime = 0;
    protected override void Attack()
    {
        base.Attack();

        // 공격 및 애니메이션 구현
        if (photonView.IsMine)
            currentTime += Time.deltaTime;
        if (currentTime > 4.0f)
        {
            //iTween.ScaleTo(stomach, iTween.Hash("x", 0.5f, "y", 1.1f, "time", 0.3f, "easetype", iTween.EaseType.easeInOutBack));
            //iTween.MoveTo(stomach, iTween.Hash("x", 0.63f, "time", 0.3f, "easetype", iTween.EaseType.easeInOutBack, "islocal", true));
            //iTween.ScaleTo(stomach, iTween.Hash("x", 1, "y", 1, "time", 0.3f, "easetype", iTween.EaseType.easeInOutBack, "delay", 0.31f));
            //iTween.MoveTo(stomach, iTween.Hash("x", 0.95f, "time", 0.3f, "easetype", iTween.EaseType.easeInOutBack, "delay", 0.31f, "islocal", true));
            //StartCoroutine("Fire");
            photonView.RPC("RPCAttack", RpcTarget.All);
            if (photonView.IsMine)
                currentTime = 0;
        }
    }

    [PunRPC]
    void RPCAttack()
    {
        iTween.ScaleTo(stomach, iTween.Hash("x", 0.5f, "y", 1.1f, "time", 0.3f, "easetype", iTween.EaseType.easeInOutBack));
        iTween.MoveTo(stomach, iTween.Hash("x", 0.63f, "time", 0.3f, "easetype", iTween.EaseType.easeInOutBack, "islocal", true));
        iTween.ScaleTo(stomach, iTween.Hash("x", 1, "y", 1, "time", 0.3f, "easetype", iTween.EaseType.easeInOutBack, "delay", 0.31f));
        iTween.MoveTo(stomach, iTween.Hash("x", 0.95f, "time", 0.3f, "easetype", iTween.EaseType.easeInOutBack, "delay", 0.31f, "islocal", true));
        StartCoroutine("Fire");
    }

    IEnumerator Fire()
    {
        yield return new WaitForSeconds(0.5f);
        GameObject bullet = Instantiate(bulletFactory);
        bullet.transform.position = bulletDispen.transform.position;
        bullet.GetComponent<KIM_InsectMissile>().dir = -transform.right;
        bullet.transform.up = -transform.right;
        //GameObject bullet2 = Instantiate(bulletFactory);
        //bullet2.transform.position = bulletDispen.transform.position;
        //bullet2.GetComponent<KIM_InsectMissile>().dir = -transform.right;
        //bullet2.GetComponent<KIM_InsectMissile>().dir = Quaternion.AngleAxis(-30, Vector3.forward) * dir;
        //bullet2.transform.up = bullet2.GetComponent<KIM_InsectMissile>().dir;
        GameObject bullet3 = Instantiate(bulletFactory);
        bullet3.transform.position = bulletDispen.transform.position;
        bullet3.GetComponent<KIM_InsectMissile>().dir = -transform.right;
        bullet3.GetComponent<KIM_InsectMissile>().dir = Quaternion.AngleAxis(-15, Vector3.forward) * dir;
        bullet3.transform.up = bullet3.GetComponent<KIM_InsectMissile>().dir;
        GameObject bullet4 = Instantiate(bulletFactory);
        bullet4.transform.position = bulletDispen.transform.position;
        bullet4.GetComponent<KIM_InsectMissile>().dir = -transform.right;
        bullet4.GetComponent<KIM_InsectMissile>().dir = Quaternion.AngleAxis(15, Vector3.forward) * dir;
        bullet4.transform.up = bullet4.GetComponent<KIM_InsectMissile>().dir;
        //GameObject bullet5 = Instantiate(bulletFactory);
        //bullet5.transform.position = bulletDispen.transform.position;
        //bullet5.GetComponent<KIM_InsectMissile>().dir = -transform.right;
        //bullet5.GetComponent<KIM_InsectMissile>().dir = Quaternion.AngleAxis(30, Vector3.forward) * dir;
        //bullet5.transform.up = bullet5.GetComponent<KIM_InsectMissile>().dir;
    }

    private void OnTriggerEnter(Collider other)
    {
        // 부딪힐 때 튕겨나가기 구현
        if (other.gameObject.layer == LayerMask.NameToLayer("Ship") || other.gameObject.layer == LayerMask.NameToLayer("Map"))
        {
            // ship.hp--;
            StartCoroutine(Collide(other.transform));
        }
        else if (other.gameObject.layer == LayerMask.NameToLayer("PlayerBullet"))
        {
            Hitted -= 1;
        }
    }

    IEnumerator Collide(Transform other)
    {
        float collideTime = 0;
        IsCollide = true;
        while (collideTime < 0.5f)
        {
            collideTime += Time.deltaTime;
            transform.position = Vector3.Lerp(transform.position, transform.position + (transform.position - other.position).normalized * 4, Time.deltaTime);
            yield return null;
        }
        IsCollide = false;
    }

    private void OnDestroy()
    {
        GameObject explo = Instantiate(dieExplo);
        explo.transform.position = transform.position;  
    }
}
