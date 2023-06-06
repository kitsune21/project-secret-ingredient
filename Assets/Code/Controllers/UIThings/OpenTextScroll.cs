using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class OpenTextScroll : MonoBehaviour
{
    private RectTransform myRect;
    public float scrollSpeed;
    public int endHeight;
    public string nextScene;
    private bool isDone;

    void Start() {
        myRect = GetComponent<RectTransform>();
    }
    
    void Update()
    {
        if(GameObject.FindGameObjectWithTag("music").GetComponent<MusicController>()) {
            GameObject.FindGameObjectWithTag("music").GetComponent<MusicController>().crossFadeClip("Menu");
        }
        myRect.anchoredPosition = new Vector2(myRect.anchoredPosition.x, myRect.anchoredPosition.y + scrollSpeed);
        if(Input.GetKey(KeyCode.Space)) {
            myRect.anchoredPosition = new Vector2(myRect.anchoredPosition.x, myRect.anchoredPosition.y + (scrollSpeed * 15));
        }
        if(myRect.anchoredPosition.y > endHeight) {
            SceneManager.LoadScene(nextScene);
            Destroy(gameObject);
        }
        if(Input.GetKeyDown(KeyCode.Escape)) {
            SceneManager.LoadScene(nextScene);
            Destroy(gameObject);
        }
    }
}
