using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassiveAbility : MonoBehaviour
{
    public string fileName;
    public bool noSimpleInfo;
    [TextArea(3, 10)] public string simpleInfo;
    [TextArea(3, 10)] public string PAInfo_start;
    public bool skipGetInfo;
    [TextArea(3, 10)] public string PAInfo_end;

    public Character.CharaStatusMod statMod;
    public List<GameObject> AMods;

    public enum PATag { special, 武器, 防具, 装飾品, 魔術, ルーン }
    public List<PATag> PATags = new List<PATag>();

    protected Character character;
    protected CharactersManager charactersManager;
    protected InfoText infoText;

    protected bool instantiated;
    protected bool applyFlag;

    public string FileName() { return fileName; }
    /// <summary>0:StE 1:Personality 2:Equipment</summary>
    int PAType;
    /// <summary>0:StE 1:Personality 2:Equipment</summary>
    public int GetPAType() { return PAType; }
    public virtual string GetPAName() { return ""; }
    public string GetPAInfo(bool simple = false)
    {
        string s = "";
        if (PATags.Count > 0)
        {
            bool f = false;
            s += "タグ：";
            foreach (PATag tag in PATags)
            {
                if (f) { s += ", "; }
                f = true;
                if (tag.ToString() == "魔術") s += "<link=U_魔術><u>[魔術]</u></link>";
                else s += $"[{tag}]";
            }
            s += "\n";
        }

        string statModInfo = statMod.GetInfo();

        if (simple && !noSimpleInfo)
        {
            if (statModInfo != "") s += $"{statModInfo}\n\n{GetSimpleInfo()}";
            else s += GetSimpleInfo();
        }
        else
        {
            if (statModInfo != "") s += statModInfo + "\n\n";
            string amodInfo = "";
            foreach (GameObject actionMod in AMods)
            {
                amodInfo = actionMod.GetComponent<ActionMod>().GetActionModStatus().GetModInfo();
                if(amodInfo != "") s += actionMod.GetComponent<ActionMod>().GetActionModStatus().GetModInfo()+"\n";

            }
            //if (amodInfo != "") s += amodInfo + "\n";
            if (PAInfo_start != "") { s += PAInfo_start + "\n\n"; }
            if (!skipGetInfo && GetPAInfo_Base() != "") s += GetPAInfo_Base() + "\n\n";
            if (PAInfo_end != "") { s += PAInfo_end + "\n"; }
            if (instantiated) { s += GetCurrentStateInfo().ColorStr(Definer.colorRef.currentState); }
        }
        return s;
    }

    /// <summary>個別のスクリプトではoverrideしない</summary>
    public virtual string GetSimpleInfo()
    {
        return simpleInfo;
    }

    public virtual string GetPAInfo_Base()
    {
        print("error:GetPAInfoのoverrideが設定されていません");
        return "error:GetPAInfoのoverrideが設定されていません";
    }
    public virtual string GetCurrentStateInfo()
    {
        return "";
    }

    public void Init(Character c, int type, InfoText it)
    {
        instantiated = true;
        character = c;
        PAType = type;
        infoText = it;
        charactersManager = CharactersManager.inst;
        if (fileName == "") { infoText.AddWarningText($"{GetPAName()}のfileNameがありません"); }

        character.ModifyStatus(statMod, true);
        foreach (GameObject mod in AMods) { character.AddActionMod(mod, true); }
        OnPAInit();
    }
    public void Disable(bool note=true)
    {
        AtTheEnd();
        character.ModifyStatus(statMod, false);
        foreach (GameObject mod in AMods) { character.AddActionMod(mod, false); }

        character.RemovePA(this);
        if (PAType == 0)
        {
            PA_StatusEffect StE = GetComponent<PA_StatusEffect>();
            PA_StatusEffect.StatusEffectStatus StEStatus = StE.GetStatusEffectStatus();
            if (note)
            {
                character.GetTargetButton().SetDamageText(string.Format("消去：{0}", StEStatus.StEName), Color.gray);//StEStatus.StEType.ToColor()
                infoText.AddLogText(string.Format("{0}の{1}が消去された", character.CharaStatus().charaName, GetPAName()));
            }
            StE.DestroyIcon();
        }
        Destroy(gameObject);
    }
    public void Log(string str)
    {
        infoText.AddLogText($"<{character.CharaStatus().charaName}の{GetPAName()}>：{str}");
        character.SetDamageText($"{GetPAName()}：{str}", Definer.colorRef.currentState);
    }

    /// <summary>指定した条件に合致する対象を探してEnqueue 実際にEnqueueしたかを返す</summary>

    public bool Enqueue_SearchTarget(Action.ActionStatus actionStatus, CharactersManager.SearchCharaCondition condition, int targetCount = 0)
    {
        Action.ActionStatus action = actionStatus;
        List<Character> target=new List<Character>();
        List<int> targetPos=new List<int>();
        if (condition.searchAsPos)
        {
            targetPos = charactersManager.SearchPosWithCondition(condition);
            action.actionTargetsInt = targetPos;
        }
        else
        {
            target = charactersManager.SearchCharaWithCondition(condition);
            action.actionTargets = target;
        }

        if (target.Count > 0|| targetPos.Count>0) 
        {
            Enqueue(action, false, target, targetCount);
            return true;
        }
        return false;
    }


    /// <summary>自身のスプライトを代入してEnqueue</summary>
    public bool Enqueue(Action.ActionStatus actionStatus, bool setTargets, List<Character> actionTargets,int targetCount=0, bool nullOwner = false)
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            actionStatus.sprite = sr.sprite;
        }
        else
        {
            infoText.AddDebugText(string.Format("{0}にSpriteRendererなし", GetPAName()));
        }
        Action.ActionStatus action = actionStatus;
        action.source = this;
       return character.Enqueue(action, setTargets, actionTargets, targetCount, nullOwner);
    }

    /// <summary>自身のスプライトを代入してEnqueue</summary>
    public bool Enqueue_Int(Action.ActionStatus actionStatus, bool setTargets, List<int> actionTargetsInt, int targetCount = 0, bool nullOwner = false)
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            actionStatus.sprite = sr.sprite;
        }
        else
        {
            infoText.AddDebugText(string.Format("{0}にSpriteRendererなし", GetPAName()));
        }
        Action.ActionStatus action = actionStatus;
        action.source = this;
        return character.Enqueue_Int(action, setTargets, actionTargetsInt, targetCount, nullOwner);
    }

    /// <summary>自身を対象にEunqueue</summary>
    public void Enqueue_Self(Action.ActionStatus actionStatus)
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            actionStatus.sprite = sr.sprite;
        }
        else
        {
            infoText.AddDebugText(string.Format("{0}にSpriteRendererなし", GetPAName()));
        }

        Action.ActionStatus action = actionStatus;
        action.source = this;
        character.Enqueue(action, true, new List<Character>() { character });
    }

    public virtual Action.ActionStatus[] ModifyAction(Action.ActionStatus statusRef, Action.ActionStatus[] actionsStatus, bool forCalcDMG)
    {
        if (statusRef.actionTargets.Count == actionsStatus.Length) { }
        else if (statusRef.actionTargetsInt == null || statusRef.actionTargetsInt.Count != actionsStatus.Length) { InfoText.inst.AddErrorText("アクション対象エラー"); }

        return actionsStatus;
    }

    public virtual Action.ActionStatus ModifyAction_Targeted(Action.ActionStatus statusRef, bool forCalcDMG)
    {
        return statusRef;
    }

    public virtual void Cast() { infoText.AddErrorText($"効果のない詠唱をしています！：{GetPAName()}"); }

    /// <summary>このターンに付与されたかのチェック</summary>
    public void StE_ApplyFlag() { applyFlag = true; }


    public virtual void OnPAInit() { }
    public virtual void AtTheEnd() { }

    public virtual void OnBattleStart() { }
    public virtual void OnRoundStart() { }
    public virtual void OnTurnOrderDecide() { }

    public virtual void OnTurnStart(bool myTurn, int turnCount) { }
    public virtual void OnTurnEnd(bool myTurn, int turnCount, bool deadTurnChara) { }
    public virtual void OnRoundEnd() { }

    /// <summary> ここでEnqueueしない!! </summary>
    public virtual void OnBattleEnd() { }


    public virtual void OnActivateAbility(List<Action.ActionResult> actionResultsList) { }
    /// <summary>攻撃時、命中したかに関わらず誘発</summary>
    public virtual void OnAttack(List<Action.OnAttackParams> onAttackParamsList) { }
    public virtual void OnDecreasedHP(int value) { }
    public virtual void OnDecreasedShield(int value) { }

    /// <summary>攻撃命中時 絶対要素数は1以上 0ダメでも呼ばれる</summary>
    public virtual void OnDamage(List<Action.OnDamageParams> onDamageParamsList) {  }
    public virtual void OnFocus(List<Action.OnFocusParams> focusParamsList) { }
    public virtual void OnKill(List<Action.OnKillParams> onKillParamsList) { }
    public virtual void OnMiss(int ID) { }
    public virtual void OnHeal(List<Action.OnHealParams> onHealParamsList) { }
    public virtual void OnApplyStE(List<Action.OnApplyStEParams> onApplyStEParamsList) { }
    public virtual void OnApplyedStE(Action.OnApplyStEParams onApplyStEParams) { }
    //public virtual void OnRemoveStE() { }

    public virtual void BecomeAbilityTarget(Character actor) { }
    public virtual void OnAttacked(Character attacker, bool evaded, bool missed) { }

    /// <summary>DMG=0の時も</summary>
    public virtual void OnDamaged(Action.OnDamageParams onDamageParams) { }
    
    public virtual void OnMoved(Action.OnMoveParams onMoveParams) { }

    /// <summary>killer:キャラの攻撃や殺害効果による時代入</summary>
    public virtual void OnDie(Character killer) { }
    public virtual void OnHealed(Character healer, Action.OnHealParams onHealParams) { }

    public virtual void OnCast(Eq_Magic cast) { }

    public virtual void OnSummon(List<Action.OnSummonParams> onSummonParamsList) { }

    public virtual void OnSummoned(Action.OnSummonParams onSummonParams) {  }

    public virtual void OnSomeoneDamaged(Action.OnDamageParams onDamageParams) { }

    public virtual void OnSomeoneMove(Action.OnMoveParams onMoveParams) { }
    public virtual void OnSomeoneFocus(List<Action.OnFocusParams> focusParamsList) { }
    public virtual void OnSomeoneSummoned(Character summoner,List<Action.OnSummonParams> onSummonParamsList) { }

    public virtual void OnSomeoneDied(Character died) { }
    public virtual void OnSomeoneApplyedStE(List<Action.OnApplyStEParams> onApplyStEParamsList) { }

}
