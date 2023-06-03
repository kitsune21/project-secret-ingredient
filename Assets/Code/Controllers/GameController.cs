using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour {

    public GameObject settingsPanel;
    private bool openMenu = false;
    public Slider textSpeedSlider;

    void Start() {
        textSpeedSlider.value = textSpeedSlider.maxValue;
        DontDestroyOnLoad(gameObject);
    }

    void Update() {
        if(Input.GetKeyDown(KeyCode.Escape)) {
            openMenu = !openMenu;
            settingsPanel.GetComponent<Animator>().SetBool("open-game-menu", openMenu);
            if(openMenu) {
                GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().GetPlayerStateController().UpdatePlayerState(PlayerState.InInventory);
            } else {
                GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().GetPlayerStateController().UpdatePlayerState(PlayerState.Playing);
            }
        }
    }

    public void QuitGame() {
        Application.Quit();
    }

    public void resetGame() {
        SceneManager.LoadScene("MainMenu");
    }

    public float GetTextSpeed() {
        return textSpeedSlider.value / 10;
    }
}
