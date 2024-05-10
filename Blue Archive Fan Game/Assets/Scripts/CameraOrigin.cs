using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraOrigin : MonoBehaviour
{

    //player position
    public PlayerMove player;
    public Camera mainCamera;

    Vector3 tempVec;
    float xCal;
    public float cameraDistance;
    public float cameraHorizontalPos;
    public float rotationMaxX;
    public float rotationMinX;
    float currentCameraRotationX;
    public LayerMask wallLayer;
    void Update(){
        //get mouse input
        // tempVec = new Vector3(-player.MouseY, 0, 0);
        //rotate that much
        // transform.Rotate(tempVec,Space.Self);
        CameraRelocation();
        if(!player.MouseImobilize){
            float cameraRotationX = player.MouseY;
            currentCameraRotationX -= cameraRotationX;
            currentCameraRotationX = Mathf.Clamp(currentCameraRotationX, -rotationMinX , rotationMaxX);
            transform.localEulerAngles = new Vector3(currentCameraRotationX, 0, 0);
        }
        
        //if the rotation value is too much after rotation, set back(limit)
        
        //cameraPosition setup
        mainCamera.transform.localPosition = new Vector3(cameraHorizontalPos,0,-cameraDistance);


        
    }
    void CameraRelocation(){
        RaycastHit hit;
        Ray ray = mainCamera.ViewportPointToRay(new Vector3(0.5f,0.5f,0));
        if(Physics.Raycast(ray, out hit, Vector3.Distance(player.transform.position, transform.position), wallLayer)){
            // targetPos = hit.point;
        }else{
            // targetPos = mainCamera.transform.position + mainCamera.transform.forward*bulletDistance;
        }
    }
}
