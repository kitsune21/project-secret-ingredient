using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Character", menuName = "Character")]
public class Character : ScriptableObject
{
    public int id;
    public string characterName;
    public string onHoverText;
    public List<Dialogue> allDialogues = new List<Dialogue>();
    public Color dialogueColor;
    public Item wantedItem;
    public Dialogue givenWantedItem;

    public Dialogue getNextDialogue() {
        foreach (Dialogue dialogue in allDialogues) {
            if(!dialogue.finishedThisOne) {
                return dialogue;
            }
        }

        return null;
    }

    public void resetDialogue() {
        foreach (Dialogue dialogue in allDialogues) {
            dialogue.finishedThisOne = false;
        }
    }

}
