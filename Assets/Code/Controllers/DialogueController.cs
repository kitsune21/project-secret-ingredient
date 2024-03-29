using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueController : MonoBehaviour
{
    private Dialogue currentDialogue;
    private DialogueLine currentLine;
    private int currentLineIndex = 0;
    public GameObject dialogueTextObject;
    public TMP_Text playerText;
    public TMP_Text otherText;
    public float durationLength;
    public float startBuffer;
    private Character characterObject;
    public GameObject optionsTextPrefab;
    public GameObject optionsTextPanel;
    private List<GameObject> optionsTextList = new List<GameObject>();
    private PlayerStateController playerState;
    private bool allowOverride;
    public GameController gameController;
    public GameObject hintText;
    public NPCCharacterController myNPC;
    public Character playerCharacter;
    public Dialogue assembleFood;
    public Dialogue putOnUniform;
    public Puzzle assemblePuzzle;
    public Dialogue finalDialogue;

    void Start() {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        playerState = player.GetComponent<PlayerController>().GetPlayerStateController();
        allowOverride = true;
        playerCharacter.allDialogues.Clear();
        playerCharacter.allDialogues.Add(assembleFood);
        assemblePuzzle.completed = false;
    }

    void Update() {
        if(playerState.GetPlayerState() == PlayerState.InDialogue) {
            if(!optionsTextPanel.activeSelf) {
                if(Input.GetKeyDown(KeyCode.Q)) {
                    AdvanceToNextLine();
                }
            }
        }
    }
    
    public void StartConversation(Character newCharacter, bool isGivenWantedItem, NPCCharacterController newNPC = null)
    {
        GameObject.FindGameObjectWithTag("DragItem").GetComponent<DragItemController>().StopDragging();
        if(hintText) {
            hintText.SetActive(true);
        }
        if(newNPC) {
            myNPC = newNPC;
        }
        StopAllCoroutines();
        optionsTextPanel.SetActive(false);
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        playerState = player.GetComponent<PlayerController>().GetPlayerStateController();
        playerState.UpdatePlayerState(PlayerState.InDialogue);
        characterObject = newCharacter;
        if(isGivenWantedItem) {
            currentDialogue = newCharacter.givenWantedItem;
        } else {
            currentDialogue = newCharacter.getNextDialogue();
        }
        if(currentDialogue) {
            clearText();
            dialogueTextObject.SetActive(true);
            currentLineIndex = 0;
            DisplayCurrentLine();
        }
    }
    
    private void DisplayCurrentLine()
    {
        playerState.UpdatePlayerState(PlayerState.InDialogue);
        if(currentDialogue.onlyUseOnce) {
            currentDialogue.finishedThisOne = true;
        }
        if(currentDialogue.overWriteDialogueAfterThisOne) {
            myNPC.OverWriteDialogue();
        }
        clearText();
        currentLine = currentDialogue.GetLine(currentLineIndex);
        if(currentLine.speakerId == 0) {
            playerText.text = currentLine.text;
        } else {
            otherText.text = currentLine.text;
        }
        if(currentLine.options.Length > 0) {
            displayOptions(currentLine);
            return;
        }
        if(currentLine.giveItem) {
            if(gameObject.GetComponentInParent<PlayerController>().GetComponentInChildren<InventoryController>().CheckIfPlayerHasItem(currentLine.giveItem)) {
                otherText.text = currentLine.playerHasItem;
            } else {
                gameObject.GetComponentInParent<PlayerController>().GetComponentInChildren<InventoryController>().AddItem(currentLine.giveItem, "Recieved ");
            }
        }
        if(currentLine.takeItem) {
            gameObject.GetComponentInParent<PlayerController>().GetComponentInChildren<InventoryController>().RemoveItem(currentLine.takeItem, "Gave ");
        }
        float dialogueDuration = calculateDialogueDuration(currentLine.text);
        StartCoroutine(StartTimer(dialogueDuration));
    }
    
    private void AdvanceToNextLine()
    {
        playerState.UpdatePlayerState(PlayerState.InDialogue);
        currentLineIndex++;
        StopAllCoroutines();
        if(hintText) {
            Destroy(hintText);
        }
        if (currentLineIndex >= currentDialogue.lines.Length)
        {
            EndConversation();
        }
        else
        {
            DisplayCurrentLine();
        }
    }

    private IEnumerator StartTimer(float duration)
    {
        yield return new WaitForSeconds(duration);

        AdvanceToNextLine();
    }

    private void clearText() { 
        playerText.text = "";
        otherText.text = "";
    }

    private void displayOptions(DialogueLine currentLine) {
        optionsTextPanel.SetActive(true);
        for (int i = 0; i < currentLine.options.Length; i++)
        {
            DialogueOption optionText = currentLine.options[i];
            if(!optionText.requiredItem) {
                createOption(optionText, i);
            } else {
                if(gameObject.GetComponentInParent<PlayerController>().GetComponentInChildren<InventoryController>().CheckIfPlayerHasItem(optionText.requiredItem)) {
                    createOption(optionText, i);
                }
            }
        }
    }

    public void selectOption(int optionIndex) {
        if(currentLine.options[optionIndex].nextDialogue) {
            currentDialogue = currentLine.options[optionIndex].nextDialogue;
            currentLineIndex = 0;
            ClearOptions();
            DisplayCurrentLine();
            return;
        }
        EndConversation();
    }

    private void EndConversation() {
        clearText();
        dialogueTextObject.SetActive(false);
        StopAllCoroutines();
        playerState.UpdatePlayerState(PlayerState.Playing);
        ClearOptions();
        optionsTextPanel.SetActive(false);
        if(currentDialogue.finalDialogue) {
            playerState.UpdatePlayerState(PlayerState.Victory);
        }
    }

    private void ClearOptions() {
        foreach(GameObject optionText in optionsTextList) {
            Destroy(optionText);
        }
        optionsTextList.Clear();
        optionsTextPanel.SetActive(false);
    }

    private void createOption(DialogueOption optionText, int i) {
        GameObject newOption = Instantiate(optionsTextPrefab, optionsTextPanel.transform);
        newOption.GetComponent<TMP_Text>().text = optionText.optionText;
        newOption.GetComponent<OptionTextController>().myOptionIndex = i;
        newOption.GetComponent<OptionTextController>().setDialogueController(this);
        optionsTextList.Add(newOption);
    }

    public void showLookAtText(string textToShow) { 
        if(!allowOverride) {
            return;
        }
        if(hintText) {
            hintText.SetActive(false);
        }
        dialogueTextObject.SetActive(true);
        optionsTextPanel.SetActive(false);
        clearText();
        playerText.text = textToShow;
        float dialogueDuration = calculateDialogueDuration(textToShow);
        StartCoroutine(DisplayLookAtText(dialogueDuration));
    }

    public void showLookAtTextNPC(string textToShow, Character npcCharacter) {
        if(hintText) {
            hintText.SetActive(false);
        }
        allowOverride = false;
        dialogueTextObject.SetActive(true);
        optionsTextPanel.SetActive(false);
        clearText();
        otherText.text = textToShow;
        float dialogueDuration = calculateDialogueDuration(textToShow);
        StartCoroutine(DisplayLookAtText(dialogueDuration));
    }

    public void showLookAtTextWithNoOverride(string textToShow) {
        if(hintText) {
            hintText.SetActive(false);
        }
        allowOverride = false;
        dialogueTextObject.SetActive(true);
        optionsTextPanel.SetActive(false);
        clearText();
        playerText.text = textToShow;
        float dialogueDuration = calculateDialogueDuration(textToShow);
        StartCoroutine(DisplayLookAtText(dialogueDuration));
    }

    private IEnumerator DisplayLookAtText(float duration)
    {
        yield return new WaitForSeconds(duration);

        clearText();
        dialogueTextObject.SetActive(false);
        StopAllCoroutines();
        allowOverride = true;
    }

    private float calculateDialogueDuration(string text) {
        return ((startBuffer + durationLength * text.Length) / 1000) * gameController.GetTextSpeed();
    }

    public void RunAssembleDialogue() {
        StartConversation(playerCharacter, false, null);
        assemblePuzzle.completed = true;
        GameObject.Find("MusicController").GetComponent<MusicController>().loopClip("Credits");
    }

    public void PutOnUniformDialogue() {
        playerCharacter.allDialogues.Clear();
        playerCharacter.allDialogues.Add(putOnUniform);
        StartConversation(playerCharacter, false, null);
    }

    public void RunFinalDialogue() {
        playerCharacter.allDialogues.Clear();
        playerCharacter.allDialogues.Add(finalDialogue);
        StartConversation(playerCharacter, false, null);
    }

    public void StartConversationNoSpecialStuff(Dialogue newDialogue)
    {
        StopAllCoroutines();
        optionsTextPanel.SetActive(false);
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        playerState = player.GetComponent<PlayerController>().GetPlayerStateController();
        playerState.UpdatePlayerState(PlayerState.InDialogue);
        currentDialogue = newDialogue;
        if(currentDialogue) {
            clearText();
            dialogueTextObject.SetActive(true);
            currentLineIndex = 0;
            DisplayCurrentLine();
        }
    }
}
