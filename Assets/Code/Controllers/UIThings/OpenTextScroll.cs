using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OpenTextScroll : MonoBehaviour
{
    private RectTransform myRect;
    public float scrollSpeed;
    public int endHeight;
    public GameObject startPosition;
    public Transform defaultPosition;
    private bool isDone;
    private bool foundMusicPlayer = false;

    void Start() {
        myRect = GetComponent<RectTransform>();
        GameObject.Find("Player").GetComponent<PlayerController>().GetPlayerStateController().UpdatePlayerState(PlayerState.InScene);
        Camera.main.GetComponent<CameraController>().UpdateStartLoction(defaultPosition);
    }
    
    void Update()
    {
        myRect.anchoredPosition = new Vector2(myRect.anchoredPosition.x, myRect.anchoredPosition.y + scrollSpeed);
        if(Input.GetKey(KeyCode.Space)) {
            myRect.anchoredPosition = new Vector2(myRect.anchoredPosition.x, myRect.anchoredPosition.y + (scrollSpeed * 4));
        }
        if(myRect.anchoredPosition.y > endHeight) {
            Camera.main.GetComponent<CameraController>().UpdateStartLoction(startPosition.transform);
            GameObject.Find("Player").GetComponent<PlayerController>().GetPlayerStateController().UpdatePlayerState(PlayerState.Playing);
            Destroy(gameObject);
        }
        if(!foundMusicPlayer) {
            if(GameObject.FindGameObjectWithTag("music").GetComponent<MusicController>()) {
                GameObject.FindGameObjectWithTag("music").GetComponent<MusicController>().loopClip("Shinagawa");
                foundMusicPlayer = true;
            }
        }
    }
}
