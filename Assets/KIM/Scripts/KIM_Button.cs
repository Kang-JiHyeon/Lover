using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KIM_Button : MonoBehaviour
{
    public bool clicked = false;

    public float enableTime = 3.0f;

    public KIM_Button button;

    public Sprite clickedSprite;
    public Sprite disableSprite;
    public Sprite enableSprite;

    SpriteRenderer render;
    // Start is called before the first frame update
    void Start()
    {
        render = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (button.clicked && clicked)
        {
            StopAllCoroutines();
            render.sprite = enableSprite;
        }
    }

    IEnumerator OnClicked()
    {
        clicked = true;
        render.sprite = clickedSprite;
        yield return new WaitForSeconds(0.2f);
        render.sprite = enableSprite;
        yield return new WaitForSeconds(enableTime);
        render.sprite = disableSprite;
        clicked = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("PlayerBullet"))
        {
            StopCoroutine("OnClicked");
            StartCoroutine("OnClicked");
        }
    }
}
