using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Xml.Schema;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;
using UnityEngine.UI;

public class PlayerMove : MonoBehaviour
{   
    //input related
    public float verticalMove;
    public float horizontalMove;
    protected bool isRun;
    protected bool isCrouch;
    protected bool isJump;
    public float jumpPower;
    [SerializeField]
    protected bool isFire;
    protected bool isAim = false;
    protected bool isUltimate = false;
    protected bool reloadPush = false;
    protected bool isASkill1 = false;
    protected bool isPSkill1 = false;
    protected bool isUSkill1 = false;


    //requirement

    //mouse input
    protected float MouseX;
    public float MouseY;
    public float lookSensitivity;



    //movement
    float movementSpeed;
    public GameObject groundCollider;
    [Range(0.0f,2.0f)]
    public float jumpCool;

    //movementRestriction
    public bool MoveImobilize = false;
    public bool MouseImobilize = false;
    public bool ModelTurnImobilize = false;
    public bool ShootImobilize = false;

    //Objects
    Rigidbody rigid;
    public bool canJump;//= chara.canJump , use this method to get canJump value

    //camera related
    public Camera mainCamera;
    public CameraOrigin cameraOrigin;
    public float targetCameraSize;
    public float aimMagniSpeed;
    protected bool customCameraMove = false;

    //bullet
    public float bulletDistance;//chara.bulletDistance
    public LayerMask TargetableLayer;
    public Vector3 targetPos;

    //player model
    GameObject curplayerModel;
    Vector3 targetFace;
    Vector3 lerpFace;
    public float modelRotationSpeed;
    Vector3 lastFacing;
    protected bool customModelRotation = false;

    //UI related
    public Text bulletCountText;

    [Header("Stamina")]
    public float maxStamina;
    public float curStamina;
    public float staminaUsagePerSec;
    public float staminaRecoveryPerSec;
    public float staminaZeroCool;
    public bool canRun;
    public Image[] staminaImages;

    [Header("Testing!")]
    public Character[] characterlist;
    public Character curChara;
    public int curCharaIndex = 0;
    public Character pastChar = null;
    public GameObject cameraHorizon;
    [Range(0.0f, 10.0f)]
    public float buffAmount;
    public float buffTime;    

    [Header("Extra Checks")]
    public bool loaded;
    public GameObject map;
    public bool player_CannotChangeChara;


    // [Header("UIs")]
    // public GameObject reloadingMark;
    // public Image curCharaHpBar;
    // public Text curcharaHPText;
    // public SkillUIManager SkillUIs;
    
    // if UIs are changing slower than the speed that it is supposed to be having, turn off UIManager Script on ScreenUI gameeobject and put that script inside of this script.
    // Then turn on the parameters above.


    void Start(){
        Time.timeScale = 0;
        curStamina = maxStamina;
        canRun = true;
        rigid = GetComponent<Rigidbody>();
        lastFacing = transform.position + transform.forward;
    }

    void Update(){
        if(!loaded && map.activeSelf){
            for(int i = 0; i < characterlist.Length; i++){
                if(!characterlist[i].isLoaded){
                    loaded = false;
                    break;
                }else{
                    Time.timeScale = 1;
                    loaded = true;
                }
            }
        }
        if(loaded){
                //reset speed;
            rigid.velocity = new Vector3(0,rigid.velocity.y,0);
            
            

            //some test input
            if(Input.GetKeyDown(KeyCode.C)){
                // Debug.Log("buffed for " + buffTime + "! amount:" + buffAmount);
                // BuffCurCharacter(buffAmount, 2f);
                // curChara.GetDamage(100f);
                for(int i = 0 ; i < characterlist.Length; i++){
                    characterlist[i].GetDamage(100f);
                }
            }
            if(Input.GetKeyDown(KeyCode.V)){
                // curChara.GetHeal(75f);
                for(int i = 0 ; i < characterlist.Length; i++){
                    characterlist[i].GetHeal(100f);
                }
            }
            //character setting
            curChara = characterlist[curCharaIndex];
            if(pastChar != curChara){
                if(pastChar){
                    pastChar.isReloading = false;
                    pastChar.gameObject.SetActive(false);
                }
                Debug.Log(curChara + ", " + curCharaIndex);
                curChara.gameObject.SetActive(true);
                pastChar = curChara;
                curplayerModel = curChara.playerModel;
            }
            if(Input.GetKeyDown(KeyCode.Alpha1) && !curChara.GetCannotChangeCharaVal() && !player_CannotChangeChara){
                curCharaIndex = 0;
            }
            if(Input.GetKeyDown(KeyCode.Alpha2) && !curChara.GetCannotChangeCharaVal() && !player_CannotChangeChara){
                if(characterlist[1]!=null)curCharaIndex = 1;
            }
            if(Input.GetKeyDown(KeyCode.Alpha3) && !curChara.GetCannotChangeCharaVal() && !player_CannotChangeChara){
                if(characterlist[2]!=null)curCharaIndex = 2;
            }
            if(Input.GetKeyDown(KeyCode.Alpha4) && !curChara.GetCannotChangeCharaVal() && !player_CannotChangeChara){
                if(characterlist.Length >= 4 && characterlist[3]!=null)curCharaIndex = 3;
            }

            
            


            //other options
            // GroundColliderOption();

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

            //stamina check
            StaminaCheck();

            //jump
            Jump();
            //inner model rotation

            PlayerModelRotation();
            //move the player
            Move();
        
        }
        
    }
    // void GroundColliderOption(){
    //     groundCollider.transform.localScale = new Vector3(radius_groundCollider, radius_groundCollider, radius_groundCollider);
    //     groundCollider.transform.localPosition = offset_groundCollider;
    // }
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
        curChara.targetPos = targetPos;
    }
    void PlayerAimingCamera(){
        if(isAim) {
            curChara.isAim = true; 
            targetCameraSize = 2;
        } else {
            curChara.isAim = false;
            targetCameraSize = 5;
        } 

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

        // isFire = Input.GetKey(KeyCode.Mouse0);
        isFire = Input.GetMouseButton(0);
        // Debug.Log(Input.GetMouseButton(0));
        // isAim = Input.GetKey(KeyCode.Mouse1);
        isAim = Input.GetMouseButton(1);
        isASkill1 = Input.GetKey(KeyCode.E);
        isUSkill1 = Input.GetKey(KeyCode.Q);
        // isUltimate = Input.GetKeyDown(KeyCode.Q);

        reloadPush = Input.GetKeyDown(KeyCode.R);
        // Debug.Log(MouseX + ", " + MouseY);
        
    }
    void CharacterRotation(){
        if(!MouseImobilize)
        cameraHorizon.transform.Rotate(new Vector3(0,MouseX,0), Space.Self);
    }
    void SetSpeed(){
        if(isRun && !isAim && canRun){
            movementSpeed = curChara.runSpeed;
        } 
        else if (isCrouch) movementSpeed = curChara.crouchSpeed;
        else movementSpeed = curChara.walkSpeed;

        if(curChara.supporter)
        movementSpeed *= 1+curChara.supporter.SpeedPercent;
    }
    void StaminaCheck(){
        if(movementSpeed >= curChara.runSpeed && isRun && (horizontalMove!=0 || verticalMove != 0)){
            curStamina -= staminaUsagePerSec*Time.deltaTime;
            if(curStamina <= 0){
                curStamina = 0;
                StartCoroutine("StaminaCover");
            }
        }else{
            curStamina += staminaRecoveryPerSec*Time.deltaTime;
            if(curStamina >= maxStamina)curStamina = maxStamina;
        }
        for(int i = 0; i < staminaImages.Length; i++){
            staminaImages[i].fillAmount = curStamina/maxStamina;
        }
    }
    IEnumerator StaminaCover(){
        canRun = false;
        yield return new WaitForSeconds(staminaZeroCool);
        canRun = true;
    }
    void Reload(){
        if(reloadPush){
            curChara.Reloading();
        }
    }
    
    void Fire(){
        if(isFire){
            curChara.Fire();
        }
    }
    
    void BulletUIUpdate(){
        bulletCountText.text = curChara.curBulletCount.ToString() + "/" + curChara.appliedMaxBulletCount.ToString();
    }
    // void Ultimate(){
    //     if(isUltimate && canUlt) StartCoroutine("Ult");
    // }
    // IEnumerator Ult(){
    //     yield return null;
    // }
    IEnumerator GroundColliderReset(float resetCool){
        player_CannotChangeChara = true;
        curChara.groundCollider.SetActive(false);
        yield return new WaitForSeconds(resetCool);
        curChara.groundCollider.SetActive(true);
        
    }
    void Jump(){
        if(canJump)player_CannotChangeChara = false;//while player is on the air, cannot change character. However, not using curchara.cannotChangeChara directly since
                                                    //curchara.cannotChangeChara is being managed by each character scripts, using that value from outside can cause errors.
                                                    //if does not want to use this and want to manage ground collider from each character scripts,
                                                    //then set ground collider of all characters to false and
        if(isJump && canJump){
            StartCoroutine(GroundColliderReset(jumpCool));
            rigid.AddForce(transform.up * curChara.jumpPower, ForceMode.Impulse);
            canJump = false; 
        }
    }
    void Move(){//movement when no skill is affecting
        if(!MoveImobilize && !curChara.cannotMove)
        transform.position += (horizontalMove*cameraHorizon.transform.right + verticalMove*cameraHorizon.transform.forward)*Time.deltaTime*movementSpeed;
    }
    void PlayerModelRotation(){//model rotation when no skill is affecting
        if(!ModelTurnImobilize && !customModelRotation){
            if(!isAim){
                targetFace = transform.position +  (horizontalMove*cameraHorizon.transform.right + verticalMove*cameraHorizon.transform.forward)*Time.deltaTime*movementSpeed;
                lerpFace = Vector3.Lerp(lerpFace, targetFace, modelRotationSpeed*Time.deltaTime);
            }else{
                targetFace = transform.position + cameraHorizon.transform.forward;
                lerpFace = targetFace;
            }
                
            curplayerModel.transform.LookAt(targetFace);
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
        curChara.ASkill1();
    }
    public virtual void PSkill1(){
        curChara.PSkill1();
    }
    public virtual void USkill1(){
        curChara.USkill1();
    }

    public void BuffAllCharacter(float buffAmount, float buffTime){
         StartCoroutine(IBuffAll(buffAmount, buffTime));
    }
    IEnumerator IBuffAll(float buffAmount, float buffTime){//

        characterlist[0].damagePercent += buffAmount;
        characterlist[1].damagePercent += buffAmount;
        characterlist[2].damagePercent += buffAmount;
        characterlist[3].damagePercent += buffAmount;

        yield return new WaitForSeconds(buffTime);
        
        characterlist[0].damagePercent -= buffAmount;
        characterlist[1].damagePercent -= buffAmount;
        characterlist[2].damagePercent -= buffAmount;
        characterlist[3].damagePercent -= buffAmount;
    }
    public void BuffCurCharacter(float buffAmount, float buffTime){
        StartCoroutine(IBuffCur(buffAmount, buffTime));
    }
    IEnumerator IBuffCur(float buffAmount, float buffTime){//
        Character tempCurChara = curChara;
        tempCurChara.damagePercent += buffAmount;

        yield return new WaitForSeconds(buffTime);
        
        tempCurChara.damagePercent -= buffAmount;
    }
}
