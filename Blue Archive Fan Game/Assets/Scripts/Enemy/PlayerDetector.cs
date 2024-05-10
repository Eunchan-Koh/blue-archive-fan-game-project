using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDetector : MonoBehaviour
{
    Enemy parentEnemy;
    void Start(){
        parentEnemy = GetComponentInParent<Enemy>();
    }
    void OnTriggerEnter(Collider other){
        if(parentEnemy.playerLayer == (parentEnemy.playerLayer | 1 << other.gameObject.layer)){
            parentEnemy.player = other.gameObject;
            parentEnemy.detectedPlayer = true; 
        }
    }
    void OnTriggerExit(Collider other){
        if(parentEnemy.playerLayer == (parentEnemy.playerLayer | 1 << other.gameObject.layer)){
            parentEnemy.detectedPlayer = false;
            parentEnemy.player = null;
        }
    }
}
