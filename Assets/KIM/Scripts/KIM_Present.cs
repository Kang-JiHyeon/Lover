using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class KIM_Present : MonoBehaviourPun
{
    [SerializeField]
    int crystal = 1;
    GameObject ship;
    Vector3 newPos;
    SpriteRenderer render;
    public Sprite[] sprites = new Sprite[3];
    // Start is called before the first frame update
    void Awake()
    {
        ship = GameObject.Find("Spaceship");
        newPos = transform.position;
        render = transform.Find("PowerUp").GetComponent<SpriteRenderer>();
    }
    float currentTime = 0;
    float randTime = 0;
    public GameObject effectF;
    // Update is called once per frame
    void Update()
    {
        if (photonView.IsMine)
            currentTime += Time.deltaTime;

        if (currentTime > 3.6f)
        {
            currentTime = 0;
        }
        else if (currentTime > 2.4f)
        {
            photonView.RPC("RPCSetPresentSpr", RpcTarget.AllBuffered, 2);
        }
        else if (currentTime > 1.2f)
        {
            photonView.RPC("RPCSetPresentSpr", RpcTarget.AllBuffered, 1);
        }
        else
        {
            if (photonView.IsMine)
                photonView.RPC("RPCSetPresentSpr", RpcTarget.AllBuffered, 0);
        }

        //if (photonView.IsMine)
        randTime += Time.deltaTime; 
        if (randTime > 1.0f)
        {
            newPos = new Vector3(transform.position.x + Random.insideUnitSphere.x,
                    transform.position.y + Random.insideUnitSphere.y, transform.position.z);
            randTime = 0;
        }
        transform.position = Vector3.Lerp(transform.position, newPos, Time.deltaTime);
    }

    [PunRPC]
    void RPCSetPresentSpr(int idx)
    {
        render.sprite = sprites[idx];
    }

    GameObject present;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Ship"))
        {
            if (crystal == 1 && photonView.IsMine)
                present = PhotonNetwork.Instantiate("PowerPresentOmni", ship.transform.position - Vector3.up * 1.3f, Quaternion.identity);
            if (crystal == 2 && photonView.IsMine)
                present = PhotonNetwork.Instantiate("MetalPresentOmni", ship.transform.position - Vector3.up * 1.3f, Quaternion.identity);
            if (crystal == 3 && photonView.IsMine)
                present = PhotonNetwork.Instantiate("BeamPresentOmni", ship.transform.position - Vector3.up * 1.3f, Quaternion.identity);

            if (photonView.IsMine)
                PhotonNetwork.Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        GameObject effect = Instantiate(effectF);
        effect.transform.position = transform.position;
        Destroy(effect, 1f);
    }
}
