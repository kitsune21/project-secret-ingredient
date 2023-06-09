using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainMap : MonoBehaviour
{
    public Transform shinjuku;
    public Transform ikebukuro;
    public Transform akihabara;
    public Transform shinagawa;
    public Transform train;
    public GameObject mapPanel;
    public string onHoverText;
    public bool isHovering;
    public bool isClicked;
    private InteractableTextController interactableTextController;
    public string currentMachi;
    private PlayerController player;

    void Start() {
        player = GameObject.Find("Player").GetComponent<PlayerController>();
        interactableTextController = GameObject.FindGameObjectWithTag("InteractableText").GetComponent<InteractableTextController>();
        mapPanel.SetActive(false);
    }

    void Update() {
        if(player.GetPlayerStateController().GetPlayerState() == PlayerState.Playing) {
            if(isHovering) {
                interactableTextController.UpdateMyText(onHoverText);
                if(Input.GetMouseButtonDown(0)) {
                    isClicked = true;
                }
            }
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

    public void UpdateMachi(string newMachi) {
        isClicked = false;
        mapPanel.SetActive(false);
        if(newMachi == "shinjuku") {
            player.SetNewDestination(shinjuku);
            player.currentMachi = newMachi;
        }
        if(newMachi == "ikebukuro") {
            if(player.GetComponentInChildren<InventoryController>().CheckHasAllRequiredItems()) {
                player.GetComponentInChildren<DialogueController>().RunAssembleDialogue();
            }
            player.SetNewDestination(ikebukuro);
            player.currentMachi = newMachi;
        }
        if(newMachi == "akihabara") {
            player.SetNewDestination(akihabara);
            player.currentMachi = newMachi;
        }
        if(newMachi == "shinagawa") {
            player.SetNewDestination(shinagawa);
            player.currentMachi = newMachi;
        }
        if(newMachi == "train") {
            player.SetNewDestination(train);
            GameObject.FindGameObjectWithTag("train").GetComponent<TrainController>().SetStartingMachi(player.currentMachi);
            player.currentMachi = newMachi;
        }
        isClicked = false;
    }

    public void HandleInteraction() {
        mapPanel.SetActive(true);
        isClicked = false;
        player.GetPlayerStateController().UpdatePlayerState(PlayerState.InScene);
    }
}
