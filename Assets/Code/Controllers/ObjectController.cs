using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ObjectController : MonoBehaviour {
    public string onHoverText;
    public Item myItem;
    public Character myCharacter;
    private bool isClicked;
    private bool isHovering = false;
    private InteractableTextController interactableTextController;
    private GameObject player;
    public Item requiredItem;
    void Start() {
        player = GameObject.FindGameObjectWithTag("Player");
        interactableTextController = GameObject.FindGameObjectWithTag("InteractableText").GetComponent<InteractableTextController>();
        if(myItem) {
            onHoverText = myItem.description;
        }
    }

    private void OnMouseEnter()
    {
        if(PlayerStateController.Instance.GetPlayerState() == PlayerState.Playing || PlayerStateController.Instance.GetPlayerState() == PlayerState.DraggingInventory) {
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
        if (isHovering)
        {
            interactableTextController.UpdateMyText(onHoverText);
            if(Input.GetMouseButtonDown(0)) {
                isClicked = true;
            }
        }
        if(isClicked) {
            float distance = Vector3.Distance(transform.position, player.transform.position);
            Debug.Log(distance);
            if(player.GetComponent<PlayerController>().GetRemainingDistance() <= 0.5f && distance < 4f) {
                if(myItem) {
                    isClicked = false;
                    isHovering = false;
                    player.GetComponentInChildren<InventoryController>().AddItem(myItem);
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
                if(requiredItem) {
                    if(player.GetComponentInChildren<InventoryController>().CheckIfPlayerHasItem(requiredItem)) {
                        player.GetComponentInChildren<InventoryController>().RemoveItem(requiredItem);
                        PlayerStateController.Instance.UpdatePlayerState(PlayerState.Victory);
                        DragItemController dragItemController = GameObject.FindGameObjectWithTag("DragItem").GetComponent<DragItemController>();
                        if(dragItemController.myItem && dragItemController.myItem.id == requiredItem.id) {
                            dragItemController.StopDragging();
                        }
                    }
                }
            }
        }
    }
}
