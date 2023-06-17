using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability : MonoBehaviour
{
    [System.Serializable]
    public struct AbilityStatus
    {
        public string abilityName;

        public GameObject abilityManager;

        public bool dontChangeSprite;
        public GameObject activateSprite;
        public int spriteIndex;

        public AbilityData.AbilityType abilityType;

        public AbilityData.TargetType targetType;
        public bool targetPlayerSide;
        public bool targetEnemySide;
        public bool targetEmpty;
        public bool selectableFront;
        public bool selectableMid;
        public bool selectableBack;
        public bool ignoreHide;

        public int cooldownOnUse;
        public bool hasRemain;
        public int remainOnBattleStart;
        public int maxRemain;

        public bool availableFront;
        public bool availableMid;
        public bool availableBack;

        public Action.ActionStatus[] actionsStatus;

        public int cooldown;
        public int remain;

        public string GetInfo(bool refCharaStatus, Character.CharacterStatus characterStatus)
        {
            string s = string.Format("種類：{0}\n", Definer.AbiltyTypeName[abilityType]);
            if (cooldownOnUse > 0) { s += string.Format("クールダウン：{0}ターン\n", cooldownOnUse); }
            if (refCharaStatus) { }
            if (hasRemain)
            {
                if (refCharaStatus) { s += string.Format("残り使用回数：{0}回\n", remain); }
                else { s += string.Format("使用回数(戦闘開始時)：{0}回\n", remainOnBattleStart); }
            }
            s += "\n";
            if (actionsStatus.Length == 1) { s += actionsStatus[0].GetInfo(refCharaStatus, characterStatus); }
            else if (actionsStatus.Length > 1)
            {
                int couter =1;
                foreach(Action.ActionStatus actionStatus in actionsStatus)
                {
                    s += string.Format("<効果{0}>\n", couter);
                    s += actionStatus.GetInfo(refCharaStatus, characterStatus);
                    s += "\n";
                    couter++;
                }
            }

            return s;
        }

        public void Init(AbilityData data)
        {
            abilityName = data.abilityName;

            abilityManager = data.abilityManager;

            dontChangeSprite = data.dontChangeSprite;
            activateSprite = data.activateSprite;
            spriteIndex = data.spriteIndex;

            abilityType = data.abilityType;

            targetType = data.targetType;
            targetPlayerSide = data.targetPlayerSide;
            targetEnemySide = data.targetEnemySide;
            targetEmpty = data.targetEmpty;
            selectableFront = data.selectableFront;
            selectableMid = data.selectableMid;
            selectableBack = data.selectableBack;
            ignoreHide = data.ignoreHide;

            cooldownOnUse = data.cooldownOnUse;
            hasRemain = data.hasRemain;
            remainOnBattleStart = data.remainOnBattleStart;
            maxRemain = data.maxRemain;

            availableFront = data.availableFront;
            availableMid = data.availableMid;
            availableBack = data.availableBack;

            actionsStatus = data.actionsStaus;

            //actionsStatus = new Action.ActionStatus[data.actions.Length];
            //for (int i = 0; i < actionsStatus.Length; i++)
            //{
            //    actionsStatus[i] = new Action.ActionStatus();
            //    actionsStatus[i].Init(data.actions[i]);
            //}

        }
    }

    Character character;
    AbilityStatus abilityStatus;
    public void Init(Character chara, AbilityStatus status)
    {
        character = chara;
        abilityStatus= status;
    }

    public virtual string GetInfo() { return abilityStatus.GetInfo(true, character.GetCharacterStatus()); }
    public virtual void StartSelectTarget()
    {
        switch (abilityStatus.targetType)
        {
            case AbilityData.TargetType.other:
                print("特殊な対象の撮り方をするアビリティは、独自のscriptを作ってください!");
                break;
            case AbilityData.TargetType.single:
                if (abilityStatus.targetPlayerSide)
                {
                    if (abilityStatus.selectableBack)
                    {

                    }
                }
                break;
        }
    }
    public virtual void SelectTarget(List<int> targetGroup) { }
    public void Cancel()
    {

    }
    public void Activate()
    {

    }
}
