using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ObjectController : MonoBehaviour {
    public string onHoverText;
    public string onHoldText;
    public Item myItem;
    public Character myCharacter;
    private bool isClicked;
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
    }

    private void OnMouseEnter()
    {
        playerState = player.GetComponent<PlayerController>().GetPlayerStateController();
        if(playerState.GetPlayerState() == PlayerState.Playing || playerState.GetPlayerState() == PlayerState.DraggingInventory) {
            isHovering = true;
        }
    }

    private void OnMouseExit()
    {
        isHovering = false;
        interactableTextController.UpdateMyText("");
    }

    private void Update()
    {
        if(playerState == null) {
            playerState = player.GetComponent<PlayerController>().GetPlayerStateController();
        }
        if(playerState.GetPlayerState() != PlayerState.Victory && playerState.GetPlayerState() != PlayerState.InDialogue) {
            if (isHovering)
            {
                interactableTextController.UpdateMyText(onHoverText);
                if(Input.GetMouseButtonDown(0)) {
                    isClicked = true;
                }
            }
            if(isClicked) {
                float distance = Vector3.Distance(transform.position, player.transform.position);
                if(player.GetComponent<PlayerController>().GetRemainingDistance() <= 0.05f && distance < 2f) {
                    if(myItem) {
                        isClicked = false;
                        isHovering = false;
                        player.GetComponentInChildren<InventoryController>().AddItem(myItem, "Picked up ");
                        interactableTextController.UpdateMyText("");
                        Destroy(gameObject);
                    }
                    if(myCharacter) {
                        DragItemController dragItemController = GameObject.FindGameObjectWithTag("DragItem").GetComponent<DragItemController>();
                        bool isGivenWantedItem = false;
                        if(dragItemController.myItem && dragItemController.myItem.id == myCharacter.wantedItem.id) {
                            isGivenWantedItem = true;
                            dragItemController.StopDragging();
                        }
                        player.GetComponentInChildren<DialogueController>().StartConversation(myCharacter, isGivenWantedItem);
                        isHovering = false;
                        isClicked = false;
                    }
                    if(myPuzzle && myPuzzle.requiredItem) {
                        DragItemController dragItemController = GameObject.FindGameObjectWithTag("DragItem").GetComponent<DragItemController>();
                        if(dragItemController.myItem && dragItemController.myItem.id == myPuzzle.requiredItem.id) {
                            dragItemController.StopDragging();
                            player.GetComponentInChildren<InventoryController>().RemoveItem(myPuzzle.requiredItem, "Used ");
                            if(myPuzzle.CheckAllPuzzles()) {
                                handleCompletePuzzle();
                                handleFinalPuzzle();
                                return;
                            } else {
                                Debug.Log("this??");
                                showFailText();
                            }
                        }
                    }
                    if(myPuzzle && !myPuzzle.requiredItem) {
                        if(myPuzzle.CheckAllPuzzles()) {
                            handleCompletePuzzle();
                            handleFinalPuzzle();
                        } else {
                            showFailText();
                        }
                    }
                    if(!myItem && !myCharacter) {
                        player.GetComponentInChildren<DialogueController>().showLookAtText(onHoldText);
                        isHovering = false;
                        isClicked = false;
                    }
                }
            }
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
    }
}
