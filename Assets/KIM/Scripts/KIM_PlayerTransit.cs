using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KIM_PlayerTransit : MonoBehaviour
{
    public static KIM_PlayerTransit Instance;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        Instance = this;
    }

    public int idx1;
    public int idx2;
    public int idx3;
    public int idx4;

    public int playerIndex;
}
