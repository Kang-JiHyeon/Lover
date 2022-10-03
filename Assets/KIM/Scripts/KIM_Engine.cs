using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class KIM_Engine : MonoBehaviourPun
{
    AudioSource source;
    public AudioClip clip;
    public AudioClip beamCharged;
    public AudioClip beamRelease;
    float currentTime = 0;
    public GameObject engineEffect;

    // Start is called before the first frame update
    void Start()
    {
        source = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void BeamCharged()
    {
        source.PlayOneShot(beamCharged);
    }

    public void BeamRelease()
    {
        source.PlayOneShot(beamRelease);
    }

    public void CreateEffect()
    {
        photonView.RPC("RPCEffect", RpcTarget.All, Time.deltaTime);
    }

    public void EndSound()
    {
        photonView.RPC("RPCEndSound", RpcTarget.All);
    }

    [PunRPC]
    void RPCEndSound()
    {
        source.Stop();
        source.PlayOneShot(clip);
    }

    [PunRPC]
    void RPCEffect(float deltaTime)
    {
        currentTime += deltaTime;

        if (!source.isPlaying)
            source.Play();

        if (currentTime > 0.15f)
        {
            GameObject effect = Instantiate(engineEffect);
            effect.transform.position = transform.Find("EngineMachine").transform.Find("EnginePos").position +
                transform.Find("EngineMachine").transform.Find("EnginePos").up * 0.5f;
            effect.transform.up = transform.Find("EngineMachine").up;
            iTween.ScaleTo(effect, iTween.Hash("x", 3f, "y", 3f, "z", 3f, "time", 1.5f, "easetype", iTween.EaseType.easeOutQuart));
            StartCoroutine(OnColor(effect));
            Destroy(effect, 1.5f);
            currentTime = 0;
        }
    }

    IEnumerator OnColor(GameObject effect)
    {
        float coCurrentTime = 0;
        Color col = effect.GetComponent<SpriteRenderer>().color;
        
        while (coCurrentTime < 1.4f)
        {
            coCurrentTime += Time.deltaTime;
            col.a = Mathf.Lerp(col.a, 0, Time.deltaTime * 1.5f);
            effect.GetComponent<SpriteRenderer>().color = col;
            yield return null;
        }
    }
}
