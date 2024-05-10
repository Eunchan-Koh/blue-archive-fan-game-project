using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GranadeExplosion : MonoBehaviour
{
    public LayerMask DamageAble;
    public float damage;
    void Start()
    {
        Destroy(gameObject, 1f);
    }
    void Update(){
        // GetComponent<ParticleSystem>().colo = new Color(0,0,0,1);
    }

    void OnTriggerEnter(Collider other){
        // Debug.Log("aa!");
        if(DamageAble == (DamageAble|1<<other.gameObject.layer)){
            // Debug.Log("entered!");
            other.gameObject.GetComponent<Enemy>().GetDamage(damage);
        }
    }
}
