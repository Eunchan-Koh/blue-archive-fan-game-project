using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class MisakiMissile : Bullet
{
    Vector3 hitPos;

    [Header("Misaki small Explosion")]
    public GameObject smallExplosion;
    public override void BulletCollider(){
        RaycastHit hit;
        Physics.Raycast(transform.position, transform.forward, out hit, Time.fixedDeltaTime*bulletSpeed, enemyLayer|wallLayer);
        if(hit.collider != null){
            // Debug.Log(hit.collider);
            if(enemyLayer == (enemyLayer|1<<hit.collider.gameObject.layer) || wallLayer == (wallLayer|1<<hit.collider.gameObject.layer)){
                //데미지 주기
                // hit.collider.gameObject.GetComponent<Enemy>().GetDamage(dmg);
                //데미지 이펙트
                hitPos = hit.point;

                //삭제
                DestroyBullet();


            }

            
        }
    }
    public override void DestroyBullet(){
        Instantiate(smallExplosion, hitPos, Quaternion.identity);
        Destroy(gameObject);
    }

    // void OnTriggerEnter(Collider other){
    //     if(enemyLayer == (enemyLayer |1<<other.gameObject.layer) || wallLayer == (wallLayer |1<<other.gameObject.layer)){
    //         Instantiate(smallExplosion, transform.position, Quaternion.identity);
    //         Destroy(gameObject);
    //     }
    // }
}
