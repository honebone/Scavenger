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

        public AudioClip SE;

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

            SE=data.SE;

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
            actionsStatus[0].SE = SE;
            for (int i = 0; i < actionsStatus.Length; i++)
            {
                actionsStatus[i].actionName = abilityName;
                actionsStatus[i].abilityEffect = true;
                actionsStatus[i].abilityType = abilityType;
                actionsStatus[i].dontChangeSprite = dontChangeSprite;
                actionsStatus[i].activateSprite=activateSprite;
                actionsStatus[i].spriteIndex = spriteIndex;
                        
            }

        }
    }

    Character character;
    CharactersManager charactersManager;
    BattleManager battleManager;
    ActionQueueManager actionQueue;
    Utility util;
    SoundManager soundManager;
    AbilityStatus abilityStatus;

    List<List<int>> targetGroups = new List<List<int>>();
    int counter;

    /// <summary>x.pos y:markedが含まれているか </summary>
    List<Vector2Int> targetIconPos=new List<Vector2Int>();
    List<List<int>> targetPool = new List<List<int>>();//対象の自動決定の際に呼ばれる
    int size;
    bool targetEmpty;


    public void Init(Character chara, AbilityStatus status)
    {
        character = chara;
        abilityStatus= status;

        charactersManager = FindObjectOfType<CharactersManager>();
        battleManager = FindObjectOfType<BattleManager>();
        actionQueue=FindObjectOfType<ActionQueueManager>();
        util = FindObjectOfType<Utility>();
        soundManager=FindObjectOfType<SoundManager>(); 
    }

    public virtual string GetInfo() { return abilityStatus.GetInfo(true, character.GetCharacterStatus()); }
    public virtual void StartSelectTarget()
    {
        charactersManager.ResetAllTargetIcons();
        Character.CharacterStatus charaStatus = character.GetCharacterStatus();
        bool playable = charaStatus.playable;

        targetIconPos = new List<Vector2Int>();
        targetPool = new List<List<int>>();

        switch (abilityStatus.actionsStatus[counter].targetType)
        {
            case Action.ActionStatus.TargetType.other:
                print("特殊な対象の撮り方をするアビリティは、独自のscriptを作ってください!");
                break;
            case Action.ActionStatus.TargetType.single:
                size = 0;
                targetEmpty = false;
                for(int i = 0; i < 18; i++)
                {
                    if (i < 9 && abilityStatus.actionsStatus[counter].targetPlayerSide)
                    {
                        if (i < 3 && abilityStatus.actionsStatus[counter].selectableBack)//潜伏やマークも後々加味すること
                        {
                            //if (playable) { charactersManager.SetTargetIcon(i, false, 0, new List<int>() { i }); }
                            //else { targetPool.Add(new List<int>() { i }); }
                            if (charactersManager.CheckCharaExist(i))
                            {
                                if (charactersManager.GetCharacterWithPos(i).GetCharacterStatus().marked > 0) { targetIconPos.Add(new Vector2Int(i, 1));  }//マークが付与されているのなら、yを１に
                                else { targetIconPos.Add(new Vector2Int(i, 0)); }
                                targetPool.Add(new List<int>() { i });
                            }
                        }
                        if (i >= 3 && i < 6 && abilityStatus.actionsStatus[counter].selectableMid)
                        {
                            if (charactersManager.CheckCharaExist(i))
                            {
                                if (charactersManager.GetCharacterWithPos(i).GetCharacterStatus().marked > 0) { targetIconPos.Add(new Vector2Int(i, 1)); }//マークが付与されているのなら、yを１に
                                else { targetIconPos.Add(new Vector2Int(i, 0)); }
                                targetPool.Add(new List<int>() { i });
                            }
                        }
                        if (i >= 6 && abilityStatus.actionsStatus[counter].selectableFront)
                        {
                            if (charactersManager.CheckCharaExist(i))
                            {
                                if (charactersManager.GetCharacterWithPos(i).GetCharacterStatus().marked > 0) { targetIconPos.Add(new Vector2Int(i, 1)); }//マークが付与されているのなら、yを１に
                                else { targetIconPos.Add(new Vector2Int(i, 0)); }
                                targetPool.Add(new List<int>() { i });
                            }
                        }
                    }
                    if (i >= 9 && abilityStatus.actionsStatus[counter].targetEnemySide)
                    {
                        if (i < 12 && abilityStatus.actionsStatus[counter].selectableFront)
                        {
                            if (charactersManager.CheckCharaExist(i))
                            {
                                if (charactersManager.GetCharacterWithPos(i).GetCharacterStatus().marked > 0) { targetIconPos.Add(new Vector2Int(i, 1)); }//マークが付与されているのなら、yを１に
                                else { targetIconPos.Add(new Vector2Int(i, 0)); }
                                targetPool.Add(new List<int>() { i });
                            }
                        }
                        if (i >= 12 && i < 15 && abilityStatus.actionsStatus[counter].selectableMid)
                        {
                            if (charactersManager.CheckCharaExist(i))
                            {
                                if (charactersManager.GetCharacterWithPos(i).GetCharacterStatus().marked > 0) { targetIconPos.Add(new Vector2Int(i, 1)); }//マークが付与されているのなら、yを１に
                                else { targetIconPos.Add(new Vector2Int(i, 0)); }
                                targetPool.Add(new List<int>() { i });
                            }
                        }
                        if (i >= 15 && abilityStatus.actionsStatus[counter].selectableBack)
                        {
                            if (charactersManager.CheckCharaExist(i))
                            {
                                if (charactersManager.GetCharacterWithPos(i).GetCharacterStatus().marked > 0) { targetIconPos.Add(new Vector2Int(i, 1)); }//マークが付与されているのなら、yを１に
                                else { targetIconPos.Add(new Vector2Int(i, 0)); }
                                targetPool.Add(new List<int>() { i });
                            }
                        }
                    }
                }
                break;
            case Action.ActionStatus.TargetType.move://操作可能キャラのみ
                size = charaStatus.size;
                targetEmpty = true;
                if (!playable) { FindObjectOfType<InfoText>().AddDebugText("error:操作不可のキャラが移動アビリティ使おうとしてるぞ"); }
                foreach(int target in charactersManager.GetMoveTargets(charaStatus.position, charaStatus.size, abilityStatus.actionsStatus[counter].moveValue))
                {
                    //charactersManager.SetTargetIcon(target, true, charaStatus.size, new List<int>() { target });
                    targetIconPos.Add(new Vector2Int(target, 0));
                    targetPool.Add(new List<int>() { target });
                }
                break;
            default:
                print("そのtargetTypeの処理は未実装");
                break;
        }

        //マークの処理
        if (playable)
        {
            for (int i = 0; i < targetPool.Count; i++)//test
            {
                charactersManager.SetTargetIcon(targetIconPos[i].x, targetEmpty, size, targetPool[i]); 
            }
        }
        else { SelectTarget(targetPool[Random.Range(0, targetPool.Count)]); }
    }
    public virtual void SelectTarget(List<int> targetGroup) {
        counter++;
        targetGroups.Add(new List<int>(targetGroup));

        if (counter == abilityStatus.actionsStatus.Length) {//action数分対象の選択をしたら
            battleManager.SetSelectingAbility(false);
            battleManager.SetSelectingTarget(false);
            charactersManager.ResetAllTargetIcons();

            string abilityName = abilityStatus.abilityName.ColorStr(abilityStatus.abilityType.ATToColor());
            FindObjectOfType<InfoText>().AddLogText(string.Format("{0}の<{1}>", character.GetCharacterStatus().charaName, abilityName));

            for (int i = 0; i < abilityStatus.actionsStatus.Length; i++)//行動主や対象を代入し、Enqueue
            {
                abilityStatus.actionsStatus[i].actionOwner = character;
                abilityStatus.actionsStatus[i].actionTargetsInt = new List<int>(targetGroups[i]);
                if (!abilityStatus.actionsStatus[i].targetEmpty && abilityStatus.actionsStatus[i].targetType != Action.ActionStatus.TargetType.move)
                {
                    abilityStatus.actionsStatus[i].actionTargets = new List<Character>(charactersManager.GetExistingCharacters(targetGroups[i], true));
                }
                else
                {
                    abilityStatus.actionsStatus[i].actionTargets = new List<Character>();
                }
                
                
                actionQueue.Enqueue(abilityStatus.actionsStatus[i]);
            }
            actionQueue.StartResolve(3);

            battleManager.ResetSelectedAbility();
        }
        else { StartSelectTarget(); }//まだ選択が残ってるなら
    }
}
