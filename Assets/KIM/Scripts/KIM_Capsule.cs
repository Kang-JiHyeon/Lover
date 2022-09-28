using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class KIM_Capsule : MonoBehaviourPun
{
    SpriteRenderer render;
    public Material flash;
    public Material defaultM;
    public Sprite[] sprites = new Sprite[4];
    int idx = 0;
    int hp = 3;
    // Start is called before the first frame update
    void Start()
    {
        render = GetComponentInChildren<SpriteRenderer>();
    }

    float currentTime = 0;
    // Update is called once per frame
    void Update()
    {
        if (photonView.IsMine)
            currentTime += Time.deltaTime;
        if (currentTime > 0.3f)
        {
            render.sprite = sprites[idx++];
            if (idx >= sprites.Length)
                idx = 0;
        }
        else if (currentTime > 0.6f)
        {
            render.sprite = sprites[idx++];
            if (idx >= sprites.Length)
                idx = 0;
        }
        else if (currentTime > 0.9f)
        {
            render.sprite = sprites[idx++];
            if (idx >= sprites.Length)
                idx = 0;
        }
        else if (currentTime > 1.2f)
        {
            render.sprite = sprites[idx++];
            if (idx >= sprites.Length)
                idx = 0;
            currentTime = 0;
        }

        if (hp <= 0)
        {
            if (photonView.IsMine)
            {
                GameObject present = PhotonNetwork.Instantiate("Present", transform.position, Quaternion.identity);
                PhotonNetwork.Destroy(gameObject);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        hp--;
        iTween.ScaleTo(gameObject, iTween.Hash("x", 0.7f, "y", 0.7f, "time", 0.1f, "easetype", iTween.EaseType.easeOutBounce));
        iTween.ScaleTo(gameObject, iTween.Hash("x", 1, "y", 1, "time", 0.1f, "easetype", iTween.EaseType.easeOutBounce, "delay", 0.16f));
        StopCoroutine("OnHitFlash");
        StartCoroutine("OnHitFlash");
    }
    IEnumerator OnHitFlash()
    {
        render.material = flash;
        yield return new WaitForSeconds(0.1f);
        render.material = defaultM;
    }
}
