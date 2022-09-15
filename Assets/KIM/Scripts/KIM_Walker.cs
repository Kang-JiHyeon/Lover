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
    public GameObject planet;

    public GameObject leg1;
    public GameObject leg2;
    public GameObject leg3;
    public GameObject leg4;

    public Sprite damaged;
    public Sprite heavyDamaged;

    public float speed = 2.0f;
    float hp = 4;
    GameObject ship;
    Vector3 dir;
    // Start is called before the first frame update
    void Start()
    {
        ship = GameObject.Find("Spaceship");
        if (planet == null)
        {
            planet = GameObject.Find("Planet");
        }
    }

    // Update is called once per frame
    void Update()
    {
        dir = ship.transform.position - transform.position;
        if (Vector3.Angle(dir, transform.up) < 90)
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

        transform.right = Vector3.Cross(transform.forward, planet.transform.position - transform.position);
        if (Vector3.Distance(planet.transform.position, transform.position) > 7.7f)
            transform.position += (planet.transform.position - transform.position) * Time.deltaTime;
    }

    void SetState()
    {
        if (dir.magnitude > 30f)
            estate = EnemyState.Idle;
        else if (dir.magnitude < 30f && dir.magnitude > 15f)
            estate = EnemyState.Move;
        else if (dir.magnitude < 15f && Vector3.Angle(dir, transform.up) < 90)
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

    float animTime = 0;
    void Move()
    {
        if (Vector3.Cross(transform.up, dir).z > 1)
        {
            // 왼쪽으로 이동
            Debug.Log("왼쪽으로 이동");
            transform.position += -transform.right * speed * Time.deltaTime;
        }
        else if (Vector3.Cross(transform.up, dir).z < -1)
        {
            // 오른쪽으로 이동
            Debug.Log("오른쪽으로 이동");
            transform.position += transform.right * speed * Time.deltaTime;
        }

        if (Mathf.Abs(Vector3.Cross(transform.up, dir).z) > 1)
        {
            animTime += Time.deltaTime;
            if (animTime > 1.01f)
            {
                iTween.MoveTo(leg1, iTween.Hash("y", -0.97f, "time", 0.5f, "easetype", iTween.EaseType.easeInCirc, "islocal", true));
                iTween.MoveTo(leg1, iTween.Hash("y", -1.25f, "time", 0.5f, "delay", 0.5f, "easetype", iTween.EaseType.easeInCirc, "islocal", true));
                iTween.MoveTo(leg2, iTween.Hash("y", -0.97f, "time", 0.5f, "delay", 0.5f, "easetype", iTween.EaseType.easeInCirc, "islocal", true));
                iTween.MoveTo(leg2, iTween.Hash("y", -1.25f, "time", 0.5f, "delay", 1.0f, "easetype", iTween.EaseType.easeInCirc, "islocal", true));
                iTween.MoveTo(leg3, iTween.Hash("y", -0.97f, "time", 0.5f, "easetype", iTween.EaseType.easeInCirc, "islocal", true));
                iTween.MoveTo(leg3, iTween.Hash("y", -1.25f, "time", 0.5f, "delay", 0.5f, "easetype", iTween.EaseType.easeInCirc, "islocal", true));
                iTween.MoveTo(leg4, iTween.Hash("y", -0.97f, "time", 0.5f, "delay", 0.5f, "easetype", iTween.EaseType.easeInCirc, "islocal", true));
                iTween.MoveTo(leg4, iTween.Hash("y", -1.25f, "time", 0.5f, "delay", 1.0f, "easetype", iTween.EaseType.easeInCirc, "islocal", true));
                animTime = 0;
            }
        }
    }

    IEnumerator Fire()
    {
        yield return new WaitForSeconds(0.5f);
        GameObject bullet = Instantiate(bulletFactory);
        bullet.transform.position = bulletDispen.transform.position;
        bullet.GetComponent<KIM_InsectMissile>().dir = turret.transform.up;
    }

    private void OnTriggerEnter(Collider other)
    {
        // 부딪힐 때 튕겨나가기 구현
        if (other.gameObject.layer == LayerMask.NameToLayer("Ship"))
        {
            // ship.hp--;
            // 배가 튕겨나가야 함
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
