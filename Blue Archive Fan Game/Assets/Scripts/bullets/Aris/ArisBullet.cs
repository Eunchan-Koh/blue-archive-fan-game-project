using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArisBullet : Bullet
{
    public override void BulletCollider(){//raycast를 raycastAll로 바꿀것 - 현재 한 프레임에 한명의 적만 타겟팅하고 있음
        RaycastHit hit;
        Physics.Raycast(transform.position, transform.forward, out hit, Time.fixedDeltaTime*bulletSpeed, enemyLayer|wallLayer);
        if(hit.collider != null){
            Debug.Log(hit.collider);
            if(enemyLayer == (enemyLayer|1<<hit.collider.gameObject.layer)){
                //데미지 주기
                hit.collider.gameObject.GetComponent<Enemy>().GetDamage(dmg);
                //데미지 이펙트


                //삭제
                DestroyBullet();


            }else if(wallLayer == (wallLayer|1<<hit.collider.gameObject.layer)){

                //삭제
                DestroyBullet();

            }
            //give damage
            
        }
    }
    public override void DestroyBullet()
    {
        
    }

    // public virtual void BulletCollider(){
    //     RaycastHit hit;
    //     Physics.Raycast(transform.position, transform.forward, out hit, Time.fixedDeltaTime*bulletSpeed);
    //     if(hit.collider != null){
    //         Debug.Log(hit.collider);
    //         if(enemyLayer == (enemyLayer|1<<hit.collider.gameObject.layer)){
    //             //데미지 주기
    //             hit.collider.gameObject.GetComponent<Enemy>().GetDamage(dmg);

    //             //데미지 이펙트


    //             //삭제
    //             DestroyBullet();


    //         }else if(wallLayer == (wallLayer|1<<hit.collider.gameObject.layer)){

    //             //삭제
    //             DestroyBullet();

    //         }
    //         //give damage
            
    //     }
    // }
}
