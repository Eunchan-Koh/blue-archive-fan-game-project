using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileForDrone : MonoBehaviour
{
    public missileDrone fromObject;
    public GameObject target;
    public float moveSpeed;
    public float damage;
    Animator anim;
    public LayerMask enemyLayer;
    bool detecting = false;
    SphereCollider coll;
    Vector3 targetOldPos;
    void Start(){
        anim = GetComponent<Animator>();
        coll = GetComponent<SphereCollider>();
    }

    void Update(){
        if(target && !target.GetComponent<Enemy>().isDead){
            transform.LookAt(target.transform.position);
            transform.position = Vector3.MoveTowards(transform.position, target.transform.position, moveSpeed*Time.deltaTime);
            targetOldPos = target.transform.position;
        }else if(!target || target.GetComponent<Enemy>().isDead){
            target = null;
            if (!detecting){
                detecting = true;
                coll.enabled = true;
                anim.SetTrigger("detectNew");
            }
            transform.position = Vector3.MoveTowards(transform.position, targetOldPos, moveSpeed*Time.deltaTime);
            // Destroy(gameObject);
        }
    }
    void OnTriggerEnter(Collider other){
        if(enemyLayer == (enemyLayer | 1 << other.gameObject.layer)){
            detecting = false;
            target = other.gameObject;
            coll.radius = 0;
            coll.enabled = false;
        }
    }
}
