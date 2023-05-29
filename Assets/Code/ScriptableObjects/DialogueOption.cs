using UnityEngine;

[CreateAssetMenu(fileName = "New Dialogue Line", menuName = "Dialogue System/Dialogue Option")]
public class DialogueOption : ScriptableObject
{
    public int id;
    [TextArea(3, 10)]
    public string optionText;
    
    public Dialogue nextDialogue;
}
