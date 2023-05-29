using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateController : MonoBehaviour 
{
    private static PlayerStateController instance;
    
    private PlayerState currentPlayerState = PlayerState.Playing;
    public GameObject victoryText;

    void Start() {
        if(instance == null) {
            instance = this;
        } else {
            Destroy(gameObject);
        }
    }

    public static PlayerStateController Instance
    {
        get { return instance; }
    }

    public PlayerState GetPlayerState() {
        return currentPlayerState;
    }

    public void UpdatePlayerState(PlayerState newPlayerState) {
        currentPlayerState = newPlayerState;
        if(newPlayerState == PlayerState.Victory) {
            victoryText.SetActive(true);
        }
    }
}
