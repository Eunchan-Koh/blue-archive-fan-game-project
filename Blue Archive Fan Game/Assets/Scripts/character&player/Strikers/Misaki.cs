using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Misaki : Character
{
    public override void DamageCalculation(){
        //damage = damage * damagePercent

        //prefab.damage = damage calculated


    }

    public override  IEnumerator Firing(){
        

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
