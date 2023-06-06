using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Logo : MonoBehaviour
{
    public string nextScene;
    public float logoDuration;

    void Start() {
        StartCoroutine(NextScene(logoDuration));
    }

    void Update() {
        if(Input.GetKeyDown(KeyCode.Escape)) {
            SceneManager.LoadScene(nextScene);
            StopAllCoroutines();
        }
    }

    private IEnumerator NextScene(float duration)
    {
        yield return new WaitForSeconds(duration);

        SceneManager.LoadScene(nextScene);
        StopAllCoroutines();
    }
}
