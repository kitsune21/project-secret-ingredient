using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcadeHoleController : MonoBehaviour
{
    
     public ArcadeGame arcadeGame;
     public int scoreAmount;
     private void OnTriggerEnter2D(Collider2D other) {
        if(other.tag == "gameball") {
            arcadeGame.AddScore(scoreAmount);
        }
    }
}
