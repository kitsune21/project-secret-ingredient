using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateController : MonoBehaviour 
{
    public PlayerState currentPlayerState = PlayerState.Playing;
    public GameObject endCredits;
    public Transform endCreditSceneCameraPosition;

    public PlayerState GetPlayerState() {
        return currentPlayerState;
    }

    public void UpdatePlayerState(PlayerState newPlayerState) {
        currentPlayerState = newPlayerState;
        if(newPlayerState == PlayerState.Victory) {
            Camera.main.GetComponent<CameraController>().UpdateStartLoction(endCreditSceneCameraPosition);
            endCredits.SetActive(true);
        }
    }

    public void WinButton() {
        UpdatePlayerState(PlayerState.Victory);
    }
}
