using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KIM_EndSlider : MonoBehaviour
{
    Slider slider;
    public bool isRecue;
    // Start is called before the first frame update
    void Start()
    {
        slider = GetComponent<Slider>();
        if (isRecue)
        {
            slider.value = (float)KIM_GameManager.Instance.RescueCount / 5f;
        }
        else
        {
            slider.value = 1f - (float)KIM_GameManager.Instance.RescueCount / 5f;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
