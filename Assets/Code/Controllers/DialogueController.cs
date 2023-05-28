using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueController : MonoBehaviour
{
    private Dialogue currentDialogue;
    private int currentLineIndex = 0;
    public GameObject dialogueTextObject;
    public TMP_Text playerText;
    public TMP_Text otherText;
    public float durationLength;
    public float startBuffer;
    private Character characterObject;
    
    public void StartConversation(Character newCharacter)
    {
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
        DialogueLine currentLine = currentDialogue.GetLine(currentLineIndex);
        if(currentLine.speakerId == 0) {
            playerText.text = currentLine.text;
        } else {
            otherText.text = currentLine.text;
            otherText.color = characterObject.dialogueColor;
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
}