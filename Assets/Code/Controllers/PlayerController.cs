using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour {
    private NavMeshAgent navMeshAgent;
    private PlayerStateController playerState;
    private InventoryController myInventory;
    private DialogueController dialogueController;

    void Start() {
        playerState = GetComponentInChildren<PlayerStateController>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.updateRotation = false;
        navMeshAgent.updateUpAxis = false;
    }

    void Update() {
        if(playerState.GetPlayerState() == PlayerState.Playing || playerState.GetPlayerState() == PlayerState.DraggingInventory) {
            if (Input.GetMouseButton(0)) {
                Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                navMeshAgent.SetDestination(mousePosition);
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D other) {
        if(other.tag == "character") {
            if(!other.GetComponent<NPCCharacterController>().isTriggered) {
                navMeshAgent.SetDestination(transform.position);
                other.GetComponent<NPCCharacterController>().StartConversationWithNPC();
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
}
