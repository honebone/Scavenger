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
        public string actionName;
        [TextArea(3, 10)]
        public string actionInfo;
        //public bool useGeneralInfo;
        [TextArea(3, 10)]
        public string targetInfo;

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

        [Header("以下には手を出すな")]
        public int actionOwner;
        public int[] actionTargets;

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

            moveChance = actionData.moveChance;
            moveUpper = actionData.moveUpper;
            moveLower = actionData.moveLower;
            moveForword = actionData.moveForword;
            moveBackword = actionData.moveBackword;
        }
    }
    public void Init(ActionQueueManager qm,ActionStatus status,ActionInfoPanel infoPanel)
    {
        actionQueueManager = qm;
        actionStatus = status;
        infoPanel.Init(actionStatus.actionName, actionStatus.GetInfo(false, new Character.CharacterStatus()));
    }

    public virtual void Resolve()
    {
        actionQueueManager.Dequeue(actionStatus.actionName);
    }
}
