using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KIM_ShipHit : MonoBehaviour
{
    public GameObject HittedEffect;
    KANG_CameraMove cm;
    // Start is called before the first frame update
    void Start()
    {
        cm = Camera.main.gameObject.GetComponent<KANG_CameraMove>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Enemy") || other.gameObject.layer == LayerMask.NameToLayer("Map"))
        {
            GameObject effect = Instantiate(HittedEffect);
            RaycastHit hit;
            if (Physics.Raycast(transform.position, other.transform.position - transform.position, out hit, 10f, LayerMask.GetMask("Enemy"))
                || Physics.Raycast(transform.position, other.transform.position - transform.position, out hit, 10f, LayerMask.GetMask("Map")))
            {
                Debug.Log("Point of contact: " + hit.point + hit.collider.name);
            }
            effect.transform.position = hit.point;
            Destroy(effect, 3.0f);

            cm.CamShake();
        }
        else if (other.gameObject.layer == LayerMask.NameToLayer("EnemyBullet"))
        {
            cm.CamShake();
        }
    }
}
