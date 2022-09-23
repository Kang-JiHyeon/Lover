using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KIM_TimeCount : MonoBehaviour
{
    Text timeText;

    // Start is called before the first frame update
    void Start()
    {
        timeText = GetComponent<Text>();

        timeText.text = ((int)KIM_GameManager.Instance.GameTime / 60).ToString() + ":" + ((int)KIM_GameManager.Instance.GameTime % 60).ToString();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
            Application.Quit();
    }
}
