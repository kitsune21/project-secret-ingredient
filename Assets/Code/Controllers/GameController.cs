using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour {

    public GameObject settingsPanel;
    private bool openMenu = false;
    public Slider textSpeedSlider;
    private PlayerStateController playerState;

    void Start() {
        textSpeedSlider.value = textSpeedSlider.maxValue;
        DontDestroyOnLoad(gameObject);
        playerState = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().GetPlayerStateController();
    }

    void Update() {
        if(playerState.GetPlayerState() == PlayerState.Playing || playerState.GetPlayerState() == PlayerState.InMenu || playerState.GetPlayerState() == PlayerState.Victory) {
            if(Input.GetKeyDown(KeyCode.Escape)) {
                openMenu = !openMenu;
                settingsPanel.GetComponent<Animator>().SetBool("open-game-menu", openMenu);
                if(openMenu) {
                    playerState.UpdatePlayerState(PlayerState.InMenu);
                } else {
                    playerState.UpdatePlayerState(PlayerState.Playing);
                }
            }
        }
    }

    public void QuitGame() {
        Application.Quit();
    }

    public void resetGame() {
        Destroy(GameObject.Find("MusicController"));
        SceneManager.LoadScene("MainMenu");
    }

    public float GetTextSpeed() {
        return textSpeedSlider.value / 10;
    }
}
