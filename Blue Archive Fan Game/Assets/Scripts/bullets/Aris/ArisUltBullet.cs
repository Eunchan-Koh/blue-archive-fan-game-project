using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArisUltBullet : Bullet
{
    public GameObject[] particleObjects;
    public Vector3[] originalDiameters;
    public Vector3 thisOriDiameter;
    public float diameterPercent;

    public override void DestroyBullet()
    {
        
    }
    public override void BulletCollider(){

    }
    void OnTriggerEnter(Collider other){
        if(enemyLayer == (enemyLayer|1<<other.gameObject.layer)){
                //데미지 주기
                other.gameObject.GetComponent<Enemy>().GetDamage(dmg);

                //데미지 이펙트



            }
    }
    public void FixDiameter(float diameterPercent){
        transform.localScale = new Vector3(thisOriDiameter.x*(1+diameterPercent), 
                                            thisOriDiameter.y*(1+diameterPercent), 
                                            thisOriDiameter.z*(1+diameterPercent));
        for(int i = 0; i < particleObjects.Length; i++){
            particleObjects[i].transform.localScale = new Vector3(originalDiameters[i].x*(1+diameterPercent),
                                                                    originalDiameters[i].y*(1+diameterPercent),
                                                                    originalDiameters[i].z*(1+diameterPercent));
        }
    }
}
