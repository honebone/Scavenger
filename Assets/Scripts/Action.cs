using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;
using System.Linq;

public class Action : MonoBehaviour
{
    ActionStatus actionStatus;
    ActionQueueManager actionQueueManager;
    CharactersManager characterManager;
    InfoText infoText;
    SoundManager soundManager;
    CameraManager cameraManager;
    BattleManager battleManager;


    [System.Serializable]
    public struct ActionStatus
    {
        [Header("アビリティのactionの場合は設定不要")]
        public string actionName;
        [TextArea(3, 10)]
        public string conditionInfo;
        [TextArea(3, 10)]
        public string targetInfo;
        [TextArea(3, 10), Header("AModのスクリプトを使わないときなどに\n条件入力して改行して変化内容書く")]
        public string AModInfo;
        [TextArea(3, 10)]
        public string actionInfo;
        public Sprite sprite;

        public AudioClip SE;
        public GameObject VE_OnTargets;
        public GameObject VE_OnOwner;

        [Header("設定しなければ汎用的なオブジェクトになる")]
        public GameObject actionObject;
        public List<GameObject> actionMods;
        //public bool targetEmpty;

        /// <summary>
        /// row:段 column:列
        /// </summary>
        public enum TargetType { other, single, all, self, row, column, singleWoSelf, allWoSelf, random, move,neigbor }
       
        [Header("\n\n\nここからアビリティのみ関係")]
        public TargetType targetType;
        public bool friendly;
        public CharactersManager.SearchCharaCondition condition;
        [Header("targetTypeがneigborの時のみ参照")]public List<Vector2Int> neigborPos;

        public bool ignoreMark;
        public bool ignoreHide;
        [Header("0:right 1:upper 2:lower 3:left(targetypeがmoveのときに使用)")]
        public List<int> moveValue;
        [Header("アビリティ使用時のポーズをなくす(非プレイヤーのみ関係)")]public bool skipAnim;
        [Header("ここまでアビリティのみ関係\n\n\n")]
        public bool consumeFocus;

        public int exTurn;

        public bool kill;
        public int decreaseHP_min;
        public int decreaseHP_max;
        public float decreaseHPPer_min;
        public float decreaseHPPer_max;
        public Vector2 decreaseHP_ATK;
        public Vector2 decreaseHP_INT;
        public List<EchoDoTParams> echoDoT;

        [Header("\n\n攻撃")]
        [TextArea(3, 10)] public string attackInfo;
        public float ATKMod_min;
        public float ATKMod_max;
        public float INTMod_min;
        public float INTMod_max;

        public float ATKMod_divide;
        public float INTMod_divide;

        public int trueATKDMG;
        public int trueINTDMG;

        public float exDMG_mul;
        public int exATKDMG_int;
        public int exINTDMG_int;
        /// <summary>この値を対象数で割った物理ダメージ</summary>
        public int ATKDMG_divide_int;
        public int INTDMG_divide_int;

        public float ACCMod;
        public float CRITCMod;
        public float CRITDMod;
        [Header("与ダメのdrain％回復")]
        public float drain;
        public bool ignoreShield;
        public bool sureHit;
        public bool unevadable;

        [Header("\n\n回復")]
        public int healValue_min;
        public int healValue_max;
        public Vector2 healINT;
        public float healPercent_min;
        public float healPercent_max;
        [Header("減少体力の割合回復")]
        public float healRegain_min;
        public float healRegain_max;

        public int trueHeal;
        public float exHeal_mul;

        [Header("\n\nSAN")]
        public int SANHeal_min;
        public int SANHeal_max;
        public int SANDamage_min;
        public int SANDamage_max;
        [Header("\n\nShield")]
        public int shieldAdd_min;
        public int shieldAdd_max;
        public int shieldPercent_min;
        public int shieldPercent_max;
        public bool shieldRemove_all;
        public int shieldRemove_min;
        public int shieldRemove_max;
        [Header("\n\nApplyStE")]
        public List<PA_StatusEffect.StatusEffectParams> applySteParams;
        [Header("ApplyPE")]
        public List<PositionEffect.PositionEffectParams> applyPEParams;


        [Header("\n\nバフの除去")]
        public int removeStE_buff;
        [Header("\nデバフの除去")]
        public int removeStE_debuff;
        //[Header("\nDoTの除去")]
        //public int removeStE_DoT;

        [Header("特定のStEの除去")]
        public List<ActionData.RemoveStE> removeStEs;
        [Header("特定のPEの除去")]
        public List<ActionData.RemovePE> removePEs;

       
        [Header("\n\n召喚")]
        public bool summon;
        //public int summonSize;
        public List<CharacterData> summonChara;
        public List<float> summonChanceWeight;
        [System.Serializable]
        public class SummonStatusInherit
        {
           [Header("全部％で")] 
            public float maxHP;
            public float ATK;
            public float INT;
            public float CRITC_TH;
            public float CRITC;
            public float CRITD_TH;
            public float CRITD;

            public float EVD_TH;
            public float EVD;
            public float ACC_TH;
            public float ACC;

            public float ACT_TH;
            public float ACT;

            public float GHeal_TH;
            public float GHeal;

            public string GetInfo(bool refStatus=false, Character.CharacterStatus status=new Character.CharacterStatus())
            {
                Character.CharaStatusMod mod = new Character.CharaStatusMod();
               if (refStatus) mod = ToStatuMod(status);
                string info = "";
                if (maxHP > 0) { info += $"・{"maxHP".ToSpr_withName()}の{(maxHP + "％").ColorStr(Definer.colorRef.emphasize)}{(refStatus ? $"({mod.maxHP_int})" : "")}\n"; }
                if (ATK > 0) { info+=$"・{"ATK".ToSpr_withLink()}の{(ATK + "％").ColorStr(Definer.colorRef.emphasize)}{(refStatus ? $"({mod.ATK_int})" : "")}\n"; }
                if (INT > 0) { info+=$"・{"INT".ToSpr_withLink()}の{(INT + "％").ColorStr(Definer.colorRef.emphasize)}{(refStatus ? $"({mod.INT_int})" : "")}\n"; }
                if (CRITC > 0) { info += ToStr(CRITC_TH, true, CRITC, $"{"CRIT".ToSpr_withLink()}率",refStatus,mod.CRITC); }
                if (CRITD > 0) { info += ToStr(CRITD_TH, true, CRITD, $"{"CRIT".ToSpr_withLink()}ダメージ", refStatus, mod.CRITD); }
                if (EVD > 0) { info += ToStr(EVD_TH, false, EVD, "EVD".ToSpr_withLink(), refStatus, mod.EVD); }
                if (ACC > 0) { info += ToStr(ACC_TH, false, ACC, "ACC".ToSpr_withLink(), refStatus, mod.ACC); }
                if (ACT > 0) { info += ToStr(ACT_TH, false, ACT, "ACT".ToSpr_withLink(), refStatus, mod.ACT); }
                if (GHeal > 0) { info += ToStr(GHeal_TH, false, GHeal, "与える回復量", refStatus, mod.GHeal); }

                return info;
            }

            public Character.CharaStatusMod ToStatuMod(Character.CharacterStatus status)
            {
                Character.CharaStatusMod mod = new Character.CharaStatusMod();
                mod.Init();

                mod.maxHP_int = Mathf.FloorToInt(status.maxHP * maxHP / 100f);
                mod.ATK_int = Mathf.FloorToInt(status.ATK * ATK / 100f);
                mod.INT_int = Mathf.FloorToInt(status.INT * INT / 100f);

                if (status.CRITC - CRITC_TH > 0) { mod.CRITC = status.CRITC * CRITC / 100f; }
                if (status.CRITD - CRITD_TH > 0) { mod.CRITD = status.CRITD * CRITD / 100f; }
                if (status.EVD - EVD_TH > 0) { mod.EVD = (status.EVD - EVD_TH) * EVD / 100f; }
                if (status.ACC - ACC_TH > 0) { mod.ACC = (status.ACC - ACC_TH) * ACC / 100f; }
                if (status.ACT - ACT_TH > 0) { mod.ACT = Mathf.FloorToInt((status.ACT - ACT_TH) * ACT / 100f); }
                if (status.GHeal - GHeal_TH > 0) { mod.GHeal = Mathf.FloorToInt((status.GHeal - GHeal_TH) * GHeal / 100f); }
                return mod;
            }

            string ToStr(float TH,bool THPercent,float ratio,string statName,bool refStatus,float value)
            {
                string str = "・";
                if (TH > 0) str += $"{(TH+(THPercent ? "％" : "")).ColorStr(Definer.colorRef.emphasize)}を超えた";
                str += $"{statName}の{(ratio+"％").ColorStr(Definer.colorRef.emphasize)}{(refStatus ? $"({value}{(THPercent ? "％" : "")})" : "")}\n";
                return str;
            }
        }
        public SummonStatusInherit summonStatusInherit;

        [Header("\n\n移動")]
        public float moveChance;
        public bool guaranteedMove;
        public int moveRandomDir;
        [Header("randomDirが1以上のとき、以下の4つはboolとして働く")]
        public int moveForword;
        public int moveUpper;
        public int moveLower;
        public int moveBackword;

        [Header("\n\n行動主のSprite変更")]
        public GameObject changeSelfSprite;

        [Header("\n\nアビリティ使用回数/クールダウン")]
        public List<ActionData.AbilityRemainControll> abilityRemainControlls;

        [Header("\n\n\n\n以下には手を出すな")]
        public bool abilityEffect;
        public bool freeAction;
        public AbilityData.AbilityType abilityType;
        public int index;//複数効果があるアビリティの際に使う

        /// <summary>AModによって追加</summary>
        public List<StEApplyBonus> StEApplyBonus;
        public float debuffChanceMod;
        public List<ActionData.RemoveStE> removeStEs_additional;

        public List<Character.CharaStatusMod> summonStatusMods;

        //public bool dontChangeSprite;
        //[Header("スプライトの直接指定")]
        //public GameObject activateSprite;

        public Character actionOwner;
        /// <summary>アクションの引き金となった原因</summary>
        public PassiveAbility source;
        //[System.NonSerialized]
        //public Character.CharacterStatus ownerStatus_notChara;
        public int targetCount;
        /// <summary>この中からtargetCount個だけランダムに選ばれる</summary>
        public List<Character> actionTargets;
        /// <summary>移動や召喚の際に使用 移動の際は移動先のposが入る</summary>
        public List<int> actionTargetsInt;

        public bool DoesDecreaseHP() { return decreaseHPPer_max > 0 || decreaseHP_max > 0 || decreaseHP_ATK.y > 0 || decreaseHP_INT.y > 0||echoDoT.Count>0; }
        public bool DoesAttack()
        {
            return ATKMod_max > 0 || INTMod_max > 0 || exATKDMG_int > 0 || exINTDMG_int > 0 || trueATKDMG > 0 || trueINTDMG > 0
                || ATKDMG_divide_int > 0 || INTDMG_divide_int > 0 || ATKMod_divide > 0 || INTMod_divide > 0;
        }
        public bool DoesHeal() { return healPercent_max > 0 || healINT.y > 0 || healValue_max > 0 || healRegain_max > 0 || trueHeal > 0; }

        public string GetTargetInfo()
        {
            return $"対象：{targetInfo}";
        }

        public string GetInfo(bool refCharaStatus=false, Character.CharacterStatus characterStatus=new Character.CharacterStatus())
        {
            string info = "";
            string s = "";

            if (conditionInfo != "") { s += $"{NL()}{conditionInfo}"; }
            if (targetInfo != "") s += $"{NL()}対象：{targetInfo}";
            if (consumeFocus) { s += $"{NL()}・対象の<color=#DD6300><sprite name=focus><link=S_フォーカス><u>フォーカス</u></link></color>を消費する".ColorStr(Definer.colorRef.statusEffectColors[3]); }
            if (exTurn > 0) { s += $"{NL()}・追加ターンを{exTurn}ターン得る"; }
            if (kill) { s += $"{NL()}・殺害する"; }
            if (decreaseHP_max > 0)
            {
                s += $"{NL()}・{"HP".ToSpr_withName()}が{GetValueRange(decreaseHP_min, decreaseHP_max)}減少";
            }
            if (decreaseHPPer_max > 0)
            {
                s += $"{NL()}・{"HP".ToSpr_withName()}が{GetValueRange(decreaseHPPer_min, decreaseHPPer_max)}％減少";
            }
            if (decreaseHP_ATK.y > 0)
            {
                s += $"{NL()}・{"HP".ToSpr_withName()}が{"ATK".ToSpr_withLink()}の{GetValueRange(decreaseHP_ATK)}％分減少";
            }
            if (decreaseHP_INT.y > 0)
            {
                s += $"{NL()}・{"HP".ToSpr_withName()}が{"INT".ToSpr_withLink()}の{GetValueRange(decreaseHP_INT)}％分減少";
            }
            echoDoT.ForEach(e =>
            {
                s += $"{NL()}・{e.GetInfo()}";
            });
            //NewBlock();

            if (DoesAttack()||attackInfo!="")//攻撃
            {
                s += (attackInfo != "") ? $"{NL()}・{attackInfo}" : "";
                if (ATKMod_max > 0 || ATKMod_divide > 0)
                {
                    s += $"{NL()}・{"ATK".ToSpr_withName("物理")}攻撃を行う\n";
                    if (ATKMod_max > 0)
                    {
                        s += $"  {"ATK".ToSpr_withLink()}の{GetValueRange(ATKMod_min, ATKMod_max)}％ダメージ";
                        if (refCharaStatus)
                        {
                            s += $"({GetValueRange(Mathf.RoundToInt(characterStatus.ATK * ATKMod_min / 100), Mathf.RoundToInt(characterStatus.ATK * ATKMod_max / 100))})";
                        }
                    }
                    if (ATKMod_divide > 0)
                    {
                        s += $"  {"ATK".ToSpr_withLink()}{ATKMod_divide}％の{"分配ダメージ".ToLinkKey()}";
                        if (refCharaStatus)
                        {
                            s += string.Format("({0})", (characterStatus.ATK * ATKMod_divide / 100).ToInt());
                        }
                    }
                }
                if (INTMod_max > 0 || INTMod_divide > 0)
                {
                    s += $"{NL()}・{"INT".ToSpr_withName("魔法")}攻撃を行う";
                    if (INTMod_max > 0)
                    {
                        s += $"{NL()}  {"INT".ToSpr_withLink()}の{GetValueRange(INTMod_min, INTMod_max)}％ダメージ";
                        if (refCharaStatus)
                        {
                            s += string.Format("({0})", GetValueRange(Mathf.RoundToInt(characterStatus.INT * INTMod_min / 100), Mathf.RoundToInt(characterStatus.INT * INTMod_max / 100)));
                        }
                    } 
                    if (INTMod_divide > 0)
                    {
                        s += $"{NL()}  {"INT".ToSpr_withLink()}{INTMod_divide}％の{"分配ダメージ".ToLinkKey()}";
                        if (refCharaStatus)
                        {
                            s += string.Format("({0})", (characterStatus.INT * INTMod_divide / 100).ToInt());
                        }
                    }
                }

                string attack = "";
                if (ACCMod != 0) { attack += $"{Extentions.NL(attack)}  {"ACC".ToSpr_withLink()}補正：{GetValueWithSign(ACCMod)}"; }
                if (CRITCMod != 0) { attack += $"{Extentions.NL(attack)}  {"CRIT".ToSpr_withLink()}率補正：{GetValueWithSign(CRITCMod)}％"; }
                if (CRITDMod != 0) { attack += $"{Extentions.NL(attack)}   {"CRIT".ToSpr_withLink()}ダメージ補正：{GetValueWithSign(CRITDMod)}％"; }
                if (drain > 0) { attack += $"{Extentions.NL(attack)}  与ダメージの{drain}％を{"HP".ToSpr_withName("回復")}"; }
                if (ignoreShield) { attack += $"{Extentions.NL(attack)}  {"shield".ToSpr_withLink()}を無視"; }
                if (sureHit) { attack += $"{Extentions.NL(attack)}  必中"; }
                if (unevadable) { attack += $"{Extentions.NL(attack)}  対象の{"EVD".ToSpr_withLink()}を無視"; }
                if (attack != "") s += $"{NL()}{attack.ColorStr(Color.gray)}";
            }
            //NewBlock();

            if (DoesHeal())//回復
            {
                if (healValue_max > 0) { s += $"{NL()}・{"HP".ToSpr_withName()}を{ GetValueRange(healValue_min, healValue_max)}回復"; }
                if (healINT.y > 0)
                {
                    s += $"{NL()}・{"HP".ToSpr_withName()}を{"INT".ToSpr_withLink()}の{GetValueRange(healINT.x, healINT.y)}％分回復";
                    if (refCharaStatus)
                    {
                        s += $"({GetValueRange(Mathf.RoundToInt(characterStatus.INT * healINT.x / 100), Mathf.RoundToInt(characterStatus.INT * healINT.y / 100))})";
                    }
                }
                if (healPercent_max > 0) { s += $"{NL()}・{"HP".ToSpr_withName()}を最大値の{ GetValueRange(healPercent_min, healPercent_max)}％回復"; }
                if (healRegain_max > 0) { s += $"{NL()}・減少した{"HP".ToSpr_withName()}の{GetValueRange(healRegain_min, healRegain_max)}％を回復"; }
                if (trueHeal > 0) { s += $"{NL()}・{"HP".ToSpr_withName()}を{trueHeal}固定量回復"; }
            }
            //NewBlock();

            if (SANHeal_max > 0) { s += $"{NL()}・{"SAN".ToSpr_withLink()}を{GetValueRange(SANHeal_min, SANHeal_max)}回復"; }
            if (SANDamage_max > 0) { s += $"{NL()}・{"SAN".ToSpr_withLink()}が{GetValueRange(SANDamage_min, SANDamage_max)}減少"; }
            //NewBlock();

            string shield = "shield".ToSpr_withLink();
            if (shieldAdd_max > 0) { s += $"{NL()}・{shield}を{GetValueRange(shieldAdd_min, shieldAdd_max)}付与"; }
            if (shieldPercent_max > 0) { s += $"{NL()}・{"maxHP".ToSpr_withName()}の{GetValueRange(shieldPercent_min, shieldPercent_max)}％に等しい{shield}を付与"; }
            if (shieldRemove_all) { s += $"・{NL()}{shield}を0にする"; }
            else if (shieldRemove_max > 0) { s += $"{NL()}・{shield}を{ GetValueRange(shieldRemove_min, shieldRemove_max)}除去"; }
            //NewBlock();

            foreach (PA_StatusEffect.StatusEffectParams StEParams in applySteParams)//StE付与
            {
                s += $"{NL()}{StEParams.GetInfo(refCharaStatus,characterStatus)}";
            }

            //NewBlock();
            foreach (PositionEffect.PositionEffectParams PEParams in applyPEParams)//PE付与
            {
                PositionEffect.PositionEffectStatus status = PEParams.applyPE.GetComponent<PositionEffect>().GetPositionEffectStatus();

                string chanceText = PEParams.guaranteed ? "確定" : $"{PEParams.applyChance}％";
                string exText = "";
                if (status.DoT)
                {
                    exText += $"{"HP".ToSpr_withName()}減少量：{(PEParams.refATK ? "ATK".ToSpr_withLink() : "INT".ToSpr_withLink())}の{PEParams.value}％\n";
                    if (refCharaStatus)
                    {
                        int baseDMG = (PEParams.refATK) ? characterStatus.ATK : characterStatus.INT;
                        int DMGPerTurn = (baseDMG * PEParams.value / 100f).ToInt();
                        exText += $"({DMGPerTurn}/ターン)\n".ColorStr(Definer.colorRef.decreaseHP);
                    }
                }
                if (status.regen)
                {
                    exText += $"{"HP".ToSpr_withName("回復")}量：{(PEParams.refATK ? "ATK".ToSpr_withLink() : "INT".ToSpr_withLink())}の{PEParams.value}％\n";
                    if (refCharaStatus)
                    {
                        int baseDMG = (PEParams.refATK) ? characterStatus.ATK : characterStatus.INT;
                        int DMGPerTurn = (baseDMG * PEParams.value / 100f).ToInt();
                        exText += $"({DMGPerTurn}/ターン)\n".ColorStr(Definer.colorRef.heal);
                    }
                }

                s += $"{NL()}・対象の地点に{status.ToLinkKey(false, PEParams.value)}を付与\n{exText}({chanceText},{PEParams.stack}スタック)\n";
            }
            //NewBlock();

            if (removeStE_buff > 0) { s += $"{NL()}・{"buff".ToSpr_withName()}を{removeStE_buff}個消去"; }
            if (removeStE_debuff > 0) { s += $"{NL()}・{"debuff".ToSpr_withName()}を{removeStE_debuff}個消去"; }
            foreach (ActionData.RemoveStE remove in removeStEs)
            {
                PA_StatusEffect.StatusEffectStatus status = remove.removeStE.GetComponent<PA_StatusEffect>().GetStatusEffectStatus();
                s += $"{NL()}・{status.ToLinkKey()}";
                if (remove.removeAll) { s += "を全て除去"; }
                else { s += string.Format("のスタック{0}", GetValueWithSign(remove.addAmount)); }
            }
            foreach (ActionData.RemovePE remove in removePEs)
            {
                PositionEffect.PositionEffectStatus status = remove.removePE.GetComponent<PositionEffect>().GetPositionEffectStatus();
                s += $"{NL()}・{status.ToLinkKey()}";
                if (remove.removeAll) { s += "を全て除去"; }
                else { s += string.Format("のスタック{0}", GetValueWithSign(remove.addAmount)); }
            }
            //NewBlock();

            if (summon)
            {
                if (summonChara.Count == 1)
                {
                    s += $"{NL()}・{summonChara[0].ToLinkKey()}を{"summon".ToSpr_withName()}";
                }
                else
                {
                    s += $"{NL()}・以下からランダムに{"summon".ToSpr_withName()}";
                    float p = 0;
                    foreach (int r in summonChanceWeight) { p += r; }
                    for (int i = 0; i < summonChanceWeight.Count; i++)
                    {
                        s += $"{NL()}{summonChara[i].ToLinkKey()}({summonChanceWeight[i] / p:#0.0%})";
                    }
                }
                if (summonStatusInherit.GetInfo() != "")
                {
                    s += $"{NL()}召喚されたキャラはステータスの一部を受け継ぐ：\n{summonStatusInherit.GetInfo(refCharaStatus, characterStatus)}";
                }
            }
            //NewBlock();

            if (guaranteedMove || moveChance > 0)
            {
                if (guaranteedMove) { s += $"{NL()}・"; }
                else { s += $"{NL()}・{moveChance}％の確率で"; }
                if (moveRandomDir > 0)
                {
                    string dir = "";
                    if(moveForword > 0 && moveUpper > 0 && moveLower > 0 && moveBackword > 0) { s += $"ランダムな方向に{moveRandomDir}マス{"move".ToSpr_withName("移動")}"; }
                    else
                    {
                        if (moveForword > 0) { dir += $"{(dir == "" ? "" : ",")}前"; }
                        if (moveUpper > 0) { dir += $"{(dir == "" ? "" : ",")}上"; }
                        if (moveLower > 0) { dir += $"{(dir == "" ? "" : ",")}下"; }
                        if (moveBackword > 0) { dir += $"{(dir == "" ? "" : ",")}後ろ"; }
                        s += $"ランダムな{dir}方向に{moveRandomDir}マス{"move".ToSpr_withName("移動")}";
                    }                                       
                }
                else
                {
                    if (moveForword > 0) { s += $"{moveForword}マス{"move".ToSpr_withName("前進")}"; }
                    if (moveUpper > 0) { s += $"{moveUpper}マス{"move".ToSpr_withName("上昇")}"; }
                    if (moveLower > 0) { s += $"{moveLower}マス{"move".ToSpr_withName("下降")}"; }
                    if (moveBackword > 0) { s += $"{moveBackword}マス{"move".ToSpr_withName("後退")}"; }
                }
                
            }
            //NewBlock();

            foreach (ActionData.AbilityRemainControll rc in abilityRemainControlls)
            {
                if (rc.value > 0)
                {
                    s += $"{NL()}・<{rc.abilityData.abilityName.ColorStr(rc.abilityData.abilityType.ToColor())}>の使用回数を";
                    if (rc.set) { s += string.Format("{0}にする", rc.value); }
                    else
                    {
                        s += string.Format("{0}増加", rc.value);
                    }
                }
                if (rc.value_CD != 0)
                {
                    s += $"{NL()}・<{rc.abilityData.abilityName.ColorStr(rc.abilityData.abilityType.ToColor())}> のクールダウンを";
                    if (rc.set_CD) { s += $"{rc.value_CD}にする"; }
                    else if (rc.value_CD < 0) s += $"{-rc.value_CD}減少";
                    else s += $"{rc.value_CD}増加";
                }
            }
            NewBlock();

            foreach (GameObject actionMod in actionMods)
            {
                string modInfo = actionMod.GetComponent<ActionMod>().GetActionModStatus().GetModInfo();
                if (modInfo != "") { s +=$"{NL()}{modInfo}"; }
            }
            if (AModInfo != "")
            {
                s += $"{NL()}○{AModInfo}";
            }
            NewBlock();

            if (actionInfo != "") { s +=$"{NL()}{actionInfo}"; }
            NewBlock();

            if (actionObject != null)
            {
                string ex = actionObject.GetComponent<Action>().GetAdditionalInfo();
                if (ex != "") { s += $"{NL()}{ex}"; }
            }
            NewBlock();

            return info;

            void NewBlock()
            {
                if (s != "")
                {
                    info += $"{Extentions.NL(info, 2)}{s}";
                    s = "";
                }
            }

            string NL(int lines=1,string lineStr="\n")
            {
                return Extentions.NL(s, lines, lineStr);
            }
        }
        public string GetValueWithSign(float value)
        {
            if (value > 0) { return "+" + value.ToString(); }
            else { return value.ToString(); }
        }
        public string GetValueRange(float min, float max)
        {
            if (min == max) { return max.ToString(); }
            else { return string.Format("{0}-{1}", min, max); }
        }

        public string GetValueRange(Vector2 range)
        {
            if (range.x == range.y) { return range.x.ToString(); }
            else { return string.Format("{0}-{1}", range.x, range.y); }
        }

        //public ActionStatus Modify_ApplyStE(List<StEApplyBonus> applyBonus)
        //{
        //    ActionStatus modifiedStatus = this;
        //    foreach (StEApplyBonus bonus in applyBonus)
        //    {
        //        bool f = false;
        //        for (int i = 0; i < modifiedStatus.StEApplyBonus.Count; i++)
        //        {
        //            if (modifiedStatus.StEApplyBonus[i].applyStE == bonus.applyStE)
        //            {
        //                modifiedStatus.StEApplyBonus[i] = modifiedStatus.StEApplyBonus[i].AddBonus(bonus, true);
        //                f = true;
        //            }
        //        }
        //        if (!f) { modifiedStatus.StEApplyBonus.Add(bonus); }
        //    }

        //    return modifiedStatus;
        //}


        public ActionStatus Modify(ActionMod.ActionModStatus mod)
        {
            ActionStatus modifiedStatus = this;
            modifiedStatus.exTurn += mod.exTurn;
            if (mod.consumeFocus) { modifiedStatus.consumeFocus = true; }

            modifiedStatus.decreaseHP_min += mod.decreaseHP;
            modifiedStatus.decreaseHP_max += mod.decreaseHP;

            modifiedStatus.ATKMod_min += mod.ATKMod;
            modifiedStatus.ATKMod_max += mod.ATKMod;
            modifiedStatus.INTMod_min += mod.INTMod;
            modifiedStatus.INTMod_max += mod.INTMod;

            modifiedStatus.trueATKDMG += mod.trueATKDMG;
            modifiedStatus.trueINTDMG += mod.trueINTDMG;

            modifiedStatus.exDMG_mul += mod.exDMG_mul;
            modifiedStatus.exATKDMG_int += mod.exATKDMG_int;
            modifiedStatus.exINTDMG_int += mod.exINTDMG_int;
            modifiedStatus.ATKMod_divide += mod.ATKDMG_divide_mul;
            modifiedStatus.INTMod_divide += mod.INTDMG_divide_mul;
            modifiedStatus.ATKDMG_divide_int += mod.ATKDMG_divide_int;
            modifiedStatus.INTDMG_divide_int += mod.INTDMG_divide_int;
            modifiedStatus.ACCMod += mod.ACCMod;
            modifiedStatus.CRITCMod += mod.CRITCMod;
            modifiedStatus.CRITDMod += mod.CRITDMod;
            modifiedStatus.drain += mod.drain;
            if (mod.ignoreShield) modifiedStatus.ignoreShield = true;
            if (mod.sureHit) { modifiedStatus.sureHit = true; }
            if (mod.unevadable) { modifiedStatus.unevadable = true; }

            modifiedStatus.exHeal_mul += mod.exHeal_mul;
            modifiedStatus.healValue_min += mod.healValue;
            modifiedStatus.healValue_max += mod.healValue;
            modifiedStatus.healPercent_min += mod.healPercent;
            modifiedStatus.healPercent_max += mod.healPercent;
            modifiedStatus.healRegain_min += mod.healRegain;
            modifiedStatus.healRegain_max += mod.healRegain;

            modifiedStatus.SANHeal_min += mod.SANHeal;
            modifiedStatus.SANHeal_max += mod.SANHeal;
            modifiedStatus.SANDamage_min += mod.SANDamage;
            modifiedStatus.SANDamage_max += mod.SANDamage;
            modifiedStatus.shieldAdd_min += mod.shieldAdd;
            modifiedStatus.shieldAdd_max += mod.shieldAdd;
            modifiedStatus.shieldPercent_min += mod.shieldAdd_percent;
            modifiedStatus.shieldPercent_max += mod.shieldAdd_percent;

            modifiedStatus.shieldRemove_min += mod.shieldRemove;
            modifiedStatus.shieldRemove_max += mod.shieldRemove;
            if (mod.shieldRemove_all) modifiedStatus.shieldRemove_all = true;

            modifiedStatus.applySteParams = new List<PA_StatusEffect.StatusEffectParams>(modifiedStatus.applySteParams);
            foreach (PA_StatusEffect.StatusEffectParams statusEffectParams in mod.applySteParams)
            {
                modifiedStatus.applySteParams.Add(statusEffectParams);
            }

            //============================================================[StEApplyBonus]===============================================
            List<StEApplyBonus> bonusTemp = new List<StEApplyBonus>();
            for (int i = 0; i < modifiedStatus.StEApplyBonus.Count; i++)
            {
                bonusTemp.Add(new StEApplyBonus(modifiedStatus.StEApplyBonus[i]));
            }
            modifiedStatus.StEApplyBonus = new List<StEApplyBonus>(bonusTemp);

            foreach (StEApplyBonus bonus in mod.applyStEBonus)
            {
                bool f = false;
                for (int i = 0; i < modifiedStatus.StEApplyBonus.Count; i++)
                {
                    if (modifiedStatus.StEApplyBonus[i].applyStE == bonus.applyStE)
                    {
                        modifiedStatus.StEApplyBonus[i].AddBonus(bonus, true);
                        f = true;
                    }
                }
                if (!f) { modifiedStatus.StEApplyBonus.Add(bonus); }
            }


            modifiedStatus.debuffChanceMod += mod.debuffChanceMod;

            modifiedStatus.removeStE_buff += mod.removeStE_buff;
            modifiedStatus.removeStE_debuff += mod.removeStE_debuff;
            //removeStE_DoT += mod.removeStE_DoT;

            foreach (ActionData.RemoveStE removeStE in mod.removeStEs)
            {
                modifiedStatus.removeStEs_additional.Add(removeStE);
            }

            List<Character.CharaStatusMod> summonModsTemp = new List<Character.CharaStatusMod>(modifiedStatus.summonStatusMods);
            summonModsTemp.Add(mod.summonStatusMod);
            modifiedStatus.summonStatusMods = new List<Character.CharaStatusMod>(summonModsTemp);
            //move

            return modifiedStatus;
        }

        //public ActionStatus Copy()
        //{
        //    ActionStatus copy = this;
            
        //}
    }

    //===========================[誘発処理の引数に使う]================================
    public class ActionParams
    {
        public Action.ActionStatus actionStatus;
        public Character target;
        public Character owner;
        public List<PA_StatusEffect.StatusEffectStatus> targetStEs_preResolve;
    }

    public class ActionResult
    {
        public Action.ActionStatus actionStatus;
        public Character target;

        public bool damage;
        public OnDamageParams onDamageParams;
        public bool kill;
        public OnKillParams onKillParams;
    }
    public class OnAttackParams
    {
        /// <summary> = !(missed||evaded)</summary>
        public bool hit;
        public bool missed;
        public bool evaded;
        public bool CRIT;
        public float toralCRITC;
        public ActionParams actionParams;
    }
    public class OnDamageParams
    {
        public int totalDMG;
        public bool ATK;
        public bool INT;
        public int ATKDMG;
        public int INTDMG;
        public int shieldDMG;
        public bool CRIT;
        public ActionParams ap;
    }
    public class OnKillParams
    {
        public bool obstacle;
        public bool CRIT;
        public Character target;
        public ActionParams ap;
    }
    public class OnHealParams
    {
        public int healValue;
        public int overheal;
        public bool ability;
        public Character target;
    }
    public class OnApplyStEParams
    {
        /// <summary>appliedParams + resistedParams</summary>
        public List<PA_StatusEffect.StatusEffectParams> attemptedParams;
        public List<PA_StatusEffect.StatusEffectParams> appliedParams;
        public List<PA_StatusEffect.StatusEffectParams> resistedParams;
        public Character taget;
    }
    public class OnFocusParams
    {
        public ActionParams actionParams;
        public bool kill;
    }

    public class OnMoveParams
    {
        public int prevPos;
        public int currentPos;
        /// <summary>0:右　1:上　2:下　3:左</summary>
        public int dir;
        public int range;
        public bool secondaryMove;

        public Character target;
    }
    public class OnSummonParams
    {
        public ActionParams actionParams;
        /// <summary>
        /// nullとなる可能性があることに注意!!
        /// </summary>
        public Character summoned;
    }
    protected Character actionOwner;

    protected List<ActionResult> actionResults = new List<ActionResult>();
    List<OnAttackParams> onAttackParamsList = new List<OnAttackParams>();
    List<OnDamageParams> onDamageParamsList = new List<OnDamageParams>();   
    List<OnKillParams> onKillParamsList = new List<OnKillParams>();
    List<OnApplyStEParams> onApplyStEParamsList = new List<OnApplyStEParams>();
    List<OnHealParams> onHealParamsList = new List<OnHealParams>();
    List<OnFocusParams> onFocusParamsList = new List<OnFocusParams>();
    List<OnSummonParams> onSummonParamsList = new List<OnSummonParams>();
    OnMoveParams onMoveParams = new OnMoveParams();

    bool shakeCamera;
    int totalDamage;//テキスト表示用
    
    Utility util;

    /// <summary>
    /// status にはactionOwner(キャラ時) もしくは　ownerStatus_notChara(非キャラ時)のいずれかを代入した状態で渡すこと!!
    /// </summary>
    public void Init(ActionQueueManager qm, ActionStatus status, ActionInfoPanel infoPanel, InfoText it, Utility u, SoundManager sm, CameraManager cam,BattleManager bm)
    {
        actionQueueManager = qm;
        actionStatus = status;
        infoText = it;
        util = u;
        infoPanel.Init(actionStatus.actionName, actionStatus.GetInfo(false, new Character.CharacterStatus()));
        characterManager = FindObjectOfType<CharactersManager>();
        soundManager = sm;
        cameraManager = cam;
        battleManager = bm;

    }
    public ActionStatus GetActionStatus() { return actionStatus; }

    public virtual void Resolve()
    {
        DebugFunction.int1++;
        Character.CharacterStatus ownerStatus = new Character.CharacterStatus();
        GameParams gp = GameManager.gameParams;
        bool notChara = false;//フィールド効果やポジション効果などによるアクション
        if (actionStatus.actionOwner != null) {
            actionOwner = actionStatus.actionOwner;
            ownerStatus = actionStatus.actionOwner.CharaStatus(); 
        }
        else
        {
            notChara = true;
            ownerStatus = Definer.nonCharaStatus;
            ownerStatus.level = ExpeditionManager.inst.enemyLVL;
        }
        if (actionStatus.targetCount == 0)
        {
            //infoText.AddWarningText($"{actionStatus.actionName}の対象指定方法が旧形式の可能性あり");
        }
        else//対象の最終設定
        {
            if (actionStatus.actionTargets.Count > 0)//キャラを対象とする効果
            {
                List<Character> finalTargetsPool = new List<Character>();
                foreach(Character c in actionStatus.actionTargets)//生存中のキャラのみを抽出
                {
                    if (c.CheckAlive()) { finalTargetsPool.Add(c); }
                }
                actionStatus.actionTargets = new List<Character>(finalTargetsPool.Sample(actionStatus.targetCount));
            }
            else if (actionStatus.summon)//召喚
            {
                List<int> finalTargetsPool = new List<int>();
                foreach (int i in actionStatus.actionTargetsInt)//空いているポジションのみを抽出
                {
                    if (!characterManager.CheckCharaExist(i))
                    {
                        finalTargetsPool.Add(i);
                    }
                }
                actionStatus.actionTargetsInt = new List<int>(finalTargetsPool.Sample(actionStatus.targetCount));
            }
            else if(actionStatus.targetType == ActionStatus.TargetType.move)//移動
            {
                //何もしなくてよい
            }
            else//その他のポジションへの効果
            {
                //何もしなくてよい
            }
        }

       
        //演出関連
        if (actionStatus.actionTargets == null) { actionStatus.actionTargets = new List<Character>(); }
        ActionStatus[] actionsStatus ;

        if (actionStatus.actionTargets.Count > 0) { actionsStatus = new ActionStatus[actionStatus.actionTargets.Count]; }
        else if (actionStatus.actionTargetsInt != null) { actionsStatus = new ActionStatus[actionStatus.actionTargetsInt.Count]; }
        else { actionsStatus = new ActionStatus[0]; }

        for (int i = 0; i < actionsStatus.Length; i++)
        {
            actionsStatus[i] = actionStatus;
            List<StEApplyBonus> bonus = new List<StEApplyBonus>();
            for(int j=0;j< actionStatus.StEApplyBonus.Count; j++)
            {
                bonus.Add(new StEApplyBonus(actionStatus.StEApplyBonus[j]));
            }
            actionsStatus[i].StEApplyBonus = bonus;
        }
        if (actionStatus.SE != null) { soundManager.PlaySE(actionStatus.SE); }

        foreach(Character character in actionStatus.actionTargets)
        {
            int pos = character.CharaStatus().position;
            if (!actionStatus.actionTargetsInt.Contains(pos)) { actionStatus.actionTargetsInt.Add(pos); }
        }
        if (!notChara) { actionStatus.actionOwner.ActAnim(); }


        if (actionStatus.freeAction)
        {
            if (!actionStatus.abilityEffect) { infoText.AddErrorText("アビリティじゃないのにフリーアクション"); }
            actionStatus.actionOwner.ContinueTurn();
        }

        List<GameObject> actionModsObj = new List<GameObject>(actionStatus.actionMods);
        //ここで色々なactionMdsを追加
        if (!notChara)
        {
            actionModsObj.AddRange(ownerStatus.actionMods);
            actionModsObj.AddRange(actionStatus.actionOwner.GetPositionManager().GetActionMods());
        }
        foreach (GameObject actionModObj in actionModsObj)
        {
            var am = Instantiate(actionModObj);
            am.GetComponent<ActionMod>().Init(characterManager);
            actionsStatus = am.GetComponent<ActionMod>().ModifyAction(actionStatus, actionsStatus);
            Destroy(am);
        }

        if (!notChara) { actionStatus.actionOwner.ModifyAction(actionStatus, actionsStatus, false); }
        for (int i = 0; i < actionStatus.actionTargets.Count; i++)
        {
            actionsStatus[i] = actionStatus.actionTargets[i].ModifyAction_Targeted(actionsStatus[i], false);
        }

        if (actionStatus.changeSelfSprite != null) { actionOwner.SetCharaSprite(actionStatus.changeSelfSprite); }

        //各対象キャラへの処理
        for (int i = 0; i < actionStatus.actionTargets.Count; i++)
        {
            Character target = actionStatus.actionTargets[i];
            Character.CharacterStatus targetStatus = target.CharaStatus();
            bool attackHit = true;//攻撃失敗時、その他の効果も発動しないようにする
            ActionResult result = new ActionResult();
            result.target = target;
            result.actionStatus = actionsStatus[i];

            ActionParams actionParams = new ActionParams();
            actionParams.actionStatus=actionsStatus[i];
            actionParams.target = target;
            actionParams.owner = actionOwner;
            actionParams.targetStEs_preResolve = new List<PA_StatusEffect.StatusEffectStatus>();
            foreach(var stat in target.GetStEs()) { actionParams.targetStEs_preResolve.Add(stat.GetStatusEffectStatus()); }

           if(actionStatus.abilityEffect) target.BecomeAbilityTarget(actionStatus.actionOwner);

            if (actionStatus.VE_OnTargets) { target.SpawnVisualEffect(actionStatus.VE_OnTargets); }
            if (actionStatus.VE_OnOwner) { actionOwner.SpawnVisualEffect(actionStatus.VE_OnOwner); }
            if (!targetStatus.dead)//対象が生きているときのみ、効果発動
            {

                if (actionStatus.sprite != null)//アクションのアイコンを表示
                {
                    if (!notChara)
                    {
                        actionStatus.actionOwner.GetTargetButton().SetActionIcon(actionStatus.sprite);
                    }
                }

              

                OnKillParams onKillParams = new OnKillParams();
                onKillParams.ap = actionParams;

                if (actionsStatus[i].kill)
                {
                    target.Kill(actionStatus.actionOwner);
                    result.kill = true;

                    onKillParams.obstacle = targetStatus.Obstacle();//resultの記述
                    onKillParams.target = target;
                    onKillParams.CRIT = false;
                    onKillParamsList.Add(onKillParams);

                    result.onKillParams = onKillParams;
                }

                if (actionsStatus[i].DoesDecreaseHP() && target.CheckAlive())//HP減少
                {
                    int decrease = Random.Range(actionsStatus[i].decreaseHP_min, actionsStatus[i].decreaseHP_max + 1);

                    float percent = Random.Range(actionsStatus[i].decreaseHPPer_min, actionsStatus[i].decreaseHPPer_max) / 100f;
                    decrease += Mathf.RoundToInt(targetStatus.maxHP * percent);

                    percent= Random.Range(actionsStatus[i].decreaseHP_ATK.x, actionsStatus[i].decreaseHP_ATK.y) / 100f;
                    decrease += (ownerStatus.ATK * percent).ToInt();

                    percent = Random.Range(actionsStatus[i].decreaseHP_INT.x, actionsStatus[i].decreaseHP_INT.y) / 100f;
                    decrease += (ownerStatus.INT * percent).ToInt();

                    int echoDMG = actionsStatus[i].echoDoT.Select(e => target.GetEchoDMG(e)).Sum();
                    if(!notChara) battleManager.GetPBR(actionOwner.GetRootChara()).decreaseHP += echoDMG;

                    target.DecreaseHP(decrease, echoDMG);
                }


                if (actionsStatus[i].DoesAttack() && target.CheckAlive())//攻撃
                {
                    OnAttackParams onAttackParams = new OnAttackParams();
                    onAttackParams.actionParams = actionParams;
                    onAttackParams.toralCRITC = ownerStatus.CRITC + actionsStatus[i].CRITCMod;
                    bool CRIT = false;
                    int ATKDMG = 0;
                    int INTDMG = 0;
                    int totalDMG = 0;


                    float ACC = ownerStatus.ACC + actionsStatus[i].ACCMod;
                    float EVD = Mathf.Clamp(targetStatus.EVD, 0, gp.maxEVD);
                    float dice = Random.value * 100f;

                    if (actionsStatus[i].sureHit || dice <= ACC)//ミス判定
                    {
                        if (actionsStatus[i].sureHit || actionsStatus[i].unevadable || dice <= ACC - EVD)//回避判定
                        {
                            

                            onAttackParams.hit = true;

                            float ATKDMGf = 0;
                            float INTDMGf = 0;
                            float ATKMod = Random.Range(actionsStatus[i].ATKMod_min, actionsStatus[i].ATKMod_max) / 100;
                            float INTMod = Random.Range(actionsStatus[i].INTMod_min, actionsStatus[i].INTMod_max) / 100;
                            if (actionsStatus[i].ATKMod_divide > 0) ATKMod += actionsStatus[i].ATKMod_divide / actionStatus.actionTargets.Count / 100;//分配ダメージ
                            if (actionsStatus[i].INTMod_divide > 0) INTMod += actionsStatus[i].INTMod_divide / actionStatus.actionTargets.Count / 100;
                            ATKDMGf += ownerStatus.ATK * ATKMod;
                            INTDMGf += ownerStatus.INT * INTMod;

                            ATKDMGf += (float)actionsStatus[i].ATKDMG_divide_int / actionStatus.actionTargets.Count;//追加分配ダメージ
                            INTDMGf += (float)actionsStatus[i].INTDMG_divide_int / actionStatus.actionTargets.Count;

                            float PROT = Mathf.Min(targetStatus.PROT, gp.maxPROT);//PROTの最大値を75に制限
                            float RDMG = Mathf.Max((PROT * -1 + 100f) / 100f, 0);//対象の被ダメージ上昇効果
                            ATKDMGf *= RDMG;
                            INTDMGf *= RDMG;

                            ATKDMGf += actionsStatus[i].exATKDMG_int;//与ダメージ上昇効果
                            ATKDMGf *= (100f + actionsStatus[i].exDMG_mul + ownerStatus.exDMG_mul) / 100f;

                            INTDMGf += actionsStatus[i].exINTDMG_int;//与ダメージ上昇効果
                            INTDMGf *= (100f + actionsStatus[i].exDMG_mul + ownerStatus.exDMG_mul) / 100f;

                            if ((ownerStatus.CRITC + actionsStatus[i].CRITCMod).Dice())//クリティカル判定
                            {
                                shakeCamera = true;
                                CRIT = true;
                                onAttackParams.CRIT = true;
                                ATKDMGf *= (100 + ownerStatus.CRITD + actionsStatus[i].CRITDMod) / 100f;
                                INTDMGf *= (100 + ownerStatus.CRITD + actionsStatus[i].CRITDMod) / 100f;
                                target.SpawnVisualEffect(Definer.VERef.CRIT);
                            }

                            ATKDMG = Mathf.Max(0, Mathf.RoundToInt(ATKDMGf));
                            INTDMG = Mathf.Max(0, Mathf.RoundToInt(INTDMGf));

                            ATKDMG += actionsStatus[i].trueATKDMG;
                            INTDMG += actionsStatus[i].trueINTDMG;

                            int shield = targetStatus.shield;
                            //int shieldDMG = Mathf.Min(ATKDMG, targetStatus.shield);
                            int shieldDMG = 0;

                            if (!actionsStatus[i].ignoreShield)
                            {
                                while (shield > 0)//シールドによるダメージ減少
                                {
                                    if (ATKDMG > 0)
                                    {
                                        ATKDMG--;
                                        shield--;
                                        shieldDMG++;
                                        if (shield == 0) { break; }
                                    }
                                    if (INTDMG > 0)
                                    {
                                        INTDMG--;
                                        shield--;
                                        shieldDMG++;
                                        if (shield == 0) { break; }
                                    }

                                    if (ATKDMG == 0 && INTDMG == 0) { break; }
                                }
                            }

                            totalDMG = ATKDMG + INTDMG;

                            //ATKDMG -= shieldDMG;
                            OnDamageParams onDamageParams = new OnDamageParams();
                            onDamageParams.totalDMG = ATKDMG + INTDMG;
                            onDamageParams.ATK = ATKDMG > 0;
                            onDamageParams.INT = INTDMG > 0;
                            onDamageParams.ATKDMG = ATKDMG;
                            onDamageParams.INTDMG = INTDMG;
                            onDamageParams.shieldDMG = shieldDMG;
                            onDamageParams.CRIT = CRIT;
                            onDamageParams.ap = actionParams;

                            result.damage = true;
                            result.onDamageParams = onDamageParams;
                            onAttackParamsList.Add(onAttackParams);
                            target.OnAttacked(onAttackParams);//被攻撃時誘発

                            bool kill = target.Damage(onDamageParams);

                            if (kill)//ダメージ処理開始
                            {//殺害したなら
                                onKillParams.obstacle = targetStatus.Obstacle();
                                onKillParams.target = target;
                                onKillParams.CRIT = CRIT;
                                onKillParamsList.Add(onKillParams);

                                result.kill = true;
                                result.onKillParams = onKillParams;
                            }

                            if (actionsStatus[i].consumeFocus && actionParams.targetStEs_preResolve.Any(p => p.StEType == PA_StatusEffect.StatusEffectStatus.StatusEffectType.focus))//フォーカスの消費
                            {
                                target.ConsumeFocus();
                                OnFocusParams onFocusParams = new OnFocusParams();
                                onFocusParams.actionParams = actionParams;
                                onFocusParams.kill = kill;
                                onFocusParamsList.Add(onFocusParams);
                            }

                            if (!notChara)
                            {
                                if (actionStatus.actionOwner.CharaStatus().position.IsPlayerPos() && !targetStatus.position.IsPlayerPos())
                                {
                                    totalDamage += totalDMG;
                                }
                                battleManager.GetPBR(actionOwner.GetRootChara()).ATKDMG += ATKDMG;
                                battleManager.GetPBR(actionOwner.GetRootChara()).INTDMG += INTDMG;

                                battleManager.GetPBR(target.GetRootChara()).RDMG += totalDMG;
                                battleManager.GetPBR(target.GetRootChara()).RShieldDMG += shieldDMG;

                                onDamageParamsList.Add(onDamageParams);
                                if (totalDMG > 0 && actionsStatus[i].drain > 0&&!notChara)//吸血処理
                                {
                                    OnHealParams onHealParams = new OnHealParams();
                                    float drainf = totalDMG * actionsStatus[i].drain / 100f;

                                    //drainf *= (100f + actionsStatus[i].exHeal_mul) / 100;
                                    drainf *=(100f+ ownerStatus.GHeal) / 100;
                                    drainf *= (100f+ownerStatus.RHeal) / 100;

                                    int drain = Mathf.Max(0, drainf.ToInt());

                                    if (drain > ownerStatus.maxHP - ownerStatus.HP)
                                    {
                                        onHealParams.overheal = drain - (ownerStatus.maxHP - ownerStatus.HP);
                                        drain = ownerStatus.maxHP - ownerStatus.HP;
                                    }

                                    onHealParams.target = actionStatus.actionOwner;
                                    onHealParams.healValue = drain;

                                    actionStatus.actionOwner.Heal(drain, actionStatus.actionOwner);
                                    actionStatus.actionOwner.OnHealed(actionStatus.actionOwner, onHealParams);
                                    battleManager.GetPBR(actionOwner.GetRootChara()).GHeal += drain;
                                    onHealParamsList.Add(onHealParams);
                                }
                            }
                        }
                        else//回避
                        {
                            target.GetTargetButton().SetDamageText($"{"EVD".ToSpr(true)}Evade", Definer.colorRef.evade);
                            infoText.AddLogText(string.Format("{0}は攻撃を回避した", targetStatus.charaName).ColorStr(Definer.colorRef.evade));
                            soundManager.PlaySE(Definer.soundRef.evade);
                            attackHit = false;
                            if (!notChara)
                            {
                                onAttackParams.evaded = true;
                            }
                            onAttackParamsList.Add(onAttackParams);
                            target.OnAttacked(onAttackParams);//被攻撃時誘発

                        }
                    }
                    else//ミス
                    {
                        if (!notChara)
                        {
                            target.GetTargetButton().SetDamageText("Miss", Definer.colorRef.failed_unavailable);
                            infoText.AddLogText(string.Format("{0}は攻撃を外した", ownerStatus.charaName).ColorStr(Definer.colorRef.failed_unavailable));
                            onAttackParams.missed = true;
                        }
                        onAttackParamsList.Add(onAttackParams);
                        target.OnAttacked(onAttackParams);//被攻撃時誘発
                        soundManager.PlaySE(Definer.soundRef.miss);
                        attackHit = false;
                    }

                    
                }

                if (attackHit && target.CheckAlive())
                {
                    if (!attackHit)//攻撃時のフォーカス消費は上で
                    {
                        if (actionsStatus[i].consumeFocus && actionParams.targetStEs_preResolve.Any(p => p.StEType == PA_StatusEffect.StatusEffectStatus.StatusEffectType.focus))//フォーカスの消費
                        {
                            target.ConsumeFocus();
                            OnFocusParams onFocusParams = new OnFocusParams();
                            onFocusParams.actionParams = actionParams;
                            onFocusParamsList.Add(onFocusParams);
                        }
                    }

                    if (actionsStatus[i].DoesHeal())//回復
                    {
                        OnHealParams onHealParams = new OnHealParams();
                        onHealParams.ability = actionStatus.abilityEffect;
                        onHealParams.target = target;
                        float fheal;
                        fheal = Random.Range(actionsStatus[i].healValue_min, actionsStatus[i].healValue_max + 1);
                        fheal += ownerStatus.INT * actionsStatus[i].healINT.Range() / 100f;
                        fheal += targetStatus.maxHP * Random.Range(actionsStatus[i].healPercent_min, actionsStatus[i].healPercent_max) / 100;
                        if (actionsStatus[i].healRegain_max > 0)
                        {
                            int decreasedHP = targetStatus.maxHP - targetStatus.HP;
                            fheal += decreasedHP * Random.Range(actionsStatus[i].healRegain_min, actionsStatus[i].healRegain_max) / 100f;
                        }
                        fheal *= (100f + actionsStatus[i].exHeal_mul) / 100;
                        fheal *= (100f + ownerStatus.GHeal) / 100;
                        fheal *= (100f + targetStatus.RHeal) / 100;
                        int heal = Mathf.Max(0, fheal.ToInt());
                        heal += actionsStatus[i].trueHeal;

                        if (heal > targetStatus.maxHP - targetStatus.HP)
                        {
                            onHealParams.overheal = heal - (targetStatus.maxHP - targetStatus.HP);
                            heal = targetStatus.maxHP - targetStatus.HP;
                        }
                        onHealParams.healValue = heal;

                        if (!notChara) battleManager.GetPBR(actionOwner.GetRootChara()).GHeal += heal;
                        target.Heal(heal, actionStatus.actionOwner);
                        target.OnHealed(actionStatus.actionOwner, onHealParams);
                        onHealParamsList.Add(onHealParams);
                    }

                    if (actionsStatus[i].SANHeal_max > 0)//SAN
                    {
                        target.SANHeal(Random.Range(actionsStatus[i].SANHeal_min, actionsStatus[i].SANHeal_max + 1));
                    }
                    if (actionsStatus[i].SANDamage_max > 0)
                    {
                        target.SANDamage(Random.Range(actionsStatus[i].SANDamage_min, actionsStatus[i].SANDamage_max + 1));
                    }


                    if (actionsStatus[i].shieldAdd_max > 0)//シールド
                    {
                        int shield = Random.Range(actionsStatus[i].shieldAdd_min, actionsStatus[i].shieldAdd_max + 1);
                        if (!notChara) battleManager.GetPBR(actionOwner.GetRootChara()).GShield += shield;
                        target.AddShield(shield);
                    }
                    if (actionsStatus[i].shieldPercent_max > 0)//割合シールド
                    {
                        int percent = Random.Range(actionsStatus[i].shieldPercent_min, actionsStatus[i].shieldPercent_max + 1);
                        int shield = Mathf.RoundToInt(targetStatus.maxHP * percent * 0.01f);
                        if (!notChara) battleManager.GetPBR(actionOwner.GetRootChara()).GShield += shield;
                        target.AddShield(shield);
                    }
                    if (actionsStatus[i].shieldRemove_all)//シールド全消去
                    {
                        target.RemoveShield(true, 0);
                    }
                    else if (actionsStatus[i].shieldRemove_max > 0)//シールド減少
                    {
                        target.RemoveShield(false, Random.Range(actionsStatus[i].shieldRemove_min, actionsStatus[i].shieldRemove_max + 1));
                    }


                    List<ActionData.RemoveStE> removeStEs = new List<ActionData.RemoveStE>(actionsStatus[i].removeStEs);
                    if (actionsStatus[i].removeStEs_additional.Count > 0) { removeStEs.AddRange(actionsStatus[i].removeStEs_additional); }
                    actionsStatus[i].removeStEs_additional.Clear();

                    if (actionsStatus[i].removeStE_buff > 0)
                    {
                        target.RemoveStE_ByType(PA_StatusEffect.StatusEffectStatus.StatusEffectType.buff, actionsStatus[i].removeStE_buff);
                    }
                    if (actionsStatus[i].removeStE_debuff > 0)
                    {
                        target.RemoveStE_ByType(PA_StatusEffect.StatusEffectStatus.StatusEffectType.debuff, actionsStatus[i].removeStE_debuff);
                    }
                    //if (actionsStatus[i].removeStE_DoT > 0)
                    //{
                    //    target.RemoveStE_ByType(PA_StatusEffect.StatusEffectStatus.StatusEffectType.DoT, actionsStatus[i].removeStE_DoT);
                    //}
                    foreach (ActionData.RemoveStE remove in removeStEs)//StE消去
                    {
                        target.RemoveStE(remove);
                    }

                    OnApplyStEParams onApplyStEParams = new OnApplyStEParams();
                    onApplyStEParams.attemptedParams = new List<PA_StatusEffect.StatusEffectParams>(actionsStatus[i].applySteParams);
                    onApplyStEParams.appliedParams = new List<PA_StatusEffect.StatusEffectParams>();
                    onApplyStEParams.resistedParams = new List<PA_StatusEffect.StatusEffectParams>();
                    onApplyStEParams.taget = target;


                    //========================================================================================[StE付与]==============================================================================

                    List<PA_StatusEffect.StatusEffectStatus.StatusEffectType> appliedType = new List<PA_StatusEffect.StatusEffectStatus.StatusEffectType>();
                    foreach (PA_StatusEffect.StatusEffectParams P in actionsStatus[i].applySteParams)//StE付与
                    {
                        PA_StatusEffect.StatusEffectParams StEParams = P;
                        PA_StatusEffect.StatusEffectStatus StEStaus = StEParams.GetStatusEffectStatus();


                        StEApplyBonus applyBonus = new StEApplyBonus(StEParams.applyStE);//bonusの設定
                        if (ownerStatus.CheckHasStEApplyBonus(StEParams.applyStE))//オーナーのボーナス
                        {
                            //ownerStatus.GetStEApplyBonus(StEParams.applyStE).Log("ownerBonus");
                            applyBonus.AddBonus(ownerStatus.GetStEApplyBonus(StEParams.applyStE));
                        }
                        foreach (StEApplyBonus bonus in actionsStatus[i].StEApplyBonus)//アクションのボーナス
                        {
                            if (bonus.applyStE == StEParams.applyStE)
                            {
                                //bonus.Log("action bonus");
                                applyBonus.AddBonus(bonus);
                            }
                        }

                        //applyBonus.Log("final bonus");
                        if (!StEParams.dontApplyBonus)
                        {
                            StEParams.applyChance += applyBonus.exChance;//bonus等をparamsに反映
                            StEParams.stack += applyBonus.exStack;
                            StEParams.value += applyBonus.exValue;
                            StEParams.DMGPerTurn += applyBonus.exDMGPerTurn;
                        }

                        if (StEParams.stack > 0)//付与スタック数が1以上なら付与処理
                        {
                            if (StEStaus.StEType == PA_StatusEffect.StatusEffectStatus.StatusEffectType.debuff)
                            {
                                StEParams.applyChance += actionsStatus[i].debuffChanceMod;
                                StEParams.applyChance += ownerStatus.debuffChance;

                                StEParams.stack += ownerStatus.debuffStacks;
                            }

                            if (StEStaus.StEType == PA_StatusEffect.StatusEffectStatus.StatusEffectType.buff)
                            {
                                StEParams.stack += ownerStatus.buffStacks;
                            }

                            if (StEStaus.DoT)
                            {
                                int baseDMG = (StEParams.refATK) ? ownerStatus.ATK : (StEParams.refTargetMaxHP) ? targetStatus.maxHP : ownerStatus.INT;
                                StEParams.DMGPerTurn += (baseDMG * StEParams.value / 100f).ToInt();
                            }
                            if (StEStaus.regen)
                            {
                                int baseValue = (StEParams.refATK) ? ownerStatus.ATK : (StEParams.refTargetMaxHP) ? targetStatus.maxHP : ownerStatus.INT;
                                StEParams.DMGPerTurn += (baseValue * StEParams.value / 100f).ToInt();
                            }

                            if (StEParams.guaranteed || (StEParams.applyChance - targetStatus.GetStERes(StEParams)).Dice())//実際に付与するかの抽選====================================
                            {
                                if (!appliedType.Contains(StEStaus.StEType)) { appliedType.Add(StEStaus.StEType); }
                                onApplyStEParams.appliedParams.Add(StEParams);

                               target.ApplyStE(StEParams, StEParams.stack, StEParams.value, actionOwner);
                            }
                            else
                            {
                                onApplyStEParams.resistedParams.Add(StEParams);
                                target.GetTargetButton().SetDamageText("Resist", Definer.colorRef.failed_unavailable);
                                infoText.AddLogText(string.Format("{0}が{1}をレジスト", targetStatus.charaName, StEParams.applyStE.GetComponent<PA_StatusEffect>().GetPAName()));
                            }
                        }

                    }
                    foreach (PA_StatusEffect.StatusEffectStatus.StatusEffectType type in appliedType)//StEのエフェクト
                    {
                        if (Definer.VERef.applyStE[(int)type])
                        {
                            target.SpawnVisualEffect(Definer.VERef.applyStE[(int)type]);
                        }
                    }
                    if (onApplyStEParams.attemptedParams.Count > 0)
                    {
                        onApplyStEParamsList.Add(onApplyStEParams);
                        target.OnApplyedStE(onApplyStEParams);//被攻撃時誘発
                    }

               
                    if (actionsStatus[i].moveBackword > 0 || actionsStatus[i].moveUpper > 0 || actionsStatus[i].moveForword > 0 || actionsStatus[i].moveLower > 0)//移動
                    {
                        if ((actionsStatus[i].guaranteedMove || (actionsStatus[i].moveChance - targetStatus.moveRes).Dice()) && !targetStatus.immovable)
                        {
                            //string test = "";
                            int moveRange = -1;
                            int moveDir = -1;
                            int moveToPos;
                            List<int> movableRanges = targetStatus.position.GetMovableRanges();
                            if (actionsStatus[i].moveRandomDir > 0)
                            {
                                List<int> movePool = new List<int>();
                                if (actionsStatus[i].moveBackword > 0)
                                {
                                    if (targetStatus.position < 9 && movableRanges[3] > 0) { movePool.Add(3); }
                                    else if (targetStatus.position >= 9 && movableRanges[0] > 0) { movePool.Add(0); }
                                }
                                if (actionsStatus[i].moveUpper > 0 && movableRanges[1] > 0) { movePool.Add(1); }
                                if (actionsStatus[i].moveForword > 0)
                                {
                                    if (targetStatus.position < 9 && movableRanges[0] > 0) { movePool.Add(0); }
                                    else if (targetStatus.position >= 9 && movableRanges[3] > 0) { movePool.Add(3); }
                                }
                                if (actionsStatus[i].moveLower > 0 && movableRanges[2] > 0) { movePool.Add(2); }

                                moveDir = movePool.Choice();
                                moveRange = actionsStatus[i].moveRandomDir;
                            }
                            else
                            {
                                if (actionsStatus[i].moveBackword > 0)
                                {
                                    moveRange = actionsStatus[i].moveBackword;
                                    if (targetStatus.position < 9) { moveDir = 3; }
                                    else { moveDir = 0; }
                                }
                                else if (actionsStatus[i].moveUpper > 0)
                                {
                                    moveRange = actionsStatus[i].moveUpper;
                                    moveDir = 1;
                                }
                                else if (actionsStatus[i].moveForword > 0)
                                {
                                    moveRange = actionsStatus[i].moveForword;
                                    if (targetStatus.position < 9) { moveDir = 0; }
                                    else { moveDir = 3; }
                                }
                                else if (actionsStatus[i].moveLower > 0)
                                {
                                    moveRange = actionsStatus[i].moveLower;
                                    moveDir = 2;
                                }
                            }
                            
                            //test += string.Format("移動方向:{0} 移動予定距離:{1} 移動可能距離:{2} ", moveDir, moveRange, movableRanges[moveDir]);

                            moveRange = Mathf.Min(moveRange, movableRanges[moveDir]);

                            onMoveParams.target = target;
                            onMoveParams.dir = moveDir;
                            onMoveParams.range = moveRange;


                            moveToPos = targetStatus.position.GetMoveToPos(moveDir, moveRange);

                            onMoveParams.prevPos = targetStatus.position;
                            onMoveParams.currentPos = moveToPos;
                            //test += string.Format("実際の移動距離:{0} 移動後のpos:{1}", moveRange, moveToPos);
                            //infoText.AddDebugText(test);
                            if (moveRange > 0)
                            {
                                bool movable = true;
                                List<Character> charasOnTravelingDir = new List<Character>(FindObjectOfType<CharactersManager>().GetTravelingDirCharas(targetStatus.position, moveDir, moveRange));
                                foreach (Character c in charasOnTravelingDir)
                                {
                                    if (c.CharaStatus().immovable)
                                    {
                                        movable = false;
                                    }
                                }

                                if (movable)
                                {
                                    target.GetTargetButton().ResetCharacter();//ターゲットボタンの参照の解除
                                    foreach (Character c in charasOnTravelingDir)
                                    {
                                        c.GetTargetButton().ResetCharacter();
                                    }

                                    target.ChangePos(moveToPos);//移動処理
                                    target.OnMoved(onMoveParams);

                                    foreach (Character c in charasOnTravelingDir)
                                    {
                                        int swapToPos = util.GetMoveToPos(c.CharaStatus().position, 3 - moveDir, 1);
                                        OnMoveParams onSwapParams = new OnMoveParams();
                                        onSwapParams.target = c;
                                        onSwapParams.dir = 3 - moveDir;
                                        onSwapParams.range = 1;
                                        onSwapParams.prevPos = c.CharaStatus().position;
                                        onSwapParams.currentPos = swapToPos;
                                        onSwapParams.secondaryMove = true;
                                        c.ChangePos(swapToPos);
                                        c.OnMoved(onSwapParams);
                                    }
                                }
                                else
                                {
                                    infoText.AddLogText(string.Format("{0}の移動は阻まれた", ownerStatus.charaName));
                                }
                            }
                        }
                        else { target.GetTargetButton().SetDamageText("MoveResist", Definer.colorRef.failed_unavailable); }
                    }


                    foreach (ActionData.AbilityRemainControll remainControll in actionsStatus[i].abilityRemainControlls)//アビリティの使用回数
                    {
                        target.AbilityRemain(remainControll);
                    }

                    if (actionsStatus[i].exTurn > 0)//追加ターン
                    {
                        target.AddTurn(actionsStatus[i].exTurn);

                    }

                }
                //else
                //{
                //    actionStatus.actionTargetsInt.Remove(i);//攻撃失敗時、その地点に対する処理も行わないようにする
                //}
            }
            else if (!notChara)
            {
                actionStatus.actionOwner.GetTargetButton().SetDamageText("対象消失", Definer.colorRef.failed_unavailable);
            }

            actionResults.Add(result);
        }


        //各対象ポジションへの処理
        if (actionStatus.summon)//召喚
        {
            ActionResult result = new ActionResult();//応急処置！！！
            result.actionStatus = actionStatus;
            actionResults.Add(result);

            soundManager.PlaySE(Definer.soundRef.summoned);
            for (int i = 0; i < actionStatus.actionTargetsInt.Count; i++)
            {
                ActionParams actionParams = new ActionParams();
                actionParams.actionStatus = actionsStatus[i];
                //actionParams.target = target;
                actionParams.owner = actionOwner;

                int targetPos = actionStatus.actionTargetsInt[i];
                if (!characterManager.CheckCharaExist(targetPos))
                {
                    CharacterData summonCharaData = actionsStatus[i].summonChara[actionsStatus[i].summonChanceWeight.ChoiceWithWeight()];
                    Character.SummonCharaStatusParams summonStatusParams = new Character.SummonCharaStatusParams();
                    OnSummonParams onSummonParams = new OnSummonParams();
                    Character summoned;

                    onSummonParams.actionParams = actionParams;

                    if (!notChara) { summonStatusParams.summoner = actionOwner; }
                    summonStatusParams.statusMods = new List<Character.CharaStatusMod>(actionsStatus[i].summonStatusMods);
                    summonStatusParams.statusMods.Add(actionsStatus[i].summonStatusInherit.ToStatuMod(ownerStatus));

                    if (actionStatus.actionTargetsInt[i] < 9) { summoned = characterManager.SpawnPlayer(summonCharaData, targetPos, ownerStatus.level, summonStatusParams); }
                    else { summoned = characterManager.SpawnEnemy(summonCharaData, targetPos, false, ownerStatus.level, summonStatusParams); }
                    summoned.OnSummoned(onSummonParams);
                    if (BattleManager.inRound) { summoned.AddTurn(summoned.CharaStatus().turnPerRound); }

                    onSummonParams.summoned = summoned;

                    onSummonParamsList.Add(onSummonParams);
                }
                else { infoText.AddDebugText("召喚能力の打消し"); }
            }
        }
        else//召喚以外の対象地点への効果
        {
            ActionResult result = new ActionResult();//応急処置！！！
            result.actionStatus = actionStatus;
            actionResults.Add(result);


            for (int i = 0; i < actionStatus.actionTargetsInt.Count; i++)
            {
                foreach (ActionData.RemovePE remove in actionStatus.removePEs)//PE除去
                {
                    characterManager.GetPositionManager(actionStatus.actionTargetsInt[i]).RemovePE(remove);
                }

                foreach (PositionEffect.PositionEffectParams PEParams in actionStatus.applyPEParams)//PE付与
                {
                    if (PEParams.guaranteed || PEParams.applyChance.Dice())
                    {
                        infoText.AddLogText(string.Format("ポジション{0}に{1}が付与", actionStatus.actionTargetsInt[i].PosIntToStr(), PEParams.applyPE.GetComponent<PositionEffect>().GetPEName(true)));
                        characterManager.GetPositionManager(actionStatus.actionTargetsInt[i]).ApplyPE(PEParams, ownerStatus);
                    }
                }
            }
        }

        if (actionStatus.targetType == ActionStatus.TargetType.move)//移動
        {
            ActionResult result = new ActionResult();//応急処置！！！
            result.actionStatus = actionStatus;
            actionResults.Add(result);


            int ownerMoveDir = -1;
            int ownerMoveRange = -1;
            int moveToPos = actionStatus.actionTargetsInt[0];//iは0の時しかないはず
            bool movable = true;
            ownerMoveDir = util.GetMoveDir(ownerStatus.position, moveToPos);
            if (ownerMoveDir == 0 || ownerMoveDir == 3)//左右移動なら
            {
                ownerMoveRange = Mathf.Abs(ownerStatus.position.PosIntToVector().x - moveToPos.PosIntToVector().x);
            }
            else if (ownerMoveDir == 1 || ownerMoveDir == 2)//上下移動なら
            {
                ownerMoveRange = Mathf.Abs(ownerStatus.position.PosIntToVector().y - moveToPos.PosIntToVector().y);
            }

            List<Character> charasOnTravelingDir = new List<Character>(FindObjectOfType<CharactersManager>().GetTravelingDirCharas(ownerStatus.position, ownerMoveDir, ownerMoveRange));
            foreach (Character c in charasOnTravelingDir)
            {
                if (c.CharaStatus().immovable)
                {
                    movable = false;
                }
            }

            if (movable)
            {
                actionStatus.actionOwner.GetTargetButton().ResetCharacter();//ターゲットボタンの解除
                foreach (Character c in charasOnTravelingDir)
                {
                    c.GetTargetButton().ResetCharacter();
                }

                onMoveParams.prevPos = ownerStatus.position;
                onMoveParams.target = actionStatus.actionOwner;
                onMoveParams.dir = ownerMoveDir;
                onMoveParams.range = ownerMoveRange;
                onMoveParams.currentPos = moveToPos;

                actionStatus.actionOwner.ChangePos(moveToPos);//移動処理
                actionStatus.actionOwner.OnMoved(onMoveParams);

                foreach (Character c in charasOnTravelingDir)
                {
                    int swapToPos= util.GetMoveToPos(c.CharaStatus().position, 3 - ownerMoveDir, 1);
                    OnMoveParams onSwapParams = new OnMoveParams();
                    onSwapParams.dir = 3 - ownerMoveDir;
                    onSwapParams.range = 1;
                    onSwapParams.prevPos = c.CharaStatus().position;
                    onSwapParams.currentPos = swapToPos;
                    onSwapParams.secondaryMove = true;
                    c.ChangePos(swapToPos);
                    c.OnMoved(onSwapParams);
                }

                
            }
            else
            {
                infoText.AddLogText(string.Format("{0}の移動は阻まれた", ownerStatus.charaName));
            }

        }
        EndResolve();
    }


    void EndResolve()
    {
        if (totalDamage > 0) { battleManager.SetTotalDamageText(totalDamage); }
        if (actionStatus.actionOwner != null)
        {
            if (onAttackParamsList.Count > 0) { actionStatus.actionOwner.OnAttack(onAttackParamsList); }//攻撃時誘発
            if (onDamageParamsList.Count > 0) { actionStatus.actionOwner.OnDamage(onDamageParamsList); }
            if (onFocusParamsList.Count > 0) { actionOwner.OnFocus(onFocusParamsList); }
            if (onKillParamsList.Count > 0) { actionStatus.actionOwner.Onkill(onKillParamsList); }//殺害時誘発
            if (onApplyStEParamsList.Count > 0)//StE付与時誘発
            {
                actionStatus.actionOwner.OnApplyStE(onApplyStEParamsList);
                battleManager.Trigger_OnSomeoneApplyedStE(onApplyStEParamsList);
            }
            if (onHealParamsList.Count > 0) { actionStatus.actionOwner.OnHeal(onHealParamsList); }//与回復時誘発
            if (onSummonParamsList.Count > 0) //召喚時誘発
            {
                actionStatus.actionOwner.OnSummon(onSummonParamsList);
                battleManager.Trigger_OnSomeoneSummoned(actionStatus.actionOwner, onSummonParamsList);
            }
        }

        if (actionStatus.index == 0 && actionStatus.abilityEffect && actionStatus.abilityType != AbilityData.AbilityType.pass)
        { actionStatus.actionOwner.OnActivateAbility(actionResults); }

        if (shakeCamera) { cameraManager.ShakeCamera(1); }

        SecondEffect();

        actionQueueManager.Dequeue();
    }

    public virtual void SecondEffect()
    {

    }

    //=====================================================[以下SecondEffect関連]=================================================
    public void Enqueue(ActionStatus actionStatus, bool setTargets, List<Character> actionTargets, int targetCount = 0, bool nullOwner = false)
    {
        ActionStatus action = actionStatus;
        actionStatus.actionOwner.Enqueue(action, setTargets, actionTargets, targetCount, nullOwner);
    }

    /// <summary>自身を対象にEunqueue</summary>
    public void Enqueue_Self(ActionStatus act)
    {
        ActionStatus action = act;
        actionStatus.actionOwner.Enqueue(action, true, new List<Character>() { actionStatus.actionOwner }, 0);
    }

    public bool Enqueue_SearchTarget(Action.ActionStatus actionStatus, CharactersManager.SearchCharaCondition condition, int targetCount = 0)
    {
        Action.ActionStatus action = actionStatus;
        List<Character> target = new List<Character>();
        List<int> targetPos = new List<int>();
        if (condition.searchAsPos)
        {
            targetPos = CharactersManager.inst.SearchPosWithCondition(condition);
            action.actionTargetsInt = targetPos;
        }
        else
        {
            target = CharactersManager.inst.SearchCharaWithCondition(condition, actionStatus.actionOwner);
            action.actionTargets = target;
        }

        if (target.Count > 0 || targetPos.Count > 0)
        {
            Enqueue(action, false, target, targetCount);
            return true;
        }
        return false;
    }

    public bool CheckIfAbilityEffect() { return actionStatus.abilityEffect; }

    public virtual string GetAdditionalInfo() { return ""; }
}

[System.Serializable]
public class EchoDoTParams
{
    public List<GameObject> targetStE;
    public bool echoAllStack;
    public bool consumeStack;
    public float ratio = 100;

    public Action.ActionParams ap;

    public string GetInfo()
    {
        string s = "";
        if (targetStE.Count > 0)
        {
            string names = "";
            targetStE.ForEach(t => names += $"{Extentions.NL(names, lineStr: ",")}{"debuff".ToSpr_withName(t.GetComponent<PA_StatusEffect>().GetPAName())}");
            s += $"{names}を";
        }
        s += $"{$"反響 <i>{{{ratio}}}</i>".ToLinkKey("反響")}".ColorStr(Definer.colorRef.echo);

        return s;
    }
}