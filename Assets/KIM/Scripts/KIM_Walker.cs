using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KIM_Walker : MonoBehaviour
{
    protected enum EnemyState
    {
        Idle,
        Move,
        Attack,
    }
    [SerializeField]
    EnemyState estate;

    public GameObject turret;
    public GameObject stomach;
    public GameObject bulletFactory;
    public GameObject bulletDispen;
    public GameObject dieExplo;

    public Sprite damaged;
    public Sprite heavyDamaged;

    float hp = 4;
    GameObject ship;
    Vector3 dir;
    // Start is called before the first frame update
    void Start()
    {
        ship = GameObject.Find("Spaceship");
    }

    // Update is called once per frame
    void Update()
    {
        dir = ship.transform.position - transform.position;
        if (dir.y > 0)
            turret.transform.up = dir;

        SetState();

        if (estate == EnemyState.Move)
            Move();
        else if (estate == EnemyState.Attack)
            Attack();

        if (hp <= 0)
            Destroy(gameObject);
        else if (hp <= 1)
        {
            stomach.GetComponent<SpriteRenderer>().sprite = heavyDamaged;
        }
        else if (hp <= 3)
        {
            stomach.GetComponent<SpriteRenderer>().sprite = damaged;
        }
    }

    void SetState()
    {
        if (dir.magnitude > 30f)
            estate = EnemyState.Idle;
        else if (dir.magnitude < 30f && dir.magnitude > 15f)
            estate = EnemyState.Move;
        else if (dir.magnitude < 15f && dir.y > 0)
            estate = EnemyState.Attack;
        else
            estate = EnemyState.Move;
    }

    float currentTime = 0;
    void Attack()
    {
        currentTime += Time.deltaTime;
        if (currentTime > 4.0f)
        {
            iTween.ScaleTo(stomach, iTween.Hash("x", 1.1f, "y", 0.7f, "time", 0.3f, "easetype", iTween.EaseType.easeInOutBack));
            iTween.ScaleTo(stomach, iTween.Hash("x", 1, "y", 1, "time", 0.3f, "easetype", iTween.EaseType.easeInOutBack, "delay", 0.31f));
            StartCoroutine("Fire");
            currentTime = 0;
        }
    }

    void Move()
    {

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
        // ºÎµúÈú ¶§ Æ¨°Ü³ª°¡±â ±¸Çö
        if (other.gameObject.layer == LayerMask.NameToLayer("Ship"))
        {
            // ship.hp--;
            // ¹è°¡ Æ¨°Ü³ª°¡¾ß ÇÔ
        }
        else if (other.gameObject.layer == LayerMask.NameToLayer("PlayerBullet"))
        {
            hp -= 1;
        }
    }

    private void OnDestroy()
    {
        GameObject explo = Instantiate(dieExplo);
        explo.transform.position = transform.position;
    }
}
