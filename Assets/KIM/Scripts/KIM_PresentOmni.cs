using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class KIM_PresentOmni : MonoBehaviourPun
{
    [SerializeField]
    int crystal = 1;
    AudioSource source;
    public AudioClip clip;
    GameObject ship;
    KIM_PlayerController1 pc;
    // Start is called before the first frame update
    void Awake()
    {
        source = GetComponent<AudioSource>();
        ship = GameObject.Find("Spaceship");
        transform.SetParent(ship.transform);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            photonView.RPC("RPCCrystalSound", RpcTarget.All);
            pc = other.GetComponent<KIM_PlayerController1>();
            if (crystal == 1) 
                pc.CrystalInit(1);
            else if (crystal == 2)
                pc.CrystalInit(2);
            else if (crystal == 3)
                pc.CrystalInit(3);
            Destroy(gameObject);
        }
    }

    [PunRPC]
    void RPCCrystalSound()
    {
        source.PlayOneShot(clip);
    }
}
