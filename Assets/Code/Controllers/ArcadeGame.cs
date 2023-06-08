using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ArcadeGame : MonoBehaviour
{
    public GameObject stick;
    private Rigidbody2D stickRb;
    public GameObject gameUI;
    public GameObject gameInstructions;
    public TMP_Text scoreText;
    private bool goLeft;
    public float leftBound;
    public float rightBound;
    public float craneSpeed;
    public List<GameObject> gameBalls = new List<GameObject>();
    private int ballsReleasedCount;
    public GameObject ballPrefab;
    private int playerScore;
    public GameObject resetPanel;
    private bool canPlay;
    private Transform currentBallTransform;
    public Transform exitLocation;
    public GameObject wonPrizeText;
    public Item prizeItem;

    void Start() {
        gameUI.SetActive(false);
    }

    void OnEnable() {
        ResetGame();
        gameUI.SetActive(true);
        goLeft = true;
        stickRb = stick.GetComponent<Rigidbody2D>();
        resetPanel.SetActive(false);
        if(gameInstructions) {
            gameInstructions.SetActive(true);
            canPlay = false;
        } else {
            canPlay = true;
        }
        ballsReleasedCount = 0;
        playerScore = 0;
        AddScore(playerScore);
    }

    void Update() {
        if(!goLeft) {
            if(stick.transform.localPosition.x < rightBound) {
                stickRb.AddForce(Vector2.right * craneSpeed);
            } else {
                goLeft = true;
            }
        } else {
            if(stick.transform.localPosition.x > leftBound) {
                stickRb.AddForce(Vector2.left * craneSpeed);
            } else {
                goLeft = false;
            }
        }
        if(canPlay) {
            if(Input.GetKeyDown(KeyCode.Space)) {
                if(ballsReleasedCount < gameBalls.Count) {
                    ballsReleasedCount += 1;
                    bool findFirstActiveBall = false;
                    foreach(GameObject ball in gameBalls) {
                        if(!findFirstActiveBall) {
                            if(ball.activeSelf) {
                                findFirstActiveBall = true;
                                currentBallTransform = ball.transform;
                                canPlay = false;
                                StartCoroutine(DelayBallDrop(0.4f));
                                StartCoroutine(TimeInBetweenBallDrops(1.8f));
                            }
                        }
                    }
                } 
                if(ballsReleasedCount == gameBalls.Count) {
                    StartCoroutine(WaitToShowReset(1.8f));
                    canPlay = false;
                }
            }
        }
    }

    public void AddScore(int amount) {
        playerScore += amount;
        scoreText.text = "Score: " + playerScore.ToString();
        if(playerScore >= 500 && wonPrizeText) {
            wonPrizeText.SetActive(true);
            Destroy(wonPrizeText, 2f);
            GameObject.Find("Player").GetComponentInParent<PlayerController>().GetComponentInChildren<InventoryController>().AddItem(prizeItem, "Won ");
        }
    }

    private IEnumerator StartTimer(float duration)
    {
        yield return new WaitForSeconds(duration);

        ResetGame();
    }

    private IEnumerator WaitToShowReset(float duration)
    {
        yield return new WaitForSeconds(duration);

        resetPanel.SetActive(true);
    }

    private IEnumerator DelayBallDrop(float duration)
    {
        yield return new WaitForSeconds(duration);

        currentBallTransform.gameObject.SetActive(false);
        GameObject tempBall = Instantiate(ballPrefab, currentBallTransform.position, Quaternion.identity);
        Destroy(tempBall, 1.9f);
    }

    private IEnumerator TimeInBetweenBallDrops(float duration)
    {
        yield return new WaitForSeconds(duration);

        canPlay = true;
    }

    public void ResetGame() {
        foreach(GameObject ball in gameBalls) {
            ball.SetActive(true);
        }
        playerScore = 0;
        scoreText.text = "Score: " + playerScore.ToString();
        ballsReleasedCount = 0;
        resetPanel.SetActive(false);
        canPlay = true;
    }

    public void QuitGame() {
        Camera.main.GetComponent<CameraController>().UpdateStartLoction(exitLocation);
        gameUI.SetActive(false);
        gameObject.SetActive(false);
        GameObject.Find("Player").GetComponentInParent<PlayerController>().GetPlayerStateController().UpdatePlayerState(PlayerState.Playing);
    }

    public void StartGame() {
        if(gameInstructions) {
            Destroy(gameInstructions);
        }
        canPlay = true;
    }
}
