using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    private static SceneController instance;
    public static SceneController Instance { get { return instance; } }

    private string previousScene;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        GameObject[] startLocations = GameObject.FindGameObjectsWithTag("Start");
        foreach(GameObject point in startLocations) {
            if(point.GetComponent<StartLocation>().previousSceneName == previousScene) {
                GameObject player = GameObject.Find("Player");
                player.transform.position = point.transform.position;
            }
        }
    }

    public string GetPreviousScene()
    {
        return previousScene;
    }

    public void LoadNextScene(string nextScene) {
        previousScene = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(nextScene);
    }
}
