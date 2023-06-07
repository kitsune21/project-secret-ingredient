using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Logo : MonoBehaviour
{
    public string nextScene;
    public float logoDuration;
    public float delayAuidoLength;
    private AudioSource myAudio;

    void Start() {
        myAudio = GetComponent<AudioSource>();
        myAudio.Stop();
        StartCoroutine(NextScene(logoDuration));
        StartCoroutine(DelayAuido());
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

    private IEnumerator DelayAuido() {
        yield return new WaitForSeconds(delayAuidoLength);

        myAudio.Play();
    }
}
