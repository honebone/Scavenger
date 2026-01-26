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

    List<GameObject> actionMods = new List<GameObject>();

    CharactersManager charactersManager;
    InfoText infoText;
    Character_TargetButton targetButton;

    private void Start()
    {
        infoText = ExpeditionRef.infoText;
        charactersManager = ExpeditionRef.charactersManager;
        targetButton = charactersManager.GetTargetButton(pos);
    }

    public void ApplyPE(PositionEffect.PositionEffectParams PEParams,Character.CharacterStatus ownerStatus)
    {
        bool f = false;
        PositionEffect PE = PEParams.applyPE.GetComponent<PositionEffect>();
        PositionEffect.PositionEffectStatus PEStatus = PE.GetPositionEffectStatus();
        if (PEParams.applyPE.GetComponent<PositionEffect>().GetPositionEffectStatus().merge)//マージするなら
        {
            foreach (PositionEffect pe in positionEffects)
            {
                if (pe.GetComponent<PositionEffect>().GetPositionEffectStatus().PEName == PE.GetPositionEffectStatus().PEName)//同種のStEがすでにあるなら
                {
                    pe.GetComponent<PositionEffect>().AddStack(PEParams.stack);

                    infoText.AddLogText(string.Format("ポジション{0}に{1}が付与された", pos.PosIntToStr(), PEStatus.PEName.ColorStr(PEStatus.ToColor())));

                    f = true;
                }
               

            }
        }
        if (!f)
        {
            var s = Instantiate(PEParams.applyPE, PEParent);
            var icon = Instantiate(Definer.positionEffectIcon, PEIconParent);
            positionEffects.Add(s.GetComponent<PositionEffect>());
            s.GetComponent<PositionEffect>().Init(character,this,PEParams,ownerStatus,icon.GetComponent<PEIcon>());
            if (PEStatus.refValue)
            {
                targetButton.SetDamageText(string.Format("+{0}{1}", PEStatus.PEName, PEParams.stack), PEStatus.ToColor());
                infoText.AddLogText(string.Format("ポジション{0}に{1}{2}が付与された", pos.PosIntToStr(), PEStatus.PEName.ColorStr(PEStatus.ToColor()), PEParams.value));
            }
            else
            {
                targetButton.SetDamageText(string.Format("+{0}", PEStatus.PEName), PEStatus.ToColor());
                infoText.AddLogText(string.Format("ポジション{0}に{1}が付与された",pos.PosIntToStr(), PEStatus.PEName.ColorStr(PEStatus.ToColor())));
            }
        }
    }

    /// <summary>
    /// PEが消去された後に呼ばれる (PEによって)
    /// </summary>
    /// <param name="positionEffect"></param>
    public void DisablePE(PositionEffect positionEffect)
    {
        //deletePEs.Add(positionEffect);
        positionEffects.Remove(positionEffect);
        infoText.AddLogText(string.Format("ポジション{0}から{1}が消去", pos.PosIntToStr(), positionEffect.GetPEName(true)));
    }

    public void RemovePE(ActionData.RemovePE removePE)
    {
        PositionEffect.PositionEffectStatus status = removePE.removePE.GetComponent<PositionEffect>().GetPositionEffectStatus();
        foreach (PositionEffect PE in new List<PositionEffect>(positionEffects))
        {
            if (PE.GetPositionEffectStatus().PEName == status.PEName)
            {
                if (removePE.removeAll) { PE.Disable(); }
                else { PE.AddStack(removePE.addAmount); }
            }
        }
        //メッセージ
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
        foreach (PositionEffect positionEffect in new List<PositionEffect>(positionEffects)) { positionEffect.SetCharacter(character); }//new List<PositionEffect>(positionEffects)
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
            foreach (PositionEffect positionEffect in new List<PositionEffect>(positionEffects)) { positionEffect.SetCharacter(null); }
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

    public void AddActionMod(GameObject mod, bool set)
    {
        if (set) { actionMods.Add(mod); }
        else { actionMods.Remove(mod); }

    }
    public List<GameObject> GetActionMods() { return actionMods; }

    public void OnBattleStart()
    {
        foreach (PositionEffect positionEffect in new List<PositionEffect>(positionEffects)) { positionEffect.OnBattleStart(); }
        RemovePE_Execute();
    }
    public void OnRoundStart()
    {
        foreach (PositionEffect positionEffect in new List<PositionEffect>(positionEffects)) { positionEffect.OnRoundStart(); }
        RemovePE_Execute();
    }
    public void OnTurnOrderDecide()
    {
        foreach (PositionEffect positionEffect in new List<PositionEffect>(positionEffects)) { positionEffect.OnTurnOrderDecide(); }
        RemovePE_Execute();
    }
    public void OnTurnStart(Character currentTurnChara,int turnCount)
    {
        foreach (PositionEffect positionEffect in new List<PositionEffect>(positionEffects)) { positionEffect.OnTurnStart(currentTurnChara, turnCount); }
        RemovePE_Execute();
    }
    public void OnTurnEnd(Character currentTurnChara, int turnCount,bool deadTurnChara)
    {
        foreach (PositionEffect positionEffect in new List<PositionEffect>(positionEffects)) { positionEffect.OnTurnEnd(currentTurnChara, turnCount, deadTurnChara); }
        RemovePE_Execute();
    }
    public void OnRoundEnd() {
        foreach (PositionEffect positionEffect in new List<PositionEffect>(positionEffects)) { positionEffect.OnRoundEnd(); }
        RemovePE_Execute();
    }
    public void OnBattleEnd()
    {
        infoText.AddDebugText($"PEs:{positionEffects.Count}");
        foreach (PositionEffect positionEffect in new List<PositionEffect>(positionEffects)) { positionEffect.OnBattleEnd(); }
        RemovePE_Execute();
    }
}
