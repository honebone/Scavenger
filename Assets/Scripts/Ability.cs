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
        public string conditionInfo;

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

        //public Character character;

        public string GetInfo(bool refCharaStatus, Character.CharacterStatus characterStatus)
        {
            string s = string.Format("種類：{0}\n", Definer.AbiltyTypeName[abilityType].ColorStr(Definer.colorRef.abilityColors[(int)abilityType]));
            if (!(availableFront && availableMid && availableBack))
            {
                s += ("発動可能列：");
                bool f=false;
                if (availableFront)
                {
                    f = true;
                    s += "前";
                }
                if (availableMid)
                {
                    if (f)
                    {
                        f = true;
                        s += "、";
                    }
                    s += "中";
                }
                if (availableBack)
                {
                    if (f)
                    {
                        f = true;
                        s += "、";
                    }
                    s += "後";
                }
                s += "列\n";
            }
            {

            }
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

        public void Init(AbilityData data,int idx)
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
            conditionInfo = data.conditionInfo;

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

            index = idx;
            remain=remainOnBattleStart;

            //character = owner;
        }
        public void AddRemain(int value) { remain = Mathf.Clamp(remain + value, 0, maxRemain); }
        public void SetRemain(int value) { remain = Mathf.Clamp(value, 0, maxRemain); }
        public void StartCoolDown() { cooldown = cooldownOnUse; }
        public void AddCoolDown(int value) { cooldown = Mathf.Clamp(cooldown + value, 0, cooldownOnUse); }
        public bool CheckAvailable(Character owner) {
            bool atProperPos = false;
            Character.CharacterStatus ownerStatus = owner.GetCharacterStatus();
            int column = ownerStatus.position.GetColumn();
            if (availableFront && column == 0) { atProperPos = true; }
            if (availableMid && column == 1) { atProperPos = true; }
            if (availableBack && column == 2) { atProperPos = true; }

            return (!hasRemain || remain > 0) && cooldown == 0 && unavailable == 0 && atProperPos; }
    }

    Character character;
    CharactersManager charactersManager;
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
    public virtual void StartSelectTarget()
    {
        charactersManager.ResetAllTargetIcons();
        Character.CharacterStatus charaStatus = character.GetCharacterStatus();
        Character.CharacterStatus targetStatus;
        Action.ActionStatus actionStatus = abilityStatus.actionsStatus[counter];
        bool playable = charaStatus.playable;

        targetIconPos = new List<Vector2Int>();
        targetPool = new List<List<int>>();
        if (!actionStatus.summon)//召喚以外のアビリティ
        {
            switch (actionStatus.targetType)
            {
                case Action.ActionStatus.TargetType.other:
                    print("特殊な対象の撮り方をするアビリティは、独自のscriptを作ってください!");
                    break;
                case Action.ActionStatus.TargetType.single://単体対象
                    targetEmpty = false;
                    for (int i = 0; i < 18; i++)
                    {
                        if (!charactersManager.CheckCharaExist(i)) { continue; }
                        if (i < 9 && !actionStatus.targetPlayerSide) { continue; }
                        if (i >= 9 && !actionStatus.targetEnemySide) { continue; }
                        if (i.GetColumn() == 0 && !actionStatus.selectableFront) { continue; }
                        if (i.GetColumn() == 1 && !actionStatus.selectableMid) { continue; }
                        if (i.GetColumn() == 2 && !actionStatus.selectableBack) { continue; }

                        targetStatus = charactersManager.GetCharacterWithPos(i).GetCharacterStatus();
                        if (targetStatus.marked > 0 && (charaStatus.position < 9) != (targetStatus.position < 9))
                        { targetIconPos.Add(new Vector2Int(i, 1)); }//マークが付与されている and 対象が敵なら、yを１に
                        else { targetIconPos.Add(new Vector2Int(i, 0)); }
                        targetPool.Add(new List<int>() { i });
                    }
                    break;
                case Action.ActionStatus.TargetType.all:
                    targetEmpty = true;
                    List<int> tp = new List<int>();
                    int iconPos = charaStatus.position; ;
                    if (actionStatus.targetPlayerSide)
                    {
                        for (int i = 0; i < 9; i++) { tp.Add(i); }
                    }
                    if (actionStatus.targetEnemySide)
                    {
                        for (int i = 9; i < 18; i++) { tp.Add(i); }
                    }
                    if (actionStatus.targetPlayerSide && actionStatus.targetEnemySide) { iconPos = charaStatus.position; }
                    else if (actionStatus.targetPlayerSide) { iconPos = 4; }
                    else if (actionStatus.targetEnemySide) { iconPos = 13; }

                    targetIconPos.Add(new Vector2Int(iconPos, 0));
                    targetPool.Add(charactersManager.GetExistingCharactersPos(tp));
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
        else//召喚アビリティ
        {
            targetEmpty = true;
            switch (actionStatus.targetType)
            {
                case Action.ActionStatus.TargetType.other:
                    print("特殊な対象の撮り方をするアビリティは、独自のscriptを作ってください!");
                    break;
                case Action.ActionStatus.TargetType.single:
                    for (int i = 0; i < 18; i++)
                    {
                        if (charactersManager.CheckCharaExist(i)) { continue; }
                        if (i < 9 && !actionStatus.targetPlayerSide) { continue; }
                        if (i >= 9 && !actionStatus.targetEnemySide) { continue; }
                        if (i.GetColumn() == 0 && !actionStatus.selectableFront) { continue; }
                        if (i.GetColumn() == 1 && !actionStatus.selectableMid) { continue; }
                        if (i.GetColumn() == 2 && !actionStatus.selectableBack) { continue; }
                        targetIconPos.Add(new Vector2Int(i, 0));
                        targetPool.Add(new List<int>() { i });
                    }
                    break;
            }
        }
       

        //マークの処理
        List<Vector2Int> targetPos = new List<Vector2Int>();//x:pos y:count
        for(int i = 0; i < targetIconPos.Count; i++)//y==1(マークが付与されている)対象がいるなら　それらのみを抽出
        {
            if (targetIconPos[i].y == 1){ targetPos.Add(new Vector2Int(targetIconPos[i].x, i)); }
        }
        if (targetPos.Count == 0|| abilityStatus.actionsStatus[counter].ignoreMark)//マークが付与されている対象がいない or マークを無視する
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
            character.OnActivateAbility();

            character.Ability_StartCoolDown(abilityStatus.index);
            if (abilityStatus.hasRemain) { character.Ability_AddRemain(-1, abilityStatus.index); }

            for (int i = 0; i < abilityStatus.actionsStatus.Length; i++)//行動主や対象を代入し、Enqueue
            {
                abilityStatus.actionsStatus[i].actionOwner = character;
                abilityStatus.actionsStatus[i].actionTargetsInt = new List<int>(targetGroups[i]);
                if (!abilityStatus.actionsStatus[i].summon && abilityStatus.actionsStatus[i].targetType != Action.ActionStatus.TargetType.move)//対象がキャラであるアビリティの場合は、actionTargetsの設定
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
