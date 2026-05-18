using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class Ability : MonoBehaviour
{
    [System.Serializable]
    public class AbilityStatus
    {
        public string abilityName;

        public GameObject abilityManager;
        public Ability instantiatedManager;

        //public bool dontChangeSprite;
        //public GameObject activateSprite;

        //public AudioClip SE;

        public AbilityData.AbilityType abilityType;

        public bool excludeRandomPool;
        public int priority;
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

        public string GetInfo(bool refCharaStatus, Character.CharacterStatus characterStatus,bool simple)
        {
            string s = "";
            if (locked) { s += $"{NL()}(未開放のアビリティ)".ColorStr(Definer.colorRef.failed_unavailable); }
            s += $"{NL()}{Definer.AbiltyTypeName[abilityType].ColorStr(Definer.colorRef.abilityColors[(int)abilityType])}アビリティ";
            string s1 = "";
            string s2 = "";
            string available = "○".ColorStr(Color.green);
            string unavailable = "><".ColorStr(Color.red);
            if (!refCharaStatus || characterStatus.position < 9)
            {
                s1 = "発動可能列：後-中-前\n";
                s2 = "            ";
                if (availableBack) { s2 += $"{available}-"; }
                else { s2 += $"{unavailable}-"; }
                if (availableMid) { s2 += $"{available}-"; }
                else { s2 += $"{unavailable}-"; }
                if (availableFront) { s2 +=$"{available}"; }
                else { s2 += $"{unavailable}"; }
            }
            else
            {
                s1 = "発動可能列：　　　前-中-後\n";
                s2 = "　　　            ";
                if (availableFront) { s2 += $"{available}-"; }
                else { s2 += $"{unavailable}-"; }
                if (availableMid) { s2 += $"{available}-"; }
                else { s2 += $"{unavailable}-"; }
                if (availableBack) { s2 += $"{available}"; }
                else { s2 += $"{unavailable}"; }
            }
            s += $"{NL()}{s1 + s2}";
            if (conditionInfo != "") s += $"{NL()}発動条件：{conditionInfo}"; 
            if (cooldownOnBattleStart > 0) { s += $"{NL()}{"初期クールダウン".ToLinkKey()}：{cooldownOnBattleStart}ターン"; }
            if (cooldownOnUse > 0) { s += $"{NL()}{"クールダウン".ToLinkKey()}：{cooldownOnUse}ターン"; }
            if (refCharaStatus) { }
            if (hasRemain)
            {
                if (refCharaStatus) { s += $"{NL()}残り使用回数：{remain}回"; }
                else { s += $"{NL()}使用回数(戦闘開始時)：{remainOnBattleStart}回"; }
            }

            string effectInfo = GetEffectInfo(refCharaStatus, characterStatus, simple);
            if (effectInfo != "") s += $"{NL()}{effectInfo}";

            return s;

            string NL(int lines = 1, string lineStr = "\n")
            {
                return Extentions.NL(s, lines, lineStr);
            }
        }

        public string GetEffectInfo(bool refCharaStatus, Character.CharacterStatus characterStatus, bool simple)
        {
            string s = "";

            if (abilityType == AbilityData.AbilityType.pass)
            {
                s += $"{NL()}・ターンをパスする(行動したとはみなされない)";
            }
            if (freeAction) s += $"{NL()}・{"クイックアビリティ".ToLinkKey()}".ColorStr(Definer.colorRef.emphasize);

            if (simple && !abilityData.noSimpleInfo && false)//test
            {
                s += $"{NL()}{abilityData.simpleInfo}";
                if (abilityData.upgradeInfo != "")
                {
                    s += $"{NL()}{"+アビリティ強化済み+".ColorStr(Definer.colorRef.emphasize)}\n{abilityData.upgradeInfo}";
                }
            }
            else
            {
                if (actionsStatus.Length == 1) { s += $"{NL(2)}{actionsStatus[0].GetInfo(refCharaStatus, characterStatus)}"; }
                else if (actionsStatus.Length > 1)
                {
                    int couter = 1;
                    foreach (Action.ActionStatus actionStatus in actionsStatus)
                    {
                        s += $"{NL(2)}<効果{couter}>\n{actionStatus.GetInfo(refCharaStatus, characterStatus)}";
                        couter++;
                    }
                }
            }

            return s;

            string NL(int lines = 1, string lineStr = "\n")
            {                
                return Extentions.NL(s, lines, lineStr);
            }
        }

        public AbilityStatus(AbilityData data,int idx)
        {
            abilityName = data.abilityName;

            abilityManager = data.abilityManager;

            //dontChangeSprite = data.dontChangeSprite;
            //activateSprite = data.activateSprite;

            //SE=data.SE;

            abilityType = data.abilityType;

            excludeRandomPool = data.excludeRandomPool;
            priority = data.priority;
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
                //actionsStatus[i].dontChangeSprite = dontChangeSprite;
                //actionsStatus[i].activateSprite=activateSprite;
            }

            locked = data.lockedDefault;
            index = idx;
            cooldown = cooldownOnBattleStart;
            remain = remainOnBattleStart;

            abilityData = data;
            //character = owner;
        }

        public void ResetTargetPrams()
        {
            for (int i = 0; i < actionsStatus.Length; i++)
            {
                actionsStatus[i].actionTargets = new List<Character>();
                actionsStatus[i].actionTargetsInt = new List<int>();
            }
        }
       
        public void SetManager(Ability m) { instantiatedManager = m; }

        public void Unlock() { locked = false; }
        public void AddRemain(int value)
        {
            int max = maxRemain == 0 ? 999 : maxRemain;
            remain = Mathf.Clamp(remain + value, 0, max);
        }
        public void SetRemain(int value)
        {
            int max = maxRemain == 0 ? 999 : maxRemain;
            remain = Mathf.Clamp(value, 0, max);
        }
        public void CoolDown_OnBattleStart() { cooldown = cooldownOnBattleStart; }
        public void CoolDown_OnUse() { cooldown = cooldownOnUse; }
        public void SetCoolDown(int value) { cooldown = Mathf.Max(0, value); }

        public void AddCoolDown(int value) { cooldown = Mathf.Max(0, cooldown + value); }

        public bool Available(bool allowCD1 = false) => instantiatedManager.CheckAvailable(allowCD1);
        public string GetAbilityName() => abilityName.ColorStr(abilityType.ToColor());
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

        //for (int i = 0;  i< this.status.actionsStatus.Length; i++)
        //{
        //    InfoText.inst.AddDebugText(this.status.actionsStatus[i].summon.ToString());
        //    InfoText.inst.AddDebugText(this.status.actionsStatus[i].actionTargets.Count.ToString());
        //}

        charactersManager = FindObjectOfType<CharactersManager>();
        battleManager = FindObjectOfType<BattleManager>();
        actionQueue = FindObjectOfType<ActionQueueManager>();
        soundManager = FindObjectOfType<SoundManager>();
    }

    public virtual string GetInfo(bool simple) { return status.GetInfo(true, character.CharaStatus(),simple); }
    public virtual Action.ActionStatus ModifyTargetParams(Action.ActionStatus actionStatus) { return actionStatus; }
    /// <summary>
    /// allowCD1:次ターンにCD解消するやつも含めるか
    /// </summary>
    /// <param name="allowCD1"></param>
    /// <returns></returns>
    public bool CheckAvailable(bool allowCD1=false)
    {
        bool atProperPos = false;
        bool hasProperTarget = true;
        bool properCondition = false;
        bool CD = status.cooldown == 0 || (status.cooldown == 1 && allowCD1);
        Character.CharacterStatus ownerStatus = character.CharaStatus();
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
        properCondition = (!status.hasSelfCondition || charactersManager.ExamineCharacter(character, status.selfCondition))&&CheckSpecialCondition();
        return !status.locked && (!status.hasRemain || status.remain > 0) && CD && status.unavailable == 0 && atProperPos && hasProperTarget && properCondition;
    }
    public List<string> GetUnavailabeInfo()
    {
        List<string> info = new List<string>();
        Character.CharacterStatus ownerStatus = character.CharaStatus();
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
        properCondition = (!status.hasSelfCondition || charactersManager.ExamineCharacter(character, status.selfCondition)) && CheckSpecialCondition();

        if (status.locked) { info.Add("未解放のアビリティ"); }
        if (!battleManager.checkIfMyTurn(character)) { info.Add("自身のターンでない"); }
        if (status.hasRemain && status.remain <= 0) { info.Add("使用可能数0"); }
        if (status.cooldown > 0) { info.Add("クールダウン中"); }
        if (!atProperPos) { info.Add("発動可能列にいない"); }
        if (!hasProperTarget) { info.Add("対象なし"); }
        if (!properCondition || status.unavailable > 0) { info.Add($"条件：\"{status.conditionInfo}\"を満たしていない"); }


        return info;
    }

    public void StartSelectTarget()
    {
        Character.CharacterStatus charaStatus = character.CharaStatus();
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
        Character.CharacterStatus charaStatus = character.CharaStatus();
        Character.CharacterStatus targetStatus;
        Action.ActionStatus actionStatus = ModifyTargetParams(status.actionsStatus[index]);

        List<List<int>> tp_noMark = new List<List<int>>();
        List<List<int>> tp_mark = new List<List<int>>();

        if (!actionStatus.condition.searchAsPos)//キャラ選択のアビリティ
        {
            List<Character> targets = charactersManager.SearchCharaWithCondition(actionStatus.condition, character);
            switch (actionStatus.targetType)
            {
                case Action.ActionStatus.TargetType.other:
                    print("特殊な対象の撮り方をするアビリティは、独自のscriptを作ってください!");
                    break;
                case Action.ActionStatus.TargetType.single://単体対象
                    foreach (Character target in targets)
                    {
                        targetStatus = target.CharaStatus();
                        int pos = targetStatus.position;
                        if (targetStatus.marked > 0 || targetStatus.hide == 0 || actionStatus.ignoreHide || actionStatus.friendly)//対象が潜伏じゃないor潜伏無視or友好アビリティ
                        {
                            if (targetStatus.marked > 0 && !(actionStatus.friendly || actionStatus.ignoreMark)) { tp_mark.Add(new List<int> { pos }); }
                            else { tp_noMark.Add(new List<int> { pos }); }
                        }
                    }
                    break;
                case Action.ActionStatus.TargetType.row:
                    List<int> tp_row;
                    bool includeMarked_row;
                    for (int i = 0; i < 3; i++)//各列に対して行う
                    {
                        tp_row = new List<int>();
                        includeMarked_row = false;
                        foreach (Character target in targets)
                        {
                            targetStatus = target.CharaStatus();
                            int pos = targetStatus.position;
                            if (pos < 9 && pos.GetRow() == i)//プレイヤー側で列がiと等しい
                            {
                                if (targetStatus.marked > 0 || targetStatus.hide == 0 || actionStatus.ignoreHide || actionStatus.friendly)//対象が潜伏じゃないor潜伏無視or友好アビリティ
                                {
                                    if (targetStatus.marked > 0 && !(actionStatus.friendly || actionStatus.ignoreMark)) { includeMarked_row = true; }
                                    tp_row.Add(pos);
                                }
                            }
                        }
                        if (tp_row.Count > 0)
                        {
                            if (includeMarked_row) { tp_mark.Add(tp_row); }
                            else { tp_noMark.Add(tp_row); }
                        }
                    }
                    for (int i = 0; i < 3; i++)//各列に対して行う
                    {
                        tp_row = new List<int>();
                        includeMarked_row = false;
                        foreach (Character target in targets)
                        {
                            targetStatus = target.CharaStatus();
                            int pos = targetStatus.position;
                            if (pos >= 9 && pos.GetRow() == i)//エネミー側で列がiと等しい
                            {
                                if (targetStatus.marked > 0 || targetStatus.hide == 0 || actionStatus.ignoreHide || actionStatus.friendly)//対象が潜伏じゃないor潜伏無視or友好アビリティ
                                {
                                    if (targetStatus.marked > 0 && !(actionStatus.friendly || actionStatus.ignoreMark)) { includeMarked_row = true; }
                                    tp_row.Add(pos);
                                }
                            }
                        }
                        if (tp_row.Count > 0)
                        {
                            if (includeMarked_row) { tp_mark.Add(tp_row); }
                            else { tp_noMark.Add(tp_row); }
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
                        foreach (Character target in targets)
                        {
                            targetStatus = target.CharaStatus();
                            int pos = targetStatus.position;
                            if (pos < 9 && pos.GetColumn() == i)//プレイヤー側で列がiと等しい
                            {
                                if (targetStatus.marked > 0 || targetStatus.hide == 0 || actionStatus.ignoreHide || actionStatus.friendly)//対象が潜伏じゃないor潜伏無視or友好アビリティ
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
                        foreach (Character target in targets)
                        {
                            targetStatus = target.CharaStatus();
                            int pos = targetStatus.position;
                            if (pos >= 9 && pos.GetColumn() == i)//エネミー側で列がiと等しい
                            {
                                if (targetStatus.marked > 0 || targetStatus.hide == 0 || actionStatus.ignoreHide || actionStatus.friendly)//対象が潜伏じゃないor潜伏無視or友好アビリティ
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
                    foreach (Character target in targets)
                    {
                        targetStatus = target.CharaStatus();
                        if (targetStatus.marked > 0 || targetStatus.hide == 0 || actionStatus.ignoreHide || actionStatus.friendly)
                        {
                            tp_all.Add(targetStatus.position);
                        }
                    }
                    tp_noMark.Add(tp_all);

                    break;
                case Action.ActionStatus.TargetType.singleWoSelf:
                    foreach (Character target in targets)
                    {
                        if (target != character)
                        {
                            targetStatus = target.CharaStatus();
                            int pos = targetStatus.position;
                            if (targetStatus.marked > 0 || targetStatus.hide == 0 || actionStatus.ignoreHide || actionStatus.friendly)//対象が潜伏じゃないor潜伏無視or友好アビリティ
                            {
                                if (targetStatus.marked > 0 && !(actionStatus.friendly || actionStatus.ignoreMark)) { tp_mark.Add(new List<int> { pos }); }
                                else { tp_noMark.Add(new List<int> { pos }); }
                            }
                        }
                    }
                    break;
                case Action.ActionStatus.TargetType.allWoSelf:
                    List<int> tp_allWoSelf = new List<int>();
                    foreach (Character target in targets)
                    {
                        if (target != character)
                        {
                            targetStatus = target.CharaStatus();
                            if (targetStatus.marked > 0 || targetStatus.hide == 0 || actionStatus.ignoreHide || actionStatus.friendly)
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

                case Action.ActionStatus.TargetType.neigbor://自身を中心とした相対座標を対象
                    List<Vector2Int> neigborVector=new List<Vector2Int>(actionStatus.neigborPos);
                    if (!charaStatus.position.IsPlayerPos())//オーナーが敵なら、相対座標を反転
                    {
                        for (int i = 0; i < neigborVector.Count; i++) { neigborVector[i] = new Vector2Int(-neigborVector[i].x, neigborVector[i].y); }
                    }
                    List<int> neigborPos = charaStatus.position.RelPosToAbs(neigborVector);
                    foreach (Character target in targets)
                    {
                        targetStatus = target.CharaStatus();
                        int pos = targetStatus.position;
                        if (neigborPos.Contains(pos))//指定した相対座標に含まれているか
                        {
                            if (targetStatus.marked > 0 || targetStatus.hide == 0 || actionStatus.ignoreHide || actionStatus.friendly)//対象が潜伏じゃないor潜伏無視or友好アビリティ
                            {
                                if (targetStatus.marked > 0 && !(actionStatus.friendly || actionStatus.ignoreMark)) { tp_mark.Add(new List<int> { pos }); }
                                else { tp_noMark.Add(new List<int> { pos }); }
                            }
                        }
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

    public virtual void SelectTarget(List<int> targetGroup) {
        counter++;
        targetGroups.Add(new List<int>(targetGroup));

        if (counter == status.actionsStatus.Length)//アビリティ効果数分対象の選択をしたら
        {
            battleManager.SetSelectingAbility(false);
            battleManager.SetSelectingTarget(false);
            charactersManager.ResetAllTargetIcons();

            string abilityName = status.abilityName.ColorStr(status.abilityType.ToColor());
            FindObjectOfType<InfoText>().AddLogText(string.Format("○{0}の<{1}>", character.CharaStatus().charaName, abilityName));

            character.Ability_StartCoolDown(status.index);
            if (status.hasRemain) { character.Ability_AddRemain(-1, status.index); }

            for (int i = 0; i < status.actionsStatus.Length; i++)//各アビリティ効果に行動主や対象を代入し、Enqueue
            {
                status.actionsStatus[i].index = i;
                status.actionsStatus[i].actionOwner = character;
                status.actionsStatus[i].actionTargetsInt = new List<int>(targetGroups[i]);
                if (!status.actionsStatus[i].condition.searchAsPos && status.actionsStatus[i].targetType != Action.ActionStatus.TargetType.move)//対象がキャラであるアビリティの場合は、actionTargetsの設定
                {
                    status.actionsStatus[i].actionTargets = new List<Character>(charactersManager.GetExistingCharacters(targetGroups[i], true));
                }//そうでない場合actionTargetsがnullとなるが、そのケアはActionのReaolve内で行っている
                
                    //対象にとる数をactionTargetsIntの数と一致させる→対象リストのすべてを対象に決定する(手動で対象を選ぶためランダム要素がない)
                    status.actionsStatus[i].targetCount = status.actionsStatus[i].actionTargetsInt.Count;

                actionQueue.Enqueue(status.actionsStatus[i],0);

                
            }
            //character.OnActivateAbility();

            ResetValue();
            battleManager.ResetSelectedAbility();
            battleManager.ResetIntentText();

            actionQueue.StartResolve(3);
        }
        else { StartSelectTarget(); }//まだ選択が残ってるなら
    }

    public virtual bool CheckSpecialCondition()
    {
        return true;
    }

    public void ResetValue()
    {
        counter = 0;
        targetPool = new List<List<int>>();
        targetGroups = new List<List<int>>();
        status.ResetTargetPrams();
    }
}
