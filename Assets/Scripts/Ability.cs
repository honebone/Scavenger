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

        public bool dontChangeSprite;
        public GameObject activateSprite;

        //public AudioClip SE;

        public AbilityData.AbilityType abilityType;

        public bool excludeRandomPool;
        public int selectWeight;

        public bool hasSelfCondition;
        public string conditionInfo;
        public CharactersManager.SearchCharaCondition selfCondition;

        public int cooldownOnBattleStart;
        public int cooldownOnUse;
        public bool hasRemain;
        public int remainOnBattleStart;
        public int maxRemain;

        public bool availableFront;
        public bool availableMid;
        public bool availableBack;

        public Action.ActionStatus[] actionsStatus;

        public int unavailable;//PAなどによって操作
        public int cooldown;
        public int remain;
        public int index;

        public AbilityData abilityData;

        //public Character character;

        public string GetInfo(bool refCharaStatus, Character.CharacterStatus characterStatus)
        {
            string s = string.Format("種類：{0}\n", Definer.AbiltyTypeName[abilityType].ColorStr(Definer.colorRef.abilityColors[(int)abilityType]));
            s += ("発動可能列：");
            if (!refCharaStatus || characterStatus.position < 9)
            {
                if (availableBack) { s += "○-"; }
                else { s += "×-"; }
                if (availableMid) { s += "○-"; }
                else { s += "×-"; }
                if (availableFront) { s += "○\n"; }
                else { s += "×\n"; }
            }
            else
            {
                s += "　　　";
                if (availableFront) { s += "○-"; }
                else { s += "×-"; }
                if (availableMid) { s += "○-"; }
                else { s += "×-"; }
                if (availableBack) { s += "○\n"; }
                else { s += "×\n"; }
            }
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
            for (int i = 0; i < actionsStatus.Length; i++)
            {
                actionsStatus[i].actionName = abilityName;
                actionsStatus[i].abilityEffect = true;
                actionsStatus[i].abilityType = abilityType;
                actionsStatus[i].dontChangeSprite = dontChangeSprite;
                actionsStatus[i].activateSprite=activateSprite;
                        
            }

            index = idx;
            cooldown = cooldownOnBattleStart;
            remain=remainOnBattleStart;

            abilityData = data;
            //character = owner;
        }
       
        public void AddRemain(int value) { remain = Mathf.Clamp(remain + value, 0, maxRemain); }
        public void SetRemain(int value) { remain = Mathf.Clamp(value, 0, maxRemain); }
        public void StartCoolDown() { cooldown = cooldownOnUse; }
        public void AddCoolDown(int value) { cooldown += value; }
        public bool CheckAvailable(Character owner,CharactersManager cm) {
            bool atProperPos = false;
            bool hasProperTarget = false;
            bool properCondition = false; ;
            Character.CharacterStatus ownerStatus = owner.GetCharacterStatus();
            int column = ownerStatus.position.GetColumn();
            if (availableFront && column == 0) { atProperPos = true; }
            if (availableMid && column == 1) { atProperPos = true; }
            if (availableBack && column == 2) { atProperPos = true; }
            hasProperTarget = !BattleManager.inBattle || HasProperTarget(cm,owner);
            properCondition = !hasSelfCondition || cm.CheckIfMatchCondition(owner, selfCondition);
            return (!hasRemain || remain > 0) && cooldown == 0 && unavailable == 0 && atProperPos && hasProperTarget && properCondition;
        }
        public bool HasProperTarget(CharactersManager charactersManager,Character actionOwner)//Initせずに使う
        {
            Character.CharacterStatus targetStatus;
            bool found = false;
            foreach (Action.ActionStatus actionStatus in actionsStatus)
            {
                found = false;
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
                                if (targetStatus.hide == 0 || actionStatus.ignoreHide || actionStatus.friendly)
                                {
                                    found = true;
                                    break;
                                }
                            }
                            if (found) { continue; }
                            return false;
                        case Action.ActionStatus.TargetType.column:

                            foreach (Character target in charactersManager.SearchCharaWithCondition(actionStatus.condition))
                            {
                                targetStatus = target.GetCharacterStatus();
                                if (targetStatus.hide == 0 || actionStatus.ignoreHide || actionStatus.friendly)
                                {
                                    found = true;
                                    break;
                                }
                            }
                            if (found) { continue; }
                            return false;
                        case Action.ActionStatus.TargetType.all:
                            foreach (Character target in charactersManager.SearchCharaWithCondition(actionStatus.condition))
                            {
                                targetStatus = target.GetCharacterStatus();
                                if (targetStatus.hide == 0 || actionStatus.ignoreHide || actionStatus.friendly)
                                {
                                    found = true;
                                    break;
                                }
                            }
                            if (found) { continue; }
                            return false;
                        case Action.ActionStatus.TargetType.allWoSelf:
                            foreach (Character target in charactersManager.SearchCharaWithCondition(actionStatus.condition))
                            {
                                if (target != actionOwner)
                                {
                                    targetStatus = target.GetCharacterStatus();
                                    if (targetStatus.hide == 0 || actionStatus.ignoreHide || actionStatus.friendly)
                                    {
                                        found = true;
                                        break;
                                    }
                                }
                            }
                            if (found) { continue; }
                            return false;
                        case Action.ActionStatus.TargetType.self:
                            continue;
                        case Action.ActionStatus.TargetType.move://操作可能キャラのみ
                            continue;
                        default:
                            FindObjectOfType<InfoText>().AddErrorText("そのtargetTypeの処理は未実装");
                            break;
                    }
                }
                else//ポジション選択のアビリティ
                {
                    switch (actionStatus.targetType)
                    {
                        case Action.ActionStatus.TargetType.other:
                            print("特殊な対象の撮り方をするアビリティは、独自のscriptを作ってください!");
                            break;
                        case Action.ActionStatus.TargetType.single:
                            if (charactersManager.SearchPosWithCondition(actionStatus.condition).Count > 0) { continue; }
                            return false;
                        default:
                            FindObjectOfType<InfoText>().AddErrorText("そのtargetTypeの処理は未実装");
                            break;
                    }
                }
            }
            return true;
        }
    }

   protected Character character;
    protected CharactersManager charactersManager;
    BattleManager battleManager;
    ActionQueueManager actionQueue;
    //Utility util;
    SoundManager soundManager;
    protected AbilityStatus abilityStatus;

    List<List<int>> targetGroups = new List<List<int>>();
    int counter;

    /// <summary>x.pos y:markedが含まれているか </summary>
    List<Vector2Int> targetIconPos=new List<Vector2Int>();
    List<List<int>> targetPool = new List<List<int>>();//対象の自動決定の際に呼ばれる
    bool targetEmpty;


    public void Init(Character chara, AbilityStatus status)
    {
        character = chara;
        abilityStatus= status;

        charactersManager = FindObjectOfType<CharactersManager>();
        battleManager = FindObjectOfType<BattleManager>();
        actionQueue=FindObjectOfType<ActionQueueManager>();
        //util = FindObjectOfType<Utility>();
        soundManager=FindObjectOfType<SoundManager>(); 
    }

    public virtual string GetInfo() { return abilityStatus.GetInfo(true, character.GetCharacterStatus()); }
    public virtual Action.ActionStatus ModifyTargetParams(Action.ActionStatus actionStatus) { return actionStatus; }
    public virtual void StartSelectTarget()
    {
        charactersManager.ResetAllTargetIcons();
        Character.CharacterStatus charaStatus = character.GetCharacterStatus();
        Character.CharacterStatus targetStatus;
        Action.ActionStatus actionStatus = ModifyTargetParams(abilityStatus.actionsStatus[counter]);
        bool playable = charaStatus.playable;

        targetIconPos = new List<Vector2Int>();
        targetPool = new List<List<int>>();
        if (!actionStatus.condition.searchAsPos)//キャラ選択のアビリティ
        {
            switch (actionStatus.targetType)
            {
                case Action.ActionStatus.TargetType.other:
                    print("特殊な対象の撮り方をするアビリティは、独自のscriptを作ってください!");
                    break;
                case Action.ActionStatus.TargetType.single://単体対象
                    targetEmpty = false;
                    
                    foreach(Character target in charactersManager.SearchCharaWithCondition(actionStatus.condition))
                    {
                        targetStatus= target.GetCharacterStatus();
                        int pos = targetStatus.position;
                        if (targetStatus.hide == 0||actionStatus.ignoreHide || actionStatus.friendly)//対象が潜伏じゃないor潜伏無視or友好アビリティ
                        {
                            if (targetStatus.marked > 0 && !actionStatus.friendly) 
                            { targetIconPos.Add(new Vector2Int(pos, 1)); }
                            else { targetIconPos.Add(new Vector2Int(pos, 0)); }
                            targetPool.Add(new List<int>() { pos });
                        }
                    }
                    break;
                case Action.ActionStatus.TargetType.column:
                    targetEmpty = false;
                    List<int> tp_column = new List<int>();
                    int includeMarked_column = 0;
                    for (int i = 0; i < 3; i++)//各列に対して行う
                    {
                        tp_column = new List<int>();
                        includeMarked_column = 0;
                        foreach (Character target in charactersManager.SearchCharaWithCondition(actionStatus.condition))
                        {
                            targetStatus = target.GetCharacterStatus();
                            int pos = targetStatus.position;
                            if (pos < 9 && pos.GetColumn() == i)//プレイヤー側で列がiと等しい
                            {
                                if (targetStatus.hide == 0 || actionStatus.ignoreHide || actionStatus.friendly)//対象が潜伏じゃないor潜伏無視or友好アビリティ
                                {
                                    if (targetStatus.marked > 0 && !actionStatus.friendly) { includeMarked_column = 1; }
                                    tp_column.Add(pos);
                                }
                            }
                        }
                        foreach (int p in tp_column) { targetIconPos.Add(new Vector2Int(p, includeMarked_column)); }
                        for (int j = 0; j < tp_column.Count; j++) { targetPool.Add(tp_column); }
                    }
                    for (int i = 0; i < 3; i++)//各列に対して行う
                    {
                        tp_column = new List<int>();
                        includeMarked_column = 0;
                        foreach (Character target in charactersManager.SearchCharaWithCondition(actionStatus.condition))
                        {
                            targetStatus = target.GetCharacterStatus();
                            int pos = targetStatus.position;
                            if (pos >= 9 && pos.GetColumn() == i)//エネミー側で列がiと等しい
                            {
                                if (targetStatus.hide == 0 || actionStatus.ignoreHide || actionStatus.friendly)//対象が潜伏じゃないor潜伏無視or友好アビリティ
                                {
                                    if (targetStatus.marked > 0 && !actionStatus.friendly) { includeMarked_column = 1; }
                                    tp_column.Add(pos);
                                }
                            }
                        }
                        foreach (int p in tp_column) { targetIconPos.Add(new Vector2Int(p, includeMarked_column)); }
                        for (int j = 0; j < tp_column.Count; j++) { targetPool.Add(tp_column); }
                    }
                    break;
                case Action.ActionStatus.TargetType.all:
                    targetEmpty = true;
                    List<int> tp = new List<int>();
                    //int iconPos = charaStatus.position;
                    foreach (Character target in charactersManager.SearchCharaWithCondition(actionStatus.condition))
                    {
                        targetStatus = target.GetCharacterStatus();
                        if (targetStatus.hide == 0 || actionStatus.ignoreHide || actionStatus.friendly)
                        {
                            int pos = targetStatus.position;
                            tp.Add(targetStatus.position);
                            targetIconPos.Add(new Vector2Int(pos, 0));
                        }
                    }
                    for(int i = 0; i < tp.Count; i++) { targetPool.Add(tp); }

                    break;
                case Action.ActionStatus.TargetType.allWoSelf:
                    targetEmpty = true;
                    List<int> tp_allWoself = new List<int>();
                    //int iconPos = charaStatus.position;
                    foreach (Character target in charactersManager.SearchCharaWithCondition(actionStatus.condition))
                    {
                        if (target != character)//自分自身を除く
                        {
                            targetStatus = target.GetCharacterStatus();
                            if (targetStatus.hide == 0 || actionStatus.ignoreHide || actionStatus.friendly)
                            {
                                int pos = targetStatus.position;
                                tp_allWoself.Add(targetStatus.position);
                                targetIconPos.Add(new Vector2Int(pos, 0));
                            }
                        }
                    }
                    for (int i = 0; i < tp_allWoself.Count; i++) { targetPool.Add(tp_allWoself); }
                    break;
                case Action.ActionStatus.TargetType.self:
                    targetEmpty = false;

                    targetIconPos.Add(new Vector2Int(charaStatus.position, 0));
                    targetPool.Add(new List<int> { charaStatus.position});
                    break;

                case Action.ActionStatus.TargetType.move://操作可能キャラのみ
                    targetEmpty = true;
                    if (!playable) { FindObjectOfType<InfoText>().AddDebugText("error:操作不可のキャラが移動アビリティ使おうとしてるぞ"); }
                    foreach (int target in charactersManager.GetMoveTargets(charaStatus.position, actionStatus.moveValue))
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
        }
        else//ポジション選択のアビリティ
        {
            targetEmpty = true;
            switch (actionStatus.targetType)
            {
                case Action.ActionStatus.TargetType.other:
                    print("特殊な対象の撮り方をするアビリティは、独自のscriptを作ってください!");
                    break;
                case Action.ActionStatus.TargetType.single:
                    foreach(int i in charactersManager.SearchPosWithCondition(actionStatus.condition))
                    {
                        targetIconPos.Add(new Vector2Int(i, 0));
                        targetPool.Add(new List<int>() { i });
                    }
                    break;
            }
        }
       

        //マークの処理
        List<Vector2Int> targetPos = new List<Vector2Int>();//x:pos y:posのindex
        if (!abilityStatus.actionsStatus[counter].ignoreMark)//マークを考慮するなら
        {
            for (int i = 0; i < targetIconPos.Count; i++)//y==1(マークが付与されている)対象がいるなら　それらのみを抽出
            {
                if (targetIconPos[i].y == 1) { targetPos.Add(new Vector2Int(targetIconPos[i].x, i)); }
            }
            if (targetPos.Count == 0)//マークが付与されている対象がいないなら、全ての対称を候補に
            {
                for (int i = 0; i < targetIconPos.Count; i++)
                {
                    targetPos.Add(new Vector2Int(targetIconPos[i].x, i));
                }

            }
        }
        else//マークを無視するなら全ての対称を候補に
        {
            for (int i = 0; i < targetIconPos.Count; i++)
            {
                targetPos.Add(new Vector2Int(targetIconPos[i].x, i));
            }
        }
       
        if (playable)
        {
            for (int i = 0; i < targetPos.Count; i++)//test
            {
                charactersManager.SetTargetIcon(targetPos[i].x, targetEmpty, targetPool[targetPos[i].y]); 
            }
        }
        else { SelectTarget(targetPool[targetPos[Random.Range(0, targetPos.Count)].y]); }
    }
    public virtual void SelectTarget(List<int> targetGroup) {
        counter++;
        targetGroups.Add(new List<int>(targetGroup));

        if (counter == abilityStatus.actionsStatus.Length) {//action数分対象の選択をしたら
            battleManager.SetSelectingAbility(false);
            battleManager.SetSelectingTarget(false);
            charactersManager.ResetAllTargetIcons();

            string abilityName = abilityStatus.abilityName.ColorStr(abilityStatus.abilityType.ToColor());
            FindObjectOfType<InfoText>().AddLogText(string.Format("○{0}の<{1}>", character.GetCharacterStatus().charaName, abilityName));

            character.Ability_StartCoolDown(abilityStatus.index);
            if (abilityStatus.hasRemain) { character.Ability_AddRemain(-1, abilityStatus.index); }

            for (int i = 0; i < abilityStatus.actionsStatus.Length; i++)//行動主や対象を代入し、Enqueue
            {
                abilityStatus.actionsStatus[i].actionOwner = character;
                abilityStatus.actionsStatus[i].actionTargetsInt = new List<int>(targetGroups[i]);
                if (!abilityStatus.actionsStatus[i].condition.searchAsPos && abilityStatus.actionsStatus[i].targetType != Action.ActionStatus.TargetType.move)//対象がキャラであるアビリティの場合は、actionTargetsの設定
                {
                    abilityStatus.actionsStatus[i].actionTargets = new List<Character>(charactersManager.GetExistingCharacters(targetGroups[i], true));
                }//そうでない場合actionTargetsがnullとなるが、そのケアはActionのReaolve内で行っている

                actionQueue.Enqueue(abilityStatus.actionsStatus[i]);
            }
            //character.OnActivateAbility();
            actionQueue.StartResolve(3);

            battleManager.ResetSelectedAbility();
        }
        else { StartSelectTarget(); }//まだ選択が残ってるなら
    }
}
