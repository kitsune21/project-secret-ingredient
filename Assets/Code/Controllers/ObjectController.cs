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

    void Start() {
        player = GameObject.FindGameObjectWithTag("Player");
        interactableTextController = GameObject.FindGameObjectWithTag("InteractableText").GetComponent<InteractableTextController>();
        if(myItem) {
            onHoverText = myItem.description;
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
            float distance = Vector3.Distance(gameObject.transform.position, player.transform.position);
            if(player.GetComponent<PlayerController>().GetRemainingDistance() <= 0.5f) {
                if(myItem) {
                    isClicked = false;
                    player.GetComponentInChildren<InventoryController>().AddItem(myItem);
                    isHovering = false;
                    interactableTextController.UpdateMyText("");
                    Destroy(gameObject);
                }
                if(myCharacter) {
                    player.GetComponentInChildren<DialogueController>().StartConversation(myCharacter.myDialogue);
                    isHovering = false;
                    isClicked = false;
                }
            }
        }
    }
}
