using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class KIM_Warp : MonoBehaviourPun
{
    AudioListener listener;
    AudioSource source;
    public AudioClip open;

    [SerializeField]
    int unlockCount;
    GameObject ship;
    CharacterController cc;
    public GameObject seal;
    public GameObject unSeal;
    KANG_CameraMove cm;

    public GameObject mapIconSeal;
    public GameObject mapIconUnSeal;

    bool isEnd;
    // Start is called before the first frame update
    void Start()
    {
        listener = GameObject.Find("Main Camera").GetComponent<AudioListener>();
        source = GetComponent<AudioSource>();
        unSeal.SetActive(false);
        ship = GameObject.Find("Spaceship");
        cc = ship.GetComponent<CharacterController>();
        cm = GameObject.Find("Main Camera").GetComponent<KANG_CameraMove>();

        mapIconSeal.SetActive(true);
        mapIconUnSeal.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (unlockCount <= KIM_GameManager.Instance.RescueCount)
        {
            if (seal.activeSelf)
            {
                cm.UnLock = true;
                source.PlayOneShot(open);

                mapIconSeal.SetActive(false);
                mapIconUnSeal.SetActive(true);
            }
         
            seal.SetActive(false);
            unSeal.SetActive(true);

        }
    }

    IEnumerator OnCollide()
    {
        float currentTime = 0;
        isEnd = true;
        while (currentTime < 3.0f)
        {
            currentTime += Time.deltaTime;
            cc.Move((transform.position - ship.transform.position) * Time.deltaTime);
            yield return null;
        }
        photonView.RPC("RPCOffListener", RpcTarget.All);
        PhotonNetwork.LoadLevel("KIM_Ending");
    }

    [PunRPC]
    void RPCOffListener()
    {
        listener.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (unSeal.activeSelf && other.gameObject.layer == LayerMask.NameToLayer("Ship") && !isEnd)
        {
            StartCoroutine("OnCollide");
        }
    }
}
