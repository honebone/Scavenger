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

        //public AbilityData.TargetType targetType;
        //public bool targetPlayerSide;
        //public bool targetEnemySide;
        //public bool targetEmpty;
        //public bool selectableFront;
        //public bool selectableMid;
        //public bool selectableBack;
        //public bool ignoreHide;

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

            //targetType = data.targetType;
            //targetPlayerSide = data.targetPlayerSide;
            //targetEnemySide = data.targetEnemySide;
            //targetEmpty = data.targetEmpty;
            //selectableFront = data.selectableFront;
            //selectableMid = data.selectableMid;
            //selectableBack = data.selectableBack;
            //ignoreHide = data.ignoreHide;

            cooldownOnUse = data.cooldownOnUse;
            hasRemain = data.hasRemain;
            remainOnBattleStart = data.remainOnBattleStart;
            maxRemain = data.maxRemain;

            availableFront = data.availableFront;
            availableMid = data.availableMid;
            availableBack = data.availableBack;

            actionsStatus = data.actionsStaus;

            //actionsStatus = new Action.ActionStatus[data.actions.Length];
            for (int i = 0; i < actionsStatus.Length; i++)
            {
                actionsStatus[i].actionName = abilityName;
                actionsStatus[i].abilityEffect = true;
                actionsStatus[i].abilityType = abilityType;
            }

        }
    }

    Character character;
    CharactersManager charactersManager;
    BattleManager battleManager;
    ActionQueueManager actionQueue;
    Utility util;
    AbilityStatus abilityStatus;

    List<List<int>> targetGroups = new List<List<int>>();
    int counter;

    public void Init(Character chara, AbilityStatus status)
    {
        character = chara;
        abilityStatus= status;

        charactersManager = FindObjectOfType<CharactersManager>();
        battleManager = FindObjectOfType<BattleManager>();
        actionQueue=FindObjectOfType<ActionQueueManager>();
        util = FindObjectOfType<Utility>();
    }

    public virtual string GetInfo() { return abilityStatus.GetInfo(true, character.GetCharacterStatus()); }
    public virtual void StartSelectTarget()
    {
        charactersManager.ResetAllTargetIcons();
        switch (abilityStatus.actionsStatus[counter].targetType)
        {
            case Action.ActionStatus.TargetType.other:
                print("特殊な対象の撮り方をするアビリティは、独自のscriptを作ってください!");
                break;
            case Action.ActionStatus.TargetType.single:
                for(int i = 0; i < 18; i++)
                {
                    if (i < 9 && abilityStatus.actionsStatus[counter].targetPlayerSide)
                    {
                        if (i < 3 && abilityStatus.actionsStatus[counter].selectableBack)
                        {
                            if (charactersManager.CheckCharaExist(i)) { charactersManager.GetCharacterWithPos(i).SetTargetIcon(new List<int>() { i }); }
                        }
                        if (i >= 3 && i < 6 && abilityStatus.actionsStatus[counter].selectableMid)
                        {
                            if (charactersManager.CheckCharaExist(i)) { charactersManager.GetCharacterWithPos(i).SetTargetIcon(new List<int>() { i }); }
                        }
                        if (i >= 6 && abilityStatus.actionsStatus[counter].selectableFront)
                        {
                            if (charactersManager.CheckCharaExist(i)) { charactersManager.GetCharacterWithPos(i).SetTargetIcon(new List<int>() { i }); }
                        }
                    }
                    if (i >= 9 && abilityStatus.actionsStatus[counter].targetEnemySide)
                    {
                        if (i < 12 && abilityStatus.actionsStatus[counter].selectableFront)
                        {
                            if (charactersManager.CheckCharaExist(i)) { charactersManager.GetCharacterWithPos(i).SetTargetIcon(new List<int>() { i }); }
                        }
                        if (i >= 12 && i < 15 && abilityStatus.actionsStatus[counter].selectableMid)
                        {
                            if (charactersManager.CheckCharaExist(i)) { charactersManager.GetCharacterWithPos(i).SetTargetIcon(new List<int>() { i }); }
                        }
                        if (i >= 15 && abilityStatus.actionsStatus[counter].selectableBack)
                        {
                            if (charactersManager.CheckCharaExist(i)) { charactersManager.GetCharacterWithPos(i).SetTargetIcon(new List<int>() { i }); }
                        }
                    }
                }
                break;
            default:
                print("そのtargetTypeの処理は未実装");
                break;
        }
    }
    public virtual void SelectTarget(List<int> targetGroup) {
        counter++;
        targetGroups.Add(new List<int>(targetGroup));

        if (counter == abilityStatus.actionsStatus.Length) {//action数分対象の選択をしたら
            battleManager.SetSelectingAbility(false);
            battleManager.SetSelectingTarget(false);
            charactersManager.ResetAllTargetIcons();

            string abilityName = util.GetColoredText(Definer.colorRef.abilityColors[(int)abilityStatus.abilityType], abilityStatus.abilityName);
            FindObjectOfType<InfoText>().AddLogText(string.Format("{0}の{1}", character.GetCharacterStatus().charaName, abilityName));

            for (int i = 0; i < abilityStatus.actionsStatus.Length; i++)//行動主や対象を代入し、Enqueue
            {
                abilityStatus.actionsStatus[i].actionOwner = character;
                abilityStatus.actionsStatus[i].actionTargets = new List<Character>(charactersManager.GetExistingCharacters(targetGroups[i]));
                
                actionQueue.Enqueue(abilityStatus.actionsStatus[i]);
                actionQueue.StartResolve(3);
            }

            battleManager.ResetSelectedAbility();
        }
        else { StartSelectTarget(); }//まだ選択が残ってるなら
    }
}
