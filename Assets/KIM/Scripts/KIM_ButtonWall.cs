using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KIM_ButtonWall : MonoBehaviour
{
    public KIM_Button button;
    public KIM_Button button2;
    Vector2 size;
    float ySize;

    bool isOpen = false;

    GameObject wall1;
    GameObject wall2;
    // Start is called before the first frame update
    void Start()
    {
        wall1 = transform.Find("Wall1").gameObject;
        wall2 = transform.Find("Wall2").gameObject;
        size = wall1.GetComponent<SpriteRenderer>().size;
        ySize = size.y;
    }

    // Update is called once per frame
    void Update()
    {
        if (button.clicked && button2.clicked)
        {
            isOpen = true;
        }

        if (isOpen)
        {
            ySize = Mathf.Lerp(ySize, 1.6f, Time.deltaTime);
            size.y = ySize;
            wall1.GetComponent<SpriteRenderer>().size = size;
            wall2.GetComponent<SpriteRenderer>().size = size;
            wall1.GetComponent<BoxCollider>().enabled = false;
            wall2.GetComponent<BoxCollider>().enabled = false;
        }
    }
}
