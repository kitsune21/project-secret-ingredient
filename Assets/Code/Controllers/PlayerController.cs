using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour {
    private NavMeshAgent navMeshAgent;

    void Start() {
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.updateRotation = false;
        navMeshAgent.updateUpAxis = false;
    }

    void Update() {
        if (Input.GetMouseButton(0)) {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            navMeshAgent.SetDestination(mousePosition);
        }
    }

    public float GetRemainingDistance() {
        if(convertToXYOnly(gameObject.transform.position) != convertToXYOnly(navMeshAgent.destination)) {
            return navMeshAgent.remainingDistance;
        }
        return 100;
    }

    private Vector2 convertToXYOnly(Vector3 myVector) {
        return new Vector2(myVector.x, myVector.y);
    }
}
