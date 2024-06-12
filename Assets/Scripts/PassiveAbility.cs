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

    /// <summary>©g‚ÌƒXƒvƒ‰ƒCƒg‚ğ‘ã“ü‚µ‚ÄEnqueue</summary>
    public void Enqueue(Action.ActionStatus actionStatus, bool setTargets, List<Character> actionTargets)
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            actionStatus.sprite = sr.sprite;
        }
        else
        {
            infoText.AddDebugText(string.Format("{0}‚ÉSpriteRenderer‚È‚µ", GetPAName()));
        }
        character.Enqueue(actionStatus, setTargets, actionTargets);
    }

    /// <summary>©g‚ğ‘ÎÛ‚ÉEunqueue</summary>
    public void Enqueue_Self(Action.ActionStatus actionStatus)
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            actionStatus.sprite = sr.sprite;
        }
        else
        {
            infoText.AddDebugText(string.Format("{0}‚ÉSpriteRenderer‚È‚µ", GetPAName()));
        }

        Action.ActionStatus action = actionStatus;
        character.Enqueue(action, true, new List<Character>() { character });
    }
    public virtual void OnPAInit() { }
    public virtual void AtTheEnd() { }

    public virtual void OnBattleStart() { }
    public virtual void OnRoundStart() { }
    public virtual void OnTurnOrderDecide() { }

    public virtual void OnTurnStart(bool myTurn, int turnCount) { }
    public virtual void OnTurnEnd() { }
    public virtual void OnRoundEnd() { }

    /// <summary> ‚±‚±‚ÅEnqueue‚µ‚È‚¢!! </summary>
    public virtual void OnBattleEnd() { }


    public virtual void OnActivateAbility() { }
    /// <summary>UŒ‚A–½’†‚µ‚½‚©‚ÉŠÖ‚í‚ç‚¸—U”­</summary>
    public virtual void OnAttack(List<Action.OnAttackParams> onAttackParamsList) { }
    public virtual void OnDecreasedHP(int value) { }

    /// <summary>UŒ‚–½’†</summary>
    public virtual void OnDamage(int DMG, Character target,Action.ActionStatus actionStatus) {  }
    public virtual void OnCRIT(int ID) { }
    public virtual void OnKill(int ID) { }
    public virtual void OnMiss(int ID) { }
    public virtual void OnHeal(List<Action.OnHealParams> onHealParamsList) { }
    public virtual void OnApplyedStE(List<Action.OnApplyStEParams> onApplyStEParamsList) { }
    //public virtual void OnRemoveStE() { }

    public virtual void BecomeAbilityTarget(Character actor) { }
    public virtual void OnAttacked(Character attacker, bool evaded, bool missed) { }

    /// <summary>DMG>0‚Ì‚Ì‚İ</summary>
    public virtual void OnDamaged(int DMG, Character attacker) { }
    
    public virtual void OnCRITed(int ID) { }
    public virtual void OnMoved(Action.OnMoveParams onMoveParams) { }

    /// <summary>killer:ƒLƒƒƒ‰‚ÌUŒ‚‚âEŠQŒø‰Ê‚É‚æ‚é‘ã“ü</summary>
    public virtual void OnDie(Character killer) { }
    public virtual void OnEvade(int ID) { }
    public virtual void OnHealed(int healedValue, int ID) { }
}
