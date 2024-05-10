using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCanvasScript : MonoBehaviour
{
    public Camera playerCamera;
    void Start(){
        playerCamera = Camera.main;
    }
    void Update()
    {
        transform.LookAt(playerCamera.transform.position);
    }
}
