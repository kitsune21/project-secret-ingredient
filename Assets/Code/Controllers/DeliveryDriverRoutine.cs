using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryDriverRoutine : MonoBehaviour
{
    public GameObject firstDeliveryDriver;
    public Transform deskPos;
    public Transform doorPos;
    public float timeAtDesk;
    public float walkTime;
    public Dialogue deliveryDriverIsAtDesk;
    public Dialogue deliveryDriverIsAway;
    public PlayerController player;

    void Start() {
        StartCoroutine(WalkToDoor(timeAtDesk));
        firstDeliveryDriver.GetComponent<NPCCharacterController>().ChangeDialogue(deliveryDriverIsAtDesk);
    }

    private IEnumerator WalkToDoor(float duration)
    {
        yield return new WaitForSeconds(duration);

        
        if(player.GetPlayerStateController().GetPlayerState() != PlayerState.InDialogue) {
            firstDeliveryDriver.GetComponent<NPCCharacterController>().SetNewDestination(doorPos);
            firstDeliveryDriver.GetComponent<NPCCharacterController>().ClearDialoge();
            StartCoroutine(WaitForWalkToDoor(walkTime));
        } else {
            StartCoroutine(WalkToDoor(timeAtDesk));
        }
    }

    private IEnumerator WaitForWalkToDoor(float duration) {
        yield return new WaitForSeconds(duration);

        firstDeliveryDriver.GetComponent<NPCCharacterController>().ChangeDialogue(deliveryDriverIsAway);
        firstDeliveryDriver.SetActive(false);
        StartCoroutine(WalkToDesk(timeAtDesk));
    }

    private IEnumerator WalkToDesk(float duration)
    {
        yield return new WaitForSeconds(duration);
        
        firstDeliveryDriver.SetActive(true);
        firstDeliveryDriver.GetComponent<NPCCharacterController>().SetNewDestination(deskPos);
        firstDeliveryDriver.GetComponent<NPCCharacterController>().ClearDialoge();
        StartCoroutine(WaitForWalkToDesk(timeAtDesk));
    }

    private IEnumerator WaitForWalkToDesk(float duration) {
        yield return new WaitForSeconds(duration);

        firstDeliveryDriver.GetComponent<NPCCharacterController>().ChangeDialogue(deliveryDriverIsAtDesk);
        StartCoroutine(WalkToDoor(timeAtDesk));
    }

    public void TalkedAtDesk() {
        StopAllCoroutines();
        StartCoroutine(WalkToDoor(timeAtDesk));
    }
}
