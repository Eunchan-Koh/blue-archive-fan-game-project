using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Supporter : MonoBehaviour
{
    [Header("Buff Stat Setting")]
    [Range(0.0f, 1.0f)]//once a character equips this supporter,
    public float DamagePercent;//increase damage of the character
    [Range(0.0f, 1.0f)]
    public float SpeedPercent;//increase movementSpeed of the character
    [Range(0.0f, 1.0f)]
    public float HpPercent;//increase Hp of the character
    [Range(0.0f, 1.0f)]
    public float SkillCooldownPercent;//increase SkillCooldown time of the character by percentage
    [Range(0.0f, 1.0f)]
    public float ReloadSpeedPercent;//increase reloading speed of the character
    [Range(0.0f, 1.0f)]
    public float AttackSpeedPercent;//increase attack speed of the character
    [Range(0.0f, 1.0f)]
    public float MagAmountPercent;//increase magazine(max bullet value) of the character
    [Range(0.0f, 1.0f)]
    public float CritRatePercent;//increase critical rate of the character(this trait is not added to character script yet.)
    bool usingSSkill;
    public void SupportSkill(){
        if(!usingSSkill){
            usingSSkill = true;
            StartCoroutine("SupportSkillExecution");
        }
    }
    public virtual IEnumerator SupportSkillExecution(){
        yield return null;
    }
}
