using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionManager : MonoBehaviour
{
    Character character;
    [SerializeField]
    int pos;
    [SerializeField]
    Transform PEParent;
    [SerializeField]
    Transform PEIconParent;

    List<PositionEffect> positionEffects = new List<PositionEffect>();
    List<PositionEffect> deletePEs = new List<PositionEffect>();

    InfoText infoText;

    private void Start()
    {
        infoText = FindObjectOfType<InfoText>();
    }

    public void ApplyPE(PositionEffect.PositionEffectParams PEParams)
    {
        bool f = false;
        if (PEParams.applyPE.GetComponent<PositionEffect>().GetPositionEffectStatus().merge)//マージするなら
        {
            PositionEffect PE = PEParams.applyPE.GetComponent<PositionEffect>();
            foreach (PositionEffect pe in positionEffects)
            {
                if (pe.GetComponent<PositionEffect>().GetPositionEffectStatus().PEName == PE.GetPositionEffectStatus().PEName)//同種のStEがすでにあるなら
                {
                    pe.GetComponent<PositionEffect>().AddStack(PEParams.stack);
                    //charaObj.SetDamageText(string.Format("付与：{0}", PE.GetPAName()), Color.white);
                    //infoText.AddLogText(string.Format("{0}は{1}を付与された", charaStatus.charaName, PE.GetPAName()));
                    f = true;
                }
            }
        }
        if (!f)
        {
            var s = Instantiate(PEParams.applyPE, PEParent);
            var icon = Instantiate(Definer.positionEffectIcon, PEIconParent);
            positionEffects.Add(s.GetComponent<PositionEffect>());
            s.GetComponent<PositionEffect>().Init(character,this,PEParams,icon.GetComponent<PEIcon>());
            //charaObj.SetDamageText(string.Format("付与：{0}", s.GetComponent<PA_StatusEffect>().GetPAName()), Color.white);
            //infoText.AddLogText(string.Format("{0}は{1}を付与された", charaStatus.charaName, s.GetComponent<PA_StatusEffect>().GetPAName()));
        }
    }
    public void RemovePE(PositionEffect positionEffect)
    {
        deletePEs.Add(positionEffect);
        infoText.AddLogText(string.Format("ポジション{0}から{1}が消去", pos.PosIntToStr(), positionEffect.GetPEName(true)));
    }
    void RemovePE_Execute()
    {
        foreach (PositionEffect deletePE in deletePEs) { positionEffects.Remove(deletePE); }
        deletePEs.Clear();
    }

    public void SetCharacter(Character chara)
    {
        //bool f = false;
        //if (character != chara)
        //{
        //    f = true;
        //    if (character != null)
        //    {
        //        FindObjectOfType<InfoText>().AddDebugText(string.Format("pm:{0}が{1}から退去", character.GetCharacterStatus().charaName, character.GetCharacterStatus().position.ToString()));
        //        foreach (PositionEffect positionEffect in positionEffects) { positionEffect.OnCharaLeave(); }
        //        RemovePE_Execute();
        //    }   
        //}
        character = chara;
        foreach (PositionEffect positionEffect in positionEffects) { positionEffect.SetCharacter(character); }
        RemovePE_Execute();
        //if (f)
        //{
        //    FindObjectOfType<InfoText>().AddDebugText(string.Format("pm:{0}が{1}に侵入!!", character.GetCharacterStatus().charaName, character.GetCharacterStatus().position.ToString()));
        //    foreach (PositionEffect positionEffect in positionEffects) { positionEffect.OnCharaEnter(); }
        //    RemovePE_Execute();
        //}
    }
    public void ResetCharacter()
    {
        if (character != null)
        {
            //FindObjectOfType<InfoText>().AddDebugText(string.Format("pm(reset):{0}が{1}から退去", character.GetCharacterStatus().charaName, character.GetCharacterStatus().position.ToString()));
            foreach (PositionEffect positionEffect in positionEffects) { positionEffect.SetCharacter(null); }
            RemovePE_Execute();
        }
        character = null;
    }

    public string GetPEInfo()
    {
        string info = "◇◇ポジション効果◇◇\n";
        if (positionEffects.Count == 0) { info += "なし\n"; }
        else
        {
            foreach (PositionEffect pe in positionEffects)
            {
                info += string.Format("<{0}>[{1}スタック]\n{2}", pe.GetPEName(true),pe.GetPositionEffectStatus().stack,pe.GetPEInfo(false));
            }
        }
        return info;
    }
    public bool CheckHasPE(GameObject PEObj)
    {
        PositionEffect.PositionEffectStatus PE = PEObj.GetComponent<PositionEffect>().GetPositionEffectStatus();
        foreach (PositionEffect pe in positionEffects)
        {
            if (pe.GetComponent<PositionEffect>().GetPositionEffectStatus().PEName == PE.PEName) { return true; }
        }
        return false;
    }

    public void OnBattleStart()
    {
        foreach (PositionEffect positionEffect in positionEffects) { positionEffect.OnBattleStart(); }
        RemovePE_Execute();
    }
    public void OnRoundStart()
    {
        foreach (PositionEffect positionEffect in positionEffects) { positionEffect.OnRoundStart(); }
        RemovePE_Execute();
    }
    public void OnTurnStart()
    {
        foreach (PositionEffect positionEffect in positionEffects) { positionEffect.OnTurnStart(); }
        RemovePE_Execute();
    }
    public void OnTurnEnd()
    {
        foreach (PositionEffect positionEffect in positionEffects) { positionEffect.OnTurnEnd(); }
        RemovePE_Execute();
    }
    public void OnRoundEnd() {
        foreach (PositionEffect positionEffect in positionEffects) { positionEffect.OnRoundEnd(); }
        RemovePE_Execute();
    }
    public void OnBattleEnd()
    {
        foreach (PositionEffect positionEffect in positionEffects) { positionEffect.OnBattleEnd(); }
        RemovePE_Execute();
    }
}
