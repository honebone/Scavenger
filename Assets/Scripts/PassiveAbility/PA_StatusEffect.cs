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
        public enum StatusEffectType { neutral, buff, debuff, focus, unique }
        public StatusEffectType StEType;
        [Header("0ならスタック制限なし")]
        public int maxStack;
        [Tooltip("同種の状態異常がすでにある場合、そのスタックを増加させるか")]
        public bool merge;
        public bool refValue;

        public bool undeletable;
        public bool DoT;
        public bool regen;

        [Header("以下は代入される")]
        public Character applyer;
        public int stack;
        public int value;

        public int DMGPerTurn;

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
        public bool guaranteed;
        public float applyChance;

        public int stack;
        public int value;

        [Header("DoT")]
        public bool refATK;
        [Header("以下は代入される")]
        public int DMGPerTurn;
        public string GetInfo()
        {
            string s = "";
            StatusEffectStatus status = applyStE.GetComponent<PA_StatusEffect>().GetStatusEffectStatus();
            string chanceText = guaranteed ? "確定" : $"{applyChance}％";
            s += $"・{status.ToLinkKey(false, value)}を付与\n({chanceText},{stack}スタック)\n";

            return s;
        }
        public StatusEffectStatus GetStatusEffectStatus()
        {
            return applyStE.GetComponent<PA_StatusEffect>().GetStatusEffectStatus();
        }
    }

    public void Init_StE(int stack, int value,int DMGPerTurn,StEIcon icon,Character applyer)
    {
        StEStatus.stack = stack;
        StEStatus.value = value;
        StEStatus.DMGPerTurn = DMGPerTurn;
        if(applyer!= null) { StEStatus.applyer = applyer; }
       
        StEIcon = icon;
        StEIcon.Init(StEStatus);
        if (StEStatus.merge && StEStatus.refValue) { FindObjectOfType<InfoText>().AddErrorText("mergeとrefValueが同時にtrueとなるStEは作ってはいけません!!"); }
    }


    public override string GetPAName()
    {
        return StEStatus.GetName();
    }
    public string GetPANameWithValue()
    {
        string s = GetPAName();
        s += (" <i>{" + StEStatus.value.ToString() + "}</i>").ColorStr(Definer.colorRef.emphasize);
        return s;
    }

    public override string GetSimpleInfo()
    {
        string s = "";
        if (StEStatus.maxStack == 0) { s += string.Format("{0}スタック", StEStatus.stack); }
        else { s += string.Format("{0}スタック(最大{1})", StEStatus.stack, StEStatus.maxStack); }
        s += string.Format("[{0}]\n", Definer.StETypeName[StEStatus.StEType].ColorStr(Definer.colorRef.statusEffectColors[(int)StEStatus.StEType]));
        s += $"{simpleInfo}\n";
        if (StEStatus.DoT) { s += $"\n減少HP：{StEStatus.DMGPerTurn}/ターン\n".ColorStr(Definer.colorRef.decreaseHP); }
        if (StEStatus.regen) { s += $"\n回復量：{StEStatus.DMGPerTurn}/ターン\n".ColorStr(Definer.colorRef.heal); }
        return s;
    }

    public override string GetPAInfo_Base()
    {
        string s ="";
        if (StEStatus.maxStack == 0) { s += string.Format("{0}スタック", StEStatus.stack); }
        else { s += string.Format("{0}スタック(最大{1})", StEStatus.stack, StEStatus.maxStack); }
        s += string.Format("[{0}]\n", Definer.StETypeName[StEStatus.StEType].ColorStr(Definer.colorRef.statusEffectColors[(int)StEStatus.StEType]));
        s += GetStEInfo_forRef();
        if (StEStatus.DoT) { s += $"\n減少HP：{StEStatus.DMGPerTurn}/ターン\n".ColorStr(Definer.colorRef.decreaseHP); }
        if (StEStatus.regen) { s += $"\n回復量：{StEStatus.DMGPerTurn}/ターン\n".ColorStr(Definer.colorRef.heal); }
        return s;
    }
    public string GetStEInfo_forRef()
    {
        string s = "";
        //string s = StEStatus.StEName;
        //if (StEStatus.refValue) { s += "X"; }
        //s += "：";
        //string statModInfo = statMod.GetInfo();
        //if (statModInfo != "") s += statModInfo + "\n";
        if (StEStatus.StEInfo != "") s += StEStatus.StEInfo + "\n";
        if (GetAdditionalInfo() != "") { s += "\n" + GetAdditionalInfo(); }
        if (StEStatus.undeletable) { s += "\n消去不可"; }

        return s;
    }

    public string GetInfo_ForLink()
    {
        string rv = StEStatus.refValue ? " <color=#FFBF69><i>{X}</i></color>" : "";
        string s = $"<{StEStatus.StEType.ToSpr()}{GetPAName()}{rv}>\n";
        string statModInfo = statMod.GetInfo();
        if (statModInfo != "") s += statModInfo + "\n";
        s += StEStatus.StEInfo;
        //s = s.ColorStr(Color.gray);
        if (GetAdditionalInfo() != "") { s += "\n\n" + GetAdditionalInfo(); }
        if (StEStatus.undeletable) { s += "\n消去不可"; }

        return s;
    }
  
    public virtual string GetAdditionalInfo()
    {
        return "";
    }

    public void AddStack(int stack, bool note = true)
    {
        int prevStack = StEStatus.stack;
        if (StEStatus.maxStack == 0)//最大スタック数が無制限なら
        {
            StEStatus.stack = Mathf.Max(0, StEStatus.stack + stack);
        }
        else { StEStatus.stack = Mathf.Clamp(StEStatus.stack + stack, 0, StEStatus.maxStack); }

        if (note && StEStatus.stack > 0)
        {
            character.GetTargetButton().SetDamageText(string.Format("{0}{1}", StEStatus.StEName, (StEStatus.stack - prevStack).GetValueWithSign()), Definer.colorRef.failed_unavailable);
            infoText.AddLogText(string.Format("{0}の{1}のスタック{2}", character.CharaStatus().charaName, GetPAName(), (StEStatus.stack - prevStack).GetValueWithSign()));
        }

        OnAddStack(StEStatus.stack - prevStack);//スタック増減時誘発;

        if (StEStatus.stack <= 0)
        {
            StEStatus.stack = 0;
            Disable(note);
        }
        else
        {
            if (StEIcon) { StEIcon.SetStackText(StEStatus.stack); }
        }
    }

    /// <summary>addは負の数や最大スタック数を超えたりしない </summary>
    public virtual void OnAddStack(int add) { }

    public void DestroyIcon()
    {
        if (StEIcon != null) { Destroy(StEIcon.gameObject); }
    }

    public override void OnBattleEnd()
    {
        Disable();
    }

    [SerializeField]
    protected StatusEffectStatus StEStatus;
    StEIcon StEIcon;
}
