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

        public float ATKMod;
        public float INTMod;

        public int trueATKDMG;
        public int trueINTDMG;

        public float exDMG_mul;
        public int exATKDMG_int;
        public int exINTDMG_int;
        public float ACCMod;
        public float CRITCMod;
        public float CRITDMod;
        public float drain;
        public bool sureHit;
        public bool unevadable;

        public float exHeal_mul;
        public int healValue;
        public float healPercent;
        public float healRegain;

        public int SANHeal;
        public int SANDamage;
        public int shieldAdd;
        public int shieldAdd_percent;
        public int shieldRemove;

        public List<PA_StatusEffect.StatusEffectParams> applySteParams;
        public List<StEApplyBonus> applyStEBonus;
        public float debuffChanceMod;

        [Header("\n\nバフの除去")]
        public int removeStE_buff;
        [Header("\n\nデバフの除去")]
        public int removeStE_debuff;
        //[Header("\n\nDoTの除去")]
        //public int removeStE_DoT;

        public List<ActionData.RemoveStE> removeStEs;

        //public float moveChance;
        //public int moveForword;
        //public int moveUpper;
        //public int moveLower;
        //public int moveBackword;

        public string GetModInfo()
        {
            string s = "";
            bool f = false;
            if (conditionInfo != "") { s += string.Format("○{0}\n", conditionInfo); }
            if (!hideValues)
            {
                if (consumeFocus) { s += "・対象のフォーカスを消費する\n".ColorStr(Definer.colorRef.statusEffectColors[3]); }
                if (decreaseHP != 0) { s += ValueToStr("・HP減少量", decreaseHP, ""); }
                //if (ATKMod != 0) { s += ValueToStr("・ATK補正", ATKMod, "％"); }
                //if (INTMod != 0) { s += ValueToStr("・INT補正", INTMod, "％"); }
                if (ATKMod > 0) { s += $"・ATK{ATKMod}％分の物理ダメージを追加で与える\n"; }
                else if (ATKMod < 0) { s += $"・ATK補正{ATKMod}％\n"; }
                if (INTMod > 0) { s += $"・INT{INTMod}％分の魔法ダメージを追加で与える\n"; }
                else if (INTMod < 0) { s += $"・INT補正{INTMod}％\n"; }
                if (trueATKDMG != 0) { s += ValueToStr("・固定物理ダメージ", trueATKDMG, ""); }
                if (trueINTDMG != 0) { s += ValueToStr("・固定魔法ダメージ", trueINTDMG, ""); }
                if (exDMG_mul != 0) { s += ValueToStr("・与ダメージ", exDMG_mul, "％"); }
                if (exATKDMG_int != 0) { s += ValueToStr("・与物理ダメージ", exATKDMG_int, ""); }
                if (exINTDMG_int != 0) { s += ValueToStr("・与魔法ダメージ", exINTDMG_int, ""); }
                if (ACCMod != 0) { s += ValueToStr("・ACC補正", ACCMod, ""); }
                if (CRITCMod != 0) { s += ValueToStr("・CRIT率補正", CRITCMod, "％"); }
                if (CRITDMod != 0) { s += ValueToStr("・CRITダメージ補正", CRITDMod, "倍"); }
                if (drain != 0) { s += ValueToStr("・与ダメージの", drain, "％を回復"); }
                if (sureHit) { s += "・攻撃が必中となる\n"; }
                if (unevadable) { s += "・EVDを無視\n"; }
                if (exHeal_mul != 0) { s += ValueToStr("・回復量", exHeal_mul, "％"); }
                if (healValue != 0) { s += ValueToStr("・回復量", healValue, ""); }
                if (healPercent != 0) { s += ValueToStr("・割合回復量", healPercent, "％"); }
                if (healRegain != 0) { s += ValueToStr("・減少したHPの", healRegain, "％を回復"); }
                if (SANHeal != 0) { s += ValueToStr("・正気度回復量", SANHeal, ""); }
                if (SANDamage != 0) { s += ValueToStr("・正気度減少", SANDamage, ""); }
                string shield = "シールド".ToLinkKey().ColorStr(Definer.colorRef.shield);
                if (shieldAdd != 0) { s += ValueToStr($"・{shield}を", shieldAdd, "付与"); }
                if (shieldAdd_percent != 0) { s += ValueToStr($"・maxHPの", shieldAdd_percent, $"％に等しい{shield}を付与"); }
                if (shieldRemove != 0) { s += ValueToStr($"・{shield}除去量", shieldRemove, ""); }

                f = false;
                foreach (PA_StatusEffect.StatusEffectParams StEParams in applySteParams)//StE付与
                {
                    PA_StatusEffect.StatusEffectStatus status = StEParams.applyStE.GetComponent<PA_StatusEffect>().GetStatusEffectStatus();

                    if (f) { s += "\n"; }
                    f = true;
                    string chanceText = StEParams.guaranteed ? "確定" : $"{StEParams.applyChance}％";
                    string exText = "";
                    if (status.DoT)
                    {
                        exText += $"HP減少量：{(StEParams.refATK ? "ATK".ColorStr(Definer.colorRef.damage) : "INT".ColorStr(Definer.colorRef.INTDamage))}の{StEParams.value}％\n";
                    }
                    if (status.regen)
                    {
                        exText += $"回復量：{(StEParams.refATK ? "ATK".ColorStr(Definer.colorRef.damage) : "INT".ColorStr(Definer.colorRef.INTDamage))}の{StEParams.value}％\n";
                    }
                    s += $"・{status.ToLinkKey(false, StEParams.value)}を付与\n{exText}({chanceText},{StEParams.stack}スタック)\n";
                }
                foreach (StEApplyBonus bonus in applyStEBonus)
                {
                    string StEName = "・"+bonus.applyStE.GetComponent<PA_StatusEffect>().GetStatusEffectStatus().StEName;
                    if (bonus.exChance != 0) { s += ValueToStr(string.Format("{0}付与確率", StEName), bonus.exChance, "％"); }
                    if (bonus.exStack != 0) { s += ValueToStr(string.Format("{0}付与スタック数", StEName), bonus.exStack, ""); }
                    if (bonus.exValue != 0) { s += ValueToStr(string.Format("付与する{0}の効果量", StEName), bonus.exValue, ""); }
                }
                if (debuffChanceMod != 0) { s += ValueToStr("・デバフ付与確率", debuffChanceMod, "％"); }

                if (removeStE_buff > 0) { s += string.Format("・{0}を{1}個消去\n", "バフ効果".ColorStr(Definer.colorRef.statusEffectColors[(int)PA_StatusEffect.StatusEffectStatus.StatusEffectType.buff]), removeStE_buff); }
                if (removeStE_debuff > 0) { s += string.Format("・{0}を{1}個消去\n", "デバフ効果".ColorStr(Definer.colorRef.statusEffectColors[(int)PA_StatusEffect.StatusEffectStatus.StatusEffectType.debuff]), removeStE_debuff); }
                //if (removeStE_DoT > 0) { s += string.Format("・{0}を{1}個消去\n", "ダメージ効果".ColorStr(Definer.colorRef.statusEffectColors[(int)PA_StatusEffect.StatusEffectStatus.StatusEffectType.DoT]), removeStE_DoT); }

                foreach (ActionData.RemoveStE remove in removeStEs)
                {
                    PA_StatusEffect.StatusEffectStatus status = remove.removeStE.GetComponent<PA_StatusEffect>().GetStatusEffectStatus();
                    s += string.Format("・{0}", status.ToLinkKey());
                    if (remove.removeAll) { s += "を全て除去\n"; }
                    else { s += ValueToStr("のスタック", remove.addAmount, ""); }
                }
            }

            if (exInfo != "") { s += exInfo+"\n"; }

            return s;//.ColorStr(Definer.colorRef.AMod)
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
