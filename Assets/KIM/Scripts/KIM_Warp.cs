using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class KIM_Warp : MonoBehaviourPun
{
    [SerializeField]
    int unlockCount;

    public GameObject seal;
    public GameObject unSeal;
    // Start is called before the first frame update
    void Start()
    {
        unSeal.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (unlockCount <= KIM_GameManager.Instance.RescueCount)
        {
            seal.SetActive(false);
            unSeal.SetActive(true);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (unSeal.activeSelf && other.gameObject.layer == LayerMask.NameToLayer("Ship"))
        {
            PhotonNetwork.LoadLevel("KIM_Ending");
        }
    }
}
