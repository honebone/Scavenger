using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability : MonoBehaviour
{
    [System.Serializable]
    public class AbilityStatus
    {
        public string abilityName;

        public GameObject abilityManager;
        public Ability instantiatedManager;

        public bool dontChangeSprite;
        public GameObject activateSprite;

        //public AudioClip SE;

        public AbilityData.AbilityType abilityType;

        public bool excludeRandomPool;
        public int selectWeight;

        public bool hasSelfCondition;
        public string conditionInfo;
        public CharactersManager.SearchCharaCondition selfCondition;

        public bool freeAction;
        public int cooldownOnBattleStart;
        public int cooldownOnUse;
        public bool hasRemain;
        public int remainOnBattleStart;
        public int maxRemain;

        public bool availableFront;
        public bool availableMid;
        public bool availableBack;

        public Action.ActionStatus[] actionsStatus;

        public bool locked;
        public int unavailable;//PAなどによって操作
        public int cooldown;
        public int remain;
        public int index;

        public AbilityData abilityData;

        //public Character character;

        public string GetInfo(bool refCharaStatus, Character.CharacterStatus characterStatus)
        {
            string s = "";
            if (locked) { s += "(未開放のアビリティ)\n".ColorStr(Definer.colorRef.failed_unavailable); }
                s+= string.Format("種類：{0}\n", Definer.AbiltyTypeName[abilityType].ColorStr(Definer.colorRef.abilityColors[(int)abilityType]));
            string s1 = "";
            string s2 = "";
            if (!refCharaStatus || characterStatus.position < 9)
            {
                s1 = "発動可能列：後-中-前\n";
                s2 = "            ";
                if (availableBack) { s2 += "○-"; }
                else { s2 += "><-"; }
                if (availableMid) { s2 += "○-"; }
                else { s2 += "><-"; }
                if (availableFront) { s2 += "○\n"; }
                else { s2 += "><\n"; }
            }
            else
            {
                s1 = "発動可能列：　　　前-中-後\n";
                s2 = "　　　            ";
                if (availableFront) { s2 += "○-"; }
                else { s2 += "><-"; }
                if (availableMid) { s2 += "○-"; }
                else { s2 += "><-"; }
                if (availableBack) { s2 += "○\n"; }
                else { s2 += "><\n"; }
            }
            s += s1 + s2;
            if (conditionInfo != "") { s += string.Format("発動条件：{0}\n",conditionInfo); }

            if (cooldownOnBattleStart > 0) { s += string.Format("初期クールダウン：{0}ターン\n", cooldownOnBattleStart); }
            if (cooldownOnUse > 0) { s += string.Format("クールダウン：{0}ターン\n", cooldownOnUse); }
            if (refCharaStatus) { }
            if (hasRemain)
            {
                if (refCharaStatus) { s += string.Format("残り使用回数：{0}回\n", remain); }
                else { s += string.Format("使用回数(戦闘開始時)：{0}回\n", remainOnBattleStart); }
            }
            s += "\n";
            if (abilityType == AbilityData.AbilityType.pass)
            {
                s += "ターンをパスする(行動したとはみなされない)\n";
            }
            if (freeAction) { s += "使用してもターンが終了しない\n"; }
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

        public AbilityStatus(AbilityData data,int idx)
        {
            abilityName = data.abilityName;

            abilityManager = data.abilityManager;

            dontChangeSprite = data.dontChangeSprite;
            activateSprite = data.activateSprite;

            //SE=data.SE;

            abilityType = data.abilityType;

            excludeRandomPool = data.excludeRandomPool;
            selectWeight = data.selectWeight;

            hasSelfCondition = data.hasSelfCondition;
            conditionInfo = data.conditionInfo;
            selfCondition = data.selfCondition;

            freeAction = data.freeAction;
            cooldownOnBattleStart = data.cooldownOnBattleStart;
            cooldownOnUse = data.cooldownOnUse;
            hasRemain = data.hasRemain;
            remainOnBattleStart = data.remainOnBattleStart;
            maxRemain = data.maxRemain;

            availableFront = data.availableFront;
            availableMid = data.availableMid;
            availableBack = data.availableBack;

            actionsStatus = data.actionsStaus;

            //actionsStatus = new Action.ActionStatus[data.actions.Length];
            //actionsStatus[0].SE = SE;
            actionsStatus[0].freeAction = freeAction;
            for (int i = 0; i < actionsStatus.Length; i++)
            {
                actionsStatus[i].actionName = abilityName;
                actionsStatus[i].abilityEffect = true;
                actionsStatus[i].abilityType = abilityType;
                actionsStatus[i].dontChangeSprite = dontChangeSprite;
                actionsStatus[i].activateSprite=activateSprite;
            }

            locked = data.lockedDefault;
            index = idx;
            cooldown = cooldownOnBattleStart;
            remain = remainOnBattleStart;

            abilityData = data;
            //character = owner;
        }
       
        public void SetManager(Ability m) { instantiatedManager = m; }

        public void Unlock() { locked = false; }
        public void AddRemain(int value) { remain = Mathf.Clamp(remain + value, 0, maxRemain); }
        public void SetRemain(int value) { remain = Mathf.Clamp(value, 0, maxRemain); }
        public void CoolDown_OnBattleStart() { cooldown = cooldownOnBattleStart; }
        public void CoolDown_OnUse() { cooldown = cooldownOnUse; }
        public void SetCoolDown(int value) { cooldown = Mathf.Max(0, value); }

        public void AddCoolDown(int value) { cooldown = Mathf.Max(0, cooldown + value); }
        //public bool CheckAvailable(Character owner,CharactersManager cm) {
        //    bool atProperPos = false;
        //    bool hasProperTarget = true;
        //    bool properCondition = false; ;
        //    Character.CharacterStatus ownerStatus = owner.GetCharacterStatus();
        //    int column = ownerStatus.position.GetColumn();
        //    if (availableFront && column == 0) { atProperPos = true; }
        //    if (availableMid && column == 1) { atProperPos = true; }
        //    if (availableBack && column == 2) { atProperPos = true; }
        //    //if (BattleManager.inBattle)
        //    //{
        //    //    Ability ability = abilityManager.GetComponent<Ability>();
        //    //    ability.SetRefs(owner, this, cm);
        //    //    for (int i = 0; i < actionsStatus.Length; i++)
        //    //    {
        //    //        if (ability.GetTargetPool(i).Count == 0)
        //    //        {
        //    //            hasProperTarget = false;
        //    //            break;
        //    //        }
        //    //    }
        //    //}
        //    properCondition = !hasSelfCondition || cm.CheckIfMatchCondition(owner, selfCondition);
        //    return !locked && (!hasRemain || remain > 0) && cooldown == 0 && unavailable == 0 && atProperPos && hasProperTarget && properCondition;
        //}

        //public List<string> GetUnavailabeInfo(Character owner, CharactersManager cm,BattleManager bm)
        //{
        //    List<string> info = new List<string>();
        //    Character.CharacterStatus ownerStatus = owner.GetCharacterStatus();
        //    if (!BattleManager.inBattle || !ownerStatus.playable) { return info; }

        //    bool atProperPos = false;
        //    bool hasProperTarget = true;
        //    bool properCondition = false;

           
        //    int column = ownerStatus.position.GetColumn();
        //    if (availableFront && column == 0) { atProperPos = true; }
        //    if (availableMid && column == 1) { atProperPos = true; }
        //    if (availableBack && column == 2) { atProperPos = true; }
        //    //Ability ability = abilityManager.GetComponent<Ability>();
        //    //ability.SetRefs(owner, this, cm);
        //    //for (int i = 0; i < actionsStatus.Length; i++)
        //    //{
        //    //    if (ability.GetTargetPool(i).Count == 0)
        //    //    {
        //    //        hasProperTarget = false;
        //    //        break;
        //    //    }
        //    //}
        //    properCondition = !hasSelfCondition || cm.CheckIfMatchCondition(owner, selfCondition);

        //    if (locked) { info.Add("未解放のアビリティ"); }
        //    if (!bm.checkIfMyTurn(owner)) { info.Add("自身のターンでない"); }
        //    if (hasRemain && remain <= 0) { info.Add("使用可能数0"); }
        //    if (cooldown>0) { info.Add("クールダウン中"); }
        //    if (!atProperPos) { info.Add("発動可能列にいない"); }
        //    if (!hasProperTarget) { info.Add("対象なし"); }
        //    if (!properCondition || unavailable > 0) { info.Add("発動条件を満たしていない"); }


        //    return info;
        //}
        //public bool HasProperTarget(CharactersManager charactersManager,Character actionOwner)//Initせずに使う
        //{
        //    Character.CharacterStatus targetStatus;
        //    bool found = false;
        //    foreach (Action.ActionStatus actionStatus in actionsStatus)
        //    {
        //        found = false;
        //        if (!actionStatus.condition.searchAsPos)//キャラ選択のアビリティ
        //        {
        //            switch (actionStatus.targetType)
        //            {
        //                case Action.ActionStatus.TargetType.other:
        //                    print("特殊な対象の撮り方をするアビリティは、独自のscriptを作ってください!");
        //                    break;
        //                case Action.ActionStatus.TargetType.single://単体対象

        //                    foreach (Character target in charactersManager.SearchCharaWithCondition(actionStatus.condition))
        //                    {
        //                        targetStatus = target.GetCharacterStatus();
        //                        if (targetStatus.hide == 0 || actionStatus.ignoreHide || actionStatus.friendly)
        //                        {
        //                            found = true;
        //                            break;
        //                        }
        //                    }
        //                    if (found) { continue; }
        //                    return false;
        //                case Action.ActionStatus.TargetType.column:

        //                    foreach (Character target in charactersManager.SearchCharaWithCondition(actionStatus.condition))
        //                    {
        //                        targetStatus = target.GetCharacterStatus();
        //                        if (targetStatus.hide == 0 || actionStatus.ignoreHide || actionStatus.friendly)
        //                        {
        //                            found = true;
        //                            break;
        //                        }
        //                    }
        //                    if (found) { continue; }
        //                    return false;
        //                case Action.ActionStatus.TargetType.all:
        //                    foreach (Character target in charactersManager.SearchCharaWithCondition(actionStatus.condition))
        //                    {
        //                        targetStatus = target.GetCharacterStatus();
        //                        if (targetStatus.hide == 0 || actionStatus.ignoreHide || actionStatus.friendly)
        //                        {
        //                            found = true;
        //                            break;
        //                        }
        //                    }
        //                    if (found) { continue; }
        //                    return false;
        //                case Action.ActionStatus.TargetType.singleWoSelf:
        //                    foreach (Character target in charactersManager.SearchCharaWithCondition(actionStatus.condition))
        //                    {
        //                        if (target != actionOwner)
        //                        {
        //                            targetStatus = target.GetCharacterStatus();
        //                            if (targetStatus.hide == 0 || actionStatus.ignoreHide || actionStatus.friendly)
        //                            {
        //                                found = true;
        //                                break;
        //                            }
        //                        }
        //                    }
        //                    if (found) { continue; }
        //                    return false;
        //                case Action.ActionStatus.TargetType.allWoSelf:
        //                    foreach (Character target in charactersManager.SearchCharaWithCondition(actionStatus.condition))
        //                    {
        //                        if (target != actionOwner)
        //                        {
        //                            targetStatus = target.GetCharacterStatus();
        //                            if (targetStatus.hide == 0 || actionStatus.ignoreHide || actionStatus.friendly)
        //                            {
        //                                found = true;
        //                                break;
        //                            }
        //                        }
        //                    }
        //                    if (found) { continue; }
        //                    return false;
        //                case Action.ActionStatus.TargetType.self:
        //                    continue;
        //                case Action.ActionStatus.TargetType.move://操作可能キャラのみ
        //                    continue;
        //                default:
        //                    FindObjectOfType<InfoText>().AddErrorText("そのtargetTypeの処理は未実装");
        //                    break;
        //            }
        //        }
        //        else//ポジション選択のアビリティ
        //        {
        //            switch (actionStatus.targetType)
        //            {
        //                case Action.ActionStatus.TargetType.other:
        //                    print("特殊な対象の撮り方をするアビリティは、独自のscriptを作ってください!");
        //                    break;
        //                case Action.ActionStatus.TargetType.single:
        //                    if (charactersManager.SearchPosWithCondition(actionStatus.condition).Count > 0) { continue; }
        //                    return false;
        //                default:
        //                    FindObjectOfType<InfoText>().AddErrorText("そのtargetTypeの処理は未実装");
        //                    break;
        //            }
        //        }
        //    }
        //    return true;
        //}
    }

   protected Character character;
    protected CharactersManager charactersManager;
    BattleManager battleManager;
    ActionQueueManager actionQueue;
    //Utility util;
    SoundManager soundManager;
    protected AbilityStatus status;

    List<List<int>> targetGroups = new List<List<int>>();
    int counter;

    List<List<int>> targetPool = new List<List<int>>();//対象の自動決定の際に呼ばれる
    //bool targetEmpty;

    public void Init(Character chara, AbilityStatus status)
    {
        character = chara;
        this.status = status;

        charactersManager = FindObjectOfType<CharactersManager>();
        battleManager = FindObjectOfType<BattleManager>();
        actionQueue = FindObjectOfType<ActionQueueManager>();
        soundManager = FindObjectOfType<SoundManager>();
    }

    public virtual string GetInfo() { return status.GetInfo(true, character.GetCharacterStatus()); }
    public virtual Action.ActionStatus ModifyTargetParams(Action.ActionStatus actionStatus) { return actionStatus; }

    public bool CheckAvailable()
    {
        bool atProperPos = false;
        bool hasProperTarget = true;
        bool properCondition = false; ;
        Character.CharacterStatus ownerStatus = character.GetCharacterStatus();
        int column = ownerStatus.position.GetColumn();
        if (status.availableFront && column == 0) { atProperPos = true; }
        if (status.availableMid && column == 1) { atProperPos = true; }
        if (status.availableBack && column == 2) { atProperPos = true; }
        if (BattleManager.inBattle)
        {
            for (int i = 0; i < status.actionsStatus.Length; i++)
            {
                if (GetTargetPool(i).Count == 0)
                {
                    hasProperTarget = false;
                    break;
                }
            }
        }
        properCondition = !status.hasSelfCondition || charactersManager.CheckIfMatchCondition(character, status.selfCondition);
        return !status.locked && (!status.hasRemain || status.remain > 0) && status.cooldown == 0 && status.unavailable == 0 && atProperPos && hasProperTarget && properCondition;
    }
    public List<string> GetUnavailabeInfo()
    {
        List<string> info = new List<string>();
        Character.CharacterStatus ownerStatus = character.GetCharacterStatus();
        if (!BattleManager.inBattle || !ownerStatus.playable) { return info; }

        bool atProperPos = false;
        bool hasProperTarget = true;
        bool properCondition = false;


        int column = ownerStatus.position.GetColumn();
        if (status.availableFront && column == 0) { atProperPos = true; }
        if (status.availableMid && column == 1) { atProperPos = true; }
        if (status.availableBack && column == 2) { atProperPos = true; }
        for (int i = 0; i < status.actionsStatus.Length; i++)
        {
            if (GetTargetPool(i).Count == 0)
            {
                hasProperTarget = false;
                break;
            }
        }
        properCondition = !status.hasSelfCondition || charactersManager.CheckIfMatchCondition(character, status.selfCondition);

        if (status.locked) { info.Add("未解放のアビリティ"); }
        if (!battleManager.checkIfMyTurn(character)) { info.Add("自身のターンでない"); }
        if (status.hasRemain && status.remain <= 0) { info.Add("使用可能数0"); }
        if (status.cooldown > 0) { info.Add("クールダウン中"); }
        if (!atProperPos) { info.Add("発動可能列にいない"); }
        if (!hasProperTarget) { info.Add("対象なし"); }
        if (!properCondition || status.unavailable > 0) { info.Add("発動条件を満たしていない"); }


        return info;
    }

    public void StartSelectTarget()
    {
        Character.CharacterStatus charaStatus = character.GetCharacterStatus();
        targetPool = GetTargetPool(counter);
        if (charaStatus.playable && (targetPool.Count > 1 || counter == 0))
        {
            foreach(List<int> targetGroup in targetPool)
            {
                foreach(int target in targetGroup)
                {
                    charactersManager.SetTargetIcon(target, targetGroup);
                }
            }
        }
        else { SelectTarget(targetPool.Choice()); }
    }
    public virtual List<List<int>> GetTargetPool(int index)//対象候補を返す
    {
        charactersManager.ResetAllTargetIcons();
        Character.CharacterStatus charaStatus = character.GetCharacterStatus();
        Character.CharacterStatus targetStatus;
        Action.ActionStatus actionStatus = ModifyTargetParams(status.actionsStatus[index]);

        List<List<int>> tp_noMark = new List<List<int>>();
        List<List<int>> tp_mark = new List<List<int>>();

        if (!actionStatus.condition.searchAsPos)//キャラ選択のアビリティ
        {
            switch (actionStatus.targetType)
            {
                case Action.ActionStatus.TargetType.other:
                    print("特殊な対象の撮り方をするアビリティは、独自のscriptを作ってください!");
                    break;
                case Action.ActionStatus.TargetType.single://単体対象
                    foreach (Character target in charactersManager.SearchCharaWithCondition(actionStatus.condition))
                    {
                        targetStatus = target.GetCharacterStatus();
                        int pos = targetStatus.position;
                        if (targetStatus.hide == 0 || actionStatus.ignoreHide || actionStatus.friendly)//対象が潜伏じゃないor潜伏無視or友好アビリティ
                        {
                            if (targetStatus.marked > 0 && !(actionStatus.friendly || actionStatus.ignoreMark)) { tp_mark.Add(new List<int> { pos }); }
                            else { tp_noMark.Add(new List<int> { pos }); }
                        }
                    }
                    break;
                case Action.ActionStatus.TargetType.column:
                    List<int> tp_column;
                    bool includeMarked_column;
                    for (int i = 0; i < 3; i++)//各列に対して行う
                    {
                        tp_column = new List<int>();
                        includeMarked_column = false;
                        foreach (Character target in charactersManager.SearchCharaWithCondition(actionStatus.condition))
                        {
                            targetStatus = target.GetCharacterStatus();
                            int pos = targetStatus.position;
                            if (pos < 9 && pos.GetColumn() == i)//プレイヤー側で列がiと等しい
                            {
                                if (targetStatus.hide == 0 || actionStatus.ignoreHide || actionStatus.friendly)//対象が潜伏じゃないor潜伏無視or友好アビリティ
                                {
                                    if (targetStatus.marked > 0 && !(actionStatus.friendly || actionStatus.ignoreMark)) { includeMarked_column = true; }
                                    tp_column.Add(pos);
                                }
                            }
                        }
                        if (tp_column.Count > 0)
                        {
                            if (includeMarked_column) { tp_mark.Add(tp_column); }
                            else { tp_noMark.Add(tp_column); }
                        }
                    }
                    for (int i = 0; i < 3; i++)//各列に対して行う
                    {
                        tp_column = new List<int>();
                        includeMarked_column = false;
                        foreach (Character target in charactersManager.SearchCharaWithCondition(actionStatus.condition))
                        {
                            targetStatus = target.GetCharacterStatus();
                            int pos = targetStatus.position;
                            if (pos >= 9 && pos.GetColumn() == i)//エネミー側で列がiと等しい
                            {
                                if (targetStatus.hide == 0 || actionStatus.ignoreHide || actionStatus.friendly)//対象が潜伏じゃないor潜伏無視or友好アビリティ
                                {
                                    if (targetStatus.marked > 0 && !(actionStatus.friendly || actionStatus.ignoreMark)) { includeMarked_column = true; }
                                    tp_column.Add(pos);
                                }
                            }
                        }
                        if (tp_column.Count > 0)
                        {
                            if (includeMarked_column) { tp_mark.Add(tp_column); }
                            else { tp_noMark.Add(tp_column); }
                        }
                    }

                    break;
                case Action.ActionStatus.TargetType.all:
                    List<int> tp_all = new List<int>();
                    foreach (Character target in charactersManager.SearchCharaWithCondition(actionStatus.condition))
                    {
                        targetStatus = target.GetCharacterStatus();
                        if (targetStatus.hide == 0 || actionStatus.ignoreHide || actionStatus.friendly)
                        {
                            tp_all.Add(targetStatus.position);
                        }
                    }
                    tp_noMark.Add(tp_all);

                    break;
                case Action.ActionStatus.TargetType.singleWoSelf:
                    foreach (Character target in charactersManager.SearchCharaWithCondition(actionStatus.condition))
                    {
                        if (target != character)
                        {
                            targetStatus = target.GetCharacterStatus();
                            int pos = targetStatus.position;
                            if (targetStatus.hide == 0 || actionStatus.ignoreHide || actionStatus.friendly)//対象が潜伏じゃないor潜伏無視or友好アビリティ
                            {
                                if (targetStatus.marked > 0 && !(actionStatus.friendly || actionStatus.ignoreMark)) { tp_mark.Add(new List<int> { pos }); }
                                else { tp_noMark.Add(new List<int> { pos }); }
                            }
                        }
                    }
                    break;
                case Action.ActionStatus.TargetType.allWoSelf:
                    List<int> tp_allWoSelf = new List<int>();
                    foreach (Character target in charactersManager.SearchCharaWithCondition(actionStatus.condition))
                    {
                        if (target != character)
                        {
                            targetStatus = target.GetCharacterStatus();
                            if (targetStatus.hide == 0 || actionStatus.ignoreHide || actionStatus.friendly)
                            {
                                tp_allWoSelf.Add(targetStatus.position);
                            }
                        }
                    }
                    tp_noMark.Add(tp_allWoSelf);
                    break;
                case Action.ActionStatus.TargetType.self:
                    tp_noMark.Add(new List<int> { charaStatus.position });
                    break;

                case Action.ActionStatus.TargetType.move://操作可能キャラのみ
                    
                    foreach (int target in charactersManager.GetMoveTargets(charaStatus.position, actionStatus.moveValue))
                    {
                        tp_noMark.Add(new List<int>() { target });
                    }
                    break;
                default:
                    print("そのtargetTypeの処理は未実装");
                    break;
            }
        }
        else//ポジション選択のアビリティ
        {
            //targetEmpty = true;
            switch (actionStatus.targetType)
            {
                case Action.ActionStatus.TargetType.other:
                    print("特殊な対象の撮り方をするアビリティは、独自のscriptを作ってください!");
                    break;
                case Action.ActionStatus.TargetType.single:
                    foreach (int i in charactersManager.SearchPosWithCondition(actionStatus.condition))
                    {
                        tp_noMark.Add(new List<int>() { i });
                    }
                    break;
                case Action.ActionStatus.TargetType.column:

                    foreach (int i in charactersManager.SearchPosWithCondition(actionStatus.condition))
                    {
                        if (i.GetRow() == 1)//対象候補のうち、中段にあるもののみを検出
                        {
                            tp_noMark.Add(new List<int>() { i - 1, i, i + 1 });//その列の下段、上段はi-1、i+1で表される
                        }
                    }
                    break;
                case Action.ActionStatus.TargetType.all:

                    tp_noMark.Add(new List<int>(charactersManager.SearchPosWithCondition(actionStatus.condition)));
                    break;
            }
        }

        if(tp_mark.Count > 0) { return tp_mark; }//マークを含む対象群があるならそれを返す
        else { return tp_noMark; }//そうでないならマークを含まない対象群を返す
    }

    //public virtual void StartSelectTarget()
    //{
    //    charactersManager.ResetAllTargetIcons();
    //    Character.CharacterStatus charaStatus = character.GetCharacterStatus();
    //    Character.CharacterStatus targetStatus;
    //    Action.ActionStatus actionStatus = ModifyTargetParams(abilityStatus.actionsStatus[counter]);
    //    bool playable = charaStatus.playable;

    //    targetIconPos = new List<Vector2Int>();
    //    targetPool = new List<List<int>>();
    //    if (!actionStatus.condition.searchAsPos)//キャラ選択のアビリティ
    //    {
    //        switch (actionStatus.targetType)
    //        {
    //            case Action.ActionStatus.TargetType.other:
    //                print("特殊な対象の撮り方をするアビリティは、独自のscriptを作ってください!");
    //                break;
    //            case Action.ActionStatus.TargetType.single://単体対象
    //                //targetEmpty = false;
                    
    //                foreach(Character target in charactersManager.SearchCharaWithCondition(actionStatus.condition))
    //                {
    //                    targetStatus= target.GetCharacterStatus();
    //                    int pos = targetStatus.position;
    //                    if (targetStatus.hide == 0||actionStatus.ignoreHide || actionStatus.friendly)//対象が潜伏じゃないor潜伏無視or友好アビリティ
    //                    {
    //                        if (targetStatus.marked > 0 && !actionStatus.friendly) 
    //                        { targetIconPos.Add(new Vector2Int(pos, 1)); }
    //                        else { targetIconPos.Add(new Vector2Int(pos, 0)); }
    //                        targetPool.Add(new List<int>() { pos });
    //                    }
    //                }
    //                break;
    //            case Action.ActionStatus.TargetType.column:
    //                //targetEmpty = false;
    //                List<int> tp_column = new List<int>();
    //                int includeMarked_column = 0;
    //                for (int i = 0; i < 3; i++)//各列に対して行う
    //                {
    //                    tp_column = new List<int>();
    //                    includeMarked_column = 0;
    //                    foreach (Character target in charactersManager.SearchCharaWithCondition(actionStatus.condition))
    //                    {
    //                        targetStatus = target.GetCharacterStatus();
    //                        int pos = targetStatus.position;
    //                        if (pos < 9 && pos.GetColumn() == i)//プレイヤー側で列がiと等しい
    //                        {
    //                            if (targetStatus.hide == 0 || actionStatus.ignoreHide || actionStatus.friendly)//対象が潜伏じゃないor潜伏無視or友好アビリティ
    //                            {
    //                                if (targetStatus.marked > 0 && !actionStatus.friendly) { includeMarked_column = 1; }
    //                                tp_column.Add(pos);
    //                            }
    //                        }
    //                    }
    //                    foreach (int p in tp_column) { targetIconPos.Add(new Vector2Int(p, includeMarked_column)); }
    //                    for (int j = 0; j < tp_column.Count; j++) { targetPool.Add(tp_column); }
    //                }
    //                for (int i = 0; i < 3; i++)//各列に対して行う
    //                {
    //                    tp_column = new List<int>();
    //                    includeMarked_column = 0;
    //                    foreach (Character target in charactersManager.SearchCharaWithCondition(actionStatus.condition))
    //                    {
    //                        targetStatus = target.GetCharacterStatus();
    //                        int pos = targetStatus.position;
    //                        if (pos >= 9 && pos.GetColumn() == i)//エネミー側で列がiと等しい
    //                        {
    //                            if (targetStatus.hide == 0 || actionStatus.ignoreHide || actionStatus.friendly)//対象が潜伏じゃないor潜伏無視or友好アビリティ
    //                            {
    //                                if (targetStatus.marked > 0 && !actionStatus.friendly) { includeMarked_column = 1; }
    //                                tp_column.Add(pos);
    //                            }
    //                        }
    //                    }
    //                    foreach (int p in tp_column) { targetIconPos.Add(new Vector2Int(p, includeMarked_column)); }
    //                    for (int j = 0; j < tp_column.Count; j++) { targetPool.Add(tp_column); }
    //                }
    //                break;
    //            case Action.ActionStatus.TargetType.all:
    //                //targetEmpty = true;
    //                List<int> tp = new List<int>();
    //                //int iconPos = charaStatus.position;
    //                foreach (Character target in charactersManager.SearchCharaWithCondition(actionStatus.condition))
    //                {
    //                    targetStatus = target.GetCharacterStatus();
    //                    if (targetStatus.hide == 0 || actionStatus.ignoreHide || actionStatus.friendly)
    //                    {
    //                        int pos = targetStatus.position;
    //                        tp.Add(targetStatus.position);
    //                        targetIconPos.Add(new Vector2Int(pos, 0));
    //                    }
    //                }
    //                for(int i = 0; i < tp.Count; i++) { targetPool.Add(tp); }

    //                break;
    //            case Action.ActionStatus.TargetType.singleWoSelf:
    //                //targetEmpty = false;

    //                foreach (Character target in charactersManager.SearchCharaWithCondition(actionStatus.condition))
    //                {
    //                    targetStatus = target.GetCharacterStatus();
    //                    int pos = targetStatus.position;

    //                    if (target != character)//自分自身を除く
    //                    {
    //                        if (targetStatus.hide == 0 || actionStatus.ignoreHide || actionStatus.friendly)//対象が潜伏じゃないor潜伏無視or友好アビリティ
    //                        {
    //                            if (targetStatus.marked > 0 && !actionStatus.friendly)
    //                            { targetIconPos.Add(new Vector2Int(pos, 1)); }
    //                            else { targetIconPos.Add(new Vector2Int(pos, 0)); }
    //                            targetPool.Add(new List<int>() { pos });
    //                        }
    //                    }
    //                }
    //                break;
    //            case Action.ActionStatus.TargetType.allWoSelf:
    //                //targetEmpty = true;
    //                List<int> tp_allWoself = new List<int>();
    //                //int iconPos = charaStatus.position;
    //                foreach (Character target in charactersManager.SearchCharaWithCondition(actionStatus.condition))
    //                {
    //                    if (target != character)//自分自身を除く
    //                    {
    //                        targetStatus = target.GetCharacterStatus();
    //                        if (targetStatus.hide == 0 || actionStatus.ignoreHide || actionStatus.friendly)
    //                        {
    //                            int pos = targetStatus.position;
    //                            tp_allWoself.Add(targetStatus.position);
    //                            targetIconPos.Add(new Vector2Int(pos, 0));
    //                        }
    //                    }
    //                }
    //                for (int i = 0; i < tp_allWoself.Count; i++) { targetPool.Add(tp_allWoself); }
    //                break;
    //            case Action.ActionStatus.TargetType.self:
    //                //targetEmpty = false;

    //                targetIconPos.Add(new Vector2Int(charaStatus.position, 0));
    //                targetPool.Add(new List<int> { charaStatus.position});
    //                break;

    //            case Action.ActionStatus.TargetType.move://操作可能キャラのみ
    //                //targetEmpty = true;
    //                if (!playable) { FindObjectOfType<InfoText>().AddDebugText("error:操作不可のキャラが移動アビリティ使おうとしてるぞ"); }
    //                foreach (int target in charactersManager.GetMoveTargets(charaStatus.position, actionStatus.moveValue))
    //                {
    //                    //charactersManager.SetTargetIcon(target, true, charaStatus.size, new List<int>() { target });
    //                    targetIconPos.Add(new Vector2Int(target, 0));
    //                    targetPool.Add(new List<int>() { target });
    //                }
    //                break;
    //            default:
    //                print("そのtargetTypeの処理は未実装");
    //                break;
    //        }
    //    }
    //    else//ポジション選択のアビリティ
    //    {
    //        //targetEmpty = true;
    //        switch (actionStatus.targetType)
    //        {
    //            case Action.ActionStatus.TargetType.other:
    //                print("特殊な対象の撮り方をするアビリティは、独自のscriptを作ってください!");
    //                break;
    //            case Action.ActionStatus.TargetType.single:
    //                foreach(int i in charactersManager.SearchPosWithCondition(actionStatus.condition))
    //                {
    //                    targetIconPos.Add(new Vector2Int(i, 0));
    //                    targetPool.Add(new List<int>() { i });
    //                }
    //                break;
    //        }
    //    }
       

    //    //マークの処理
    //    List<Vector2Int> targetPos = new List<Vector2Int>();//x:pos y:posのindex
    //    if (!abilityStatus.actionsStatus[counter].ignoreMark)//マークを考慮するなら
    //    {
    //        for (int i = 0; i < targetIconPos.Count; i++)//y==1(マークが付与されている)対象がいるなら　それらのみを抽出
    //        {
    //            if (targetIconPos[i].y == 1) { targetPos.Add(new Vector2Int(targetIconPos[i].x, i)); }
    //        }
    //        if (targetPos.Count == 0)//マークが付与されている対象がいないなら、全ての対称を候補に
    //        {
    //            for (int i = 0; i < targetIconPos.Count; i++)
    //            {
    //                targetPos.Add(new Vector2Int(targetIconPos[i].x, i));
    //            }

    //        }
    //    }
    //    else//マークを無視するなら全ての対称を候補に
    //    {
    //        for (int i = 0; i < targetIconPos.Count; i++)
    //        {
    //            targetPos.Add(new Vector2Int(targetIconPos[i].x, i));
    //        }
    //    }

    //    if (playable && (targetPos.Count > 1 || counter == 0))
    //    {
    //        for (int i = 0; i < targetPos.Count; i++)//test
    //        {
    //            charactersManager.SetTargetIcon(targetPos[i].x, targetPool[targetPos[i].y]);//, targetEmpty
    //        }
    //    }
    //    else { SelectTarget(targetPool[targetPos[Random.Range(0, targetPos.Count)].y]); }
    //}

   

    public virtual void SelectTarget(List<int> targetGroup) {
        counter++;
        targetGroups.Add(new List<int>(targetGroup));

        if (counter == status.actionsStatus.Length) {//action数分対象の選択をしたら
            battleManager.SetSelectingAbility(false);
            battleManager.SetSelectingTarget(false);
            charactersManager.ResetAllTargetIcons();

            string abilityName = status.abilityName.ColorStr(status.abilityType.ToColor());
            FindObjectOfType<InfoText>().AddLogText(string.Format("○{0}の<{1}>", character.GetCharacterStatus().charaName, abilityName));

            character.Ability_StartCoolDown(status.index);
            if (status.hasRemain) { character.Ability_AddRemain(-1, status.index); }

            for (int i = 0; i < status.actionsStatus.Length; i++)//行動主や対象を代入し、Enqueue
            {
                status.actionsStatus[i].index = i;
                status.actionsStatus[i].actionOwner = character;
                status.actionsStatus[i].actionTargetsInt = new List<int>(targetGroups[i]);
                if (!status.actionsStatus[i].condition.searchAsPos && status.actionsStatus[i].targetType != Action.ActionStatus.TargetType.move)//対象がキャラであるアビリティの場合は、actionTargetsの設定
                {
                    status.actionsStatus[i].actionTargets = new List<Character>(charactersManager.GetExistingCharacters(targetGroups[i], true));
                }//そうでない場合actionTargetsがnullとなるが、そのケアはActionのReaolve内で行っている

                actionQueue.Enqueue(status.actionsStatus[i]);
            }
            //character.OnActivateAbility();

            ResetValue();
            battleManager.ResetSelectedAbility();

            actionQueue.StartResolve(3);
        }
        else { StartSelectTarget(); }//まだ選択が残ってるなら
    }

    public void ResetValue()
    {
        counter = 0;
        targetPool = new List<List<int>>();
        targetGroups = new List<List<int>>();
    }
}
