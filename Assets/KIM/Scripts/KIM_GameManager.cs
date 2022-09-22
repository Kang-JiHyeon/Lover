using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class KIM_GameManager : MonoBehaviourPunCallbacks
{
    public static KIM_GameManager Instance;

    GameObject ship;

    int rescueCount;
    public int RescueCount
    {
        get { return rescueCount; }
        set { rescueCount = value; }
    }

    float gameTime;
    public float GameTime
    {
        get { return gameTime; }
    }

    // Start is called before the first frame update
    void Awake()
    {
        Instance = this;

        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        //OnPhotonSerializeView »£√‚ ∫Ûµµ
        PhotonNetwork.SerializationRate = 60;
        //Rpc »£√‚ ∫Ûµµ
        PhotonNetwork.SendRate = 60;

        ship = GameObject.Find("Spaceship");
        if (PhotonNetwork.IsMasterClient)
        {
            GameObject player = PhotonNetwork.Instantiate("Player", ship.transform.position, Quaternion.identity);
            player.transform.SetParent(ship.transform);
            player.gameObject.name = "Player";
        }
        else
        {
            GameObject player = PhotonNetwork.Instantiate("Player2", ship.transform.position, Quaternion.identity);
            player.transform.SetParent(ship.transform);
            player.gameObject.name = "Player";
        } 
    }

    // Update is called once per frame
    void Update()
    {
        gameTime += Time.deltaTime;
    }
}
