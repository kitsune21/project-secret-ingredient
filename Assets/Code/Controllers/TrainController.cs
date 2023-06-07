using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainController : MonoBehaviour
{
    public string currentMachi;
    public float secondsTillNextStop;
    public float secondsAtTheStop;
    public GameObject doorShinjuku;
    public GameObject doorIkebukuro;
    public GameObject doorAkihabara;
    public GameObject doorShinagawa;
    public GameObject closedDoors;

    public void SetStartingMachi(string startingMachi) {
        closedDoors.SetActive(true);
        doorShinjuku.SetActive(false);
        doorIkebukuro.SetActive(false);
        doorAkihabara.SetActive(false);
        doorShinagawa.SetActive(false);
        currentMachi = startingMachi;
        StartCoroutine(TimerTillNextStop(secondsTillNextStop));
    }

    public void ExitTrain() {
        currentMachi = "";
        StopAllCoroutines();
    }

    private IEnumerator TimerTillNextStop(float duration) {
        yield return new WaitForSeconds(duration);

        updateCurrentMachi();
    }

    private IEnumerator TimerTillTrainMoves(float duration) {
        yield return new WaitForSeconds(duration);

        closedDoors.SetActive(true);
        doorShinjuku.SetActive(false);
        doorIkebukuro.SetActive(false);
        doorAkihabara.SetActive(false);
        doorShinagawa.SetActive(false);
        StartCoroutine(TimerTillNextStop(secondsAtTheStop));
    }

    private void updateCurrentMachi() {
        if(currentMachi == "shinjuku") {
            currentMachi = "ikebukuro";
            doorIkebukuro.SetActive(true);
        } else if(currentMachi == "ikebukuro") {
            currentMachi = "akihabara";
            doorAkihabara.SetActive(true);
        } else if(currentMachi == "akihabara") {
            currentMachi = "shinagawa";
            doorShinagawa.SetActive(true);
        } else if(currentMachi == "shinagawa") {
            currentMachi = "shinjuku";
            doorShinjuku.SetActive(true);
        }
        closedDoors.SetActive(false);
        StartCoroutine(TimerTillTrainMoves(secondsAtTheStop));
    }
}
