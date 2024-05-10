using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    // Start is called before the first frame update
    public LayerMask DestroyOnCollide;
    public Vector3 ToHere;
    public float bulletSpeed;
    public float dmg;
    public LayerMask enemyLayer;
    public LayerMask wallLayer;

    void Start()
    {

        transform.LookAt(ToHere);
        GetComponent<Rigidbody>().AddForce((ToHere-transform.position).normalized*bulletSpeed, ForceMode.Impulse);
        Destroy(this.gameObject, 3);
    }

        
    void FixedUpdate(){
        BulletCollider();
        //transform.position + (ToHere - transform.position)*100
        // transform.position = Vector3.MoveTowards(transform.position, ToHere, bulletSpeed*Time.deltaTime);
        
    }
    public virtual void BulletCollider(){
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
    public virtual void DestroyBullet(){
        //데미지 리셋, 한놈만 공격가능한 불릿임.
        dmg = 0;
        Destroy(gameObject, Time.deltaTime);
    }

    // void OnTriggerEnter(Collider collision){
    //     if(DestroyOnCollide == (DestroyOnCollide| (1<<collision.gameObject.layer))){
    //         // Debug.Log(collision);
    //         Destroy(gameObject);
    //     }
    // }

}
