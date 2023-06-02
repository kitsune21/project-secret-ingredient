using UnityEngine;

[CreateAssetMenu(fileName = "New Dialogue Line", menuName = "Dialogue System/Dialogue Line")]
public class DialogueLine : ScriptableObject
{
    public int id;
    public int speakerId; // The character speaking this line
    [TextArea(3, 10)]
    public string text; // The actual dialogue text
    [TextArea(3, 10)]
    public string playerHasItem;
    public DialogueOption[] options; // Array of dialogue options
    public Item giveItem;
    public Item takeItem;
}
