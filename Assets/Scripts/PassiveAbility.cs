using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassiveAbility : MonoBehaviour
{
    protected Character character;
    protected CharactersManager charactersManager;
    protected InfoText infoText;

    protected bool instantiated;
    /// <summary>0:StE 1:Personality 2:Equipment</summary>
    int PAType;
    /// <summary>0:StE 1:Personality 2:Equipment</summary>
    public int GetPAType() { return PAType; }
    public virtual string GetPAName() { return ""; }
    public string GetPAInfo()
    {
        string s = GetPAInfo_Base();
        if (instantiated) { s += GetCurrentStateInfo().ColorStr(Definer.colorRef.currentState); }
        return s;
    }
    public virtual string GetPAInfo_Base()
    {
        print("error:GetPAInfo‚Ìoverride‚ªİ’è‚³‚ê‚Ä‚¢‚Ü‚¹‚ñ");
        return "error:GetPAInfo‚Ìoverride‚ªİ’è‚³‚ê‚Ä‚¢‚Ü‚¹‚ñ";
    }
    public virtual string GetCurrentStateInfo()
    {
        return "";
    }

    public void Init(Character c,int type,InfoText it)
    {
        instantiated = true;
        character = c;
        PAType = type;
        infoText = it;
        charactersManager=FindObjectOfType<CharactersManager>();
        OnPAInit();
    }
    public void Disable()
    {
        AtTheEnd();
        character.RemovePA(this);
        if (PAType == 0)
        {
            PA_StatusEffect StE = GetComponent<PA_StatusEffect>();
            PA_StatusEffect.StatusEffectStatus StEStatus = StE.GetStatusEffectStatus();
            character.GetCharacter_Object().SetDamageText(string.Format("-{0}", StEStatus.StEName), StEStatus.StEType.ToColor());
            infoText.AddLogText(string.Format("{0}‚Ì{1}‚ªÁ‹‚³‚ê‚½", character.GetCharacterStatus().charaName, GetPAName()));
            StE.DestroyIcon();
        }
        Destroy(gameObject);
    }
    public virtual void OnPAInit() { }
    public virtual void AtTheEnd() { }

    public virtual void OnBattleStart() { }
    public virtual void OnRoundStart() { }
    public virtual void OnTurnOrderDecide() { }

    public virtual void OnTurnStart(bool myTurn, int turnCount) { }
    public virtual void OnTurnEnd() { }
    public virtual void OnRoundEnd() { }
    public virtual void OnBattleEnd() { }


    public virtual void OnActivateAbility() { }
    /// <summary>UŒ‚A–½’†‚µ‚½‚©‚ÉŠÖ‚í‚ç‚¸—U”­</summary>
    public virtual void OnAttack(List<Action.OnAttackParams> onAttackParamsList) { }
    /// <summary>UŒ‚–½’†</summary>
    public virtual void OnDamage(int DMG, Character target,Action.ActionStatus actionStatus) {  }
    public virtual void OnCRIT(int ID) { }
    public virtual void OnKill(int ID) { }
    public virtual void OnMiss(int ID) { }
    public virtual void OnHeal(List<Action.OnHealParams> onHealParamsList) { }
    //public virtual void OnApplyStE() { }
    //public virtual void OnRemoveStE() { }

    public virtual void BecomeAbilityTarget(Character actor) { }
    public virtual void OnAttacked(Character attacker, bool evaded, bool missed) { }

    public virtual void OnDamaged(int DMG, Character attacker) { }
    
    public virtual void OnCRITed(int ID) { }
    /// <summary>killer:ƒLƒƒƒ‰‚ÌUŒ‚‚âEŠQŒø‰Ê‚É‚æ‚é‘ã“ü</summary>
    public virtual void OnDie(Character killer) { }
    public virtual void OnEvade(int ID) { }
    public virtual void OnHealed(int healedValue, int ID) { }
}
