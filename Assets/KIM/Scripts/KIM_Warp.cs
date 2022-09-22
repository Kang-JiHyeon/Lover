using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class KIM_Warp : MonoBehaviourPun
{
    [SerializeField]
    int unlockCount;
    GameObject ship;
    CharacterController cc;
    public GameObject seal;
    public GameObject unSeal;

    bool isEnd;
    // Start is called before the first frame update
    void Start()
    {
        unSeal.SetActive(false);
        ship = GameObject.Find("Spaceship");
        cc = ship.GetComponent<CharacterController>();
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
        PhotonNetwork.LoadLevel("KIM_Ending");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (unSeal.activeSelf && other.gameObject.layer == LayerMask.NameToLayer("Ship") && !isEnd)
        {
            StartCoroutine("OnCollide");
        }
    }
}
