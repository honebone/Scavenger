using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PA_StatusEffect : PassiveAbility
{
    [System.Serializable]
    public struct StatusEffectStatus
    {
        public string StEName;
        [TextArea(3,10)]
        public string StEInfo;
        public Sprite StEIcon;
        public enum StatusEffectType { neutral, buff, debuff, focus,unique }
        public StatusEffectType StEType;
        [Header("0ならスタック制限なし")]
        public int maxStack;
        [Tooltip("同種の状態異常がすでにある場合、そのスタックを増加させるか")]
        public bool merge;
        public bool refValue;

        [Header("以下は代入される")]
        public int stack;
        public int value;

        public string GetName()
        {
            string s = StEName;
            //if (refValue) { s += value.ToString(); }
            return s.ColorStr(StEType.ToColor());
        }
    }
    public StatusEffectStatus GetStatusEffectStatus() { return StEStatus; }

    [System.Serializable]
    public struct StatusEffectParams
    {
        public GameObject applyStE;
        public float applyChance;

        public int stack;
        public int value;
    }

    [SerializeField]
    Action.ActionStatus remove;

    public void Init(StatusEffectParams StEParams,StEIcon icon,StEApplyBonus applyBonus)
    {
        StEStatus.stack = StEParams.stack;
        StEStatus.value = StEParams.value;
        if (applyBonus != null)
        {
            StEStatus.stack += applyBonus.exStack;
            StEStatus.value += applyBonus.exValue;
        }
        StEIcon = icon;
        StEIcon.Init(StEStatus);
        if (StEStatus.merge && StEStatus.refValue) { FindObjectOfType<InfoText>().AddErrorText("mergeとrefValueが同時にtrueとなるStEは作ってはいけません!!"); }
    }


    public override string GetPAName()
    {
        return StEStatus.GetName();
    }
    public override string GetPAInfo()
    {
        string s = string.Format("({0}スタック)\n", StEStatus.stack);
        s += GetStEInfo_forRef();
        return s;
    }
    public string GetStEInfo_forRef()
    {
        string s = StEStatus.StEName;
        if (StEStatus.refValue) { s += "X"; }
        s += "：";
        s += StEStatus.StEInfo + "\n";
        s += GetAdditionalInfo();
        return s.ColorStr(Color.gray);
    }
  
    public virtual string GetAdditionalInfo()
    {
        return "";
    }
    //public void Enqueue_AddStack(int add)
    //{
    //    ActionData.RemoveStE removeStE= new ActionData.RemoveStE();
    //    removeStE.removeStE = gameObject;
    //    removeStE.addAmount = add;

    //    Action.ActionStatus action = remove;
    //    if (add > 0) { action.actionName = "スタック増加"; }
    //    else { action.actionName = "スタック減少"; }
    //    action.targetInfo = "自身";
    //    action.removeStE_asStE = true;
    //    action.removeStE_bySelf = removeStE;

    //    character.Enqueue(action, true, new List<Character>() { character });
    //}
    //public void Enqueue_Disable()
    //{
    //    ActionData.RemoveStE removeStE = new ActionData.RemoveStE();
    //    removeStE.removeStE = gameObject;
    //    removeStE.removeAll = true;

    //    Action.ActionStatus action = remove;
    //    action.actionName = "消去";
    //    action.targetInfo = "自身";
    //    action.removeStE_asStE = true;
    //    action.removeStE_bySelf = removeStE;

    //    character.Enqueue(action, true, new List<Character>() { character });
    //}
    public void AddStack(int stack)
    {
        int prevStack = StEStatus.stack;
        if (StEStatus.maxStack == 0)//最大スタック数が無制限なら
        {
            StEStatus.stack += stack;
        }
        else { StEStatus.stack = Mathf.Clamp(StEStatus.stack + stack, 0, StEStatus.maxStack); }

        character.GetCharacter_Object().SetDamageText(string.Format("{0}{1}", StEStatus.StEName, (StEStatus.stack - prevStack).GetValueWithSign()), Definer.colorRef.failed_unavailable);
        infoText.AddLogText(string.Format("{0}の{1}のスタック{2}", character.GetCharacterStatus().charaName, GetPAName(), (StEStatus.stack - prevStack).GetValueWithSign()));

        OnAddStack(StEStatus.stack - prevStack);//スタック増減時誘発;

        if (StEStatus.stack <= 0)
        {
            StEStatus.stack = 0;
            Disable();
        }
        else { StEIcon.SetStackText(StEStatus.stack); }
    }

    public virtual void OnAddStack(int add) { }

    public void DestroyIcon()
    {
        Destroy(StEIcon.gameObject);
    }

    [SerializeField]
    protected StatusEffectStatus StEStatus;
    StEIcon StEIcon;
}
