using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OpenTextScroll : MonoBehaviour
{
    private RectTransform myRect;
    public float scrollSpeed;
    public GameObject startPosition;
    public Transform defaultPosition;
    private bool isDone;

    void Start() {
        myRect = GetComponent<RectTransform>();
        GameObject.Find("Player").GetComponent<PlayerController>().GetPlayerStateController().UpdatePlayerState(PlayerState.InScene);
        GameObject.FindGameObjectWithTag("music").GetComponent<MusicController>().crossFadeClip("Akihabara");
        Camera.main.GetComponent<CameraController>().UpdateStartLoction(defaultPosition);
    }
    
    void Update()
    {
        myRect.anchoredPosition = new Vector2(myRect.anchoredPosition.x, myRect.anchoredPosition.y + scrollSpeed);
        if(Input.GetKey(KeyCode.Space)) {
            myRect.anchoredPosition = new Vector2(myRect.anchoredPosition.x, myRect.anchoredPosition.y + (scrollSpeed * 4));
        }
        if(myRect.anchoredPosition.y > 950) {
            Camera.main.GetComponent<CameraController>().UpdateStartLoction(startPosition.transform);
            GameObject.Find("Player").GetComponent<PlayerController>().GetPlayerStateController().UpdatePlayerState(PlayerState.Playing);
            Destroy(gameObject);
        }
    }
}
