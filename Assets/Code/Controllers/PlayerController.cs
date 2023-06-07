using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour {
    private NavMeshAgent navMeshAgent;
    private PlayerStateController playerState;
    private InventoryController myInventory;
    private DialogueController dialogueController;
    public string currentMachi;
    private Animator myAnim;
    private SpriteRenderer mySprite;
    private bool faceLeft;

    void Start() {
        playerState = GetComponentInChildren<PlayerStateController>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.updateRotation = false;
        navMeshAgent.updateUpAxis = false;
        GameObject.FindGameObjectWithTag("music").GetComponent<MusicController>().crossFadeClip("Shinagawa");
        currentMachi = "shinagawa";
        faceLeft = false;
        myAnim = GetComponent<Animator>();
        mySprite = GetComponent<SpriteRenderer>();
    }

    void Update() {
        if(playerState.GetPlayerState() == PlayerState.Playing || playerState.GetPlayerState() == PlayerState.DraggingInventory) {
            if (Input.GetMouseButton(0)) {
                Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                navMeshAgent.SetDestination(mousePosition);
                if(mousePosition.x > transform.position.x) {
                    faceLeft = false;
                    mySprite.flipX = faceLeft;
                } else {
                    faceLeft = true;
                    mySprite.flipX = faceLeft;
                }
                myAnim.SetBool("Walk", true);
            }
        }
        if(navMeshAgent.remainingDistance < 1) {
            myAnim.SetBool("Walk", false);
        }
    }
    private void OnTriggerEnter2D(Collider2D other) {
        if(other.tag == "character") {
            if(!other.GetComponent<NPCCharacterController>().isTriggered) {
                if(other.GetComponent<NPCCharacterController>().isClicked) {
                    navMeshAgent.SetDestination(transform.position);
                    other.GetComponent<NPCCharacterController>().StartConversationWithNPC();
                    other.GetComponent<NPCCharacterController>().isClicked = false;
                }
            }
        }
        if(other.tag == "object") {
            if(other.GetComponent<ObjectController>()) {
                if(other.GetComponent<ObjectController>().isClicked) {
                    navMeshAgent.SetDestination(transform.position);
                    other.GetComponent<ObjectController>().HandleInteraction();
                    myAnim.SetBool("Walk", false);
                    myAnim.SetTrigger("Interact");
                }
            }
        }
        if(other.tag == "train_map") {
            if(other.GetComponent<TrainMap>()) {
                if(other.GetComponent<TrainMap>().isClicked) {
                    navMeshAgent.SetDestination(transform.position);
                    other.GetComponent<TrainMap>().HandleInteraction();
                    myAnim.SetBool("Walk", false);
                    myAnim.SetTrigger("Interact");
                }
            }
        }
    }

    public float GetRemainingDistance() {
        if((Vector2)transform.position != (Vector2)navMeshAgent.destination) {
            return navMeshAgent.remainingDistance;
        }
        return 100;
    }

    public void SetNewDestination(Transform newPosition) {
        navMeshAgent.enabled = false;
        transform.position = newPosition.position;
        navMeshAgent.enabled = true;
        navMeshAgent.SetDestination(newPosition.position);
    }

    public void SetNewDestinationVector(Vector3 newPosition) {
        navMeshAgent.SetDestination(newPosition);
    }

    public PlayerStateController GetPlayerStateController() {
        return playerState;
    }

    public void TriggerIneraction() {
        myAnim.SetTrigger("Interact");
    }
}
