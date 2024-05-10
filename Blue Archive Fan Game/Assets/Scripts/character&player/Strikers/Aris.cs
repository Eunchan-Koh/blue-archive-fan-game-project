using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.Analytics;

public class Aris : Character
{
    [Header("Aris Weapons")]
    public GameObject UltBullet;

    [Header("Damages")]
    public float bulletDamage;
    public float originalBulletDamage;
    public float UltBulletDamage;
    public float originalUltBulletDamage;

    [Header("Aris ActionSkill Related")]
    [Range(0, 2)]
    public int chargeAmount;
    [Range(0.0f,1.0f)]
    public float diameterIncreasePerCharge;
    [Range(0.0f, 1.0f)]
    public float ActionSkillsDamageBuffAmount;
    public override void DamageCalculation(){
        bulletDamage = originalBulletDamage * damagePercent * (1+supporter.DamagePercent);
        UltBulletDamage = originalUltBulletDamage * damagePercent * (1+supporter.DamagePercent);

        Bullets[0].GetComponent<Bullet>().dmg = bulletDamage;
        UltBullet.GetComponent<ArisUltBullet>().dmg = UltBulletDamage;
    }

    public override IEnumerator Firing(){
        
        
        //shoot bullet
        Bullets[0].GetComponent<Bullet>().ToHere = targetPos;
        Instantiate(Bullets[0], BulletPos.position, transform.rotation);
        curBulletCount -= 1;
        // Debug.Log("shooted!");
        //wait for delay
        yield return new WaitForSeconds(appliedShootingDelay);

        //can fire now
        canFire = true;
        cannotChangeCharacterFire = false;
    }

    public override IEnumerator ReloadAction(){
        yield return new WaitForSeconds(appliedReloadingTimeBefore);
        curBulletCount = appliedMaxBulletCount;
        yield return new WaitForSeconds(appliedReloadingTimeLate);
        isReloading = false;

    }
    public override IEnumerator ASkillAction(){
        int tempCharge = chargeAmount + 1;
        if(tempCharge>=2) chargeAmount = 2;
        else chargeAmount = tempCharge;

        //diameter buff
        UltBullet.GetComponent<ArisUltBullet>().FixDiameter(diameterIncreasePerCharge*chargeAmount);

        //UI - change image to half-filled/fully-filled battery

        //buff damage for 0.8* of actionSkillCool after fixing buff method
        player.BuffCurCharacter(ActionSkillsDamageBuffAmount, ASkillCool-2);

        yield return null;
        UsingASkill = false;
        cannotChangeCharacterAction = false;
    }
    public override void PSkill1(){//passive
        
    }

    public override IEnumerator USkillAction(){
        //cancel reload, cancel restrictions from reload
        CancelReloading();


        cannotShoot = true;
        cannotMove = true;
        // MoveImobilize = true;
        // customCameraMove = true;//custom camera move required for aris ult
        // customModelRotation = true;//custom model rotation requried for aris ult
        yield return new WaitForSecondsRealtime(1);
        UltBullet.GetComponent<Bullet>().ToHere = targetPos;
        Instantiate(UltBullet, transform.position, transform.rotation);
        yield return new WaitForSecondsRealtime(0.5f);
        cannotShoot = false;
        cannotMove = false;
        UsingUSkill = false;

        chargeAmount = 0;
        UltBullet.GetComponent<ArisUltBullet>().FixDiameter(0);
        yield return new WaitForSecondsRealtime(0.3f);
        cannotChangeCharacterUlt = false;
        // MoveImobilize = false;
        // customCameraMove = false;
        // customModelRotation = false;
    }
    public override void CustomCameraMove(){
        targetCameraSize = 2;
        // cameraOrigin.cameraDistance = Mathf.Lerp(cameraOrigin.cameraDistance, targetCameraSize, aimMagniSpeed*Time.deltaTime);
    }
    public override void CustomModelRotation(){
        Vector3 targetFace = transform.position + transform.forward;
            playerModel.transform.LookAt(targetFace);
    }

    public override void ResetValues(){
        
    }
    
}
