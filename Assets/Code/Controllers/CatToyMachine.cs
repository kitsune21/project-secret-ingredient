using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CatToyMachine : MonoBehaviour {
    public string onHoverText;
    public string onHoldText;
    public Item myItem;
    public Character myCharacter;
    public bool isClicked;
    private bool isHovering = false;
    private InteractableTextController interactableTextController;
    private GameObject player;
    private PlayerStateController playerState;
    public Puzzle myPuzzle;
    public GameObject guyPlayingTheGame;
    public GameObject arcadeGameObject;
    void Start() {
        arcadeGameObject.SetActive(false);
        player = GameObject.FindGameObjectWithTag("Player");
        playerState = player.GetComponent<PlayerController>().GetPlayerStateController();
        interactableTextController = GameObject.FindGameObjectWithTag("InteractableText").GetComponent<InteractableTextController>();
        if(myItem) {
            onHoverText = myItem.itemName;
            onHoldText = myItem.onHoldText;
        }
        if(myPuzzle) {
            myPuzzle.completed = false;
        }
        if(myCharacter) {
            myCharacter.resetDialogue();
        }
    }

    private void OnMouseEnter()
    {
        isHovering = true;
    }

    private void OnMouseExit()
    {
        isHovering = false;
        interactableTextController.UpdateMyText("");
    }

    private void Update() {
        if(playerState == null) {
            playerState = player.GetComponent<PlayerController>().GetPlayerStateController();
        }
        if(playerState.GetPlayerState() != PlayerState.Victory && playerState.GetPlayerState() != PlayerState.InDialogue) {
            if (isHovering)
            {
                interactableTextController.UpdateMyText(onHoverText);
                if(playerState.GetPlayerState() == PlayerState.InDialogue) {
                    interactableTextController.UpdateMyText("");
                }
                if(Input.GetMouseButtonDown(0)) {
                    isClicked = true;
                }
            }
        }
        if(guyPlayingTheGame.activeSelf) {
            if(myPuzzle.completed) {
                guyPlayingTheGame.SetActive(false);
            }
        }
    }

    private void OnTriggerStay2D(Collider2D other) {
        if(other.tag == "Player") {
            if(isClicked) {
                HandleInteraction();
                other.GetComponent<PlayerController>().TriggerIneraction();
            }
        }
    }

    public void HandleInteraction()
    {
        isClicked = false;
        if(guyPlayingTheGame.activeSelf) {
            player.GetComponentInChildren<DialogueController>().StartConversation(myCharacter, false);
        } else {
            arcadeGameObject.SetActive(true);
            Camera.main.GetComponent<CameraController>().UpdateStartLoction(arcadeGameObject.transform);
            playerState.UpdatePlayerState(PlayerState.InScene);
        }
    }
}
