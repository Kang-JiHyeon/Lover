using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KIM_Crystal : MonoBehaviour
{
    public GameObject effectF;

    // Start is called before the first frame update
    void Start()
    {
         
    }

    // Update is called once per frame
    void Update()
    {
        transform.localPosition = new Vector3(0, 1f, 0);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Module"))
        {
            //�ʿ� �ε�����
            if (other.gameObject.name.Contains("Map"))
                return;
            //��⿡ �ε��� �� ��������Ʈ �����Ű�� �ı���
            if (!gameObject.name.Contains("CrowBar"))
            {
                other.transform.Find("CrystalPot").GetComponent<SpriteRenderer>().sprite = GetComponentInChildren<SpriteRenderer>().sprite;
                GameObject effect = Instantiate(effectF);
                effect.transform.position = transform.position;
                Destroy(gameObject);
            }
            //���� ũ�ο�ٶ�� ��⿡ �ε��� �� ��������Ʈ �����Ű�� �ڽ��� ��������Ʈ�� ����, ���ÿ� �̸��� ����
            else
            {
                GetComponentInChildren<SpriteRenderer>().sprite = other.transform.Find("CrystalPot").GetComponent<SpriteRenderer>().sprite;
                if (other.name.Contains("Beam") || other.name.Contains("Metal"))
                {
                    transform.GetChild(0).localScale = Vector3.one * 0.5f;
                }
                gameObject.name = GetComponentInChildren<SpriteRenderer>().sprite.name;
                other.transform.Find("CrystalPot").GetComponent<SpriteRenderer>().sprite = null;
            }
        }
    }
}
