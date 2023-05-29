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

    void Start() {
        playerState = PlayerStateController.Instance;
    }
    
    public void StartConversation(Character newCharacter)
    {
        playerState.UpdatePlayerState(PlayerState.InDialogue);
        characterObject = newCharacter;
        clearText();
        dialogueTextObject.SetActive(true);
        currentDialogue = newCharacter.myDialogue;
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
        StartCoroutine(StartTimer(dialogueDuration));
    }
    
    private void AdvanceToNextLine()
    {
        currentLineIndex++;
        if (currentLineIndex >= currentDialogue.lines.Length)
        {
            clearText();
            dialogueTextObject.SetActive(false);
            StopAllCoroutines();
            playerState.UpdatePlayerState(PlayerState.Playing);
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
        for (int i = 0; i < currentLine.options.Length; i++)
        {
            DialogueOption optionText = currentLine.options[i];
            GameObject newOption = Instantiate(optionsTextPrefab, optionsTextPanel.transform);
            newOption.GetComponent<TMP_Text>().text = optionText.optionText;
            newOption.GetComponent<OptionTextController>().myOptionIndex = i;
            newOption.GetComponent<OptionTextController>().setDialogueController(this);
            optionsTextList.Add(newOption);
        }
    }

    public void selectOption(int optionIndex) {
        currentDialogue = currentLine.options[optionIndex].nextDialogue;
        currentLineIndex = 0;
        foreach(GameObject optionText in optionsTextList) {
            Destroy(optionText);
        }
        optionsTextList.Clear();
        DisplayCurrentLine();
    }
}