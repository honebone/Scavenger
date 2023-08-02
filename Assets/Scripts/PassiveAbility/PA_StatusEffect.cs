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
        public enum StatusEffectType { neutral, buff, debuff, focus }
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
            if (refValue) { s += " " + value.ToString(); }
            return s.ColorStr(StEType.ToColor());
        }
        public string GetStEInfo_forRef()
        {
            string s = StEName;
            if (refValue) { s += " X"; }
            s += "：";
            s += StEInfo;
            return s.ColorStr(Color.gray);
        }
    }
    public StatusEffectStatus GetStatusEffectStatus() { return StEStatus; }

    public enum StEID { other, 出血, 火傷, 毒 }
    [System.Serializable]
    public struct StatusEffectParams
    {
        public StEID applyStE;
        public float applyChance;

        public int stack;
        public int value;
    }
    public void Init(StatusEffectParams StEParams)
    {
        StEStatus.stack = StEParams.stack;
        StEStatus.value = StEParams.value;
    }


    public override string GetPAName()
    {
        return StEStatus.GetName();
    }
    public override string GetPAInfo()
    {
        string s = string.Format("({0}スタック)\n", StEStatus.stack);
        s += StEStatus.StEInfo;
        return s;
    }
    public void AddStack(int stack)
    {
        StEStatus.stack += stack;
        if (StEStatus.stack <= 0)
        {
            StEStatus.stack = 0;
            Disable();
        }
    }

    [SerializeField]
    protected StatusEffectStatus StEStatus;
}
