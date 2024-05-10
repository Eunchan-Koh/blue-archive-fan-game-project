using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSkillCoolManager : MonoBehaviour
{
    public Character[] characterlist;
    public PlayerMove player;
    public Character curChara;
    void Update(){
        curChara = player.curChara;
        //characters skill check
        for(int i = 0; i < characterlist.Length; i++){
            characterlist[i].SkillAvailabilityCheck();
        }

        
    }
    //also manage buffcheck method in this script
}
