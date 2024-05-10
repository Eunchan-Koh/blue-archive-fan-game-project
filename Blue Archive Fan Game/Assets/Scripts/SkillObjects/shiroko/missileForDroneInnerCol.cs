using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class missileForDroneInnerCol : MonoBehaviour
{
    public GameObject missile;
    public GameObject target;
    public float damage;
    MissileForDrone parent;
    public LayerMask floorLayer;
    void Start(){
        parent = GetComponentInParent<MissileForDrone>();
    }
    void Update(){
        target = parent.target;
        damage = parent.damage;
    }
    void OnTriggerEnter(Collider other){
        if(target && target == other.gameObject){
            target.GetComponent<Enemy>().GetDamage(damage);
            Destroy(missile);
        }else if(floorLayer == (floorLayer | 1 << other.gameObject.layer)){
            Destroy(missile);
        }
    }
}
