using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OpenTextScroll : MonoBehaviour
{
    private RectTransform myRect;
    public float scrollSpeed;
    public GameObject startPosition;
    private bool isDone;

    void Start() {
        myRect = GetComponent<RectTransform>();
        GameObject.Find("Player").GetComponent<PlayerController>().GetPlayerStateController().UpdatePlayerState(PlayerState.InDialogue);
    }
    
    void Update()
    {
        myRect.anchoredPosition = new Vector2(myRect.anchoredPosition.x, myRect.anchoredPosition.y + scrollSpeed);
        if(myRect.anchoredPosition.y > 950) {
            Camera.main.GetComponent<CameraController>().UpdateStartLoction(startPosition.transform);
            GameObject.Find("Player").GetComponent<PlayerController>().GetPlayerStateController().UpdatePlayerState(PlayerState.Playing);
            Destroy(gameObject);
        }
    }
}
