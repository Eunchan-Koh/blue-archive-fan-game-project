using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public PlayerMove player;
    Character curChara;
    public GameObject reloadingMark;
    public Image curCharaHpBar;
    public Text curcharaHPText;
    public SkillUIManager SkillUIs;

    public Image[] characterHPBars;
    public GameObject[] HPBarObjects;
    public Text[] characterNameTexts;
    


    void Update()
    {
        if(player.curChara){//player value is loaded
            curChara = player.curChara;
            //reloading ui
            if(curChara.isReloading){
                reloadingMark.SetActive(true);
            }else{
                reloadingMark.SetActive(false);
            }
            //curChara hpBar UI
            curCharaHpBar.fillAmount = curChara.curHp/curChara.appliedMaxHp;
            curcharaHPText.text = curChara.curHp + "/" + curChara.appliedMaxHp;

            //all character hpBar UIs
            for(int i = 0; i < 4; i++){//can form a party of up to 4
                if(player.characterlist.Length>i){//make sure that ith character exist in party - party of 1~3 can be made too.
                    characterNameTexts[i].text = player.characterlist[i].name;
                    characterHPBars[i].fillAmount = player.characterlist[i].curHp/player.characterlist[i].appliedMaxHp;
                }else{
                    // characterNameTexts[i].text = "";
                    HPBarObjects[i].SetActive(false);
                }
            }


            //Skill Cooldown UIs
            if(curChara.canUseASkill){
                SkillUIs.ActionSkill.coolDownText.text = "";
                SkillUIs.ActionSkill.FillSection_cool.fillAmount = 1;
            }else{
                SkillUIs.ActionSkill.coolDownText.text = ((int)(curChara.appliedASkillCool-curChara.ASkillCoolWaiting)).ToString();
                SkillUIs.ActionSkill.FillSection_cool.fillAmount = (float)(curChara.ASkillCoolWaiting/curChara.appliedASkillCool);
            }
            if(curChara.canUseUSkill){
                SkillUIs.UltSkill.coolDownText.text = "";
                SkillUIs.UltSkill.FillSection_cool.fillAmount = 1;
            }else{
                SkillUIs.UltSkill.coolDownText.text = ((int)(curChara.appliedUSkillCool-curChara.USkillCoolWaiting)).ToString();
                SkillUIs.UltSkill.FillSection_cool.fillAmount = curChara.USkillCoolWaiting/curChara.appliedUSkillCool;
            }

            if(!curChara.GetCannotChangeCharaVal() && !player.player_CannotChangeChara){
                SkillUIs.SwapSkill.coolDownText.text = "";
                SkillUIs.SwapSkill.FillSection_cool.fillAmount = 1;
            }else{
                SkillUIs.SwapSkill.coolDownText.text = "X";
                SkillUIs.SwapSkill.FillSection_cool.fillAmount = 0;
            } 
        }
        
    }
}
