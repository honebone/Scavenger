using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

        [Header("設定しなければ汎用的なオブジェクトになる")]
        public GameObject actionObject;
        public List<GameObject> actionMods;
        //public bool targetEmpty;

        /// <summary>
        /// row:段 column:列
        /// </summary>
        public enum TargetType { other, single, all, self, row, column, singleWoSelf, allWoSelf, random, move }
        [System.Serializable]
        public class ActionTargetParams
        {
            public TargetType targetType;
            public bool friendly;
            public CharactersManager.SearchCharaCondition condition;

            public bool ignoreMark;
            public bool ignoreHide;
            [Header("0:right 1:upper 2:lower 3:left(targetypeがmoveのときに使用)")]
            public List<int> moveValue;
        }
        [Header("\n\n\nここからアビリティのみ関係")]
        public TargetType targetType;
        public bool friendly;
        public CharactersManager.SearchCharaCondition condition;

        public bool ignoreMark;
        public bool ignoreHide;
        //public ActionTargetParams targetParams;
        [Header("0:right 1:upper 2:lower 3:left(targetypeがmoveのときに使用)")]
        public List<int> moveValue;
        [Header("ここまでアビリティのみ関係\n\n\n")]
        public bool consumeFocus;
        public bool kill;
        public int decreaseHP_min;
        public int decreaseHP_max;
        public float decreaseHPPer_min;
        public float decreaseHPPer_max;

        [Header("\n\n攻撃")]
        [TextArea(3, 10)] public string attackInfo;
        public float ATKMod_min;
        public float ATKMod_max;
        public float INTMod_min;
        public float INTMod_max;

        public int trueATKDMG;
        public int trueINTDMG;

        public float exDMG_mul;
        public int exATKDMG_int;
        public int exINTDMG_int;

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
        public float healPercent_min;
        public float healPercent_max;
        [Header("減少体力の割合回復")]
        public float healRegain_min;
        public float healRegain_max;

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

        [Header("\n\n召喚")]
        public bool summon;
        //public int summonSize;
        public List<CharacterData> summonChara;
        public List<float> summonChanceWeight;

        [Header("\n\n移動")]
        public float moveChance;
        public bool guaranteedMove;
        public int moveForword;
        public int moveUpper;
        public int moveLower;
        public int moveBackword;

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

        public bool dontChangeSprite;
        [Header("スプライトの直接指定")]
        public GameObject activateSprite;

        public Character actionOwner;
        //[System.NonSerialized]
        //public Character.CharacterStatus ownerStatus_notChara;
        public int targetCount;
        /// <summary>この中からtargetCount個だけランダムに選ばれる</summary>
        public List<Character> actionTargets;
        /// <summary>移動や召喚の際に使用 移動の際は移動先のposが入る</summary>
        public List<int> actionTargetsInt;

        public bool DoesAttack() { return ATKMod_max > 0 || INTMod_max > 0 || trueATKDMG > 0 || trueINTDMG > 0; }
        public bool DoesHeal() { return healPercent_max > 0 || healValue_max > 0 || healRegain_max > 0; }

        public string GetInfo(bool refCharaStatus, Character.CharacterStatus characterStatus)
        {
            string s = "";
            bool nb = false;
            bool f = false;

            //if (friendly) { s += "友好アクション\n"; }
            if (conditionInfo != "") { s += string.Format("{0}：\n", conditionInfo); }
            s += string.Format("対象：{0}\n", targetInfo);
            if (kill) { s += "・殺害する\n"; }
            if (decreaseHP_max > 0)
            {
                s += string.Format("・HPが{0}減少\n", GetValueRange(decreaseHP_min, decreaseHP_max));
            }
            if (decreaseHPPer_max > 0)
            {
                s += string.Format("・HPが{0}％減少\n", GetValueRange(decreaseHPPer_min, decreaseHPPer_max));
            }

            if (DoesAttack()||attackInfo!="")//攻撃
            {
                s += (attackInfo != "") ? $"・{attackInfo}\n" : "";
                if (ATKMod_max > 0)
                {
                    s += $"・{"物理".ColorStr(Definer.colorRef.damage)}攻撃を行う\n";
                    s += string.Format("ATKの{0}％ダメージ", GetValueRange(ATKMod_min, ATKMod_max));
                    if (refCharaStatus)
                    {
                        s += string.Format("({0})", GetValueRange(Mathf.RoundToInt(characterStatus.ATK * ATKMod_min / 100), Mathf.RoundToInt(characterStatus.ATK * ATKMod_max / 100)));
                    }
                    s += "\n";
                }
                if (INTMod_max > 0)
                {
                    s += $"・{"魔法".ColorStr(Definer.colorRef.INTDamage)}攻撃を行う\n";
                    s += string.Format("INTの{0}％ダメージ", GetValueRange(INTMod_min, INTMod_max));
                    if (refCharaStatus)
                    {
                        s += string.Format("({0})", GetValueRange(Mathf.RoundToInt(characterStatus.INT * INTMod_min / 100), Mathf.RoundToInt(characterStatus.INT * INTMod_max / 100)));
                    }
                    s += "\n";
                }

                string attack = "";
                if (ACCMod != 0) { attack += string.Format("ACC補正：{0}\n", GetValueWithSign(ACCMod)); }
                if (CRITCMod != 0) { attack += string.Format("CRIT率補正：{0}％\n", GetValueWithSign(CRITCMod)); }
                if (CRITDMod != 0) { attack += string.Format("CRITダメージ補正：{0}％\n", GetValueWithSign(CRITDMod)); }
                if (drain > 0) { attack += string.Format("与ダメージの{0}％を回復\n", drain); }
                if (ignoreShield) { attack += "シールドを無視\n"; }
                if (sureHit) { attack += "必中\n"; }
                if (unevadable) { attack += "EVDを無視\n"; }
                s += attack.ColorStr(Color.gray);
            }
            CheckNewBlock();

            if (healValue_max > 0 || healPercent_max > 0 || healRegain_max > 0)//回復
            {
                if (healValue_max > 0) { s += string.Format("・HPを{0}回復\n", GetValueRange(healValue_min, healValue_max)); }
                if (healPercent_max > 0) { s += string.Format("・HPを最大値の{0}％回復\n", GetValueRange(healPercent_min, healPercent_max)); }
                if (healRegain_max > 0) { s += string.Format("・減少したHPの{0}％を回復\n", GetValueRange(healRegain_min, healRegain_max)); }
            }
            CheckNewBlock();

            if (SANHeal_max > 0) { s += string.Format("・正気度を{0}回復\n", GetValueRange(SANHeal_min, SANHeal_max)); }
            if (SANDamage_max > 0) { s += string.Format("・正気度が{0}減少\n", GetValueRange(SANDamage_min, SANDamage_max)); }
            if (shieldAdd_max > 0) { s += string.Format("・シールドを{0}付与\n", GetValueRange(shieldAdd_min, shieldAdd_max)); }
            if (shieldPercent_max > 0) { s += string.Format("・maxHPの{0}％に等しいシールドを付与\n", GetValueRange(shieldPercent_min, shieldPercent_max)); }
            if (shieldRemove_all) { s += "・シールドを0にする\n"; }
            else if (shieldRemove_max > 0) { s += string.Format("・シールドを{0}除去\n", GetValueRange(shieldRemove_min, shieldRemove_max)); }
            CheckNewBlock();

            f = false;
            foreach (PA_StatusEffect.StatusEffectParams StEParams in applySteParams)//StE付与
            {
                PA_StatusEffect.StatusEffectStatus status = StEParams.applyStE.GetComponent<PA_StatusEffect>().GetStatusEffectStatus();
                if (f) { s += "\n"; }
                f = true;

                string chanceText = StEParams.guaranteed ? "確定" : $"{StEParams.applyChance}％";
                s += $"・{status.ToLinkKey(false, StEParams.value)}を付与\n({chanceText},{StEParams.stack}スタック)\n";
            }

            CheckNewBlock();
            f = false;
            foreach (PositionEffect.PositionEffectParams PEParams in applyPEParams)//PE付与
            {
                PositionEffect.PositionEffectStatus status = PEParams.applyPE.GetComponent<PositionEffect>().GetPositionEffectStatus();
                if (f) { s += "\n"; }
                f = true;

                string chanceText = PEParams.guaranteed ? "確定" : $"{PEParams.applyChance}％";
                s += $"・対象の地点に{status.ToLinkKey(false, PEParams.value)}を付与\n({chanceText},{PEParams.stack}スタック)\n";
            }

            CheckNewBlock();
            if (removeStE_buff > 0) { s += string.Format("・{0}を{1}個消去\n"
                , "バフ効果".ColorStr(Definer.colorRef.statusEffectColors[(int)PA_StatusEffect.StatusEffectStatus.StatusEffectType.buff]), removeStE_buff); }
            if (removeStE_debuff > 0) { s += string.Format("・{0}を{1}個消去\n"
                , "デバフ効果".ColorStr(Definer.colorRef.statusEffectColors[(int)PA_StatusEffect.StatusEffectStatus.StatusEffectType.debuff]), removeStE_debuff); }
            foreach (ActionData.RemoveStE remove in removeStEs)
            {
                PA_StatusEffect.StatusEffectStatus status = remove.removeStE.GetComponent<PA_StatusEffect>().GetStatusEffectStatus();
                s += string.Format("・{0}", status.ToLinkKey());
                if (remove.removeAll) { s += "を全て除去\n"; }
                else { s += string.Format("のスタック{0}\n", GetValueWithSign(remove.addAmount)); }
            }
            CheckNewBlock();

            if (summon)
            {
                if (summonChara.Count == 1)
                {
                    s += string.Format("・{0}を召喚", summonChara[0].charaName);
                }
                else
                {
                    s += "・以下からランダムに召喚\n";
                    float p = 0;
                    foreach (int r in summonChanceWeight) { p += r; }
                    for (int i = 0; i < summonChanceWeight.Count; i++)
                    {
                        s += string.Format("{0}({1})\n", summonChara[i].charaName, (summonChanceWeight[i] / p).ToString("#0.0%"));
                    }
                }
            }
            CheckNewBlock();

            if (guaranteedMove || moveChance > 0)
            {
                if (guaranteedMove) { s += "・"; }
                else { s += string.Format("・{0}％の確率で", moveChance); }
                if (moveForword > 0) { s += string.Format("{0}前進\n", moveForword); }
                if (moveUpper > 0) { s += string.Format("{0}上昇\n", moveUpper); }
                if (moveLower > 0) { s += string.Format("{0}下降\n", moveLower); }
                if (moveBackword > 0) { s += string.Format("{0}後退\n", moveBackword); }
            }
            CheckNewBlock();

            foreach (ActionData.AbilityRemainControll remainControll in abilityRemainControlls)
            {
                s += string.Format("・<{0}>の使用回数を", remainControll.abilityData.abilityName.ColorStr(remainControll.abilityData.abilityType.ToColor()));
                if (remainControll.set) { s += string.Format("{0}にする\n", remainControll.value); }
                else
                {
                    s += string.Format("{0}増加\n", remainControll.value);
                }
            }
            CheckNewBlock();

            if (actionMods.Count > 0) { s += "\n"; }
            foreach (GameObject actionMod in actionMods)
            {
                string modInfo = actionMod.GetComponent<ActionMod>().GetActionModStatus().GetModInfo();
                if (modInfo != "") { s += modInfo + "\n"; }
            }
            if (AModInfo != "")
            {
                s += string.Format("\n○{0}", AModInfo);//.ColorStr(Definer.colorRef.AMod)
            }
            CheckNewBlock();

            if (actionInfo != "") { s +="\n"+ actionInfo + "\n"; }

            return s;

            void CheckNewBlock()
            {
                //s += nb ? "\n" : "";
                //nb=true;
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


        public ActionStatus Modify(ActionMod.ActionModStatus mod)
        {
            ActionStatus modifiedStatus = this;
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
            modifiedStatus.ACCMod += mod.ACCMod;
            modifiedStatus.CRITCMod += mod.CRITCMod;
            modifiedStatus.CRITDMod += mod.CRITDMod;
            modifiedStatus.drain += mod.drain;
            if (mod.sureHit) { modifiedStatus.sureHit = true; }
            if (mod.unevadable) { modifiedStatus.unevadable = true; }

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
            modifiedStatus.shieldRemove_min += mod.shieldRemove;
            modifiedStatus.shieldRemove_max += mod.shieldRemove;

            modifiedStatus.applySteParams = new List<PA_StatusEffect.StatusEffectParams>(modifiedStatus.applySteParams);
            foreach (PA_StatusEffect.StatusEffectParams statusEffectParams in mod.applySteParams)
            {
                modifiedStatus.applySteParams.Add(statusEffectParams);
            }
            foreach (StEApplyBonus bonus in mod.applyStEBonus)
            {
                bool f = false;
                for(int i=0; i< modifiedStatus.StEApplyBonus.Count;i++)
                {
                    if (modifiedStatus.StEApplyBonus[i].applyStE == bonus.applyStE)
                    {
                        modifiedStatus.StEApplyBonus[i]= modifiedStatus.StEApplyBonus[i].AddBonus(bonus, true);
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
            //move

            return modifiedStatus;
        }

        //public ActionStatus Copy()
        //{
        //    ActionStatus copy = this;
            
        //}
    }

    //===========================[誘発処理の引数に使う]================================
    public struct ActionResult
    {
        public Action.ActionStatus actionStatus;
        public Character target;

        public OnDamageParams onDamageParams;
    }
    public struct OnAttackParams
    {
        /// <summary> = !(missed||evaded)</summary>
        public bool hit;
        public bool missed;
        public bool evaded;
        public bool CRIT;
        public float toralCRITC;
        public Action.ActionStatus actionStatus;
        public Character target;
    }
    public struct OnDamageParams
    {
        public int totalDMG;
        public bool ATK;
        public bool INT;
        public int ATKDMG;
        public int INTDMG;
        public int shieldDMG;
        public bool CRIT;
        public Action.ActionStatus actionStatus;
        public Character owner;
        public Character target;
    }
    public struct OnKillParams
    {
        public bool obstacle;
        public bool CRIT;
        public Character target;
    }
    public struct OnHealParams
    {
        public int healValue;
        public Character target;
    }
    public struct OnApplyStEParams
    {
        /// <summary>appliedParams + resistedParams</summary>
        public List<PA_StatusEffect.StatusEffectParams> attemptedParams;
        public List<PA_StatusEffect.StatusEffectParams> appliedParams;
        public List<PA_StatusEffect.StatusEffectParams> resistedParams;
        public Character taget;
    }
    public struct OnMoveParams
    {
        public int prevPos;
        public int currentPos;
        public int dir;
        public int range;
        public bool secondaryMove;

        public Character target;
    }
    protected Character actionOwner;

    protected List<ActionResult> actionResults = new List<ActionResult>();
    List<OnAttackParams> onAttackParamsList = new List<OnAttackParams>();
    List<OnDamageParams> onDamageParamsList = new List<OnDamageParams>();   
    List<OnKillParams> onKillParamsList = new List<OnKillParams>();
    List<OnApplyStEParams> onApplyStEParamsList = new List<OnApplyStEParams>();
    List<OnHealParams> onHealParamsList = new List<OnHealParams>();
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
        Character.CharacterStatus ownerStatus = new Character.CharacterStatus();
        bool notChara = false;//フィールド効果やポジション効果などによるアクション
        if (actionStatus.actionOwner != null) {
            actionOwner = actionStatus.actionOwner;
            ownerStatus = actionStatus.actionOwner.GetCharacterStatus(); 
        }
        else
        {
            notChara = true;
            ownerStatus = Definer.nonCharaStatus;
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
        ActionStatus[] actionsStatus =new ActionStatus[actionStatus.actionTargets.Count];
        for (int i = 0; i < actionsStatus.Length; i++)
        {
            actionsStatus[i] = actionStatus;
            actionsStatus[i].StEApplyBonus = new List<StEApplyBonus>(actionStatus.StEApplyBonus);
        }
        if (actionStatus.SE != null) { soundManager.PlaySE(actionStatus.SE); }

        foreach(Character character in actionStatus.actionTargets)
        {
            int pos = character.GetCharacterStatus().position;
            if (!actionStatus.actionTargetsInt.Contains(pos)) { actionStatus.actionTargetsInt.Add(pos); }
        }
        if (!notChara) { actionStatus.actionOwner.ActAnim(); }


        if (actionStatus.freeAction)
        {
            if (!actionStatus.abilityEffect) { infoText.AddErrorText("アビリティじゃないのにフリーアクション"); }
            infoText.AddDebugText("FreeAction");
            actionStatus.actionOwner.ContinueTurn();
        }

        List<GameObject> actionModsObj = new List<GameObject>(actionStatus.actionMods);
        //ここで色々なactionMdsを追加
        if (!notChara) { 
            actionModsObj.AddRange(ownerStatus.actionMods);
            actionModsObj.AddRange(actionStatus.actionOwner.GetPositionManager().GetActionMods());
        }
        foreach(GameObject actionModObj in actionModsObj)
        {
            var am = Instantiate(actionModObj);
            am.GetComponent<ActionMod>().Init(characterManager);
            actionsStatus = am.GetComponent<ActionMod>().ModifyAction(actionStatus, actionsStatus);
            Destroy(am);
        }

        if (!notChara) { actionStatus.actionOwner.ModifyAction(actionStatus, actionsStatus, false); }

        //各対象キャラへの処理
        for (int i = 0; i < actionStatus.actionTargets.Count; i++)
        {
            Character target = actionStatus.actionTargets[i];
            Character.CharacterStatus targetStatus = target.GetCharacterStatus();
            bool attackHit = true;//攻撃失敗時、その他の効果も発動しないようにする
            ActionResult result = new ActionResult();
            result.target = target;
            result.actionStatus = actionsStatus[i];

            target.BecomeAbilityTarget(actionStatus.actionOwner);

            if (actionStatus.VE_OnTargets)
            {
                //Vector2 VEPos = characterManager.GetCharacterWorldPos(targetStatus.position);
                //Vector2 VEOffset = actionStatus.VE_OnTargets.GetComponent<VisualEffect>().GetOffset();
                //if (targetStatus.position < 9) { VEOffset.x *= -1; }
                //var v = Instantiate(actionStatus.VE_OnTargets, VEPos + VEOffset, actionStatus.VE_OnTargets.transform.rotation);
                //if (targetStatus.position < 9) { v.transform.Rotate(new Vector3(0, 180, 0)); }//プレイヤー対象の時左右反転
                target.SpawnVisualEffect(actionStatus.VE_OnTargets);
            }
            if (!targetStatus.dead)//対象が生きているときのみ、効果発動
            {

                if (actionStatus.sprite != null)//アクションのアイコンを表示
                {
                    if (!notChara)
                    {
                        actionStatus.actionOwner.GetTargetButton().SetActionIcon(actionStatus.sprite);
                    }
                }

                if (actionsStatus[i].consumeFocus)//フォーカスの消費
                {
                    target.ConsumeFocus();
                }


                if (actionsStatus[i].kill)
                {
                    target.Kill(actionStatus.actionOwner);
                }

                if (actionsStatus[i].decreaseHP_max > 0 && target.CheckAlive())//HP減少
                {
                    target.DecreaseHP(Random.Range(actionsStatus[i].decreaseHP_min, actionsStatus[i].decreaseHP_max + 1));
                }
                if (actionsStatus[i].decreaseHPPer_max > 0 && target.CheckAlive())//HP減少
                {
                    float percent = Random.Range(actionsStatus[i].decreaseHPPer_min, actionsStatus[i].decreaseHPPer_max) / 100f;
                    target.DecreaseHP(Mathf.RoundToInt(targetStatus.maxHP * percent));
                }


                if (actionsStatus[i].DoesAttack() && target.CheckAlive())//攻撃
                {
                    OnAttackParams onAttackParams = new OnAttackParams();
                    onAttackParams.actionStatus = actionsStatus[i];
                    onAttackParams.target = target;
                    onAttackParams.toralCRITC = ownerStatus.CRITC + actionsStatus[i].CRITCMod;
                    bool CRIT = false;
                    int ATKDMG = 0;
                    int INTDMG = 0;
                    int totalDMG = 0;


                    float ACC = ownerStatus.ACC + actionsStatus[i].ACCMod;
                    float EVD = Mathf.Max(0, targetStatus.EVD);
                    float dice = Random.value * 100f;

                    if (actionsStatus[i].sureHit || dice <= ownerStatus.ACC + actionsStatus[i].ACCMod)//ミス判定
                    {
                        if (actionsStatus[i].sureHit || actionsStatus[i].unevadable || dice <= ACC - EVD)//回避判定
                        {
                            onAttackParams.hit = true;

                            float ATKDMGf = 0;
                            float INTDMGf = 0;
                            float ATKMod = Random.Range(actionsStatus[i].ATKMod_min, actionsStatus[i].ATKMod_max) / 100;
                            float INTMod = Random.Range(actionsStatus[i].INTMod_min, actionsStatus[i].INTMod_max) / 100;
                            ATKDMGf += ownerStatus.ATK * ATKMod;
                            INTDMGf += ownerStatus.INT * INTMod;


                            if ((ownerStatus.CRITC + actionsStatus[i].CRITCMod).Dice())//クリティカル判定
                            {
                                shakeCamera = true;
                                CRIT = true;
                                onAttackParams.CRIT = true;
                                ATKDMGf *= (ownerStatus.CRITD + actionsStatus[i].CRITDMod) / 100f;
                                INTDMGf *= (ownerStatus.CRITD + actionsStatus[i].CRITDMod) / 100f;
                                target.SpawnVisualEffect(Definer.VERef.CRIT);
                            }

                            float RDMG = Mathf.Max((targetStatus.PROT * -1 + 100f) / 100f, 0);//対象の被ダメージ上昇効果
                            ATKDMGf *= RDMG;
                            INTDMGf *= RDMG;

                            ATKDMGf *= (100f + actionsStatus[i].exDMG_mul) / 100f;//与ダメージ上昇効果
                            ATKDMGf += actionsStatus[i].exATKDMG_int;

                            INTDMGf *= (100f + actionsStatus[i].exDMG_mul) / 100f;//与ダメージ上昇効果
                            INTDMGf += actionsStatus[i].exINTDMG_int;

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
                            onDamageParams.owner = actionStatus.actionOwner;
                            onDamageParams.target = target;
                            onDamageParams.actionStatus = actionsStatus[i];

                            result.onDamageParams = onDamageParams;
                            onAttackParamsList.Add(onAttackParams);
                            target.OnAttacked(actionStatus.actionOwner, false, false);//被攻撃時誘発
                            if(target.Damage(onDamageParams))//ダメージ処理開始
                            {//殺害したなら
                                OnKillParams onKillParams = new OnKillParams();
                                onKillParams.obstacle = targetStatus.obstacle;
                                onKillParams.target = target;
                                onKillParams.CRIT = CRIT;
                                onKillParamsList.Add(onKillParams);
                            }

                            if (!notChara)
                            {
                                if (actionStatus.actionOwner.GetCharacterStatus().position.IsPlayerPos() && !targetStatus.position.IsPlayerPos())
                                {
                                    totalDamage += totalDMG;
                                }

                                onDamageParamsList.Add(onDamageParams);
                                if (totalDMG > 0 && actionsStatus[i].drain > 0)//吸血処理
                                {
                                    OnHealParams onHealParams = new OnHealParams();
                                    float drainf = totalDMG * actionsStatus[i].drain / 100f;
                                    int drain = Mathf.RoundToInt(drainf);

                                    onHealParams.target = actionStatus.actionOwner;
                                    onHealParams.healValue = drain;

                                    actionStatus.actionOwner.Heal(drain, actionStatus.actionOwner);
                                    onHealParamsList.Add(onHealParams);
                                }
                            }
                        }
                        else//回避
                        {
                            target.GetTargetButton().SetDamageText("Evade", Definer.colorRef.evade);
                            infoText.AddLogText(util.GetColoredText(Definer.colorRef.evade, string.Format("{0}は攻撃を回避した", targetStatus.charaName)));
                            soundManager.PlaySE(Definer.soundRef.evade);
                            attackHit = false;
                            if (!notChara)
                            {
                                onAttackParams.evaded = true;
                            }
                            onAttackParamsList.Add(onAttackParams);
                            target.OnAttacked(actionStatus.actionOwner, true, false);//被攻撃時誘発

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
                        target.OnAttacked(actionStatus.actionOwner, false, true);//被攻撃時誘発
                        soundManager.PlaySE(Definer.soundRef.miss);
                        attackHit = false;
                    }

                    
                }

                if (attackHit && target.CheckAlive())
                {
                    if (actionsStatus[i].DoesHeal())//回復
                    {
                        OnHealParams onHealParams = new OnHealParams();
                        onHealParams.target = target;
                        float fheal;
                        fheal = Random.Range(actionsStatus[i].healValue_min, actionsStatus[i].healValue_max + 1);
                        fheal += targetStatus.maxHP * Random.Range(actionsStatus[i].healPercent_min, actionsStatus[i].healPercent_max) / 100;
                        if(actionsStatus[i].healRegain_max > 0)
                        {
                            int decreasedHP = targetStatus.maxHP - targetStatus.HP;
                            fheal += decreasedHP * Random.Range(actionsStatus[i].healRegain_min, actionsStatus[i].healRegain_max) / 100f;
                        }
                        fheal *= ownerStatus.GHeal / 100;
                        fheal *= targetStatus.RHeal / 100;
                        int heal = Mathf.RoundToInt(fheal);
                        onHealParams.healValue = heal;

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
                        target.AddShield(Random.Range(actionsStatus[i].shieldAdd_min, actionsStatus[i].shieldAdd_max + 1));
                    }
                    if (actionsStatus[i].shieldPercent_max > 0)//割合シールド
                    {
                        int percent = Random.Range(actionsStatus[i].shieldPercent_min, actionsStatus[i].shieldPercent_max + 1);
                        target.AddShield(Mathf.RoundToInt(targetStatus.maxHP * percent * 0.01f));
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

                    List<PA_StatusEffect.StatusEffectStatus.StatusEffectType> appliedType = new List<PA_StatusEffect.StatusEffectStatus.StatusEffectType>();
                    foreach (PA_StatusEffect.StatusEffectParams P in actionsStatus[i].applySteParams)//StE付与
                    {
                        PA_StatusEffect.StatusEffectParams StEParams = P;
                        PA_StatusEffect.StatusEffectStatus StEStaus = StEParams.GetStatusEffectStatus();


                        StEApplyBonus applyBonus = new StEApplyBonus();//bonusの設定
                        if (ownerStatus.GetStEApplyBonus(StEParams.applyStE) != null)//オーナーのボーナス
                        {
                            applyBonus= applyBonus.AddBonus((StEApplyBonus)ownerStatus.GetStEApplyBonus(StEParams.applyStE));
                        }
                        foreach (StEApplyBonus bonus in actionsStatus[i].StEApplyBonus)//アクションのボーナス
                        {
                            if (bonus.applyStE == StEParams.applyStE) { applyBonus= applyBonus.AddBonus(bonus); }
                        }

                        StEParams.applyChance += applyBonus.exChance;//bonus等をparamsに反映
                        StEParams.stack += applyBonus.exStack;
                        StEParams.value+=applyBonus.exValue;
                        if (StEStaus.StEType == PA_StatusEffect.StatusEffectStatus.StatusEffectType.debuff)
                        {
                            StEParams.applyChance += actionsStatus[i].debuffChanceMod;
                        }
                        if (StEStaus.scaleStackByLVL) { StEParams.stack += Mathf.Max(0, Mathf.FloorToInt((ownerStatus.level - 1) / 2f)); }

                        if (StEParams.guaranteed || (StEParams.applyChance - targetStatus.GetStERes(StEParams)).Dice())//抽選
                        {
                            if (!appliedType.Contains(StEStaus.StEType)) { appliedType.Add(StEStaus.StEType); }
                            onApplyStEParams.appliedParams.Add(StEParams);

                            target.ApplyStE(StEParams, StEParams.stack, StEParams.value);
                        }
                        else
                        {
                            onApplyStEParams.resistedParams.Add(StEParams);
                            target.GetTargetButton().SetDamageText("Resist", Definer.colorRef.failed_unavailable);
                            infoText.AddLogText(string.Format("{0}が{1}をレジスト", targetStatus.charaName, StEParams.applyStE.GetComponent<PA_StatusEffect>().GetPAName()));
                        }
                    }
                    foreach(PA_StatusEffect.StatusEffectStatus.StatusEffectType type in appliedType)//StEのエフェクト
                    {
                        if (Definer.VERef.applyStE[(int)type])
                        {
                            target.SpawnVisualEffect(Definer.VERef.applyStE[(int)type]);
                        }
                    }
                    //actionsStatus[i].StEApplyBonus.Clear();
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
                                    if (c.GetCharacterStatus().immovable)
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
                                        int swapToPos = util.GetMoveToPos(c.GetCharacterStatus().position, 3 - moveDir, 1);
                                        OnMoveParams onSwapParams = new OnMoveParams();
                                        onSwapParams.target = c;
                                        onSwapParams.dir = 3 - moveDir;
                                        onSwapParams.range = 1;
                                        onSwapParams.prevPos = c.GetCharacterStatus().position;
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


            //infoText.AddDebugText("召喚処理は未完成です\n召喚しようとしている場所が空欄であるかどうかを確かめる必要があります");
            soundManager.PlaySE(Definer.soundRef.summoned);
            for (int i = 0; i < actionStatus.actionTargetsInt.Count; i++)
            {
                if (!characterManager.CheckCharaExist(actionStatus.actionTargetsInt[i]))
                {
                    if (actionStatus.actionTargetsInt[i] < 9) { characterManager.SpawnPlayer(actionStatus.summonChara[actionStatus.summonChanceWeight.ChoiceWithWeight()], actionStatus.actionTargetsInt[i]); }
                    else { characterManager.SpawnEnemy(actionStatus.summonChara[actionStatus.summonChanceWeight.ChoiceWithWeight()], actionStatus.actionTargetsInt[i], false); }
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
                foreach (PositionEffect.PositionEffectParams PEParams in actionStatus.applyPEParams)//PE付与
                {
                    if (PEParams.guaranteed||PEParams.applyChance.Dice())
                    {
                        infoText.AddLogText(string.Format("ポジション{0}に{1}が付与", actionStatus.actionTargetsInt[i].PosIntToStr(), PEParams.applyPE.GetComponent<PositionEffect>().GetPEName(true)));
                        characterManager.GetPositionManager(actionStatus.actionTargetsInt[i]).ApplyPE(PEParams);
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
                ownerMoveRange = Mathf.Abs(util.posIntToVector(ownerStatus.position).x - util.posIntToVector(moveToPos).x);
            }
            else if (ownerMoveDir == 1 || ownerMoveDir == 2)//上下移動なら
            {
                ownerMoveRange = Mathf.Abs(util.posIntToVector(ownerStatus.position).y - util.posIntToVector(moveToPos).y);
            }

            List<Character> charasOnTravelingDir = new List<Character>(FindObjectOfType<CharactersManager>().GetTravelingDirCharas(ownerStatus.position, ownerMoveDir, ownerMoveRange));
            foreach (Character c in charasOnTravelingDir)
            {
                if (c.GetCharacterStatus().immovable)
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
                    int swapToPos= util.GetMoveToPos(c.GetCharacterStatus().position, 3 - ownerMoveDir, 1);
                    OnMoveParams onSwapParams = new OnMoveParams();
                    onSwapParams.dir = 3 - ownerMoveDir;
                    onSwapParams.range = 1;
                    onSwapParams.prevPos = c.GetCharacterStatus().position;
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
            if (onKillParamsList.Count > 0) { actionStatus.actionOwner.Onkill(onKillParamsList); }//殺害時誘発
            if (onApplyStEParamsList.Count > 0)//StE付与時誘発
            {
                actionStatus.actionOwner.OnApplyStE(onApplyStEParamsList);
                battleManager.Trigger_OnSomeoneApplyedStE(onApplyStEParamsList);
            }
            if (onHealParamsList.Count > 0) { actionStatus.actionOwner.OnHeal(onHealParamsList); }//与回復時誘発
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
    /// <summary>自身のスプライトを代入してEnqueue</summary>
    public void Enqueue(ActionStatus actionStatus, bool setTargets, List<Character> actionTargets, int targetCount = 0, bool nullOwner = false)
    {
        actionStatus.actionOwner.Enqueue(actionStatus, setTargets, actionTargets, targetCount, nullOwner);
    }

    /// <summary>自身を対象にEunqueue</summary>
    public void Enqueue_Self(ActionStatus act)
    {
        ActionStatus action = act;
        actionStatus.actionOwner.Enqueue(action, true, new List<Character>() { actionStatus.actionOwner }, 0);
    }

    public bool CheckIfAbilityEffect() { return actionStatus.abilityEffect; }
}
