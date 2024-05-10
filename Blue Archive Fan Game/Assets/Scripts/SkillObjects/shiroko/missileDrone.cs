using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class missileDrone : MonoBehaviour
{
    public float initialDetectionRadius;
    public float detectionRadius;
    SphereCollider col;
    public LayerMask enemyLayer;
    public GameObject enemy;
    Enemy enemyScript;
    public MissileForDrone missile;
    GameObject enemyTargetedMark;
    bool shooting = false;
    public GameObject curEnemy;

    public float waitTime;
    public float existTime;
    public bool over = false;
    public float curTime;
    public GameObject[] movepoints;
    int movePointIndex = 0;
    [Range(0.0f, 10.0f)]
    public float AnimMoveSpeed;
    bool reached = false;
    //missile damage
    public float damage;
    void Start(){
        col = GetComponent<SphereCollider>();
        detectionRadius = initialDetectionRadius;
        curTime = 0;
        
    }
    IEnumerator MovingAnim(){
        float rn = Random.Range(2.0f, 5.0f);
        yield return new WaitForSeconds(rn);
        movePointIndex++;
        if(movePointIndex >= movepoints.Length){
            movePointIndex = 0;
        }
    }
    void Update(){
        curTime += Time.deltaTime;
        transform.position = Vector3.Lerp(transform.position, movepoints[movePointIndex].transform.position, Time.deltaTime*AnimMoveSpeed);
        // transform.position = Vector3.zero;
        // Debug.Log("moving to: " + movepoints[movePointIndex]);
        // Debug.Log(transform.position + ", " + movepoints[movePointIndex].transform.position);
        if(!reached && transform.position == movepoints[movePointIndex].transform.position){
            reached = true;
            StartCoroutine("MovingAnim");
        }
        
        if(!over && curTime>=existTime){
            over = true;
            // anim.SetTrigger("over");
            StopCoroutine("Shoot");
            shooting = false;
            curEnemy = null;
            missile.target = null;
            Invoke("DisablingDrone", 1f);
        }else if(!over && curTime >= waitTime){
            RaycastHit hit;
            Vector3 p1 = Camera.main.transform.position + Camera.main.transform.up;
            Vector3 p2 = Camera.main.transform.position - Camera.main.transform.up;
            // Debug.Log(p1);
            float radius = 1;
            if(Physics.CapsuleCast(p1,p2, radius,Camera.main.transform.forward, out hit, 50, enemyLayer)){
                if(enemy != hit.collider.gameObject){
                    
                    enemyTargetedMark?.SetActive(false);
                    enemy = hit.collider.gameObject;
                    enemyScript = enemy.GetComponent<Enemy>();
                    enemyTargetedMark = enemyScript.targetedMark;
                    enemyTargetedMark.SetActive(true);
                }
            }
            if(enemyScript != null && enemyScript.isDead) EnemyReset();
            if(enemy && !shooting)StartCoroutine("Shoot");
        }
        
    }
    void DisablingDrone(){
        gameObject.SetActive(false);
        curTime = 0;
        over = false;
        EnemyReset();
    }
    
    void EnemyReset(){
        enemy = null;
        curEnemy = null;
        enemyTargetedMark?.SetActive(false);
        enemyTargetedMark = null;
        enemyScript = null;
        
    }
    IEnumerator Shoot(){
        shooting = true;
        curEnemy = enemy;
        missile.target = curEnemy;
        missile.fromObject = this.gameObject.GetComponent<missileDrone>();
        missile.damage = damage;
        Instantiate(missile, transform.position, transform.rotation);
        yield return new WaitForSeconds(0.2f);
        Instantiate(missile, transform.position, transform.rotation);
        yield return new WaitForSeconds(0.2f);
        Instantiate(missile, transform.position, transform.rotation);
        yield return new WaitForSeconds(0.2f);
        Instantiate(missile, transform.position, transform.rotation);
        yield return new WaitForSeconds(1f);
        shooting = false;
    }
    void DetectAgain(){
        detectionRadius += initialDetectionRadius/(0.1f/Time.deltaTime);
    }
}
