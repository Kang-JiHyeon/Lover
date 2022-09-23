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
        //OnPhotonSerializeView 호출 빈도
        PhotonNetwork.SerializationRate = 60;
        //Rpc 호출 빈도
        PhotonNetwork.SendRate = 60;

        ship = GameObject.Find("Spaceship");

        if (PhotonNetwork.IsMasterClient)
        {
            if (KIM_PlayerTransit.Instance.idx1 == 0)
            {
                GameObject player = PhotonNetwork.Instantiate("Player", ship.transform.position, Quaternion.identity);
                player.transform.SetParent(ship.transform);
                player.gameObject.name = "Player";
            }
            if (KIM_PlayerTransit.Instance.idx1 == 1)
            {
                GameObject player = PhotonNetwork.Instantiate("Player2", ship.transform.position, Quaternion.identity);
                player.transform.SetParent(ship.transform);
                player.gameObject.name = "Player";
            }
            if (KIM_PlayerTransit.Instance.idx1 == 2)
            {
                GameObject player = PhotonNetwork.Instantiate("Player3", ship.transform.position, Quaternion.identity);
                player.transform.SetParent(ship.transform);
                player.gameObject.name = "Player";
            }
            if (KIM_PlayerTransit.Instance.idx1 == 3)
            {
                GameObject player = PhotonNetwork.Instantiate("Player4", ship.transform.position, Quaternion.identity);
                player.transform.SetParent(ship.transform);
                player.gameObject.name = "Player";
            }
        }
        else
        {
            if (KIM_PlayerTransit.Instance.idx2 == 0)
            {
                GameObject player = PhotonNetwork.Instantiate("Player", ship.transform.position, Quaternion.identity);
                player.transform.SetParent(ship.transform);
                player.gameObject.name = "Player";
            }
            if (KIM_PlayerTransit.Instance.idx2 == 1)
            {
                GameObject player = PhotonNetwork.Instantiate("Player2", ship.transform.position, Quaternion.identity);
                player.transform.SetParent(ship.transform);
                player.gameObject.name = "Player";
            }
            if (KIM_PlayerTransit.Instance.idx2 == 2)
            {
                GameObject player = PhotonNetwork.Instantiate("Player3", ship.transform.position, Quaternion.identity);
                player.transform.SetParent(ship.transform);
                player.gameObject.name = "Player";
            }
            if (KIM_PlayerTransit.Instance.idx2 == 3)
            {
                GameObject player = PhotonNetwork.Instantiate("Player4", ship.transform.position, Quaternion.identity);
                player.transform.SetParent(ship.transform);
                player.gameObject.name = "Player";
            }
        }
    }


    // Update is called once per frame
    void Update()
    {
        gameTime += Time.deltaTime;
    }
}
