using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    public Transform doorExit;
    public Transform newCameraStartLocation;
    private InteractableTextController interactableTextController;
    private bool isHovering;
    private bool isClicked;
    private string onHoverText;
    public Room myRoom;
    public GameObject myRoomMesh;
    public GameObject myCurrentRoomMesh;
    private GameObject player;
    private PlayerStateController playerState;
    public ClipScript mySong;
    public Item requiredItem;
    public string noItemYet;
    public string hideRoomName;
    public string newCurrentMachi;
    public Puzzle myPuzzle;

    void Start() {
        interactableTextController = GameObject.FindGameObjectWithTag("InteractableText").GetComponent<InteractableTextController>();
        onHoverText = "Go to " + myRoom.roomName;
        if(hideRoomName.Length > 0) {
            onHoverText = "Go to " + hideRoomName;
        }
        player = GameObject.FindGameObjectWithTag("Player");
        playerState = player.GetComponent<PlayerController>().GetPlayerStateController();
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

    void Update() {
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
        if(isClicked) {
            isHovering = false;
            float distance = Vector3.Distance(transform.position, player.transform.position);
            if(player.GetComponent<PlayerController>().GetRemainingDistance() <= 0.5f && distance < 4f) {
                if(myPuzzle && !myPuzzle.completed) {
                    player.GetComponentInChildren<DialogueController>().showLookAtTextWithNoOverride(noItemYet);
                    return;
                }
                if(myRoom) {
                    if(requiredItem) {
                        if(player.GetComponentInChildren<InventoryController>().CheckIfPlayerHasItem(requiredItem)) {
                            interactableTextController.UpdateMyText("");
                            myCurrentRoomMesh.SetActive(false);
                            myRoomMesh.SetActive(true);
                            player.GetComponent<PlayerController>().SetNewDestination(doorExit);
                            Camera.main.GetComponent<CameraController>().UpdateStartLoction(newCameraStartLocation);
                            isClicked = false;
                            myCurrentRoomMesh.SetActive(true);
                            if(mySong) {
                                GameObject.FindGameObjectWithTag("music").GetComponent<MusicController>().crossFadeClip(mySong.clipName);
                            }
                        } else {
                            player.GetComponentInChildren<DialogueController>().showLookAtTextWithNoOverride(noItemYet);
                        }
                    } else {
                        interactableTextController.UpdateMyText("");
                        myCurrentRoomMesh.SetActive(false);
                        myRoomMesh.SetActive(true);
                        player.GetComponent<PlayerController>().SetNewDestination(doorExit);
                        Camera.main.GetComponent<CameraController>().UpdateStartLoction(newCameraStartLocation);
                        isClicked = false;
                        myCurrentRoomMesh.SetActive(true);
                        if(mySong) {
                            GameObject.FindGameObjectWithTag("music").GetComponent<MusicController>().crossFadeClip(mySong.clipName);
                        }
                    }
                    GameObject.FindGameObjectWithTag("train").GetComponent<TrainController>().ExitTrain();
                    if(newCurrentMachi.Length > 0) {
                        player.GetComponent<PlayerController>().currentMachi = newCurrentMachi;
                    }
                }
            }
        }
    }
} 
