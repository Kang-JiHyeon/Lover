using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class KIM_Capsule : MonoBehaviourPun
{
    [SerializeField]
    int crystal = 1;
    GameObject prop;
    SpriteRenderer render;
    public Material flash;
    public Material defaultM;
    public Sprite[] sprites = new Sprite[4];
    public GameObject effectF;
    int hp = 3;
    // Start is called before the first frame update
    void Awake()
    {
        prop = transform.Find("Propellor").gameObject;
        render = transform.Find("Capsule").GetComponent<SpriteRenderer>();
    }

    float currentTime = 0;
    // Update is called once per frame
    void Update()
    {
        prop.transform.Rotate(0, 0, 500f * Time.deltaTime);

        if (photonView.IsMine)
            currentTime += Time.deltaTime;
        
        if (currentTime > 1.2f)
        {
            currentTime = 0;
        }
        else if (currentTime > 0.9f)
        {
            photonView.RPC("RPCSetCapsuleSpr", RpcTarget.AllBuffered, 3);
            photonView.RPC("RPCRotate", RpcTarget.AllBuffered, 30f, Time.deltaTime);
        }
        else if (currentTime > 0.6f)
        {
            photonView.RPC("RPCSetCapsuleSpr", RpcTarget.AllBuffered, 2);
            photonView.RPC("RPCRotate", RpcTarget.AllBuffered, 30f, Time.deltaTime);
        }
        else if (currentTime > 0.3f)
        {
            photonView.RPC("RPCSetCapsuleSpr", RpcTarget.AllBuffered, 1);
            photonView.RPC("RPCRotate", RpcTarget.AllBuffered, -30f, Time.deltaTime);
        }
        else
        {
            if (photonView.IsMine)
            {
                photonView.RPC("RPCSetCapsuleSpr", RpcTarget.AllBuffered, 0);
                photonView.RPC("RPCRotate", RpcTarget.AllBuffered, -30f, Time.deltaTime);
            }
        }

        if (hp <= 0)
        {
            if (photonView.IsMine && crystal == 1)
            {
                GameObject present = PhotonNetwork.Instantiate("PowerPresent", transform.position, Quaternion.identity);
                PhotonNetwork.Destroy(gameObject);
            }
            else if (photonView.IsMine && crystal == 2)
            {
                GameObject present = PhotonNetwork.Instantiate("MetalPresent", transform.position, Quaternion.identity);
                PhotonNetwork.Destroy(gameObject);
            }
            else if (photonView.IsMine && crystal == 3)
            {
                GameObject present = PhotonNetwork.Instantiate("BeamPresent", transform.position, Quaternion.identity);
                PhotonNetwork.Destroy(gameObject);
            }
            else if (photonView.IsMine && crystal == 4)
            {
                GameObject present = PhotonNetwork.Instantiate("CrowBarPresent", transform.position, Quaternion.identity);
                PhotonNetwork.Destroy(gameObject);
            }
        }
    }

    [PunRPC]
    void RPCRotate(float angle, float deltaTime)
    {
        transform.Rotate(0, 0, angle * deltaTime);
    }

    [PunRPC]
    void RPCSetCapsuleSpr(int idx)
    {
        render.sprite = sprites[idx];
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("PlayerBullet"))
        {
            hp--;
            iTween.ScaleTo(gameObject, iTween.Hash("x", 0.7f, "y", 0.7f, "time", 0.1f, "easetype", iTween.EaseType.easeOutBounce));
            iTween.ScaleTo(gameObject, iTween.Hash("x", 1, "y", 1, "time", 0.1f, "easetype", iTween.EaseType.easeOutBounce, "delay", 0.11f));
            StopCoroutine("OnHitFlash");
            StartCoroutine("OnHitFlash");
        }
    }
    IEnumerator OnHitFlash()
    {
        render.material = flash;
        yield return new WaitForSeconds(0.1f);
        render.material = defaultM;
    }

    private void OnDestroy()
    {
        GameObject effect = Instantiate(effectF);
        effect.transform.position = transform.position;
    }
}
