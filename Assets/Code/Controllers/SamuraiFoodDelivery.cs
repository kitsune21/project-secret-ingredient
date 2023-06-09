using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SamuraiFoodDelivery : MonoBehaviour
{
    public NPCCharacterController mySamurai;
    public Dialogue foodAndNoUniform;
    public Dialogue foodWithSleepingPill;
    public Dialogue foodNoSleepingPill;
    public Dialogue wrongFood;
    public Dialogue noFood;

    public Item uniform;
    public List<Item> wrongFoods = new List<Item>();
    public Item tonkatsu;
    public Item sleepingPill;
    public Item suspiciousTonkatsu;
    public Transform conversationTransform;
    private CapsuleCollider2D myCollider;
    public Puzzle learnSamuraiName;
    public Puzzle getPassedSamurai;
    private bool useCollider;

    void Start() {
        myCollider = GetComponent<CapsuleCollider2D>();
        useCollider = false;
        learnSamuraiName.completed = false;
        getPassedSamurai.completed = false;
    }

    void Update() {
        if(!useCollider) {
            if(learnSamuraiName.completed) {
                turnOnCollider();
            }
        }
    }

    private IEnumerator WaitABitBeforeTurningOnCollider(float duration) {
        yield return new WaitForSeconds(duration);

        useCollider = true;
    }

    public void turnOnCollider() {
        StartCoroutine(WaitABitBeforeTurningOnCollider(20f));
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(!useCollider) {
            return;
        }
        if(other.tag == "Player") {
            PlayerController pc = other.GetComponent<PlayerController>();
            InventoryController ic = other.GetComponentInChildren<InventoryController>();
            DialogueController dc = other.GetComponentInChildren<DialogueController>();
            pc.SetNewDestinationVector(conversationTransform.position);
            if(ic.CheckIfPlayerHasItem(suspiciousTonkatsu) && ic.CheckIfHasUniform()) {
                Debug.Log("foodYesSleepingPill");
                dc.StartConversationNoSpecialStuff(foodWithSleepingPill);
                getPassedSamurai.completed = true;
                mySamurai.noInteraction = true;
                Destroy(this);
                return;
            } else if(ic.CheckIfPlayerHasItem(tonkatsu)) {
                if(ic.CheckIfPlayerHasItem(uniform)) {
                    Debug.Log("foodNoSleepingPill");
                    dc.StartConversationNoSpecialStuff(foodNoSleepingPill);
                    return;
                } else {
                    dc.StartConversationNoSpecialStuff(foodAndNoUniform);
                    Debug.Log("no uniform");
                    return;
                }
            } else {
                if(ic.CheckIfHasUniform()) {
                    bool checkIfHasWrongFood = false;
                    foreach(Item wrongFood in wrongFoods) {
                        if(!checkIfHasWrongFood) {
                            if(ic.CheckIfPlayerHasItem(wrongFood)) {
                                checkIfHasWrongFood = true;
                            }
                        }
                    }
                    if(checkIfHasWrongFood) {
                        dc.StartConversationNoSpecialStuff(wrongFood);
                        Debug.Log("wrong food");
                        return;
                    } else {
                        dc.StartConversationNoSpecialStuff(noFood);
                        Debug.Log("no food");
                        return;
                    }
                } else {
                    dc.StartConversationNoSpecialStuff(foodAndNoUniform);
                    Debug.Log("no uniform");
                    return;
                }
            }
        }
    }
}
