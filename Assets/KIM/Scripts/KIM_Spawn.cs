using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KIM_Spawn : MonoBehaviour
{
    public GameObject spawn;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    float currentTime = 0;
    public float spawnTime;
    // Update is called once per frame
    void Update()
    {
        currentTime += Time.deltaTime;
        if (currentTime > spawnTime)
        {
            GameObject enemy = Instantiate(spawn);
            enemy.transform.position = transform.position;
            currentTime = 0;
        }
    }
}
