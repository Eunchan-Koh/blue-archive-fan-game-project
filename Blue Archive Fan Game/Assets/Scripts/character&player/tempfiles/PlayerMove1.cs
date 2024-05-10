using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Xml.Schema;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class PlayerMove1 : MonoBehaviour
{   
    //input related
    protected float verticalMove;
    protected float horizontalMove;
    protected bool isRun;
    protected bool isCrouch;
    protected bool isJump;
    public float jumpPower;
    protected bool isFire;
    protected bool isAim = false;
    protected bool isUltimate = false;
    protected bool reloadPush = false;
    protected bool isASkill1 = false;
    protected bool isPSkill1 = false;
    protected bool isUSkill1 = false;


    //requirement
    protected bool canFire = true;
    [SerializeField]
    protected float shootingDelay;
    protected bool canUlt = true;
    protected bool isReloading = false;

    //mouse input
    protected float MouseX;
    public float MouseY;
    public float lookSensitivity;



    //movement
    float movementSpeed;
    public float crouchSpeed;
    public float walkSpeed;
    public float runSpeed;

    //movementRestriction
    public bool MoveImobilize = false;
    public bool MouseImobilize = false;
    public bool ModelTurnImobilize = false;
    public bool ShootImobilize = false;

    //Objects
    Rigidbody rigid;
    public GameObject groundCollider;
    //ground collider options
    public float radius_groundCollider;
    public Vector3 offset_groundCollider;
    public bool canJump;
    public GameObject normalBullet;

    //camera related
    public Camera mainCamera;
    public CameraOrigin cameraOrigin;
    public float targetCameraSize;
    public float aimMagniSpeed;
    protected bool customCameraMove = false;

    //bullet
    public float bulletDistance;
    public LayerMask TargetableLayer;
    public Vector3 targetPos;
    public Transform BulletPos;
    public int curBulletCount;
    public int maxBulletCount;
    public float reloadingTimeBefore;
    public float reloadingTimeLate;

    //player model
    public GameObject playerModel;
    Vector3 targetFace;
    Vector3 lerpFace;
    public float modelRotationSpeed;
    Vector3 lastFacing;
    protected bool customModelRotation = false;

    //UI related
    public Text bulletCountText;


    void Start(){
        rigid = GetComponent<Rigidbody>();
        lastFacing = transform.position + transform.forward;
    }

    void Update(){
        //other options
        GroundColliderOption();

        //camera relocation if colliding
        //aim
        AimingBasic();
        CameraMotion();

        //gets input
        GetInput();

        //character + camera rotation
        CharacterRotation();

        //reload
        Reload();

        //attack
        Fire();

        //bullet ui
        BulletUIUpdate();

        //skill
        SkillCheck();
        // //Ultimate
        // Ultimate();

        //set movement speed, to walkspeed or runspeed, crouch, etc.
        SetSpeed();

        //jump
        Jump();
        //inner model rotation

        PlayerModelRotation();
        //move the player
        Move();
        
    }
    void GroundColliderOption(){
        groundCollider.transform.localScale = new Vector3(radius_groundCollider, radius_groundCollider, radius_groundCollider);
        groundCollider.transform.localPosition = offset_groundCollider;
    }
    void CameraMotion(){
        if(!customCameraMove)
            PlayerAimingCamera();
        else{
            CustomCameraMove();
        }
    }
    void AimingBasic(){
        RaycastHit hit;
        Ray ray = mainCamera.ViewportPointToRay(new Vector3(0.5f,0.5f,0));
        if(Physics.Raycast(ray, out hit, bulletDistance, TargetableLayer)){
            targetPos = hit.point;
        }else{
            targetPos = mainCamera.transform.position + mainCamera.transform.forward*bulletDistance;
        }
    }
    void PlayerAimingCamera(){
        if(isAim) targetCameraSize = 2;
        else targetCameraSize = 5;

        cameraOrigin.cameraDistance = Mathf.Lerp(cameraOrigin.cameraDistance, targetCameraSize, aimMagniSpeed*Time.deltaTime);
    }
    public virtual void CustomCameraMove(){

    }
    public virtual void CustomModelRotation(){

    }
    void GetInput(){
        horizontalMove = Input.GetAxisRaw("Horizontal");
        verticalMove = Input.GetAxisRaw("Vertical");
        isRun = Input.GetKey(KeyCode.LeftShift);
        isCrouch = Input.GetKey(KeyCode.LeftControl);
        isJump = Input.GetKeyDown(KeyCode.Space);

        MouseX = Input.GetAxis("Mouse X")*lookSensitivity;
        MouseY = Input.GetAxis("Mouse Y")*lookSensitivity;

        isFire = Input.GetKey(KeyCode.Mouse0);
        isAim = Input.GetKey(KeyCode.Mouse1);
        isASkill1 = Input.GetKey(KeyCode.E);
        isUSkill1 = Input.GetKey(KeyCode.Q);
        // isUltimate = Input.GetKeyDown(KeyCode.Q);

        reloadPush = Input.GetKeyDown(KeyCode.R);
        // Debug.Log(MouseX + ", " + MouseY);
        
    }
    void CharacterRotation(){
        if(!MouseImobilize)
        transform.Rotate(new Vector3(0,MouseX,0), Space.Self);
    }
    void SetSpeed(){
        if(isRun && !isAim) movementSpeed = runSpeed;
        else if (isCrouch) movementSpeed = crouchSpeed;
        else movementSpeed = walkSpeed;
    }
    void Reload(){
        if(!isReloading && reloadPush && curBulletCount < maxBulletCount){
            StartCoroutine("ReloadAction");
        }
    }
    IEnumerator ReloadAction(){
        isReloading = true;
        yield return new WaitForSeconds(reloadingTimeBefore);
        curBulletCount = maxBulletCount;
        yield return new WaitForSeconds(reloadingTimeLate);
        isReloading = false;

    }
    void Fire(){
        if(!ShootImobilize && isFire && canFire && isAim && !isReloading && curBulletCount > 0) StartCoroutine("Firing");
    }
    IEnumerator Firing(){
        //flag
        canFire = false;

        //shoot bullet
        normalBullet.GetComponent<Bullet>().ToHere = targetPos;
        Instantiate(normalBullet, BulletPos.position, transform.rotation);
        curBulletCount -= 1;
        // Debug.Log("shooted!");
        //wait for delay
        yield return new WaitForSeconds(shootingDelay);

        //can fire now
        canFire = true;
    }
    void BulletUIUpdate(){
        bulletCountText.text = curBulletCount.ToString() + "/" + maxBulletCount.ToString();
    }
    void Ultimate(){
        if(isUltimate && canUlt) StartCoroutine("Ult");
    }
    IEnumerator Ult(){
        yield return null;
    }
    void Jump(){
        if(isJump && canJump){
              
            rigid.AddForce(transform.up * jumpPower, ForceMode.Impulse);
            canJump = false; 
        }
    }
    void Move(){//movement when no skill is affecting
        if(!MoveImobilize)
        transform.position += (horizontalMove*transform.right + verticalMove*transform.forward)*Time.deltaTime*movementSpeed;
    }
    void PlayerModelRotation(){//model rotation when no skill is affecting
        if(!ModelTurnImobilize && !customModelRotation){
            if(!isAim){
                targetFace = transform.position +  (horizontalMove*transform.right + verticalMove*transform.forward)*Time.deltaTime*movementSpeed;
                lerpFace = Vector3.Lerp(lerpFace, targetFace, modelRotationSpeed*Time.deltaTime);
            }else{
                targetFace = transform.position + transform.forward;
                lerpFace = targetFace;
            }
                
            playerModel.transform.LookAt(targetFace);
        }else if(!ModelTurnImobilize && customModelRotation){
            CustomModelRotation();
        }
        
        // Debug.Log(transform.forward);
    }
    void SkillCheck(){
        if(isASkill1){
            ASkill1();
        }
        if(isPSkill1){
            PSkill1();
        }
        if(isUSkill1){
            USkill1();
        }
    }

    public virtual void ASkill1(){

    }
    public virtual void PSkill1(){
        
    }
    public virtual void USkill1(){
        
    }
}
