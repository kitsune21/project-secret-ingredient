using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ObjectController : MonoBehaviour {
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
    public NPCCharacterController characterToTrigger;
    void Start() {
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
        if(myItem) {
            player.GetComponentInChildren<InventoryController>().AddItem(myItem, "Picked up ");
            interactableTextController.UpdateMyText("");
            Destroy(gameObject, 0.7f);
        }
        if(myCharacter) {
            DragItemController dragItemController = GameObject.FindGameObjectWithTag("DragItem").GetComponent<DragItemController>();
            bool isGivenWantedItem = false;
            if(dragItemController.myItem && dragItemController.myItem.id == myCharacter.wantedItem.id) {
                isGivenWantedItem = true;
                dragItemController.StopDragging();
            }
            player.GetComponentInChildren<DialogueController>().StartConversation(myCharacter, isGivenWantedItem);
        }
        if(myPuzzle && myPuzzle.requiredItem) {
            DragItemController dragItemController = GameObject.FindGameObjectWithTag("DragItem").GetComponent<DragItemController>();
            if(dragItemController.myItem && dragItemController.myItem.id == myPuzzle.requiredItem.id) {
                playerState.UpdatePlayerState(PlayerState.Playing);
                dragItemController.StopDragging();
                if(!myPuzzle.requiredItem.permanent) {
                    player.GetComponentInChildren<InventoryController>().RemoveItem(myPuzzle.requiredItem, "Used ");
                }
                if(myPuzzle.CheckAllPuzzles()) {
                    handleCompletePuzzle();
                    handleFinalPuzzle();
                    return;
                } else {
                    showFailText();
                }
            }
        }
        if(myPuzzle && !myPuzzle.requiredItem) {
            playerState.UpdatePlayerState(PlayerState.Playing);
            if(myPuzzle.CheckAllPuzzles()) {
                handleCompletePuzzle();
                handleFinalPuzzle();
            } else {
                showFailText();
            }
        }
        if(!myItem && !myCharacter) {
            player.GetComponentInChildren<DialogueController>().showLookAtText(onHoldText);
        }
    }

    private void showFailText() {
        if(myPuzzle && myPuzzle.failText.Length > 0) {
            player.GetComponentInChildren<DialogueController>().showLookAtTextWithNoOverride(myPuzzle.failText);
        }
    }

    private void handleFinalPuzzle() {
        if(myPuzzle && myPuzzle.finalPuzzle) {
            playerState.UpdatePlayerState(PlayerState.Victory);
        }
    }

    private void handleCompletePuzzle() {
        myPuzzle.completed = true;
        if(characterToTrigger) {
            characterToTrigger.TriggerCharacter(transform.position);
        }
        if(myPuzzle.giveItem) {
            player.GetComponentInChildren<InventoryController>().AddItem(myPuzzle.giveItem, "Picked up ");
        }
    }
}
