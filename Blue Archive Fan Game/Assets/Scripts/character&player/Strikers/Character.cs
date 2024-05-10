using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class Character : MonoBehaviour
{
    [Header("Player")]
    public PlayerMove player;//for using buff, if a character give buff to all allies characters, then that 'all' is the perspective of character since 
                             //player chose the characters in a party with member of 4. Player will have Buff method, and characters will use it.
    [Header("Supporter")]
    public Supporter supporter;
    [Header("Character Settings")]
    public int CharacterId;
    public bool isLoaded;
    [Header("Camera Setting")]
    public float targetCameraSize;
    public CameraOrigin cameraOrigin;
    public GameObject cameraHorizon;

    [Header("Character Model")]
    public GameObject playerModel;
    public GameObject groundCollider;

    public Vector3 offset_groundCollider;
    public float radius_groundCollider;

    [Header("Animator Setting")]
    public Animator anim;
    [Header("Sound Setting")]
    public AudioClip GunSound;
    
    [Header("Speed Setting")]
    public float runSpeed;
    public float walkSpeed;
    public float crouchSpeed;
    public float appliedMovementSpeed;

    [Header("Jump Setting")]
    public float jumpPower;
    public bool canJump;
    [Header("HP Setting")]
    public float maxHp;
    public float appliedMaxHp;
    public float curHp;
    public bool isDead = false;

    [Header("Attack Setting")]
    [Range(0.0f, 5.0f)]
    public float shootingDelay;
    public float appliedShootingDelay;
    public bool isReloading;
    public float reloadingTimeBefore;//장전되는데 걸리는 시간
    public float appliedReloadingTimeBefore;
    public float reloadingTimeLate;//장전 후딜, 이 시간동안 공격(fire) 불가
    public float appliedReloadingTimeLate;
    protected bool canFire = true;

    [Header("Aim Setting")]
    public bool isAim;


    [Header("Weapon & Bullet Setting")]
    public GameObject[] Weapons;
    public GameObject[] Bullets;
    public Transform BulletPos;
    public float bulletDistance;
    public int maxBulletCount;
    public int appliedMaxBulletCount;
    public int curBulletCount;
    public Vector3 targetPos;


    [Header("Skill Setting")]
    [Range(0.0f, 60.0f)]
    public float ASkillCool;
    public float appliedASkillCool;
    public float ASkillCoolWaiting = 0;
    public bool canUseASkill = true;
    [Range(0.0f, 60.0f)]
    public float USkillCool;
    public float appliedUSkillCool;
    public float USkillCoolWaiting = 0;
    public bool canUseUSkill = true;
    public bool UsingASkill = false;
    public bool UsingUSkill = false;

    [Header("motion restriction")]
    public bool cannotShoot;
    public bool cannotMove;
    protected bool cannotChangeCharacterFire;
    protected bool cannotChangeCharacterAction;
    protected bool cannotChangeCharacterUlt;
    protected bool cannotChangeCharacter;

    [Header("Buff Setting")]
    public float damagePercent;
    
    void Awake(){
        // Time.timeScale = 0;
        SupporterCalculation();
        curBulletCount = appliedMaxBulletCount;
        curHp = appliedMaxHp;
        this.gameObject.SetActive(false);
        isLoaded = true;
    }
    void Update(){
        
        GroundColliderOption();
        DamageCalculation();
        // SkillAvailabilityCheck();
        ChangeCharacterReqCheck();
    }
    void SupporterCalculation(){
        if(supporter){
            appliedASkillCool = ASkillCool*(1-supporter.SkillCooldownPercent); // v
            appliedMaxBulletCount = (int)(maxBulletCount*(1+supporter.MagAmountPercent)); // v
            appliedMaxHp = maxHp*(1+supporter.HpPercent); // v
            appliedReloadingTimeBefore = reloadingTimeBefore*(1-supporter.ReloadSpeedPercent); // v
            appliedReloadingTimeLate = reloadingTimeLate*(1-supporter.ReloadSpeedPercent); // v
            appliedShootingDelay = shootingDelay*(1-supporter.AttackSpeedPercent); // v
            appliedUSkillCool = USkillCool*(1-supporter.SkillCooldownPercent); // v 
        }else{
            appliedASkillCool = ASkillCool; // v
            appliedMaxBulletCount = maxBulletCount; // v
            appliedMaxHp = maxHp; // v
            appliedReloadingTimeBefore = reloadingTimeBefore; // v
            appliedReloadingTimeLate = reloadingTimeLate; // v
            appliedShootingDelay = shootingDelay; // v
            appliedUSkillCool = USkillCool; // v 
        }
        
    }
    void GroundColliderOption(){
        groundCollider.transform.localScale = new Vector3(radius_groundCollider, radius_groundCollider, radius_groundCollider);
        groundCollider.transform.localPosition = offset_groundCollider;
    }
    public virtual void DamageCalculation(){

    }
    public void SkillAvailabilityCheck(){
        if(!canUseASkill){
            ASkillCoolWaiting+=Time.deltaTime;
        }
        if(ASkillCoolWaiting>=appliedASkillCool){
            canUseASkill = true;
            ASkillCoolWaiting = 0;
        }

        if(!canUseUSkill){
            USkillCoolWaiting+=Time.deltaTime;
        }
        if(USkillCoolWaiting>=appliedUSkillCool){
            canUseUSkill = true;
            USkillCoolWaiting = 0;
        }
    }
    void ChangeCharacterReqCheck(){
        cannotChangeCharacter = cannotChangeCharacterFire || cannotChangeCharacterAction || cannotChangeCharacterUlt;
    }
    public bool GetCannotChangeCharaVal(){
        return cannotChangeCharacter;
    }

    public virtual void Aim(){

    }
    // public virtual void Jump(){

    // }
    
    public void Fire(){
        if(!cannotShoot && canFire && isAim && !isReloading && curBulletCount > 0){
            //flag
            canFire = false;
            cannotChangeCharacterFire = true;
            StartCoroutine("Firing");
        } 
    }
    public virtual IEnumerator Firing(){
        yield return null;
    }
    public void Reloading(){
        if(!isReloading && curBulletCount < appliedMaxBulletCount){
            isReloading = true;
            StartCoroutine("ReloadAction");
        } 
    }
    public virtual IEnumerator ReloadAction(){
        yield return null;
    }
    public void CancelReloading(){
        StopCoroutine("ReloadAction");
        isReloading = false;
    }
    public void ASkill1(){
        if(!UsingASkill && !UsingUSkill && canUseASkill){
            UsingASkill = true;//to check if skill motion requires its own animation time, disabling other behaviours
            canUseASkill = false;
            cannotChangeCharacterAction = true;
            StartCoroutine("ASkillAction");
        }
    }
    public virtual IEnumerator ASkillAction(){
        yield return null;
    }
    public virtual void PSkill1(){

    }
    public void USkill1(){
        if(!UsingUSkill && !UsingUSkill && canUseUSkill){
            UsingUSkill = true;
            canUseUSkill = false;
            cannotChangeCharacterUlt = true;
            StartCoroutine("USkillAction");
        }
    }
    public virtual IEnumerator USkillAction(){
        yield return null;
    }
    public virtual void CustomCameraMove(){
        // targetCameraSize = 2;
        // cameraOrigin.cameraDistance = Mathf.Lerp(cameraOrigin.cameraDistance, targetCameraSize, aimMagniSpeed*Time.deltaTime);
    }
    public virtual void CustomModelRotation(){
        Vector3 targetFace = transform.position + transform.forward;
            playerModel.transform.LookAt(targetFace);
    }
    // Buff is given by playermove script.

    // public virtual void GetBuff(float buffAmount, float buffTime){//needs to be fixed; add buff method on playermove to share buffTime, or use other methods
    //     StartCoroutine(BuffRoutine(buffAmount, buffTime));
    // }
    // public virtual IEnumerator BuffRoutine(float buffAmount, float buffTime){//
    //     damagePercent += buffAmount;

    //     yield return new WaitForSeconds(buffTime);
        
    //     damagePercent -= buffAmount;
    // }
    
    public virtual void GetDamage(float damageAmount){
        curHp-=damageAmount;
        if(curHp<=0){
            curHp = 0;
            isDead = true;
        }
    }
    public virtual void GetHeal(float healAmount){
        curHp += healAmount;
        if(curHp>=appliedMaxHp)curHp = appliedMaxHp;

    }
    //on disabling each character, resetting values(such as bool, float, etc.)
    public virtual void ResetValues(){

    }
}
