using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneTrigger : MonoBehaviour
{
    public NPCCharacterController character1;
    public NPCCharacterController character2;
    private bool isWatchingScene;
    private bool characterTriggered;
    private PlayerState currentState;
    private PlayerController player;
    public Transform character1TriggerPos;

    void FixedUpdate() {
        if(isWatchingScene) {
            currentState = player.GetPlayerStateController().GetPlayerState();
            if(currentState == PlayerState.Playing) {
                if(!characterTriggered) {
                    character1.TriggerCharacter(character1TriggerPos.position);
                    characterTriggered = true;
                    Destroy(gameObject, 3f);
                    Destroy(character1.gameObject, 2.5f);
                }
            }
        }
    }

    private void OnTriggerStay2D(Collider2D other) {
        if(!isWatchingScene) {
            if(other.tag == "Player") {
                player = other.GetComponent<PlayerController>();
                player.GetPlayerStateController().UpdatePlayerState(PlayerState.InDialogue);
                character1.StartConversationWithNPC();
                player.SetNewDestinationVector(other.transform.position);
                isWatchingScene = true;
                Debug.Log("should stop");
            }
        }
    }
}
