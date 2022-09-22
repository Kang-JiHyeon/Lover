using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KIM_UIAnim : MonoBehaviour
{
    float time = 0;
    public float fadeTime = 0.6f;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        if (time < fadeTime)
            GetComponent<Text>().color = new Color(1, 1, 1, 1f - time / fadeTime);
        else if (time >= fadeTime && time < fadeTime * 2)
            GetComponent<Text>().color = new Color(1, 1, 1, (time - fadeTime) / fadeTime);
        else
            time = 0;
    }
}
