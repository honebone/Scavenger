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
        public bool hideValues;
        [TextArea(3, 10)]
        public string exInfo;

        public bool consumeFocus;
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
        public float healRegain;

        public int SANHeal;
        public int SANDamage;
        public int shieldAdd;
        public int shieldRemove;

        public List<PA_StatusEffect.StatusEffectParams> applySteParams;
        public List<StEApplyBonus> applyStEBonus;

        [Header("\n\nバフの除去")]
        public int removeStE_buff;
        [Header("\n\nデバフの除去")]
        public int removeStE_debuff;
        [Header("\n\nDoTの除去")]
        public int removeStE_DoT;

        public List<ActionData.RemoveStE> removeStEs;

        //public float moveChance;
        //public int moveForword;
        //public int moveUpper;
        //public int moveLower;
        //public int moveBackword;

        public string GetModInfo()
        {
            string s = "";
            if (conditionInfo != "") { s += string.Format("○{0}\n", conditionInfo); }
            if (!hideValues)
            {
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
                if (healRegain != 0) { s += ValueToStr("・減少したHPの", healRegain, "％を回復"); }
                if (SANHeal != 0) { s += ValueToStr("・正気度回復量", SANHeal, ""); }
                if (SANDamage != 0) { s += ValueToStr("・正気度割合回復量", SANDamage, "％"); }
                if (shieldAdd != 0) { s += ValueToStr("・シールド付与量", shieldAdd, ""); }
                if (shieldRemove != 0) { s += ValueToStr("・シールド除去量", shieldRemove, ""); }

                foreach (PA_StatusEffect.StatusEffectParams StEParams in applySteParams)//StE付与
                {
                    PA_StatusEffect.StatusEffectStatus status = StEParams.applyStE.GetComponent<PA_StatusEffect>().GetStatusEffectStatus();
                    if (StEParams.guaranteed) { s += "・"; }
                    else { s += string.Format("・{0}％の確率で", StEParams.applyChance); }
                    if (status.refValue) { s += string.Format("{0}を{1}スタック付与\n", (status.StEName + StEParams.value.ToString()).ColorStr(status.StEType.ToColor()), StEParams.stack); }
                    else { s += string.Format("{0}を{1}スタック付与\n", status.StEName.ColorStr(status.StEType.ToColor()), StEParams.stack); }
                    s += StEParams.applyStE.GetComponent<PA_StatusEffect>().GetStEInfo_forRef();
                }
                foreach (StEApplyBonus bonus in applyStEBonus)
                {
                    string StEName = bonus.applyStE.GetComponent<PA_StatusEffect>().GetStatusEffectStatus().StEName;
                    if (bonus.exChance != 0) { s += ValueToStr(string.Format("{0}付与確率", StEName), bonus.exChance, "％"); }
                    if (bonus.exStack != 0) { s += ValueToStr(string.Format("{0}付与スタック数", StEName), bonus.exStack, ""); }
                    if (bonus.exValue != 0) { s += ValueToStr(string.Format("付与する{0}の値", StEName), bonus.exValue, ""); }
                }

                if (removeStE_buff > 0) { s += string.Format("・{0}を{1}個消去\n", "バフ効果".ColorStr(Definer.colorRef.statusEffectColors[(int)PA_StatusEffect.StatusEffectStatus.StatusEffectType.buff]), removeStE_buff); }
                if (removeStE_debuff > 0) { s += string.Format("・{0}を{1}個消去\n", "デバフ効果".ColorStr(Definer.colorRef.statusEffectColors[(int)PA_StatusEffect.StatusEffectStatus.StatusEffectType.debuff]), removeStE_debuff); }
                if (removeStE_DoT > 0) { s += string.Format("・{0}を{1}個消去\n", "ダメージ効果".ColorStr(Definer.colorRef.statusEffectColors[(int)PA_StatusEffect.StatusEffectStatus.StatusEffectType.DoT]), removeStE_DoT); }

                foreach (ActionData.RemoveStE remove in removeStEs)
                {
                    PA_StatusEffect.StatusEffectStatus status = remove.removeStE.GetComponent<PA_StatusEffect>().GetStatusEffectStatus();
                    s += string.Format("・{0}", status.StEName.ColorStr(status.StEType.ToColor()));
                    if (remove.removeAll) { s += "を全て除去\n"; }
                    else { s += ValueToStr("のスタック", remove.addAmount, ""); }
                }
            }

            if (exInfo != "") { s += exInfo+"\n"; }

            return s.ColorStr(Definer.colorRef.AMod);
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
    protected CharactersManager charactersManager;

    public void Init(CharactersManager cm)
    {
        charactersManager = cm;
    }
    public virtual Action.ActionStatus[] ModifyAction(Action.ActionStatus statusRef, Action.ActionStatus[] actionsStatus)
    {
        if (statusRef.actionTargets.Count != actionsStatus.Length) { FindObjectOfType<InfoText>().AddErrorText("対象の数と行動内容の数が一致しません"); }
       
        return actionsStatus;
    }
    public ActionModStatus GetActionModStatus() { return actionModStatus; }
}
