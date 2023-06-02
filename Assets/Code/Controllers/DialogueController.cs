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

    void Start() {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        playerState = player.GetComponent<PlayerController>().GetPlayerStateController();
        allowOverride = true;
    }
    
    public void StartConversation(Character newCharacter, bool isGivenWantedItem)
    {
        StopAllCoroutines();
        optionsTextPanel.SetActive(false);
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        playerState = player.GetComponent<PlayerController>().GetPlayerStateController();
        playerState.UpdatePlayerState(PlayerState.InDialogue);
        characterObject = newCharacter;
        if(isGivenWantedItem) {
            currentDialogue = newCharacter.givenWantedItem;
        } else {
            currentDialogue = newCharacter.myDialogue;
        }
        clearText();
        dialogueTextObject.SetActive(true);
        currentLineIndex = 0;
        DisplayCurrentLine();
    }
    
    private void DisplayCurrentLine()
    {
        clearText();
        currentLine = currentDialogue.GetLine(currentLineIndex);
        if(currentLine.speakerId == 0) {
            playerText.text = currentLine.text;
        } else {
            otherText.text = currentLine.text;
            otherText.color = characterObject.dialogueColor;
        }
        if(currentLine.options.Length > 0) {
            displayOptions(currentLine);
            return;
        }
        float dialogueDuration = (startBuffer + durationLength * currentLine.text.Length) / 1000;
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
        
        StartCoroutine(StartTimer(dialogueDuration));
    }
    
    private void AdvanceToNextLine()
    {
        currentLineIndex++;
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
        otherText.color = Color.white;
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
        dialogueTextObject.SetActive(true);
        optionsTextPanel.SetActive(false);
        clearText();
        playerText.text = textToShow;
        float dialogueDuration = (startBuffer + durationLength * textToShow.Length) / 1000;
        StartCoroutine(DisplayLookAtText(dialogueDuration));
    }

    public void showLookAtTextNPC(string textToShow, Character npcCharacter) {
        allowOverride = false;
        dialogueTextObject.SetActive(true);
        optionsTextPanel.SetActive(false);
        clearText();
        otherText.text = textToShow;
        otherText.color = npcCharacter.dialogueColor;
        float dialogueDuration = (startBuffer + durationLength * textToShow.Length) / 1000;
        StartCoroutine(DisplayLookAtText(dialogueDuration));
    }

    public void showLookAtTextWithNoOverride(string textToShow) {
        allowOverride = false;
        dialogueTextObject.SetActive(true);
        optionsTextPanel.SetActive(false);
        clearText();
        playerText.text = textToShow;
        float dialogueDuration = (startBuffer + durationLength * textToShow.Length) / 1000;
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
}

