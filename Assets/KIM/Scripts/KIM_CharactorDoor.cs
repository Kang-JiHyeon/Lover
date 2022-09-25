using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class KIM_CharactorDoor : MonoBehaviourPun
{
    [SerializeField]
    int playerIdx;

    public GameObject playerText;
    SpriteRenderer render;
    float size = 0;
    // Start is called before the first frame update
    void Start()
    {
        render = GetComponent<SpriteRenderer>();
        size = render.size.x;
    }

    // Update is called once per frame
    void Update()
    {
        if (PhotonNetwork.PlayerList.Length >= playerIdx)
        {
            size = Mathf.Lerp(size, 0.57f, Time.deltaTime * 10);
            render.size = new Vector2(size, 2.56f);
            playerText.SetActive(true);
        }
        else
        {
            size = Mathf.Lerp(size, 1.28f, Time.deltaTime * 10);
            render.size = new Vector2(size, 2.56f);
            playerText.SetActive(false);
        }
    }
}
