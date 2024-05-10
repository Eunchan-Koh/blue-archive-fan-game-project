using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    public float maxHP;
    public float curHP;
    public bool isDead = false;
    public Image hpUI;
    public GameObject targetedMark;
    MeshRenderer meshr;
    public float dyingTime;
    public float timeAfterDead;
    SphereCollider detectCol;
    public bool detectedPlayer;
    public LayerMask playerLayer;
    public GameObject player;
    void Start(){
        meshr = GetComponent<MeshRenderer>();
        detectCol = GetComponent<SphereCollider>();
    }
    void Update(){
        HPUIUpdate();
        if(curHP<=0 && !isDead){
            isDead = true;
            gameObject.layer = 8;
            DeadPush();
            Destroy(gameObject, dyingTime);
        }
        if(isDead){
            timeAfterDead += Time.deltaTime;
            meshr.material.color =new Color(1f, 1f, 1f, (dyingTime - (timeAfterDead+0f))/dyingTime);
        }

        // if(detectedPlayer){
        //     if(player)transform.LookAt(player.transform.position);
        // }
    }

    void DeadPush(){
        GetComponent<Rigidbody>().AddForce(-transform.forward*50 + -transform.up*50, ForceMode.Impulse);
    }
    void HPUIUpdate(){
        hpUI.fillAmount = curHP/maxHP;
    }
    public void GetDamage(float dmg){
        curHP -= dmg;
    }

}
