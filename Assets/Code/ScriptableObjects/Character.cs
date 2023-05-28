using UnityEngine;

[CreateAssetMenu(fileName = "New Character", menuName = "Character")]
public class Character : ScriptableObject
{
    public int id;
    public string characterName;
    public Dialogue myDialogue;
    public Color dialogueColor;

}
