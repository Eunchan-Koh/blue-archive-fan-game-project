using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class missileTurret : MonoBehaviour
{
    public float initialDetectionRadius;
    public float detectionRadius;
    public SphereCollider col;
    public LayerMask enemyLayer;
    public GameObject enemy;
    Enemy enemyScript;
    public GameObject missile;
    GameObject enemyTargetedMark;
    bool enemyDeadReset;
    void Start(){
        col = GetComponent<SphereCollider>();
        detectionRadius = initialDetectionRadius;
    }
    void Update(){
        col.radius = detectionRadius;
        if(enemy && enemyScript.isDead)EnemyReset();
        if(enemy)transform.LookAt(enemy.transform.position);
        if(!enemy && (detectionRadius >= initialDetectionRadius||enemyDeadReset)){
            detectionRadius = 0;
            enemyDeadReset = false;
        } 
        if(!enemy)DetectAgain();
    }
    void OnTriggerEnter(Collider other){
        if(enemyLayer == (enemyLayer | 1 << other.gameObject.layer)){
            if(!enemy){
                detectionRadius = initialDetectionRadius;
                enemy = other.gameObject;
                enemyScript = enemy.GetComponent<Enemy>();
                enemyTargetedMark = enemyScript.targetedMark;
                enemyTargetedMark.SetActive(true);
            }
        }
    }
    void OnTriggerExit(Collider other){
        if(other.gameObject == enemy){
            EnemyReset();
        }
    }
    void EnemyReset(){
        enemyDeadReset = true;
        enemy = null;
        enemyTargetedMark.SetActive(false);
        enemyTargetedMark = null;
        enemyScript = null;
        
    }
    void DetectAgain(){
        detectionRadius += initialDetectionRadius/(0.1f/Time.deltaTime);
    }
}
