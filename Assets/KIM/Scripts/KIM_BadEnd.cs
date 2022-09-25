using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class KIM_BadEnd : MonoBehaviourPun
{
    AudioSource source;
    public AudioClip destroyedSound;

    public GameObject dieEffect;
    KANG_ShipHP sh;
    bool isEnd = false;
    public bool IsEnd
    {
        get { return isEnd; }
        set
        {
            if (value != isEnd && value == true)
            {
                StartCoroutine("OnEnd");
            }
            isEnd = value;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        source = GetComponent<AudioSource>();
        sh = GetComponent<KANG_ShipHP>();
    }

    // Update is called once per frame
    void Update()
    {
        if (sh.HP <= 0)
            IsEnd = true;
    }

    IEnumerator OnEnd()
    {
        source.PlayOneShot(destroyedSound);
        GameObject effect = Instantiate(dieEffect);
        effect.transform.position = transform.position;
        GetComponent<CharacterController>().enabled = false;
        GetComponent<SphereCollider>().enabled = false;

        float currentTime = 0;
        while (currentTime < 3.0f)
        {
            currentTime += Time.deltaTime;
            transform.Rotate(0, 0, 3f * Time.deltaTime);
            transform.position += Vector3.down * 3f * Time.deltaTime;
            yield return null;
        }

        PhotonNetwork.LoadLevel("KIM_BadEnding");
    }
}
