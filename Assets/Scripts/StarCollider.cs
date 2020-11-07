using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarCollider : MonoBehaviour
{
    [SerializeField] private int DMG;
    
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "Damaged"){
            transform.parent.SendMessage("EndStar");
            other.transform.parent.SendMessage("Damage", DMG);
            Destroy(gameObject);
        }        
    }
}
