using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Action : MonoBehaviour
{
    [SerializeField]//tst
    ActionStatus actionStatus;
    ActionQueueManager actionQueueManager;
    CharactersManager characterManager;
    InfoText infoText;
    SoundManager soundManager;
    CameraManager cameraManager;


    [System.Serializable]
    public struct ActionStatus
    {
        [Header("アビリティのactionの場合は設定不要")]
        public string actionName;
        [TextArea(3, 10)]
        public string conditionInfo;
        [TextArea(3, 10)]
        public string targetInfo;
        [TextArea(3, 10),Header("AModのスクリプトを使わないときなどに\n条件入力して改行して変化内容書く")]
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
        public enum TargetType { other, single, all, self, row, column, singleWoSelf, allWoSelf, random, move}
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
        public bool cantCounter;
        [Header("0:melee 1:ranged 2:magic 3:counter")]
        /// <summary>0:melee 1:ranged 2:magic 3:counter</summary>
        public int attackType;
        public float ATKMod_min;
        public float ATKMod_max;
        public float exDMG_mul;
        public int exDMG_int;
        public float ACCMod;
        public float CRITCMod;
        public float CRITDMod;
        [Header("与ダメのdrain％回復")] 
        public float drain;
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
        [Header("\nDoTの除去")]
        public int removeStE_DoT;

        [Header("特定のStEの除去")]
        public List<ActionData.RemoveStE> removeStEs;

        [Header("\n\n召喚")]
        public bool summon;
        //public int summonSize;
        public CharacterData[] summonChara;
        public float[] summonChanceWeight;

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
        public List<ActionData.RemoveStE> removeStEs_additional;

        public bool dontChangeSprite;
        [Header("スプライトの直接指定")]
        public GameObject activateSprite;

        public Character actionOwner;
        [System.NonSerialized]
        //public Character.CharacterStatus ownerStatus_notChara;
        public List<Character> actionTargets;
        /// <summary>移動や召喚の際に使用 移動の際は移動先のposが入る</summary>
        public List<int> actionTargetsInt;

        public string GetInfo(bool refCharaStatus, Character.CharacterStatus characterStatus)
        {
            string s = "";

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

            if (ATKMod_max > 0)//攻撃
            {
                if (cantCounter) { s += "カウンター不可\n"; }
                switch (attackType)
                {
                    case 0:
                        s += "・近接攻撃を行う\n";
                        break;
                    case 1:
                        s += "・遠距離攻撃を行う\n";
                        break;
                    case 2:
                        s += "・魔術攻撃を行う\n";
                        break;
                    case 3:
                        s += "・カウンター攻撃を行う\n";
                        break;
                }
                s += string.Format("ATKの{0}％ダメージ", GetValueRange(ATKMod_min, ATKMod_max));
                if (refCharaStatus)
                {
                    s += string.Format("({0})", GetValueRange(Mathf.RoundToInt(characterStatus.ATK * ATKMod_min / 100), Mathf.RoundToInt(characterStatus.ATK * ATKMod_max / 100)));                   
                }
                s += "\n";
                if (ACCMod != 0) { s += string.Format("ACC補正：{0}\n", GetValueWithSign(ACCMod)); }
                if (CRITCMod != 0) { s += string.Format("CRIT率補正：{0}％\n", GetValueWithSign(CRITCMod)); }
                if (CRITDMod != 0) { s += string.Format("CRITダメージ補正：{0}％\n", GetValueWithSign(CRITDMod)); }
                if (drain > 0) { s += string.Format("与ダメージの{0}％を回復\n", drain); }
                if (sureHit) { s += "必中\n"; }
                if (unevadable) { s += "EVDを無視\n"; }
                s += "\n";
            }

            if (healValue_max > 0 || healPercent_max > 0 || healRegain_max > 0)//回復
            {
                if (healValue_max > 0) { s += string.Format("・HPを{0}回復\n", GetValueRange(healValue_min, healValue_max)); }
                if (healPercent_max > 0) { s += string.Format("・HPを最大値の{0}％回復\n", GetValueRange(healPercent_min, healPercent_max)); }
                if (healRegain_max > 0) { s += string.Format("・減少したHPの{0}％を回復\n", GetValueRange(healRegain_min, healRegain_max)); }
                s += "\n";
            }

            if (SANHeal_max > 0) { s += string.Format("・正気度を{0}回復\n", GetValueRange(SANHeal_min, SANHeal_max)); }
            if (SANDamage_max > 0) { s += string.Format("・正気度が{0}減少\n", GetValueRange(SANDamage_min, SANDamage_max)); }
            if (shieldAdd_max > 0) { s += string.Format("・シールドを{0}付与\n", GetValueRange(shieldAdd_min, shieldAdd_max)); }
            if (shieldPercent_max > 0) { s += string.Format("・maxHPの{0}％に等しいシールドを付与\n", GetValueRange(shieldPercent_min, shieldPercent_max)); }
            if (shieldRemove_all) { s += "・シールドを0にする\n"; }
            else if (shieldRemove_max > 0) { s += string.Format("・シールドを{0}除去\n", GetValueRange(shieldRemove_min, shieldRemove_max)); }

            foreach (PA_StatusEffect.StatusEffectParams StEParams in applySteParams)//StE付与
            {
                PA_StatusEffect.StatusEffectStatus status = StEParams.applyStE.GetComponent<PA_StatusEffect>().GetStatusEffectStatus();
                if (StEParams.guaranteed) { s += "・"; }
                else { s += string.Format("・{0}％の確率で", StEParams.applyChance); }
                if (status.refValue) { s += string.Format("{0}を{1}スタック付与\n", (status.StEName + StEParams.value.ToString()).ColorStr(status.StEType.ToColor()), StEParams.stack); }
                else { s += string.Format("{0}を{1}スタック付与\n", status.StEName.ColorStr(status.StEType.ToColor()), StEParams.stack); }
                //s += string.Format("{0}を{1}スタック付与\n", status.StEName.ColorStr(status.StEType.ToColor()), StEParams.stack);
                s += StEParams.applyStE.GetComponent<PA_StatusEffect>().GetStEInfo_forRef();
                if (status.maxStack > 0) { s += string.Format("(最大{0}スタック)\n",status.maxStack).ColorStr(Color.gray); }
            }
            foreach (PositionEffect.PositionEffectParams PEParams in applyPEParams)//PE付与
            {
                PositionEffect.PositionEffectStatus status = PEParams.applyPE.GetComponent<PositionEffect>().GetPositionEffectStatus();
                if(PEParams.guaranteed) { s += "・対象の地点に"; }
                else { s += string.Format("・{0}％の確率で対象の地点に", PEParams.applyChance); }
                if (status.refValue) { s += string.Format("{0}{1}を{2}スタック付与\n", status.PEName.ColorStr(status.PEType.ToColor()), PEParams.value, PEParams.stack); }
                else { s += string.Format("{0}を{1}スタック付与\n", status.PEName.ColorStr(status.PEType.ToColor()), PEParams.stack); }
                s += PEParams.applyPE.GetComponent<PositionEffect>().GetPEInfo(true);
            }
            if (removeStE_buff > 0) { s += string.Format("・{0}を{1}個消去\n"
                , "バフ効果".ColorStr(Definer.colorRef.statusEffectColors[(int)PA_StatusEffect.StatusEffectStatus.StatusEffectType.buff]), removeStE_buff); }
            if (removeStE_debuff > 0) { s += string.Format("・{0}を{1}個消去\n"
                , "デバフ効果".ColorStr(Definer.colorRef.statusEffectColors[(int)PA_StatusEffect.StatusEffectStatus.StatusEffectType.debuff]), removeStE_debuff); }
            if (removeStE_DoT > 0) { s += string.Format("・{0}を{1}個消去\n"
                , "ダメージ効果".ColorStr(Definer.colorRef.statusEffectColors[(int)PA_StatusEffect.StatusEffectStatus.StatusEffectType.DoT]), removeStE_DoT); }
            foreach (ActionData.RemoveStE remove in removeStEs)
            {
                PA_StatusEffect.StatusEffectStatus status = remove.removeStE.GetComponent<PA_StatusEffect>().GetStatusEffectStatus();
                s += string.Format("・{0}", status.StEName.ColorStr(status.StEType.ToColor()));
                if (remove.removeAll) { s += "を全て除去\n"; }
                else { s += string.Format("のスタック{0}\n", GetValueWithSign(remove.addAmount)); }
            }

            if (summon)
            {
                if (summonChara.Length == 1)
                {
                    s += string.Format("・{0}を召喚", summonChara[0].charaName);
                }
                else
                {
                    s += "・以下からランダムに召喚\n";
                    float p = 0;
                    foreach (int r in summonChanceWeight) { p += r; }
                    for (int i = 0; i < summonChanceWeight.Length; i++)
                    {
                        s += string.Format("{0}({1})\n", summonChara[i].charaName, (summonChanceWeight[i] / p).ToString("#0.0%"));
                    }
                }
            }

            if (guaranteedMove||moveChance > 0)
            {
                if (guaranteedMove) { s += "・"; }
                else { s += string.Format("・{0}％の確率で", moveChance); }
                if (moveForword > 0) { s += string.Format("{0}前進\n", moveForword); }
                if (moveUpper > 0) { s += string.Format("{0}上昇\n", moveUpper); }
                if (moveLower > 0) { s += string.Format("{0}下降\n", moveLower); }
                if (moveBackword > 0) { s += string.Format("{0}後退\n", moveBackword); }
            }

            foreach(ActionData.AbilityRemainControll remainControll in abilityRemainControlls)
            {
                s += string.Format("・<{0}>の使用回数を", remainControll.abilityData.abilityName.ColorStr(remainControll.abilityData.abilityType.ToColor()));
                if (remainControll.set) { s += string.Format("{0}にする\n", remainControll.value); }
                else
                {
                    s += string.Format("{0}増加\n", remainControll.value);
                }
            }
            s += "\n";
            foreach (GameObject actionMod in actionMods)
            {
                s += actionMod.GetComponent<ActionMod>().GetActionModStatus().GetModInfo();
            }
            if (AModInfo != "")
            {
                s += string.Format("○{0}", AModInfo).ColorStr(Definer.colorRef.AMod);
            }
            if (actionInfo != "") { s += actionInfo + "\n"; }

            return s;
        }
        public string GetValueWithSign(float value)
        {
            if (value > 0) { return "+" + value.ToString(); }
            else { return value.ToString(); }
        }
        public string GetValueRange(float min,float max)
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

            if (mod.cantCounter) { modifiedStatus.cantCounter = true; }
            modifiedStatus.ATKMod_min += mod.ATKMod;
            modifiedStatus.ATKMod_max += mod.ATKMod;
            modifiedStatus.exDMG_mul += mod.exDMG_mul;
            modifiedStatus.exDMG_int += mod.exDMG_int;
            modifiedStatus.ACCMod += mod.ACCMod;
            modifiedStatus.CRITCMod += mod.CRITCMod;
            modifiedStatus.CRITDMod += mod.CRITDMod;
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
            foreach(PA_StatusEffect.StatusEffectParams statusEffectParams in mod.applySteParams)
            {
                modifiedStatus.applySteParams.Add(statusEffectParams);
            }
            foreach (StEApplyBonus bonus in mod.applyStEBonus)
            {
                bool f = false;
                foreach (StEApplyBonus applyBonus in modifiedStatus.StEApplyBonus)
                {
                    if (applyBonus.applyStE == bonus.applyStE)
                    {
                        applyBonus.AddBonus(bonus, true);
                        f = true;
                    }
                }
                if (!f) { modifiedStatus.StEApplyBonus.Add(bonus); }
            }

            removeStE_buff += mod.removeStE_buff;
            removeStE_debuff += mod.removeStE_debuff;
            removeStE_DoT += mod.removeStE_DoT;

            foreach(ActionData.RemoveStE removeStE in mod.removeStEs)
            {
                modifiedStatus.removeStEs_additional.Add(removeStE);
            }
            //move

            return modifiedStatus;
        }
    }

    //===========================[誘発処理の引数に使う]================================
    public struct OnAttackParams
    {
        /// <summary> = !(missed||evaded)</summary>
        public bool hit;
        public bool missed;
        public bool evaded;
        public bool CRIT;
        public float toralCRITC;
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
        public List<PA_StatusEffect.StatusEffectParams> appliedParams;
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

    List<OnAttackParams> onAttackParamsList = new List<OnAttackParams>();
    List<OnKillParams> onKillParamsList = new List<OnKillParams>();
    List<OnApplyStEParams> onApplyStEParamsList = new List<OnApplyStEParams>();
    List<OnHealParams> onHealParamsList = new List<OnHealParams>();
    OnMoveParams onMoveParams = new OnMoveParams();

    bool shakeCamera;
    
    Utility util;

    /// <summary>
    /// status にはactionOwner(キャラ時) もしくは　ownerStatus_notChara(非キャラ時)のいずれかを代入した状態で渡すこと!!
    /// </summary>
    public void Init(ActionQueueManager qm, ActionStatus status, ActionInfoPanel infoPanel, InfoText it, Utility u, SoundManager sm, CameraManager cam)
    {
        actionQueueManager = qm;
        actionStatus = status;
        infoText = it;
        util = u;
        infoPanel.Init(actionStatus.actionName, actionStatus.GetInfo(false, new Character.CharacterStatus()));
        characterManager = FindObjectOfType<CharactersManager>();
        soundManager = sm;
        cameraManager = cam;
    }
    public ActionStatus GetActionStatus() { return actionStatus; }

    public virtual void Resolve()
    {
        Character.CharacterStatus ownerStatus = new Character.CharacterStatus();
        bool notChara = false;//フィールド効果やポジション効果などによるアクション
        if (actionStatus.actionOwner != null) { ownerStatus = actionStatus.actionOwner.GetCharacterStatus(); }
        else
        {
            notChara = true;
            ownerStatus = Definer.nonCharaStatus;
        }

        //if (!notChara && ownerStatus.dead)//発動時は発動主が死んでいた場合はそのままDequeue
        //{
        //    actionQueueManager.Dequeue();
        //    return;
        //}

        if (actionStatus.actionTargets == null) { actionStatus.actionTargets = new List<Character>(); }
        ActionStatus[] actionsStatus =new ActionStatus[actionStatus.actionTargets.Count];
        for (int i = 0; i < actionsStatus.Length; i++)
        {
            actionsStatus[i] = actionStatus;
        }
        if (actionStatus.SE != null) { soundManager.PlaySE(actionStatus.SE); }

        foreach(Character character in actionStatus.actionTargets)
        {
            int pos = character.GetCharacterStatus().position;
            if (!actionStatus.actionTargetsInt.Contains(pos)) { actionStatus.actionTargetsInt.Add(pos); }
        }

        if (actionStatus.freeAction)
        {
            if (!actionStatus.abilityEffect) { infoText.AddErrorText("アビリティじゃないのにフリーアクション"); }
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

        //各対象キャラへの処理
        for (int i = 0; i < actionStatus.actionTargets.Count; i++)
        {
            Character target = actionStatus.actionTargets[i];
            Character.CharacterStatus targetStatus = target.GetCharacterStatus();
            bool attackHit = true;//攻撃失敗時、その他の効果も発動しないようにする
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
                        actionStatus.actionOwner.GetCharacter_TargetButton().SetActionIcon(actionStatus.sprite);
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


                if (actionsStatus[i].ATKMod_max > 0 && target.CheckAlive())//攻撃
                {
                    OnAttackParams onAttackParams = new OnAttackParams();
                    onAttackParams.target = target;
                    onAttackParams.toralCRITC = ownerStatus.CRITC + actionsStatus[i].CRITCMod;
                    bool CRIT = false;
                    int DMG = 0;


                    float ACC = ownerStatus.ACC + actionsStatus[i].ACCMod;
                    float EVD = Mathf.Max(0, targetStatus.EVD);
                    float dice = Random.value * 100f;

                    if (actionsStatus[i].sureHit || dice <= ownerStatus.ACC + actionsStatus[i].ACCMod)//ミス判定
                    {
                        if (actionsStatus[i].sureHit || actionsStatus[i].unevadable || dice <= ACC - EVD)//回避判定
                        {
                            onAttackParams.hit = true;

                            float fDMG = ownerStatus.exATK;
                            float ATKMod = Random.Range(actionsStatus[i].ATKMod_min, actionsStatus[i].ATKMod_max) / 100;
                            fDMG += ownerStatus.ATK * ATKMod;
                            if ((ownerStatus.CRITC + actionsStatus[i].CRITCMod).Dice())//クリティカル判定
                            {
                                shakeCamera = true;
                                CRIT = true;
                                onAttackParams.CRIT = true;
                                fDMG *= (ownerStatus.CRITD + actionsStatus[i].CRITDMod) / 100f;
                                target.SpawnVisualEffect(Definer.VERef.CRIT);
                            }

                            float RDMG = Mathf.Max((targetStatus.PROT * -1 + 100f) / 100f, 0);//対象の被ダメージ上昇効果
                            fDMG *= RDMG;

                            fDMG *= (100f + actionsStatus[i].exDMG_mul) / 100f;//与ダメージ上昇効果
                            fDMG += actionsStatus[i].exDMG_int;

                            DMG = Mathf.Max(0, Mathf.RoundToInt(fDMG));
                            int shieldDMG = Mathf.Min(DMG, targetStatus.shield);
                            DMG -= shieldDMG;

                            if (!notChara)
                            {
                                actionStatus.actionOwner.OnDamage(DMG, target, actionsStatus[i]);//与ダメ時誘発
                                if (DMG > 0 && actionStatus.drain > 0)//吸血処理
                                {
                                    OnHealParams onHealParams = new OnHealParams();
                                    float drainf = DMG * actionStatus.drain / 100f;
                                    int drain = Mathf.RoundToInt(drainf);

                                    onHealParams.target = actionStatus.actionOwner;
                                    onHealParams.healValue = drain;

                                    actionStatus.actionOwner.Heal(drain, actionStatus.actionOwner);
                                    onHealParamsList.Add(onHealParams);
                                }
                            }
                            onAttackParamsList.Add(onAttackParams);
                            target.OnAttacked(actionStatus.actionOwner, false, false);//被攻撃時誘発
                            if(target.Damage(DMG, CRIT, shieldDMG, actionStatus.actionOwner))//ダメージ処理開始
                            {//殺害したなら
                                OnKillParams onKillParams = new OnKillParams();
                                onKillParams.obstacle = targetStatus.obstacle;
                                onKillParams.target = target;
                                onKillParams.CRIT = CRIT;
                                onKillParamsList.Add(onKillParams);
                            }

                            infoText.AddDebugText($"出目：{dice} ACC：{ACC} ACC-EVD：{ACC - EVD} sureHit：{actionsStatus[i].sureHit} unevadable：{actionsStatus[i].unevadable} 結果：命中");//test
                        }
                        else//回避
                        {
                            target.GetCharacter_Object().SetDamageText("Evade", Definer.colorRef.evade);
                            infoText.AddLogText(util.GetColoredText(Definer.colorRef.evade, string.Format("{0}は攻撃を回避した", targetStatus.charaName)));
                            soundManager.PlaySE(Definer.soundRef.evade);
                            attackHit = false;
                            if (!notChara)
                            {
                                onAttackParams.evaded = true;
                            }
                            onAttackParamsList.Add(onAttackParams);
                            target.OnAttacked(actionStatus.actionOwner, true, false);//被攻撃時誘発

                            infoText.AddDebugText($"出目：{dice} ACC：{ACC} ACC-EVD：{ACC-EVD} sureHit：{actionsStatus[i].sureHit} unevadable：{actionsStatus[i].unevadable} 結果：回避");//test
                        }
                    }
                    else//ミス
                    {
                        if (!notChara)
                        {
                            target.GetCharacter_Object().SetDamageText("Miss", Definer.colorRef.failed_unavailable);
                            infoText.AddLogText(string.Format("{0}は攻撃を外した", ownerStatus.charaName).ColorStr(Definer.colorRef.failed_unavailable));
                            onAttackParams.missed = true;
                        }
                        onAttackParamsList.Add(onAttackParams);
                        target.OnAttacked(actionStatus.actionOwner, false, true);//被攻撃時誘発
                        infoText.AddDebugText($"出目：{dice} ACC：{ACC} ACC-EVD：{ACC - EVD} sureHit：{actionsStatus[i].sureHit} unevadable：{actionsStatus[i].unevadable} 結果：ミス");//test
                        soundManager.PlaySE(Definer.soundRef.miss);
                        attackHit = false;
                    }

                    
                }

                if (attackHit && target.CheckAlive())
                {
                    if (actionsStatus[i].healPercent_max > 0 || actionsStatus[i].healValue_max > 0||actionsStatus[i].healRegain_max>0)//回復
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

                    OnApplyStEParams onApplyStEParams = new OnApplyStEParams();
                    onApplyStEParams.appliedParams = new List<PA_StatusEffect.StatusEffectParams>();
                    onApplyStEParams.taget = target;

                    List<PA_StatusEffect.StatusEffectStatus.StatusEffectType> appliedType = new List<PA_StatusEffect.StatusEffectStatus.StatusEffectType>();
                    foreach (PA_StatusEffect.StatusEffectParams StEParams in actionsStatus[i].applySteParams)//StE付与
                    {
                        PA_StatusEffect.StatusEffectStatus StEStaus = StEParams.GetStatusEffectStatus();
                        StEApplyBonus applyBonus = new StEApplyBonus();
                        if (ownerStatus.GetStEApplyBonus(StEParams.applyStE) != null) { applyBonus.AddBonus(ownerStatus.GetStEApplyBonus(StEParams.applyStE)); }
                        foreach (StEApplyBonus bonus in actionsStatus[i].StEApplyBonus)
                        {
                            if (bonus.applyStE == StEParams.applyStE) { applyBonus.AddBonus(bonus); }
                        }

                        if (StEParams.guaranteed || (StEParams.applyChance + applyBonus.exChance - targetStatus.GetStERes(StEParams.applyStE)).Dice())
                        {
                            if (!appliedType.Contains(StEStaus.StEType)) { appliedType.Add(StEStaus.StEType); }
                            onApplyStEParams.appliedParams.Add(StEParams);

                            target.ApplyStE(StEParams, applyBonus);
                        }
                        else
                        {
                            target.GetCharacter_Object().SetDamageText("Resist", Definer.colorRef.failed_unavailable);
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
                    actionsStatus[i].StEApplyBonus.Clear();
                    if (onApplyStEParams.appliedParams.Count > 0) { onApplyStEParamsList.Add(onApplyStEParams); }


                    List< ActionData.RemoveStE > removeStEs=new List< ActionData.RemoveStE >(actionsStatus[i].removeStEs);
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
                    if (actionsStatus[i].removeStE_DoT > 0)
                    {
                        target.RemoveStE_ByType(PA_StatusEffect.StatusEffectStatus.StatusEffectType.DoT, actionsStatus[i].removeStE_DoT);
                    }
                    foreach (ActionData.RemoveStE remove in removeStEs)//StE消去
                    {
                        target.RemoveStE(remove);
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
                                    target.GetCharacter_TargetButton().ResetCharacter();//ターゲットボタンの参照の解除
                                    foreach (Character c in charasOnTravelingDir)
                                    {
                                        c.GetCharacter_TargetButton().ResetCharacter();
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
                        else { target.GetCharacter_Object().SetDamageText("MoveResist", Definer.colorRef.failed_unavailable); }
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
                actionStatus.actionOwner.GetCharacter_Object().SetDamageText("対象消失", Definer.colorRef.failed_unavailable);
            }
        }


        //各対象ポジションへの処理
        if (actionStatus.summon)//召喚
        {
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
            //print(ownerMoveRange);            

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
                actionStatus.actionOwner.GetCharacter_TargetButton().ResetCharacter();//ターゲットボタンの解除
                foreach (Character c in charasOnTravelingDir)
                {
                    c.GetCharacter_TargetButton().ResetCharacter();
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
        if (actionStatus.actionOwner != null)
        {
            if (onAttackParamsList.Count > 0) { actionStatus.actionOwner.OnAttack(onAttackParamsList); }//攻撃時誘発
            if (onKillParamsList.Count > 0) { actionStatus.actionOwner.Onkill(onKillParamsList); }//殺害時誘発
            if (onApplyStEParamsList.Count > 0) { actionStatus.actionOwner.OnApplyedStE(onApplyStEParamsList); }//StE付与時誘発
            if (onHealParamsList.Count > 0) { actionStatus.actionOwner.OnHeal(onHealParamsList); }//与回復時誘発
        }

        if (actionStatus.index == 0 && actionStatus.abilityEffect && actionStatus.abilityType != AbilityData.AbilityType.pass)
        { actionStatus.actionOwner.OnActivateAbility(); }

        if (shakeCamera) { cameraManager.ShakeCamera(1); }

        actionQueueManager.Dequeue();
    }

    public bool CheckIfAbilityEffect() { return actionStatus.abilityEffect; }
}
