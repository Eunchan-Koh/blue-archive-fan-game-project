using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCollider : MonoBehaviour
{
    public LayerMask groundLayer;
    public PlayerMove player;
    void OnTriggerEnter(Collider other){
        // Debug.Log(other.gameObject);
        if(groundLayer == (groundLayer | 1 << other.gameObject.layer)){
            
            player.canJump = true;
        }
    }
}
