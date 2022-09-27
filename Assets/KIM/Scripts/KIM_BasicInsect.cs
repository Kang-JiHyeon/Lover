using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class KIM_BasicInsect : KIM_InsectController
{
    public GameObject stomach;
    public GameObject bulletFactory;
    public GameObject bulletDispen;

    public GameObject dieExplo;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        hp = 2;
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

        // 공격 및 애니메이션 구현
        if (photonView.IsMine)
            currentTime += Time.deltaTime; 
        if (currentTime > 4.0f)
        {
            //iTween.ScaleTo(stomach, iTween.Hash("x", 0.5f, "y", 1.1f, "time", 0.3f, "easetype", iTween.EaseType.easeInOutBack));
            //iTween.MoveTo(stomach, iTween.Hash("x", 0.985f, "time", 0.3f, "easetype", iTween.EaseType.easeInOutBack, "islocal", true));
            //iTween.ScaleTo(stomach, iTween.Hash("x", 1, "y", 1, "time", 0.3f, "easetype", iTween.EaseType.easeInOutBack, "delay", 0.31f));
            //iTween.MoveTo(stomach, iTween.Hash("x", 1.278f, "time", 0.3f, "easetype", iTween.EaseType.easeInOutBack, "delay", 0.31f, "islocal", true));
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
        iTween.MoveTo(stomach, iTween.Hash("x", 0.985f, "time", 0.3f, "easetype", iTween.EaseType.easeInOutBack, "islocal", true));
        iTween.ScaleTo(stomach, iTween.Hash("x", 1, "y", 1, "time", 0.3f, "easetype", iTween.EaseType.easeInOutBack, "delay", 0.31f));
        iTween.MoveTo(stomach, iTween.Hash("x", 1.278f, "time", 0.3f, "easetype", iTween.EaseType.easeInOutBack, "delay", 0.31f, "islocal", true));
        StartCoroutine("Fire");
    }

    IEnumerator Fire()
    {
        yield return new WaitForSeconds(0.5f);
        source.PlayOneShot(attackSound);
        GameObject bullet = Instantiate(bulletFactory);
        bullet.transform.position = bulletDispen.transform.position;
        bullet.GetComponent<KIM_InsectMissile>().dir = -transform.right;
    }

    private void OnTriggerEnter(Collider other)
    {
        // 부딪힐 때 튕겨나가기 구현
        if (other.gameObject.layer == LayerMask.NameToLayer("Ship") || other.gameObject.layer == LayerMask.NameToLayer("Map"))
        {
            // ship.hp--;
            source.PlayOneShot(hittedSound);
            StartCoroutine(Collide(other.transform));
        }
        else if (other.gameObject.layer == LayerMask.NameToLayer("PlayerBullet"))
        {
            Hitted -= 1;
            source.PlayOneShot(hittedSound);
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
