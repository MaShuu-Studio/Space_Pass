using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destination : MonoBehaviour
{
    [SerializeField] private GameController gameController;
    
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "Player"){
            gameController.Clear();
        }
    }
}
