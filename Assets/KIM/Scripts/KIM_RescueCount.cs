using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KIM_RescueCount : MonoBehaviour
{
    Text rescueCount;
    // Start is called before the first frame update
    void Start()
    {
        rescueCount = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        rescueCount.text = KIM_GameManager.Instance.RescueCount.ToString() + "/5";
    }
}
