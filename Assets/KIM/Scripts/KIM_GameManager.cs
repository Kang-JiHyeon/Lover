using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class KIM_GameManager : MonoBehaviourPunCallbacks
{
    public static KIM_GameManager instance;

    GameObject ship;
    
    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        //OnPhotonSerializeView »£√‚ ∫Ûµµ
        PhotonNetwork.SerializationRate = 60;
        //Rpc »£√‚ ∫Ûµµ
        PhotonNetwork.SendRate = 60;

        ship = GameObject.Find("Spaceship");
        GameObject player = PhotonNetwork.Instantiate("Player", ship.transform.position, Quaternion.identity);
        player.transform.SetParent(ship.transform);
        player.gameObject.name = "Player";
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
