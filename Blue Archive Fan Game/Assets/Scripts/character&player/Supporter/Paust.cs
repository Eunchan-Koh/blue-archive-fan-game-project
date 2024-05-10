using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paust : Supporter
{
    public Character character;
    public GameObject MissileBombardment;
    public override IEnumerator SupportSkillExecution(){
        Instantiate(MissileBombardment, character.transform.forward, character.transform.rotation);
        yield return null;
    }
}
