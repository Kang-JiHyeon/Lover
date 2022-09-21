using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class KIM_InsectController : MonoBehaviourPun
{
    protected enum EnemyState
    {
        Idle,
        Move,
        Attack,
    }
    protected EnemyState estate;
    protected Vector3 dir;
    protected GameObject ship;
    protected CharacterController cc;
    protected KIM_InsectRotate ir;

    protected int hp;

    public float moveSpeed = 0.4f;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        ir = GetComponentInChildren<KIM_InsectRotate>();
        cc = GetComponent<CharacterController>();   
        ship = GameObject.Find("Spaceship");
        estate = EnemyState.Idle;
    }
    // Update is called once per frame
    protected virtual void Update()
    {
        dir = ship.transform.position - transform.position;
        transform.right = Vector3.Lerp(transform.right, -dir, Time.deltaTime * 10);

        SetState();

        if (estate == EnemyState.Move)
            Move();
        else if (estate == EnemyState.Attack)
            Attack();
    }

    protected Vector3 moveDir = Vector3.zero;
    protected float currentTime_base = 0;
    protected virtual void Attack()
    {
        currentTime_base += Time.deltaTime;
        if (currentTime_base > 2.0f)
        {
            moveDir = (Vector3)Random.insideUnitCircle * 3 + transform.position;
            currentTime_base = 0;
        }

        if (moveDir != Vector3.zero && Vector3.Distance(moveDir, ship.transform.position) > 5.5f)
        {
            transform.position = Vector3.Lerp(transform.position, moveDir, Time.deltaTime);
            ir.rotSpeed = Vector3.Distance(transform.position, moveDir) * 2 + 1;
        }
    }

    protected virtual void Move()
    {
        cc.Move(dir * moveSpeed * Time.deltaTime);
        ir.rotSpeed = dir.magnitude / 4;
    }

    protected virtual void SetState()
    {
        if (dir.magnitude > 15f)
            estate = EnemyState.Idle;
        else if (dir.magnitude < 15f && dir.magnitude > 8f)
            estate = EnemyState.Move;
        else if (dir.magnitude < 8f)
            estate = EnemyState.Attack;
    }
}
