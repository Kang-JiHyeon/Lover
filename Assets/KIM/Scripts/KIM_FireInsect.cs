using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class KIM_FireInsect : KIM_InsectController
{
    public GameObject stomach;
    public GameObject bulletFactory;
    public GameObject bulletDispen;
    public GameObject dieExplosion;
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

        // 공격 및 애니메이션 구현
        if (photonView.IsMine)
            currentTime += Time.deltaTime;
        if (currentTime > 4.0f)
        {
            //iTween.ScaleTo(stomach, iTween.Hash("x", 0.5f, "y", 1.1f, "time", 0.3f, "easetype", iTween.EaseType.easeInOutBack));
            //iTween.ScaleTo(stomach, iTween.Hash("x", 1, "y", 1, "time", 0.3f, "easetype", iTween.EaseType.easeInOutBack, "delay", 0.31f));
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
        iTween.ScaleTo(stomach, iTween.Hash("x", 1, "y", 1, "time", 0.3f, "easetype", iTween.EaseType.easeInOutBack, "delay", 0.31f));
        StartCoroutine("Fire");
    }

    IEnumerator Fire()
    {
        yield return new WaitForSeconds(0.5f);
        GameObject bullet = Instantiate(bulletFactory);
        bullet.transform.position = bulletDispen.transform.position;
        bullet.GetComponent<KIM_InsectMissile>().dir = -transform.right;
    }

    private void OnTriggerEnter(Collider other)
    {
        // 부딪힐 때 튕겨나가기 구현
        if (other.gameObject.layer == LayerMask.NameToLayer("Ship") || other.gameObject.layer == LayerMask.NameToLayer("PlayerBullet") || other.gameObject.CompareTag("Map"))
        {
            // ship.hp--;
            Debug.Log(other.gameObject.name);
            GameObject explo = Instantiate(dieExplosion);
            explo.transform.position = transform.position;
            Destroy(gameObject);
        }
    }
}
