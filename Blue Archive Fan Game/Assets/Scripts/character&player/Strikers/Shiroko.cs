using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Security.Cryptography;
using UnityEngine;

public class Shiroko : Character
{
    [Header("Shiroko Weapons")]
    public GameObject granade;
    public GameObject MissileDrone;
    public float DroneExistTime;

    [Header("Damages")]
    public float bulletDamage;
    public float originalBulletDamage;
    public float granadeDamage;
    public float originalGranadeDamage;
    public float droneMissileDamage;
    public float originalDroneMissileDamage;

    // public override void Jump(){
    //     rigid.AddForce(transform.up * jumpPower, ForceMode.Impulse);
    // }
    public override void DamageCalculation(){
            bulletDamage = originalBulletDamage * damagePercent;
            granadeDamage = originalGranadeDamage * damagePercent;
            droneMissileDamage = originalDroneMissileDamage * damagePercent;
        if(supporter){
            bulletDamage *= 1+supporter.DamagePercent;
            granadeDamage *= 1+supporter.DamagePercent;
            droneMissileDamage *= 1+supporter.DamagePercent;
        }
        

        Bullets[0].GetComponent<Bullet>().dmg = bulletDamage;
        granade.GetComponent<Granade>().damage = granadeDamage;
        MissileDrone.GetComponent<missileDrone>().damage = droneMissileDamage;
        // missileDrone.GetComponent<missileDrone>().;
    }

    public override  IEnumerator Firing(){
        

        //shoot bullet
        Bullets[0].GetComponent<Bullet>().ToHere = targetPos;
        Instantiate(Bullets[0], BulletPos.position, transform.rotation);
        GetComponent<AudioSource>().Stop();
        GetComponent<AudioSource>().clip = GunSound;
        GetComponent<AudioSource>().Play();
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
        GameObject tempGranade = Instantiate(granade,transform.position, transform.rotation);
        tempGranade.GetComponent<Rigidbody>().AddForce((cameraHorizon.transform.forward+transform.up)*20, ForceMode.Impulse);
        yield return null;
        UsingASkill = false;
        cannotChangeCharacterAction = false;
    }
    public override void PSkill1(){//passive
        
    }
    public override IEnumerator USkillAction(){
        //cancel reload, cancel restrictions from reload
        CancelReloading();

        UsingUSkill = false;
        MissileDrone.GetComponent<missileDrone>().existTime = DroneExistTime;
        MissileDrone.SetActive(true);
        cannotChangeCharacterUlt = false;
        yield return null;
    }
    public override void CustomCameraMove(){
        // targetCameraSize = 2;
        // cameraOrigin.cameraDistance = Mathf.Lerp(cameraOrigin.cameraDistance, targetCameraSize, aimMagniSpeed*Time.deltaTime);
    }
    public override void CustomModelRotation(){
        Vector3 targetFace = transform.position + transform.forward;
            playerModel.transform.LookAt(targetFace);
    }
    public override void ResetValues(){
        
    }

    
}