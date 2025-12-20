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

        public int exTurn;

        public bool consumeFocus;
        public int decreaseHP;

        public float ATKMod;
        public float INTMod;

        public int trueATKDMG;
        public int trueINTDMG;

        public float exDMG_mul;
        public int exATKDMG_int;
        public int exINTDMG_int;
        public int ATKDMG_divide_mul;
        public int INTDMG_divide_mul;
        public int ATKDMG_divide_int;
        public int INTDMG_divide_int;
        public float ACCMod;
        public float CRITCMod;
        public float CRITDMod;
        public float drain;
        public bool ignoreShield;
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
        public bool shieldRemove_all;

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

        public Character.CharaStatusMod summonStatusMod;

        //public float moveChance;
        //public int moveForword;
        //public int moveUpper;
        //public int moveLower;
        //public int moveBackword;

        public string GetModInfo()
        {
            string s = "";
            //bool f = false;
            if (conditionInfo != "") { s += string.Format("○{0}\n", conditionInfo); }
            if (!hideValues)
            {
                if (exTurn > 0) { s += $"追加ターンを{exTurn}得る\n"; }
                if (consumeFocus) { s += $"・対象の{"focus".ToSpr()}を消費する\n"; }
                if (decreaseHP != 0) { s += ValueToStr($"・{"HP".ToSpr_withName()}減少量", decreaseHP, ""); }
                if (ATKMod != 0) { s += ValueToStr($"・{"ATK".ToSpr()}{"<color=#C30000>ATK</color>補正".ToLinkKey("ATK(INT)補正")}", ATKMod, "％"); }
                if (INTMod != 0) { s += ValueToStr($"・{"INT".ToSpr()}{"<color=#256CC8>INT</color>補正".ToLinkKey("ATK(INT)補正")}", INTMod, "％"); }
                //if (ATKMod > 0) { s += $"・ATK{ATKMod}％分の物理ダメージを追加で与える\n"; }
                //else if (ATKMod < 0) { s += $"・ATK補正{ATKMod}％\n"; }
                //if (INTMod > 0) { s += $"・INT{INTMod}％分の魔法ダメージを追加で与える\n"; }
                //else if (INTMod < 0) { s += $"・INT補正{INTMod}％\n"; }
                if (trueATKDMG != 0) { s += ValueToStr($"・固定{"ATK".ToSpr_withName("物理")}ダメージ", trueATKDMG, ""); }
                if (trueINTDMG != 0) { s += ValueToStr($"・固定{"INT".ToSpr_withName("魔法")}ダメージ", trueINTDMG, ""); }
                if (exDMG_mul != 0) { s += ValueToStr("・与ダメージ", exDMG_mul, "％"); }
                if (exATKDMG_int != 0) { s += ValueToStr($"・与{"ATK".ToSpr_withName("物理")}ダメージ", exATKDMG_int, ""); }
                if (exINTDMG_int != 0) { s += ValueToStr($"・与{"INT".ToSpr_withName("魔法")}ダメージ", exINTDMG_int, ""); }
                if (ATKDMG_divide_mul != 0) { s += ValueToStr($"・{"ATK".ToSpr_withLink()}", ATKDMG_divide_mul, $"％分の{"ATK".ToSpr_withName("物理")}ダメージを割り振る"); }
                if (INTDMG_divide_mul != 0) { s += ValueToStr($"・{"INT".ToSpr_withLink()}", INTDMG_divide_mul, $"％分の{"INT".ToSpr_withName("魔法")}ダメージを割り振る"); }
                //if (ATKDMG_divide_int != 0) { s+= }
                //if (INTDMG_divide_int != 0) { s += ValueToStr($"・与{"INT".ToSpr_withName("魔法")}ダメージ", INTDMG_divide_int, ""); }
                if (ACCMod != 0) { s += ValueToStr($"・{"ACC".ToSpr_withLink()}補正", ACCMod, ""); }
                if (CRITCMod != 0) { s += ValueToStr($"・{"CRIT".ToSpr_withLink()}率補正", CRITCMod, "％"); }
                if (CRITDMod != 0) { s += ValueToStr($"・{"CRIT".ToSpr_withLink()}ダメージ補正", CRITDMod, "％"); }
                if (drain != 0) { s += ValueToStr("・与ダメージの", drain, $"％を{"HP".ToSpr_withName("回復")}"); }
                if (ignoreShield) s += $"・{"shield".ToSpr_withLink()}を無視するようになる\n";
                if (sureHit) { s += "・攻撃が必中となる\n"; }
                if (unevadable) { s += $"・対象の{"EVD".ToSpr_withLink()}を無視\n"; }
                if (exHeal_mul != 0) { s += ValueToStr($"・{"HP".ToSpr_withName("回復")}量", exHeal_mul, "％"); }
                if (healValue != 0) { s += ValueToStr($"・{"HP".ToSpr_withName("回復")}量", healValue, ""); }
                if (healPercent != 0) { s += ValueToStr($"・割合{"HP".ToSpr_withName("回復")}量", healPercent, "％"); }
                if (healRegain != 0) { s += ValueToStr($"・減少した{"HP".ToSpr_withName()}の", healRegain, $"％を{"HP".ToSpr_withName("回復")}"); }
                if (SANHeal != 0) { s += ValueToStr($"・{"SAN".ToSpr_withLink()}回復量", SANHeal, ""); }
                if (SANDamage != 0) { s += ValueToStr($"・{"SAN".ToSpr_withLink()}減少", SANDamage, ""); }
                string shield = "shield".ToSpr_withLink();
                if (shieldAdd != 0) { s += ValueToStr($"・{shield}を", shieldAdd, "付与"); }
                if (shieldAdd_percent != 0) { s += ValueToStr($"・{"maxHP".ToSpr_withName()}の", shieldAdd_percent, $"％に等しい{shield}を付与"); }
                if (shieldRemove != 0) { s += ValueToStr($"・{shield}除去量", shieldRemove, ""); }
                if (shieldRemove_all) { s += $"・{shield}をすべて除去する\n"; }

                //f = false;
                foreach (PA_StatusEffect.StatusEffectParams StEParams in applySteParams)//StE付与
                {
                    s += StEParams.GetInfo();
                }
                foreach (StEApplyBonus bonus in applyStEBonus)
                {
                    s+= bonus.GetInfo();
                }
                if (debuffChanceMod != 0) { s += ValueToStr($"・{"debuff".ToSpr_withName()}付与確率", debuffChanceMod, "％"); }

                if (removeStE_buff > 0) { s += string.Format("・{0}を{1}個消去\n", "buff".ToSpr_withName(), removeStE_buff); }
                if (removeStE_debuff > 0) { s += string.Format("・{0}を{1}個消去\n", "debuff".ToSpr_withName(), removeStE_debuff); }
                //if (removeStE_DoT > 0) { s += string.Format("・{0}を{1}個消去\n", "ダメージ効果".ColorStr(Definer.colorRef.statusEffectColors[(int)PA_StatusEffect.StatusEffectStatus.StatusEffectType.DoT]), removeStE_DoT); }

                foreach (ActionData.RemoveStE remove in removeStEs)
                {
                    PA_StatusEffect.StatusEffectStatus status = remove.removeStE.GetComponent<PA_StatusEffect>().GetStatusEffectStatus();
                    s += string.Format("・{0}", status.ToLinkKey());
                    if (remove.removeAll) { s += "を全て除去\n"; }
                    else { s += ValueToStr("のスタック", remove.addAmount, ""); }
                }
                string summonMod = summonStatusMod.GetInfo();
                if (summonMod != "") { s += $"・{"summon".ToSpr_withName("召喚体")}のステータスが増加：\n{summonMod}\n"; }
            }

            if (exInfo != "") { s += exInfo + "\n"; }

            return s;//.ColorStr(Definer.colorRef.AMod)
        }
        public string ValueToStr(string start, float value, string end)
        {
            if (value == 0) { return ""; }
            string s = start;
            if (value < 0) { s += value.ToString().ColorStr(Color.red); }
            else { s += ("+" + value.ToString()).ColorStr(Color.green); }
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
