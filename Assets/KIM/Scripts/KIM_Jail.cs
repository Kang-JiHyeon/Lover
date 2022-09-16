using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KIM_Jail : MonoBehaviour
{
    float hp = 8;

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

    bool isRescue = false;
    // Start is called before the first frame update
    void Start()
    {
        ship = GameObject.Find("Spaceship");

        hostage = transform.Find("Hostage").gameObject;
        destroyed = transform.Find("Destroyed").gameObject;
        notDestroyed = transform.Find("NotDestroyed").gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (hp <= 0)
        {
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
        GameObject effect = Instantiate(rescueEffect);
        effect.transform.position = ship.transform.position;
        effect.transform.SetParent(ship.transform);
        Destroy(effect, 2.5f);
        yield return new WaitForSeconds(2.0f);
        hostage.SetActive(false);
        destroyed.transform.Find("Alarm").gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("PlayerBullet"))
        {
            hp--;
        }
    }
}
