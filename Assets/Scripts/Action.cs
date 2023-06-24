using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Action : MonoBehaviour
{
    [SerializeField]//tst
    ActionStatus actionStatus;
    ActionQueueManager actionQueueManager;
    [System.Serializable]
    public class ActionStatus
    {
        [Header("アビリティのactionの場合は設定不要")]
        public string actionName;
        [TextArea(3, 10)]
        public string actionInfo;
        //public bool useGeneralInfo;
        [TextArea(3, 10)]
        public string targetInfo;

        [Header("設定しなければ汎用的なオブジェクトになる")]
        public GameObject actionObject;

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
        public int attackRound = 1;
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
        //StE

        public float moveChance;
        public int moveUpper;
        public int moveLower;
        public int moveForword;
        public int moveBackword;
        
        public enum TargetType { other, single, all, self, row, column, singleWoSelf,allWoSelf, random }
        [Header("以下アビリティのみ関係")]
        public TargetType targetType;
        public bool targetPlayerSide;
        public bool targetEnemySide;
        public bool targetEmpty;
        public bool selectableFront;
        public bool selectableMid;
        public bool selectableBack;
        [Header("味方を対象とするアビリティはignoreHideにチェック!!")]
        public bool ignoreHide;

        [Header("以下には手を出すな")]
        public bool abilityEffect;
        public AbilityData.AbilityType abilityType;

        public Character actionOwner;
        public List<Character> actionTargets;

        public string GetInfo(bool refCharaStatus, Character.CharacterStatus characterStatus)
        {
            string s = "";
            if (actionInfo != "") { s += actionInfo + "\n"; }

            s += string.Format("対象：{0}\n", targetInfo);
            if(decreaseHP_max > 0)
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
                if (attackRound > 1) { s += "x" + attackRound.ToString() + "回攻撃"; }
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
            actionInfo = actionData.actionInfo;
            //useGeneralInfo = actionData.useGeneralInfo;

            targetInfo = actionData.targetInfo;

            actionObject = actionData.actionObject;

            decreaseHP_min = actionData.decreaseHP_min;
            decreaseHP_max = actionData.decreaseHP_max;

            cantCounter = actionData.cantCounter;
            attackType = actionData.AttackType;
            ATKMod_min = actionData.ATKMod_min;
            ATKMod_max = actionData.ATKMod_max;
            ACCMod = actionData.ACCMod;
            CRITCMod = actionData.CRITCMod;
            CRITDMod = actionData.CRITDMod;
            attackRound = actionData.attackRound;
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

            moveChance = actionData.moveChance;
            moveUpper = actionData.moveUpper;
            moveLower = actionData.moveLower;
            moveForword = actionData.moveForword;
            moveBackword = actionData.moveBackword;
        }
    }
    Utility util;

    public void Init(ActionQueueManager qm,ActionStatus status,ActionInfoPanel infoPanel,Utility u)
    {
        actionQueueManager = qm;
        actionStatus = status;
        util = u;
        infoPanel.Init(actionStatus.actionName, actionStatus.GetInfo(false, new Character.CharacterStatus()));
    }
    public ActionStatus GetActionStatus() { return actionStatus; }

    public virtual void Resolve()
    {
        Character.CharacterStatus ownerStatus = actionStatus.actionOwner.GetCharacterStatus();
       
        for(int i = 0; i < actionStatus.actionTargets.Count; i++)
        {
            Character.CharacterStatus targetStatus = actionStatus.actionTargets[i].GetCharacterStatus();
            if (actionStatus.decreaseHP_max > 0)//HP減少
            {
                actionStatus.actionTargets[i].DecreaseHP(Random.Range(actionStatus.decreaseHP_min, actionStatus.decreaseHP_max + 1));
            }


            if (actionStatus.ATKMod_max > 0)//攻撃
            {
                bool CRIT = false;
                int DMG = 0;

                if (actionStatus.sureHit || util.Probability(ownerStatus.ACC + actionStatus.ACCMod))
                {
                    if (actionStatus.unevadable || util.Probability(100f - targetStatus.EVD))//攻撃命中
                    {
                        float fDMG = ownerStatus.exATK;
                        float ATKMod = Random.Range(actionStatus.ATKMod_min, actionStatus.ATKMod_max) / 100;
                        fDMG += ownerStatus.ATK * ATKMod;
                        if (util.Probability(ownerStatus.CRITC + actionStatus.CRITCMod))
                        {
                            CRIT = true;
                            fDMG *= ownerStatus.CRITD + actionStatus.CRITDMod;                            
                        }
                        fDMG -= targetStatus.shield;

                        DMG = Mathf.Max(0, Mathf.RoundToInt(fDMG));
                        actionStatus.actionTargets[i].Damage(DMG, CRIT, actionStatus.cantCounter, actionStatus.actionOwner);
                    }
                    else
                    {
                        actionStatus.actionTargets[i].GetCharacter_Object().SetDamageText("Evade", Definer.colorRef.evade);
                        FindObjectOfType<InfoText>().AddLogText(util.GetColoredText(Definer.colorRef.evade, string.Format("{0}は攻撃を回避した", targetStatus.charaName)));
                    }
                }
                else
                {
                    actionStatus.actionOwner.GetCharacter_Object().SetDamageText("Miss", Definer.colorRef.failed_unavailable);
                    FindObjectOfType<InfoText>().AddLogText(util.GetColoredText(Definer.colorRef.failed_unavailable, string.Format("{0}は攻撃を外した", ownerStatus.charaName)));
                }
            }


            if (actionStatus.healPercent_max > 0 || actionStatus.healValue_max > 0)
            {
                float fheal;
                fheal = Random.Range(actionStatus.healValue_min, actionStatus.healValue_max + 1);
                fheal += targetStatus.maxHP * Random.Range(actionStatus.healPercent_min, actionStatus.healPercent_max) / 100;
                fheal *= ownerStatus.GHeal / 100;
                fheal *= targetStatus.RHeal / 100;
                int heal = Mathf.RoundToInt(fheal);

                actionStatus.actionTargets[i].Heal(heal, actionStatus.actionOwner);
            }

            if (actionStatus.SANHeal_max > 0)
            {
                actionStatus.actionTargets[i].SANHeal(Random.Range(actionStatus.SANHeal_min, actionStatus.SANHeal_max + 1));
            }
            if (actionStatus.SANDamage_max > 0)
            {
                actionStatus.actionTargets[i].SANDamage(Random.Range(actionStatus.SANDamage_min, actionStatus.SANDamage_max + 1));
            }


            if (actionStatus.shieldAdd_max > 0)
            {
                actionStatus.actionTargets[i].AddShield(Random.Range(actionStatus.shieldAdd_min, actionStatus.shieldAdd_max + 1));
            }
        }
        actionQueueManager.Dequeue(actionStatus.actionName);
    }
}
