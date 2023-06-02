using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPCCharacterController : MonoBehaviour
{
    public Character myCharacter;
    private bool isClicked;
    private bool isHovering = false;
    public bool isTriggered;
    private NavMeshAgent navMeshAgent;
    private DialogueController dialogueController;
    private PlayerController player;
    private PlayerStateController playerState;
    private InteractableTextController interactableTextController;
    public string triggeredText;

    void Start() {
        player = GameObject.Find("Player").GetComponent<PlayerController>();
        dialogueController = player.GetComponentInChildren<DialogueController>();
        interactableTextController = GameObject.FindGameObjectWithTag("InteractableText").GetComponent<InteractableTextController>();
        playerState = player.GetPlayerStateController();
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.updateRotation = false;
        navMeshAgent.updateUpAxis = false;
        navMeshAgent.isStopped = true;
    }

    void FixedUpdate() {
        if(playerState == null) {
            playerState = player.GetPlayerStateController();
        }
        if (isHovering)
        {
            interactableTextController.UpdateMyText(myCharacter.onHoverText);
            if(Input.GetMouseButtonDown(0)) {
                isClicked = true;
            }
        }
        if(isClicked) {
            float distance = Vector3.Distance(transform.position, player.transform.position);
            if(player.GetComponent<PlayerController>().GetRemainingDistance() <= 0.5f && distance < 4f) {
                DragItemController dragItemController = GameObject.FindGameObjectWithTag("DragItem").GetComponent<DragItemController>();
                bool isGivenWantedItem = false;
                if(dragItemController.myItem && dragItemController.myItem.id == myCharacter.wantedItem.id) {
                    isGivenWantedItem = true;
                    dragItemController.StopDragging();
                }
                dialogueController.StartConversation(myCharacter, isGivenWantedItem);
                interactableTextController.UpdateMyText("");
                isHovering = false;
                isClicked = false;
            }
        }
    }

    private void OnMouseEnter()
    {
        if(playerState == null) {
            playerState = player.GetPlayerStateController();
        }
        if(playerState.GetPlayerState() == PlayerState.Playing || playerState.GetPlayerState() == PlayerState.DraggingInventory) {
            isHovering = true;
        }
    }

    private void OnMouseExit()
    {
        isHovering = false;
    }

    public void StartConversationWithNPC() {
        dialogueController.StartConversation(myCharacter, false);
        interactableTextController.UpdateMyText("");
        isHovering = false;
        isClicked = false;
    }

    public void TriggerCharacter(Vector3 newPosition) {
        navMeshAgent.SetDestination(newPosition);
        isTriggered = true;
        dialogueController.showLookAtTextNPC(triggeredText, myCharacter);
        isHovering = false;
        isClicked = false;
    }
}
