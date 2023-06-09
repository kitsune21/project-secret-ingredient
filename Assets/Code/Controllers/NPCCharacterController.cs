using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPCCharacterController : MonoBehaviour
{
    public Character myCharacter;
    public bool isClicked;
    private bool isHovering = false;
    public bool isTriggered;
    private NavMeshAgent navMeshAgent;
    private DialogueController dialogueController;
    private PlayerController player;
    private PlayerStateController playerState;
    private InteractableTextController interactableTextController;
    public string triggeredText;
    public bool noInteraction;
    public Puzzle myPuzzle;
    public Dialogue overwriteDialogueObj;
    public Puzzle assemblePuzzle;
    public Transform finalCameraPos;
    public Transform finalPlayerPos;

    void Start() {
        player = GameObject.Find("Player").GetComponent<PlayerController>();
        dialogueController = player.GetComponentInChildren<DialogueController>();
        interactableTextController = GameObject.FindGameObjectWithTag("InteractableText").GetComponent<InteractableTextController>();
        playerState = player.GetPlayerStateController();
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.updateRotation = false;
        navMeshAgent.updateUpAxis = false;
        myCharacter.resetDialogue();
    }

    void Update() {
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
        if(isClicked && !noInteraction) {
            float distance = Vector3.Distance(transform.position, player.transform.position);
            if(playerState.GetPlayerState() == PlayerState.Playing) {
                if(player.GetComponent<PlayerController>().GetRemainingDistance() <= 0.5f && distance < 4f) {
                    DragItemController dragItemController = GameObject.FindGameObjectWithTag("DragItem").GetComponent<DragItemController>();
                    bool isGivenWantedItem = false;
                    if(dragItemController.myItem && dragItemController.myItem.id == myCharacter.wantedItem.id) {
                        isGivenWantedItem = true;
                    }
                    dragItemController.StopDragging();
                    dialogueController.StartConversation(myCharacter, isGivenWantedItem, this);
                    interactableTextController.UpdateMyText("");
                    if(myPuzzle) {
                        myPuzzle.completed = true;
                    }
                    isClicked = false;
                }
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
        interactableTextController.UpdateMyText("");
    }

    public void StartConversationWithNPC() {
        if(assemblePuzzle && assemblePuzzle.completed) {
            player.GetComponent<PlayerController>().SetNewDestination(finalPlayerPos);
            Camera.main.GetComponent<CameraController>().UpdateStartLoction(finalCameraPos);
            dialogueController.RunFinalDialogue();
        } else {
            dialogueController.StartConversation(myCharacter, false, this);
            interactableTextController.UpdateMyText("");
            if(myPuzzle) {
                myPuzzle.completed = true;
            }

        }
        isClicked = false;
    }

    public void TriggerCharacter(Vector3 newPosition) {
        navMeshAgent.SetDestination(newPosition);
        isTriggered = true;
        dialogueController.showLookAtTextNPC(triggeredText, myCharacter);
        isClicked = false;
    }

    public void SetNewDestination(Transform newPos) {
        navMeshAgent.SetDestination(newPos.position);
    }

    public void ChangeDialogue(Dialogue newDialogue) {
        myCharacter.allDialogues.Clear();
        myCharacter.allDialogues.Add(newDialogue);
    }

    public void ClearDialoge() {
        myCharacter.allDialogues.Clear();
    }

    public void OverWriteDialogue() {
        myCharacter.allDialogues.Clear();
        myCharacter.allDialogues.Add(overwriteDialogueObj);
    }
}
