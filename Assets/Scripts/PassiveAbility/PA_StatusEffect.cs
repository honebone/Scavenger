using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PA_StatusEffect : PassiveAbility
{
    [System.Serializable]
    public struct StatusEffectStatus
    {
        public string StEName;
        public enum StatusEffectType { neutral, buff, debuff, focus }
        public StatusEffectType StEType;
        [Tooltip("同種の状態異常がすでにある場合、そのスタックを増加させるか")]
        public bool addStackToSameStE;

        [Header("以下は代入される")]
        public int stack;
        public float value;

        public string GetName()
        {
           
            return StEName.ColorStr(StEType.ToColor());
        }
    }
    public override string GetPAName()
    {
        return statusEffectStatus.GetName();
    }
    [SerializeField]
    protected StatusEffectStatus statusEffectStatus;
}
