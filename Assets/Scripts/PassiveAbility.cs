using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassiveAbility : MonoBehaviour
{
    [SerializeField, TextArea(3, 10)] string PAInfo_start;
    [SerializeField, TextArea(3, 10)] string PAInfo_end;
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
        string s = "";
        if (PAInfo_start != "") { s+=PAInfo_start + "\n"; }
        s += GetPAInfo_Base() + "\n";
        if (PAInfo_end != "") { s += PAInfo_end + "\n"; }
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
    public void Disable(bool note=true)
    {
        AtTheEnd();
        character.RemovePA(this);
        if (PAType == 0)
        {
            PA_StatusEffect StE = GetComponent<PA_StatusEffect>();
            PA_StatusEffect.StatusEffectStatus StEStatus = StE.GetStatusEffectStatus();
            if (note)
            {
                character.GetTargetButton().SetDamageText(string.Format("-{0}", StEStatus.StEName), StEStatus.StEType.ToColor());
                infoText.AddLogText(string.Format("{0}‚Ì{1}‚ªÁ‹‚³‚ê‚½", character.CharaStatus().charaName, GetPAName()));
            }
            StE.DestroyIcon();
        }
        Destroy(gameObject);
    }

    /// <summary>©g‚ÌƒXƒvƒ‰ƒCƒg‚ğ‘ã“ü‚µ‚ÄEnqueue</summary>
    public void Enqueue(Action.ActionStatus actionStatus, bool setTargets, List<Character> actionTargets,int targetCount=0, bool nullOwner = false)
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
        character.Enqueue(action, setTargets, actionTargets, targetCount, nullOwner);
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

    public virtual Action.ActionStatus[] ModifyAction(Action.ActionStatus statusRef, Action.ActionStatus[] actionsStatus,bool forCalcDMG)
    {
        if (statusRef.actionTargets.Count != actionsStatus.Length) { FindObjectOfType<InfoText>().AddErrorText("‘ÎÛ‚Ì”‚Æs“®“à—e‚Ì”‚ªˆê’v‚µ‚Ü‚¹‚ñ"); }

        return actionsStatus;
    }

    public virtual Action.ActionStatus ModifyAction_Targeted(Action.ActionStatus statusRef , bool forCalcDMG)
    {
        return statusRef;
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


    public virtual void OnActivateAbility(List<Action.ActionResult> actionResultsList) { }
    /// <summary>UŒ‚A–½’†‚µ‚½‚©‚ÉŠÖ‚í‚ç‚¸—U”­</summary>
    public virtual void OnAttack(List<Action.OnAttackParams> onAttackParamsList) { }
    public virtual void OnDecreasedHP(int value) { }

    /// <summary>UŒ‚–½’†</summary>
    public virtual void OnDamage(List<Action.OnDamageParams> onDamageParamsList) {  }
    public virtual void OnCRIT(int ID) { }
    public virtual void OnKill(List<Action.OnKillParams> onKillParamsList) { }
    public virtual void OnMiss(int ID) { }
    public virtual void OnHeal(List<Action.OnHealParams> onHealParamsList) { }
    public virtual void OnApplyStE(List<Action.OnApplyStEParams> onApplyStEParamsList) { }
    public virtual void OnApplyedStE(Action.OnApplyStEParams onApplyStEParams) { }
    //public virtual void OnRemoveStE() { }

    public virtual void BecomeAbilityTarget(Character actor) { }
    public virtual void OnAttacked(Character attacker, bool evaded, bool missed) { }

    /// <summary>DMG=0‚Ì‚à</summary>
    public virtual void OnDamaged(Action.OnDamageParams onDamageParams) { }
    
    public virtual void OnCRITed(int ID) { }
    public virtual void OnMoved(Action.OnMoveParams onMoveParams) { }

    /// <summary>killer:ƒLƒƒƒ‰‚ÌUŒ‚‚âEŠQŒø‰Ê‚É‚æ‚é‘ã“ü</summary>
    public virtual void OnDie(Character killer) { }
    public virtual void OnEvade(int ID) { }
    public virtual void OnHealed(Character healer, Action.OnHealParams onHealParams) { }

    public virtual void OnSomeoneMove(Action.OnMoveParams onMoveParams) { }
    public virtual void OnSomeoneDied(Character died) { }
    public virtual void OnSomeoneApplyedStE(List<Action.OnApplyStEParams> onApplyStEParamsList) { }

}
