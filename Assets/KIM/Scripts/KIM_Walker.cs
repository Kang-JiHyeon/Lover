using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class KIM_Walker : MonoBehaviourPun
{
    AudioSource source;

    public AudioClip hittedSound;
    public AudioClip destroyedSound;
    public AudioClip attackSound;

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
    int hp = 4;
    public int Hitted
    {
        get { return hp; }
        set
        {
            if (value != hp && value <= hp)
            {
                iTween.ScaleTo(gameObject, iTween.Hash("x", 0.5f, "y", 0.5f, "time", 0.1f, "easetype", iTween.EaseType.easeOutBounce));
                iTween.ScaleTo(gameObject, iTween.Hash("x", 0.7f, "y", 0.7f, "time", 0.1f, "easetype", iTween.EaseType.easeOutBounce, "delay", 0.16f));
            }
            hp = value;
        }
    }

    GameObject ship;
    Vector3 dir;

    // Start is called before the first frame update
    void Start()
    {
        source = GetComponent<AudioSource>();
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
        if (Vector3.Distance(planet.transform.position, transform.position) > planet.GetComponent<KIM_Planet>().Radius)
            transform.position += (planet.transform.position - transform.position).normalized * Time.deltaTime;
    }

    void SetState()
    {
        if (dir.magnitude > 15f)
            estate = EnemyState.Idle;
        else if (dir.magnitude < 15f && dir.magnitude > 8f)
            estate = EnemyState.Move;
        else if (dir.magnitude < 8f && Vector3.Angle(dir, transform.up) < 90)
            estate = EnemyState.Attack;
        else
            estate = EnemyState.Move;
    }

    float currentTime = 0;
    void Attack()
    {
        if (photonView.IsMine)
            currentTime += Time.deltaTime;
        if (currentTime > 4.0f)
        {
            photonView.RPC("RPCAttack", RpcTarget.All);
            if (photonView.IsMine) 
                currentTime = 0;
        }
    }

    [PunRPC]
    void RPCAttack()
    {
        iTween.ScaleTo(stomach, iTween.Hash("x", 1.1f, "y", 0.7f, "time", 0.3f, "easetype", iTween.EaseType.easeInOutBack));
        iTween.MoveTo(stomach, iTween.Hash("y", -0.36f, "time", 0.3f, "easetype", iTween.EaseType.easeInOutBack, "islocal", true));
        iTween.MoveTo(turret, iTween.Hash("y", 0.15f, "time", 0.3f, "easetype", iTween.EaseType.easeInOutBack, "islocal", true));
        iTween.ScaleTo(stomach, iTween.Hash("x", 1, "y", 1, "time", 0.3f, "easetype", iTween.EaseType.easeInOutBack, "delay", 0.31f));
        iTween.MoveTo(stomach, iTween.Hash("y", 0f, "time", 0.3f, "easetype", iTween.EaseType.easeInOutBack, "islocal", true, "delay", 0.31f));
        iTween.MoveTo(turret, iTween.Hash("y", 0.52f, "time", 0.3f, "easetype", iTween.EaseType.easeInOutBack, "islocal", true, "delay", 0.31f));
        StartCoroutine("Fire");
        currentTime = 0;
    }

    float animTime = 0;
    void Move()
    {
        if (Vector3.Cross(transform.up, dir).z > 1)
        {
            // �������� �̵�
            Debug.Log("�������� �̵�");
            transform.position += -transform.right * speed * Time.deltaTime;
        }
        else if (Vector3.Cross(transform.up, dir).z < -1)
        {
            // ���������� �̵�
            Debug.Log("���������� �̵�");
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
        source.PlayOneShot(attackSound);
        GameObject bullet = Instantiate(bulletFactory);
        bullet.transform.position = bulletDispen.transform.position;
        bullet.GetComponent<KIM_InsectMissile>().dir = turret.transform.up;
    }

    private void OnTriggerEnter(Collider other)
    {
        // �ε��� �� ƨ�ܳ����� ����
        if (other.gameObject.layer == LayerMask.NameToLayer("Ship"))
        {
            // ship.hp--;
            // �谡 ƨ�ܳ����� ��
        }
        else if (other.gameObject.layer == LayerMask.NameToLayer("PlayerBullet"))
        {
            Hitted -= 1;
            source.PlayOneShot(hittedSound);
        }
    }

    private void OnDestroy()
    {
        GameObject explo = Instantiate(dieExplo);
        explo.transform.position = transform.position;
    }
}
