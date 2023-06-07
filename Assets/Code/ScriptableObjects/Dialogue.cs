using UnityEngine;

[CreateAssetMenu(fileName = "New Dialogue Conversation", menuName = "Dialogue System/Dialogue")]
public class Dialogue : ScriptableObject
{
    public int id;
    public DialogueLine[] lines;
    public bool finishedThisOne;
    public bool onlyUseOnce;
    
    public DialogueLine GetLine(int index)
    {
        if (index >= 0 && index < lines.Length)
        {
            return lines[index];
        }
        else
        {
            Debug.LogError("Invalid dialogue line index: " + index);
            return null;
        }
    }
}
