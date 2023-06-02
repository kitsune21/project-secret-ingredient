using UnityEngine;

[CreateAssetMenu(fileName = "New Character", menuName = "Character")]
public class Character : ScriptableObject
{
    public int id;
    public string characterName;
    public string onHoverText;
    public Dialogue myDialogue;
    public Color dialogueColor;
    public Item wantedItem;
    public Dialogue givenWantedItem;

}
