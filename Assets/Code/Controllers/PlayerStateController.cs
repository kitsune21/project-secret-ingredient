using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateController : MonoBehaviour 
{
    public PlayerState currentPlayerState = PlayerState.Playing;
    public GameObject victoryText;

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
