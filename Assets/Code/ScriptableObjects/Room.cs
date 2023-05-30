using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Room", menuName = "Rooms")]
public class Room : ScriptableObject
{
    public int id;
    public string roomName;
    public Transform cameraStartingPoint;
}
