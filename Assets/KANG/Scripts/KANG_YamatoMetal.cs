using Photon.Pun;
using UnityEngine;

// 1. metal�� 180�� ȸ���ϰ� �ʹ�.
// 2. �������� y Scale�� �ø���, Blade�� y ��ġ�� �ø��� �ʹ�. 
// 3. Blade�� ũ�⸦ �ø��� �ʹ�.

public class KANG_YamatoMetal : MonoBehaviourPun
{
    AudioSource source;

    public KANG_Yamato yamato;
    public Transform yamatoMetal;
    public Transform axis;
    public Transform blade;
    public Transform bladeBack;
    Vector3 currentRot;
    float curAxisScaleY;
    float curBladePosY;
    float curBladeScale;

    public float rotSpeed = 400f;
    public float bladeRotSpeed = 1.5f;
    public float axisYScale = 180;
    public float bladePos;
    public float bladeScale;

    public float attackTime = 3f;
    float curTime = 0f;

    // target ����
    float targetRotZ;
    float targetAxisScaleY, targetBladePosY;
    float targetBladeScale;
    float op;

    public enum BladeState
    {
        Idle,
        Rotate,
        MoveY,
        Scale,
        Attack
    }
    public enum MoveState
    {
        Up,
        Down
    }

    public BladeState state = BladeState.Idle;
    public MoveState moveState = MoveState.Up;

    // Start is called before the first frame update
    void Start()
    {
        source = GetComponent<AudioSource>();

        yamatoMetal = transform.GetChild(0);
        axis = yamatoMetal.GetChild(0);
        blade = yamatoMetal.GetChild(1);
        bladeBack = blade.GetChild(1);

        currentRot = yamatoMetal.localEulerAngles;
        curAxisScaleY = axis.localScale.y;
        curBladePosY = blade.localPosition.y;
        curBladeScale = bladeBack.localScale.x;

        SetTarget();
    }

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case BladeState.Idle:
                break;
            case BladeState.Rotate:
                Rotate();
                break;
            case BladeState.MoveY:
                Move();
                break;
            case BladeState.Scale:
                Scale();
                break;
            case BladeState.Attack:
                Attack();
                break;
        }

        // ġƮŰ
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            state = BladeState.Rotate;
            moveState = MoveState.Up;
            SetTarget();
        }
    }

    // target ���� �ʱ�ȭ
    public void SetTarget()
    {
        if(moveState == MoveState.Up)
        {
            targetRotZ = 0f;
            targetAxisScaleY = 3.7f;
            targetBladePosY = 0.9f;
            targetBladeScale= 1.1f;
            op = 1;
        }
        else
        {
            targetRotZ = -180f;
            targetAxisScaleY = 1.2f;
            targetBladePosY = 0.3f;
            targetBladeScale = 0f;
            op = -1;
        }
    }

    // 1. metal�� 180�� ȸ���ϰ� �ʹ�.
    private void Rotate()
    {
        currentRot.z += Time.deltaTime * rotSpeed * op;
        currentRot.z = currentRot.z > 180 ? currentRot.z - 360 : currentRot.z;
        currentRot.z = Mathf.Clamp(currentRot.z, -180, 0);
        yamatoMetal.localRotation = Quaternion.Euler(0, 0, currentRot.z);


        if (op > 0 && currentRot.z >= targetRotZ)
        {
            state = BladeState.MoveY;
        }
        else if (op < 0 && currentRot.z <= targetRotZ)
        {
            state = BladeState.Idle;
            yamato.ChangeYState(KANG_Yamato.YamatoState.Disable);
            yamato.SetTexture(false);
        }
    }

    // 2. Blade�� �÷ȴ� ���ȴ� �ϰ� �ʹ�.
    private void Move()
    {
        // ������ ������ ����
        curAxisScaleY += Time.deltaTime * 10 * op;
        curAxisScaleY = Mathf.Clamp(curAxisScaleY, 1.2f, 3.7f);
        axis.localScale = new Vector3(axis.localScale.x, curAxisScaleY, axis.localScale.z);

        // ���̵� ��ġ ����
        curBladePosY += Time.deltaTime * 2f * op;
        curBladePosY = Mathf.Clamp(curBladePosY, 0.3f, 0.9f);
        blade.localPosition = new Vector3(blade.localPosition.x, curBladePosY, blade.localPosition.z);


        if (op > 0 && curAxisScaleY >= targetAxisScaleY && curBladePosY >= targetBladePosY)
        {
            state = BladeState.Scale;
        }
        else if (op < 0 && curAxisScaleY <= targetAxisScaleY && curBladePosY <= targetBladePosY)
        {
            state = BladeState.Rotate;
        }
    }

    // 3. Blade�� ũ�⸦ �ø��ų� ���̰� �ʹ�.
    private void Scale()
    {
        curBladeScale += Time.deltaTime * 8 * op;
        curBladeScale = Mathf.Clamp(curBladeScale, 0f, 1.1f);
        bladeBack.localScale = new Vector3(curBladeScale, curBladeScale, curBladeScale);

        if (moveState == MoveState.Up && curBladeScale >= targetBladeScale)
        {
            state = BladeState.Attack;
        }
        else if (moveState == MoveState.Down && curBladeScale <= targetBladeScale)
        {
            state = BladeState.MoveY;
        }
    }

    // 4. ���� ���� �� ���̵带 ȸ����Ű�� �ʹ�.
    private void Attack()
    {
        curTime += Time.deltaTime;
        photonView.RPC("RPCMYSound", RpcTarget.All, true);
        if (curTime > attackTime)
        {
            curTime = 0f;
            state = BladeState.Scale;
            moveState = MoveState.Down;
            SetTarget();
            photonView.RPC("RPCMYSound", RpcTarget.All, false);
        }
        blade.Rotate(-blade.forward, bladeRotSpeed);
    }

    [PunRPC]
    void RPCMYSound(bool value)
    {
        if (value)
        {
            if (!source.isPlaying)
            {
                source.Play();
            }
        }
        else
            source.Stop();
    }
}
