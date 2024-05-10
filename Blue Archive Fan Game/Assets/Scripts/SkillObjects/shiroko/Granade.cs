using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Granade : MonoBehaviour
{
    public GameObject explosion;
    public float damage;
    public float TimeToExplode;
    public float curTimeSpent;
    void Update()
    {
        curTimeSpent += Time.deltaTime;
        if(curTimeSpent>=TimeToExplode){
            explosion.GetComponent<GranadeExplosion>().damage = damage;
            Instantiate(explosion, transform.position, transform.rotation);
            Destroy(gameObject);
        }
        
    }
}
