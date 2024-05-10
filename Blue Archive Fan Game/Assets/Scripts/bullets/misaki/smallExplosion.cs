using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class smallExplosion : MonoBehaviour
{
    float a;
    [Header("Explosion ends in")]
    public float existTime;
    float timeSpent = 0;
    public float damage;
    Material mat;
    public LayerMask enemyLayer;
    List<GameObject> hitEnemies;
    void Start(){
        mat = GetComponent<MeshRenderer>().material;
        hitEnemies = new List<GameObject>();
    }
    void Update(){
        timeSpent+=Time.deltaTime;
        if(timeSpent>= existTime){
            hitEnemies.Clear();
            Destroy(gameObject);
        }
        a = 1-(timeSpent/existTime);
        mat.color = new Color(1,1,1,a);
    }
    void OnTriggerEnter(Collider other){
        if(enemyLayer == (enemyLayer|1<<other.gameObject.layer)){
            // hitEnemies.Add(other.gameObject);
            other.GetComponent<Enemy>().GetDamage(damage);
        }
    }

}
