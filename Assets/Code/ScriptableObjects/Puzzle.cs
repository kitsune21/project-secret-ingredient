using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Puzzle", menuName = "Puzzle")]
public class Puzzle : ScriptableObject
{
    public int id;
    public string description;
    public Item requiredItem;
    public List<Puzzle> requiredPuzzles = new List<Puzzle>();
    public bool completed;
    public string failText;
    public bool finalPuzzle;

    public bool CheckAllPuzzles() {
        foreach(Puzzle checkPuzzle in requiredPuzzles) {
            if(!checkPuzzle.completed) {
                return false;
            }
        }
        return true;
    }
}
