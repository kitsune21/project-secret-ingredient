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

    void Start() {
        interactableTextController = GameObject.FindGameObjectWithTag("InteractableText").GetComponent<InteractableTextController>();
        onHoverText = "Go to " + myRoom.name;
        player = GameObject.FindGameObjectWithTag("Player");
        playerState = player.GetComponent<PlayerController>().GetPlayerStateController();
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

    void Update() {
        if (isHovering)
        {
            interactableTextController.UpdateMyText(onHoverText);
            if(Input.GetMouseButtonDown(0)) {
                isClicked = true;
            }
        }
        if(isClicked) {
            float distance = Vector3.Distance(transform.position, player.transform.position);
            if(player.GetComponent<PlayerController>().GetRemainingDistance() <= 0.5f && distance < 4f) {
                if(myRoom) {
                    interactableTextController.UpdateMyText("");
                    myCurrentRoomMesh.SetActive(false);
                    myRoomMesh.SetActive(true);
                    player.GetComponent<PlayerController>().SetNewDestination(doorExit);
                    Camera.main.GetComponent<CameraController>().UpdateStartLoction(newCameraStartLocation);
                    isHovering = false;
                    isClicked = false;
                    if(mySong) {
                        GameObject.FindGameObjectWithTag("music").GetComponent<MusicController>().crossFadeClip(mySong.clipName);
                    }
                }
            }
        }
    }
} 
