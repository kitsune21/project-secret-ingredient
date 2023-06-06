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
    private string currentMachi = "shinjuku";
    private PlayerController player;

    void Start() {
        player = GameObject.Find("Player").GetComponent<PlayerController>();
        interactableTextController = GameObject.FindGameObjectWithTag("InteractableText").GetComponent<InteractableTextController>();
        mapPanel.SetActive(false);
    }

    void Update() {
        if(isHovering) {
            interactableTextController.UpdateMyText(onHoverText);
            if(Input.GetMouseButtonDown(0)) {
                isClicked = true;
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
        currentMachi = newMachi;
        if(currentMachi == "shinjuku") {
            player.SetNewDestination(shinjuku);
        }
        if(currentMachi == "ikebukuro") {
            player.SetNewDestination(ikebukuro);
        }
        if(currentMachi == "akihabara") {
            player.SetNewDestination(akihabara);
        }
        if(currentMachi == "shinagawa") {
            player.SetNewDestination(shinagawa);
        }
        if(currentMachi == "train") {
            player.SetNewDestination(train);
        }
        mapPanel.SetActive(false);
    }

    public void HandleInteraction() {
        mapPanel.SetActive(true);
        isClicked = false;
    }
}
