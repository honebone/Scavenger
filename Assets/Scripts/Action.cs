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


    [System.Serializable]
    public struct ActionStatus
    {
        [Header("アビリティのactionの場合は設定不要")]
        public string actionName;
        [TextArea(3, 10)]
        public string conditionInfo;
        //public bool useGeneralInfo;
        [TextArea(3, 10)]
        public string targetInfo;
[TextArea(3, 10)]
        public string actionInfo;
        [Header("設定しなければ汎用的なオブジェクトになる")]
        public GameObject actionObject;
        public List<GameObject> actionMods;
        //public bool targetEmpty;
        
        public enum TargetType { other, single, all, self, row, column, singleWoSelf, allWoSelf, random, move}
        [Header("ここからアビリティのみ関係")]
        public TargetType targetType;
        public bool targetPlayerSide;
        public bool targetEnemySide;
       
        public bool selectableFront;
        public bool selectableMid;
        public bool selectableBack;
        public bool ignoreMark;
        [Header("味方を対象とするアビリティはignoreHideにチェック!!")]
        public bool ignoreHide;
        [Header("0:right 1:upper 2:lower 3:left(targetypeがmoveのときに使用)")]
        public List<int> moveValue;
        [Header("ここまでアビリティのみ関係")]

        public int decreaseHP_min;
        public int decreaseHP_max;

        public bool cantCounter;
        [Header("0:melee 1:ranged 2:magic")]
        /// <summary>0:melee 1:ranged 2:magic</summary>
        public int attackType;
        public float ATKMod_min;
        public float ATKMod_max;
        public float ACCMod;
        public float CRITCMod;
        public float CRITDMod;
        public bool sureHit;
        public bool unevadable;

        public int healValue_min;
        public int healValue_max;
        public float healPercent_min;
        public float healPercent_max;

        public int SANHeal_min;
        public int SANHeal_max;
        public int SANDamage_min;
        public int SANDamage_max;
        public int shieldAdd_min;
        public int shieldAdd_max;
        public int shieldRemove_min;
        public int shieldRemove_max;
        [Header("ApplyStE")]
        public PA_StatusEffect.StatusEffectParams[] applySteParams;

        public bool summon;
        public int summonSize;
        public CharacterData[] summonChara;
        public float[] summonChanceWeight;

        public float moveChance;
        public int moveForword;
        public int moveUpper;
        public int moveLower;
        public int moveBackword;

        [Header("以下には手を出すな")]
        public bool abilityEffect;
        public AbilityData.AbilityType abilityType;
        public AudioClip SE;
        public bool dontChangeSprite;
        [Header("スプライトの直接指定")]
        public GameObject activateSprite;
        [Header("汎用スプライトの番号")]
        public int spriteIndex;

        public Character actionOwner;
        public List<Character> actionTargets;
        /// <summary>移動や召喚の際に使用 移動の際は移動先のposが入る</summary>
        public List<int> actionTargetsInt;

        public string GetInfo(bool refCharaStatus, Character.CharacterStatus characterStatus)
        {
            string s = "";

            if (conditionInfo != "") { s += string.Format("{0}：\n", conditionInfo); }
            s += string.Format("対象：{0}\n", targetInfo);
            if (decreaseHP_max > 0)
            {
                if(decreaseHP_min == decreaseHP_max) { s += string.Format("HPが{0}減少\n", decreaseHP_max); }
                else { s += string.Format("HPが{0}-{1}減少\n", decreaseHP_min, decreaseHP_max); }
            }

            if (ATKMod_max > 0)//攻撃
            {
                if (cantCounter) { s += "カウンター不可\n"; }
                switch (attackType)
                {
                    case 0:
                        s += "近接攻撃を行う\n";
                        break;
                    case 1:
                        s += "遠距離攻撃を行う\n";
                        break;
                    case 2:
                        s += "魔術攻撃を行う\n";
                        break;
                }
                s += string.Format("ATKの{0}％ダメージ", GetValueRange(ATKMod_min, ATKMod_max));
                if (refCharaStatus)
                {
                    s += string.Format("({0})", GetValueRange(Mathf.RoundToInt(characterStatus.ATK * ATKMod_min / 100), Mathf.RoundToInt(characterStatus.ATK * ATKMod_max / 100)));                   
                }
                s += "\n";
                if (ACCMod != 0) { s += string.Format("ACC補正：{0}\n", GetValueWithSign(ACCMod)); }
                if (CRITCMod != 0) { s += string.Format("CRIT率補正：{0}％(加算)\n", GetValueWithSign(CRITCMod)); }
                if (CRITDMod != 0) { s += string.Format("CRITダメージ補正：{0}倍\n", GetValueWithSign(CRITDMod)); }
                if (sureHit) { s += "必中\n"; }
                if (unevadable) { s += "回避不可\n"; }
                s += "\n";
            }

            if (healValue_max > 0 || healPercent_max > 0)//回復
            {
                if (healValue_max > 0) { s += string.Format("HPを{0}回復\n", GetValueRange(healValue_min, healValue_max)); }
                if (healPercent_max > 0) { s += string.Format("HPを{0}％回復\n", GetValueRange(healPercent_min, healPercent_max)); }
                s += "\n";
            }

            if (SANHeal_max > 0) { s += string.Format("正気度を{0}回復\n", GetValueRange(SANHeal_min, SANHeal_max)); }
            if (SANDamage_max > 0) { s += string.Format("正気度が{0}減少\n", GetValueRange(SANDamage_min, SANDamage_max)); }
            if (shieldAdd_max > 0) { s += string.Format("シールドを{0}付与\n", GetValueRange(shieldAdd_min, shieldAdd_max)); }
            if (shieldRemove_max > 0) { s += string.Format("シールドを{0}除去\n", GetValueRange(shieldRemove_min, shieldRemove_max)); }

            foreach (PA_StatusEffect.StatusEffectParams StEParams in applySteParams)//StE付与
            {
                PA_StatusEffect.StatusEffectStatus status = StEParams.applyStE.GetComponent<PA_StatusEffect>().GetStatusEffectStatus();
                s += string.Format("{0}％の確率で", StEParams.applyChance);
                if (status.refValue) { s += string.Format("{0}{1}を{2}スタック付与\n", status.StEName, StEParams.value, StEParams.stack); }
                else { s += string.Format("{0}を{1}スタック付与\n", status.StEName, StEParams.stack); }
                s += status.GetStEInfo_forRef();
                s += "\n";
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

        public void Init(ActionData actionData)
        {
            actionName = actionData.actionName;
            conditionInfo = actionData.conditionInfo;
            actionInfo = actionData.actionInfo;
            //useGeneralInfo = actionData.useGeneralInfo;

            targetInfo = actionData.targetInfo;

            actionObject = actionData.actionObject;
            actionMods = new List<GameObject>(actionData.actionMods);

            decreaseHP_min = actionData.decreaseHP_min;
            decreaseHP_max = actionData.decreaseHP_max;

            cantCounter = actionData.cantCounter;
            attackType = actionData.AttackType;
            ATKMod_min = actionData.ATKMod_min;
            ATKMod_max = actionData.ATKMod_max;
            ACCMod = actionData.ACCMod;
            CRITCMod = actionData.CRITCMod;
            CRITDMod = actionData.CRITDMod;
            sureHit = actionData.sureHit;
            unevadable = actionData.unevadable;

            healValue_min = actionData.healValue_min;
            healValue_max = actionData.healValue_max;
            healPercent_min = actionData.healPercent_min;
            healPercent_max = actionData.healPercent_max;

            SANHeal_min = actionData.SANHeal_min;
            SANHeal_max = actionData.SANHeal_max;
            SANDamage_min = actionData.SANDamage_min;
            SANDamage_max = actionData.SANDamage_max;
            shieldAdd_min = actionData.shieldAdd_min;
            shieldAdd_max = actionData.shieldAdd_max;
            shieldRemove_min = actionData.shieldRemove_min;
            shieldRemove_max = actionData.shieldRemove_max;
            applySteParams = actionData.applyStEParams;

            summon = actionData.summon;
            summonSize = actionData.summonSize;
            summonChara = actionData.summonChara;
            summonChanceWeight = actionData.summonChanceWeight;

        moveChance = actionData.moveChance;
            moveUpper = actionData.moveUpper;
            moveLower = actionData.moveLower;
            moveForword = actionData.moveForword;
            moveBackword = actionData.moveBackword;
        }

        public ActionStatus Modify(ActionMod.ActionModStatus mod)
        {
            ActionStatus modifiedStatus = this;

            modifiedStatus.decreaseHP_min += mod.decreaseHP;
            modifiedStatus.decreaseHP_max += mod.decreaseHP;

            if (mod.cantCounter) { modifiedStatus.cantCounter = true; }
            modifiedStatus.ATKMod_min += mod.ATKMod;
            modifiedStatus.ATKMod_max += mod.ATKMod;
            modifiedStatus.ACCMod += mod.ACCMod;
            modifiedStatus.CRITCMod += mod.CRITCMod;
            modifiedStatus.CRITDMod += mod.CRITDMod;
            if (mod.sureHit) { modifiedStatus.sureHit = true; }
            if (unevadable) { modifiedStatus.unevadable = true; }

            modifiedStatus.healValue_min += mod.healValue;
            modifiedStatus.healValue_max += mod.healValue;
            modifiedStatus.healPercent_min += mod.healPercent;
            modifiedStatus.healPercent_max += mod.healPercent;

            modifiedStatus.SANHeal_min += mod.SANHeal;
            modifiedStatus.SANHeal_max += mod.SANHeal;
            modifiedStatus.SANDamage_min += mod.SANDamage;
            modifiedStatus.SANDamage_max += mod.SANDamage;
            modifiedStatus.shieldAdd_min += mod.shieldAdd;
            modifiedStatus.shieldAdd_max += mod.shieldAdd;
            modifiedStatus.shieldRemove_min += mod.shieldRemove;
            modifiedStatus.shieldRemove_max += mod.shieldRemove;


            //StE
            //move

            return modifiedStatus;
        }
    }

    
    Utility util;

    public void Init(ActionQueueManager qm,ActionStatus status,ActionInfoPanel infoPanel,Utility u,SoundManager sm)
    {
        actionQueueManager = qm;
        actionStatus = status;
        util = u;
        infoPanel.Init(actionStatus.actionName, actionStatus.GetInfo(false, new Character.CharacterStatus()));
        soundManager = sm;
    }
    public ActionStatus GetActionStatus() { return actionStatus; }

    public virtual void Resolve()
    {
        Character.CharacterStatus ownerStatus = actionStatus.actionOwner.GetCharacterStatus();
        ActionStatus[] actionsStatus =new ActionStatus[actionStatus.actionTargets.Count];
        for (int i = 0; i < actionsStatus.Length; i++)
        {
            actionsStatus[i] = actionStatus;
        }
        if (actionStatus.SE != null) { soundManager.PlaySE(actionStatus.SE); }

        List<GameObject> actionModsObj = new List<GameObject>(actionStatus.actionMods);
        //ここで色々なactionMdsを追加
        foreach(GameObject actionModObj in actionModsObj)
        {
            var am = Instantiate(actionModObj);
            actionsStatus = am.GetComponent<ActionMod>().ModifyAction(actionStatus, actionsStatus);
            Destroy(am);
        }


        for (int i = 0; i < actionStatus.actionTargets.Count; i++)
        {
            Character.CharacterStatus targetStatus = actionStatus.actionTargets[i].GetCharacterStatus();
            actionStatus.actionTargets[i].BecomeAbilityTarget(actionStatus.actionOwner);
            if (!targetStatus.dead)
            {
                if (actionsStatus[i].decreaseHP_max > 0)//HP減少
                {
                    actionStatus.actionTargets[i].DecreaseHP(Random.Range(actionsStatus[i].decreaseHP_min, actionsStatus[i].decreaseHP_max + 1));
                }


                if (actionsStatus[i].ATKMod_max > 0)//攻撃
                {
                    bool CRIT = false;
                    int DMG = 0;

                    if (actionsStatus[i].sureHit || util.Probability(ownerStatus.ACC + actionsStatus[i].ACCMod))
                    {
                        if (actionsStatus[i].unevadable || util.Probability(100f - targetStatus.EVD))//攻撃命中
                        {
                            float fDMG = ownerStatus.exATK;
                            float ATKMod = Random.Range(actionsStatus[i].ATKMod_min, actionsStatus[i].ATKMod_max) / 100;
                            fDMG += ownerStatus.ATK * ATKMod;
                            if (util.Probability(ownerStatus.CRITC + actionsStatus[i].CRITCMod))
                            {
                                CRIT = true;
                                fDMG *= ownerStatus.CRITD + actionsStatus[i].CRITDMod;
                            }
                            fDMG -= targetStatus.shield;

                            DMG = Mathf.Max(0, Mathf.RoundToInt(fDMG));

                            actionStatus.actionOwner.OnDamage(DMG, actionStatus.actionTargets[i]);//与ダメ時誘発
                            actionStatus.actionTargets[i].Damage(DMG, CRIT, actionsStatus[i].cantCounter, actionStatus.actionOwner);//ダメージ処理開始
                        }
                        else
                        {
                            actionStatus.actionTargets[i].GetCharacter_Object().SetDamageText("Evade", Definer.colorRef.evade);
                            FindObjectOfType<InfoText>().AddLogText(util.GetColoredText(Definer.colorRef.evade, string.Format("{0}は攻撃を回避した", targetStatus.charaName)));
                            soundManager.PlaySE(Definer.soundRef.evade);
                        }
                    }
                    else
                    {
                        actionStatus.actionOwner.GetCharacter_Object().SetDamageText("Miss", Definer.colorRef.failed_unavailable);
                        FindObjectOfType<InfoText>().AddLogText(util.GetColoredText(Definer.colorRef.failed_unavailable, string.Format("{0}は攻撃を外した", ownerStatus.charaName)));
                        soundManager.PlaySE(Definer.soundRef.miss);
                    }
                }


                if (actionsStatus[i].healPercent_max > 0 || actionsStatus[i].healValue_max > 0)//回復
                {
                    float fheal;
                    fheal = Random.Range(actionsStatus[i].healValue_min, actionsStatus[i].healValue_max + 1);
                    fheal += targetStatus.maxHP * Random.Range(actionsStatus[i].healPercent_min, actionsStatus[i].healPercent_max) / 100;
                    fheal *= ownerStatus.GHeal / 100;
                    fheal *= targetStatus.RHeal / 100;
                    int heal = Mathf.RoundToInt(fheal);

                    actionStatus.actionTargets[i].Heal(heal, actionStatus.actionOwner);
                }

                if (actionsStatus[i].SANHeal_max > 0)//SAN
                {
                    actionStatus.actionTargets[i].SANHeal(Random.Range(actionsStatus[i].SANHeal_min, actionsStatus[i].SANHeal_max + 1));
                }
                if (actionsStatus[i].SANDamage_max > 0)
                {
                    actionStatus.actionTargets[i].SANDamage(Random.Range(actionsStatus[i].SANDamage_min, actionsStatus[i].SANDamage_max + 1));
                }


                if (actionsStatus[i].shieldAdd_max > 0)//シールド
                {
                    actionStatus.actionTargets[i].AddShield(Random.Range(actionsStatus[i].shieldAdd_min, actionsStatus[i].shieldAdd_max + 1));
                }

                foreach(PA_StatusEffect.StatusEffectParams StEParams in actionsStatus[i].applySteParams)//StE付与
                {
                    if (StEParams.applyChance.Probability()) { actionStatus.actionTargets[i].ApplyStE(StEParams); }
                }
            }
            else { infoText.AddDebugText("対象の消失"); }
        }

        //if (actionStatus.targetType == ActionStatus.TargetType.summon)//移動
        //{ 
        
        //}
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
            print(ownerMoveRange);

            List<Character> charasOnTravelingDir = new List<Character>(FindObjectOfType<CharactersManager>().GetTravelingDirCharas(ownerStatus.position, ownerMoveDir, ownerMoveRange));
            foreach (Character c in charasOnTravelingDir)
            {
                if (c.GetCharacterStatus().size >= 2 || c.GetCharacterStatus().immovable)
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

                actionStatus.actionOwner.ChangePos(moveToPos);//移動処理
                foreach (Character c in charasOnTravelingDir)
                {
                    c.ChangePos(util.GetMoveToPos(c.GetCharacterStatus().position, 3 - ownerMoveDir, 1));
                }
            }
            else
            {
                infoText.AddLogText(string.Format("{0}の移動は阻まれた", ownerStatus.charaName));
            }

        }

        actionQueueManager.Dequeue(actionStatus.actionName);
    }
}
