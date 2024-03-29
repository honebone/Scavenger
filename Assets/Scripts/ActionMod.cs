using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionMod : MonoBehaviour
{
    
    [System.Serializable]
    public struct ActionModStatus
    {
        [TextArea(3, 10)]
        public string conditionInfo;

        public int decreaseHP;

        public bool cantCounter;

        public float ATKMod;
        public float exDMG_mul;
        public int exDMG_int;
        public float ACCMod;
        public float CRITCMod;
        public float CRITDMod;
        public bool sureHit;
        public bool unevadable;

        public int healValue;
        public float healPercent;

        public int SANHeal;
        public int SANDamage;
        public int shieldAdd;
        public int shieldRemove;

        public List<PA_StatusEffect.StatusEffectParams> applySteParams;

        //public float moveChance;
        //public int moveForword;
        //public int moveUpper;
        //public int moveLower;
        //public int moveBackword;

        public string GetModInfo()
        {
            string s = "";
            if (conditionInfo != "") { s+=string.Format("○{0}：\n",conditionInfo); }
            if (decreaseHP != 0) { s += ValueToStr("・HP減少量", decreaseHP, ""); }
            if (ATKMod != 0) { s += ValueToStr("・ATK補正", ATKMod, "％"); }
            if (exDMG_mul != 0) { s += ValueToStr("・与ダメージ", exDMG_mul, "％"); }
            if (exDMG_int != 0) { s += ValueToStr("・与ダメージ", exDMG_int, ""); }
            if (ACCMod != 0) { s += ValueToStr("・ACC補正", ACCMod, ""); }
            if (CRITCMod != 0) { s += ValueToStr("・CRIT率補正", CRITCMod, "％"); }
            if (CRITDMod != 0) { s += ValueToStr("・CRITダメージ補正", CRITDMod, "倍"); }
            if (sureHit) { s += "・攻撃が必中となる\n"; }
            if (unevadable) { s += "・攻撃が回避不可となる\n"; }
            if (healValue != 0) { s += ValueToStr("・回復量", healValue, ""); }
            if (healPercent != 0) { s += ValueToStr("・割合回復量", healPercent, "％"); }
            if (SANHeal != 0) { s += ValueToStr("・正気度回復量", SANHeal, ""); }
            if (SANDamage != 0) { s += ValueToStr("・正気度割合回復量", SANDamage, "％"); }
            if (shieldAdd != 0) { s += ValueToStr("・シールド付与量", shieldAdd, ""); }
            if (shieldRemove != 0) { s += ValueToStr("・シールド除去量", shieldRemove, ""); }

            foreach (PA_StatusEffect.StatusEffectParams StEParams in applySteParams)//StE付与
            {
                PA_StatusEffect.StatusEffectStatus status = StEParams.applyStE.GetComponent<PA_StatusEffect>().GetStatusEffectStatus();
                s += string.Format("・{0}％の確率で", StEParams.applyChance);
                //if (status.refValue) { s += string.Format("{0}{1}を{2}スタック付与\n", status.StEName.ColorStr(status.StEType.ToColor()), StEParams.value, StEParams.stack); }
                //else { s += string.Format("{0}を{1}スタック付与\n", status.StEName.ColorStr(status.StEType.ToColor()), StEParams.stack); }
                s += string.Format("{0}を{1}スタック付与\n", status.StEName.ColorStr(status.StEType.ToColor()), StEParams.stack);
                s += StEParams.applyStE.GetComponent<PA_StatusEffect>().GetStEInfo_forRef();
                s += "\n";
            }
            return s;
        }
        public string ValueToStr(string start, float value, string end)
        {
            if (value == 0) { return ""; }
            string s = start;
            if (value < 0) { s += value.ToString(); }
            else { s += "+" + value.ToString(); }
            s += end + "\n";
            return s;
        }
    }
    
    [SerializeField]
    protected ActionModStatus actionModStatus;
    
    public virtual Action.ActionStatus[] ModifyAction(Action.ActionStatus statusRef, Action.ActionStatus[] actionsStatus)
    {
        if (statusRef.actionTargets.Count != actionsStatus.Length) { FindObjectOfType<InfoText>().AddErrorText("対象の数と行動内容の数が一致しません"); }
       
        return actionsStatus;
    }
    public ActionModStatus GetActionModStatus() { return actionModStatus; }
}
