using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KIM_Jail : MonoBehaviour
{
    AudioSource source;

    GameObject friend;

    public AudioClip alarm;
    public AudioClip hittedSound;
    public AudioClip destroyedSound;
    public AudioClip rescueSound;

    float hp = 8;
    public float HP
    {
        get { return hp; }
        set
        {
            if (value != hp)
            {
                iTween.ScaleTo(gameObject, iTween.Hash("x", 0.7f, "y", 0.7f, "time", 0.1f, "easetype", iTween.EaseType.easeOutBounce));
                iTween.ScaleTo(gameObject, iTween.Hash("x", 1f, "y", 1f, "time", 0.1f, "easetype", iTween.EaseType.easeOutBounce, "delay", 0.16f));
                StopCoroutine("OnHitFlash");
                StartCoroutine("OnHitFlash");
            }
            hp = value;
        }
    }

    IEnumerator OnHitFlash()
    {
        jailBody.GetComponent<SpriteRenderer>().material = flash;
        jailHead.GetComponent<SpriteRenderer>().material = flash;
        yield return new WaitForSeconds(0.1f);
        jailBody.GetComponent<SpriteRenderer>().material = defaultM;
        jailHead.GetComponent<SpriteRenderer>().material = defaultM;
    }

    public GameObject jailHead;
    public GameObject jailBody;
    public GameObject rescueEffect;

    public Sprite damaged;
    public Sprite damaged2;
    public Sprite damaged3;
    public Sprite bodyDamaged;
    public Sprite bodyDamaged2;
    public Sprite bodyDamaged3;

    public Sprite rescued;

    GameObject ship;
    GameObject destroyed;
    GameObject notDestroyed;
    GameObject hostage;

    public Material flash;
    public Material defaultM;

    bool isRescue = false;
    // Start is called before the first frame update
    void Start()
    {
        friend = transform.Find("Friend").gameObject;
        ship = GameObject.Find("Spaceship");
        source = GetComponent<AudioSource>();
        hostage = transform.Find("Hostage").gameObject;
        destroyed = transform.Find("Destroyed").gameObject;
        notDestroyed = transform.Find("NotDestroyed").gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (hp <= 0)
        {
            if (notDestroyed.activeSelf)
            {
                source.PlayOneShot(alarm);
                source.PlayOneShot(destroyedSound);
                GetComponent<BoxCollider>().enabled = false;
                friend.SetActive(false);
            }
            hostage.transform.up = Vector3.up;
            notDestroyed.SetActive(false);
            destroyed.SetActive(true);
            hostage.GetComponent<SpriteRenderer>().sprite = rescued;

            if (Vector3.Distance(ship.transform.position, hostage.transform.position) > 3.0f && hostage)
            {
                hostage.transform.position = Vector3.Lerp(hostage.transform.position, ship.transform.position, Time.deltaTime); 
                hostage.transform.localScale = Vector3.one * 0.56f;
            }
            else if (hostage)
            {
                hostage.transform.position = Vector3.Lerp(hostage.transform.position, ship.transform.position, Time.deltaTime);
                hostage.transform.localScale = Vector3.one * 0.56f;
                if (!isRescue)
                    StartCoroutine(Rescue());
                isRescue = true;
            }
        }
        else if (hp <= 2)
        {
            jailHead.GetComponent<SpriteRenderer>().sprite = damaged3;
            jailBody.GetComponent<SpriteRenderer>().sprite = bodyDamaged3;
        }
        else if (hp <= 4)
        {
            jailHead.GetComponent<SpriteRenderer>().sprite = damaged2;
            jailBody.GetComponent<SpriteRenderer>().sprite = bodyDamaged2;
        }
        else if (hp <= 6)
        {
            jailHead.GetComponent<SpriteRenderer>().sprite = damaged;
            jailBody.GetComponent<SpriteRenderer>().sprite = bodyDamaged;
        }
    }

    IEnumerator Rescue()
    {
        KIM_GameManager.Instance.RescueCount++;
        GameObject effect = Instantiate(rescueEffect);
        effect.transform.position = ship.transform.position;
        effect.transform.SetParent(ship.transform);
        Destroy(effect, 2.5f);
        yield return new WaitForSeconds(2.0f);
        source.PlayOneShot(rescueSound);
        hostage.SetActive(false);
        destroyed.transform.Find("Alarm").gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("PlayerBullet"))
        {
            HP--;
            source.PlayOneShot(hittedSound);
        }
    }
}
