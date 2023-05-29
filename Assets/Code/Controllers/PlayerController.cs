using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour {
    private NavMeshAgent navMeshAgent;
    private PlayerStateController playerState;
    private InventoryController myInventory;
    private DialogueController dialogueController;

    void Start() {
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.updateRotation = false;
        navMeshAgent.updateUpAxis = false;
        playerState = PlayerStateController.Instance;
    }

    void Update() {
        if(playerState.GetPlayerState() == PlayerState.Playing || playerState.GetPlayerState() == PlayerState.DraggingInventory) {
            if (Input.GetMouseButton(0)) {
                Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                navMeshAgent.SetDestination(mousePosition);
            }
        }
    }

    public float GetRemainingDistance() {
        if((Vector2)transform.position != (Vector2)navMeshAgent.destination) {
            return navMeshAgent.remainingDistance;
        }
        return 100;
    }
}
