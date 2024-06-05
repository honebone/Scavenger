using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleManager : MonoBehaviour
{
    [SerializeField]
    Transform selectedAbilityParent;
    [SerializeField]
    Transform turnOrderIconParent;

    [SerializeField]
    Transform fieldEffectP;

    [SerializeField]
    Text roundText;

    [SerializeField]
    AudioClip SE_FE;
    [SerializeField]
    Animator anim_battleIcon;
    [SerializeField]
    Text battleText;
    [SerializeField]
    Animator anim_FEName;
    [SerializeField]
    Text FENameText;
     [SerializeField]
    Animator anim_FEInfo;
    [SerializeField]
    Text FEInfoText;

    [SerializeField]
    GameObject turnOrderIcon;

    [SerializeField]
    AudioClip SE_battleStart;

    [SerializeField]
    Action.ActionStatus moveFrontLine;
    [SerializeField]
    CharactersManager.SearchCharaCondition moveFrontLineCondition;

    [SerializeField] TutorialData tutorial_Battle;
    [SerializeField] TutorialData tutorial_ability;
    [SerializeField] TutorialData tutorial_corpse;

    CharactersManager charactersManager;
    InfoText infoText;
    MessageText messageText;
    ExpeditionManager expeditionManager;
    ActionQueueManager actionQueue;
    SoundManager soundManager;
    PositionManager[] positionManagers;
    ExpeditionManager.PartyStatus partyStatus;
    TutorialManager tutorialManager;

    FieldEffect fieldEffect;
    int roundCount;

    public class Turn
    {
        public Character character;
        public Battle_TurnOrderIcon turnIcon;

        public Turn(Character chara, Battle_TurnOrderIcon icon)
        {
            character = chara;
            turnIcon = icon;
        }
    }
    List<Turn> turns;
    //List<Battle_TurnOrderIcon> turnOrderIcons=new List<Battle_TurnOrderIcon>();
    Turn currentTurn;
    int currentTurnCount;
    /// <summary>nextRoundButtonを押せるかどうか</summary>
    //bool roundEnd;

    public static bool inBattle;
    public static bool inRound;
    public static bool selectingAbility;
    public static bool selectingTarget;
    Ability selectedAbility;
    private void Start()
    {
        charactersManager = FindObjectOfType<CharactersManager>();
        infoText = FindObjectOfType<InfoText>();
        messageText = FindObjectOfType<MessageText>();
        expeditionManager = FindObjectOfType<ExpeditionManager>();
        partyStatus = expeditionManager.GetPartyStatus();
        actionQueue = FindObjectOfType<ActionQueueManager>();
        soundManager = FindObjectOfType<SoundManager>();
        tutorialManager = FindObjectOfType<TutorialManager>();
        positionManagers = charactersManager.GetPositionManagers();

        turns = new List<Turn>();
    }

    public void BattleStart(GameObject fieldEffectObj)
    {
        if (fieldEffectObj != null)
        {
            var f = Instantiate(fieldEffectObj, fieldEffectP);
            fieldEffect = f.GetComponent<FieldEffect>();
            fieldEffect.Init(charactersManager, actionQueue, infoText);
        }
        infoText.AddLogText("\n◇◇◇◇戦闘開始◇◇◇◇");
        inBattle = true;
        soundManager.PlaySE(SE_battleStart);
        
        roundCount=0;
        StartCoroutine(BattleStartAnim());
    }
    IEnumerator BattleStartAnim()
    {
        anim_battleIcon.SetTrigger("BattleStart");
        battleText.text = "<color=#890000>Battle!</color>";
        yield return new WaitForSeconds(1.5f);
        battleText.text = "";

        if (fieldEffect != null)
        {
            //soundManager.PlaySE(SE_FE);
            anim_FEName.SetBool("display",true);
            FENameText.text = fieldEffect.GetFEName();
            yield return new WaitForSeconds(1.25f);
            anim_FEInfo.SetBool("display",true);
            FEInfoText.text = fieldEffect.GetSimpleInfo();
            yield return new WaitForSeconds(1.25f);
            anim_FEName.SetBool("display", false);
            anim_FEInfo.SetBool("display", false);

        }
        //戦闘開始時誘発
        Trigger_BattleStart();
    }

   public void RoundStart()
    {
        roundCount++;
        roundText.text=roundCount.ToString();
        infoText.AddLogText(string.Format("\n◇◇ラウンド{0}◇◇", roundCount));
        Trigger_RoundStart();
    }
    public void DicideTurnOrder()
    {
        currentTurn = null;
        currentTurnCount = 0;
        turns.Clear();
        List<Character> charas = new List<Character>();
        List<int> turnsPerRound = new List<int>();//i番目のラウンド毎ターン数
        List<float> ACT = new List<float>();//i番目のACT
        foreach(Character chara in new List<Character>(charactersManager.GetExistingCharacters_All()))
        {
            if (chara.GetCharacterStatus().turnPerRound > 0)
            {
                charas.Add(chara);
            }
        }

        foreach (Character chara in charas)
        {
            turnsPerRound.Add(chara.GetCharacterStatus().turnPerRound);
            ACT.Add(chara.GetCharacterStatus().ACT);
            chara.SetTurnIcon();
        }
        int minACT = 1;
        foreach (int act in ACT) { if (act < minACT) { minACT = act; } }
        if (minACT <= 0)//ACTの最低値が0以下なら
        {
            for (int i = 0; i < ACT.Count; i++) { ACT[i] += Mathf.Abs(minACT) + 1; }//|ACTの最低値|+1を全ACTに足す(=0以下のACTがなくなる)
        }
        for (int i = 0; turnsPerRound.Count > 0; i++)
        {
            int index = ACT.ChoiceWithWeight();
            //infoText.AddDebugText(string.Format("index:{0} name:{1}", index, charas[index].GetCharacterStatus().charaName));
            //if(turnsPerRound[index] > 0)
            //{
            //    //characterInTurnOrder.Add(charas[a]);
            //    var t = Instantiate(turnOrderIcon, turnOrderIconParent);
            //    t.GetComponent<Battle_TurnOrderIcon>().Init(charas[index], i < partyStatus.turnOrderReveal + 1);
            //    //turnOrderIcons.Add(t.GetComponent<Battle_TurnOrderIcon>());
            //    turns.Add(new Turn(charas[index], t.GetComponent<Battle_TurnOrderIcon>()));
            //    turnsPerRound[index]--;
            //}
            //else
            //{
            //    infoText.AddDebugText("skip");
            //    charas.RemoveAt(index);
            //    turnsPerRound.RemoveAt(index);
            //    ACT.RemoveAt(index);
            //}


            var t = Instantiate(turnOrderIcon, turnOrderIconParent);
            t.GetComponent<Battle_TurnOrderIcon>().Init(charas[index], i < partyStatus.turnOrderReveal + 1);
            //turnOrderIcons.Add(t.GetComponent<Battle_TurnOrderIcon>());
            turns.Add(new Turn(charas[index], t.GetComponent<Battle_TurnOrderIcon>()));
            turnsPerRound[index]--;

            if (turnsPerRound[index] == 0)
            {
                //infoText.AddDebugText("skip");
                charas.RemoveAt(index);
                turnsPerRound.RemoveAt(index);
                ACT.RemoveAt(index);
            }
            else if (turnsPerRound[index] < 0) { infoText.AddErrorText(""); }
        }

        for (int i = 0; i < Mathf.Min(turns.Count, partyStatus.turnOrderReveal + 1); i++) { turns[i].turnIcon.Reveal(); }

        Trigger_TurnOrderDecide();
    }

    /// <summary>消滅時に呼ばれる</summary>
    public void RemoveTurn(Character chara)
    {
        List<Turn> removeTurns = new List<Turn>();
        foreach (Turn turn in turns)
        {
            if (turn.character == chara) { removeTurns.Add(turn); }
        }
        foreach (Turn remove in removeTurns)
        {
            turns.Remove(remove);
            remove.turnIcon.RemoveTurnOrderIcon();
        }
        //for (int i = 0; i < turnOrderIcons.Count; i++)
        //{
        //    if (turnOrderIcons[i].GetCharacter() == chara)
        //    {
        //       List<Battle_TurnOrderIcon> removeTurns=new List<Battle_TurnOrderIcon>();
        //        foreach(Battle_TurnOrderIcon turn in turnOrderIcons)
        //        {
        //            if (turn.GetCharacter() == chara) { removeTurns.Add(turn); }
        //        }
        //        foreach(Battle_TurnOrderIcon remove in removeTurns)
        //        {
        //            turnOrderIcons.Remove(remove);
        //            Destroy(remove.gameObject);
        //        }
        //    }
        //}
        //foreach(Battle_TurnOrderIcon turnOrderIcon in turnOrderIcons) { turnOrderIcon.RemoveTurnOrderIcon(chara); }
    }


    public void EndTrigger_TurnOrderDecide() { StartCoroutine(RoundStartEffect()); }
    IEnumerator RoundStartEffect()
    {
        messageText.SetText(string.Format("ラウンド {0}", roundCount));
        yield return new WaitForSeconds(1f);
        inRound = true;
        messageText.ResetText();

        currentTurn = turns[0];
        currentTurn.character.MyTurnStart();
        turns.RemoveAt(0);
    }
    public void TurnEnd()
    {
        currentTurn.turnIcon.RemoveTurnOrderIcon();
        if (turns.Count == 0) { RoundEnd(); }
        else
        {
            for (int i = 0; i < Mathf.Min(turns.Count, partyStatus.turnOrderReveal + 1); i++) { turns[i].turnIcon.Reveal(); }

            currentTurn = turns[0];
            currentTurnCount++;
            currentTurn.character.MyTurnStart();
            turns.RemoveAt(0);
        }
    }

    public void RoundEnd()
    {
        inRound = false;
        infoText.AddLogText(string.Format("\n◇◇ラウンド{0}終了◇◇", roundCount));
        Trigger_RoundEnd();
    }

    public void BattleEnd()
    {
        infoText.AddLogText("\n◇◇◇◇戦闘終了◇◇◇◇");
        currentTurnCount = 0;
        inRound = false;
        inBattle = false;
        if (selectedAbility) { infoText.AddErrorText("アビリティ選択中に戦闘が終了しました"); }
        if (selectingTarget) { infoText.AddErrorText("対象選択中に戦闘が終了しました"); }

        if (fieldEffect != null)
        {
            Destroy(fieldEffectP.GetChild(0).gameObject);
            fieldEffect = null;
        }
        turns = new List<Turn>();
        //characterInTurnOrder = new List<Character>();
        //turnOrderIcons = new List<Battle_TurnOrderIcon>();
        for (int i = 0; i < turnOrderIconParent.childCount; i++) { Destroy(turnOrderIconParent.GetChild(i).gameObject); }

        //各キャラクターにも知らせる
        //generatedCharaから死亡しているキャラを消去

        List<Character> test=new List<Character>(charactersManager.GetExistingCharacters_All());
        foreach(Character chara in test)//プレイヤーでないキャラ全てを消去
        {
            if (!chara.GetCharacterStatus().player)
            {
                chara.Retreat();
            }
        }

        Trigger_BattleEnd();
    }
    public void EndTrigger_BattleEnd() { StartCoroutine(BattleEndAnim()); }

    IEnumerator BattleEndAnim()
    {
        yield return new WaitForSeconds(1f);
        anim_battleIcon.SetTrigger("BattleEnd");
        battleText.text = "<color=#FFFF00>Victory!</color>";
        yield return new WaitForSeconds(1.5f);
        battleText.text = "";


        expeditionManager.OnEndBattle();
    }

    public void Trigger_BattleStart()
    {
        tutorialManager.StartTutorial(tutorial_Battle);

        if (fieldEffect != null) { fieldEffect.OnBattleStart(); }
        foreach (Character character in charactersManager.GetExistingCharacters_All())
        {
            character.OnBattleStart();
        }
        foreach(PositionManager positionManager in positionManagers)
        {
            positionManager.OnBattleStart();
        }
        actionQueue.StartResolve(0);
    }
    public void Trigger_RoundStart()
    {
        MoveFrontLine(true);//前進処理
        MoveFrontLine(false);

        if (fieldEffect != null) { fieldEffect.OnRoundStart(); }
        foreach (Character character in charactersManager.GetExistingCharacters_All())
        {
            character.OnRoundStart();
        }
        foreach (PositionManager positionManager in positionManagers)
        {
            positionManager.OnRoundStart();
        }
        actionQueue.StartResolve(1);
    }
    public void Trigger_TurnStart()
    {
        if (fieldEffect != null) { fieldEffect.OnTurnStart(currentTurnCount + 1); }
        foreach (Character character in charactersManager.GetExistingCharacters_All())
        {
            character.OnTurnStart(checkIfMyTurn(character), currentTurnCount + 1);
        }
        foreach (PositionManager positionManager in positionManagers)
        {
            positionManager.OnTurnStart(currentTurn.character, currentTurnCount + 1);
        }
        actionQueue.StartResolve(2);
    }
    public void Trigger_TurnOrderDecide()
    {
        if (fieldEffect != null) { fieldEffect.OnTurnOrderDecide(); }
        foreach (Character character in charactersManager.GetExistingCharacters_All())
        {
            character.OnTurnOrderDecide();
        }
        foreach (PositionManager positionManager in positionManagers)
        {
            positionManager.OnTurnOrderDecide();
        }
        actionQueue.StartResolve(6);
    }
    public void Trigger_RoundEnd()
    {
        if (fieldEffect != null) { fieldEffect.OnRoundEnd(); }
        foreach (Character character in charactersManager.GetExistingCharacters_All())
        {
            character.OnRoundEnd();
        }
        foreach (PositionManager positionManager in positionManagers)
        {
            positionManager.OnRoundEnd();
        }
        actionQueue.StartResolve(5);
    }
    public void Trigger_BattleEnd()
    {
        if (fieldEffect != null) { fieldEffect.OnBattleEnd(); }
        foreach (Character character in charactersManager.GetExistingCharacters_All())
        {
            character.OnBattleEnd();
        }
        foreach (PositionManager positionManager in positionManagers)
        {
            positionManager.OnBattleEnd();
        }
        actionQueue.StartResolve(7);
    }
    void MoveFrontLine(bool player)
    {
        bool emptyFront = true;
        bool emptyMid = true;
        bool emptyBack = true;
        int colmun, start, end;
        string s = "";
        Action.ActionStatus actionStatus = moveFrontLine;
        CharactersManager.SearchCharaCondition condition = moveFrontLineCondition;
        if (player)
        {
            condition.player = true;
            start = 0;
            end = 9;
            s = "プレイヤー";
        }
        else
        {
            condition.enemy = true;
            start = 9;
            end = 18;
            s = "エネミー";
        }
        for (int i = start; i < end; i++)
        {// && !charactersManager.GetCharacterWithPos(i).GetCharacterStatus().immovable
            if (charactersManager.CheckCharaExist(i))//移動不可でないキャラがポジションiに存在しているなら
            {
                colmun = charactersManager.GetCharacterWithPos(i).GetCharacterStatus().position.GetColumn();
                if (colmun == 0) { emptyFront = false; }
                else if (colmun == 1) { emptyMid = false; }
                else if (colmun == 2) { emptyBack = false; }
            }
        }
        if ((emptyFront && emptyMid) && !emptyBack)//後列のみにキャラがいるなら
        {
            condition.back = true;
            actionStatus.moveForword = 2;
            actionStatus.actionTargets = charactersManager.SearchCharaWithCondition(condition);
            actionStatus.targetInfo = string.Format("後列の{0}全て", s);
            actionQueue.Enqueue(actionStatus);
        }
        else
        {
            actionStatus.moveForword = 1;
            if (emptyFront && !emptyMid)//前列にキャラがいない　かつ　中列にキャラが1体でもいるなら
            {
                condition.mid = true;
                actionStatus.actionTargets = charactersManager.SearchCharaWithCondition(condition);
                actionStatus.targetInfo = string.Format("中列の{0}全て", s);
                actionQueue.Enqueue(actionStatus);
            }
            if ((emptyFront||emptyMid) && !emptyBack)//中、前列のいずれかが開いている　かつ　後列にキャラが1体でもいるなら
            {
                condition.mid = false;
                condition.back = true;
                actionStatus.actionTargets = charactersManager.SearchCharaWithCondition(condition);
                actionStatus.targetInfo = string.Format("後列の{0}全て", s);
                actionQueue.Enqueue(actionStatus);
            }

        }
    }

    public void StartTutorial_Ability() { tutorialManager.StartTutorial(tutorial_ability); }
    public void StartTutorial_Corpse() { tutorialManager.StartTutorial(tutorial_corpse); }

    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Space) && roundEnd)//test
        //{
        //    roundEnd = false;
        //    RoundStart();
        //}
    }
    /// <summary>アビリティボタンをクリックしたときに呼ぶ </summary>
    public void SetSelectedAbility(Ability.AbilityStatus abilityStatus,Character character)
    {
        if (selectedAbility != null) { selectedAbility.ResetValue(); }
        selectedAbility = abilityStatus.instantiatedManager;
        if (selectingAbility) { SetSelectingTarget(true); }
    }
    /// <summary>アビリティの対象選択が終了したときに呼ぶ </summary>
    public void ResetSelectedAbility()
    {
        for (int i = 0; i < selectedAbilityParent.childCount; i++) { Destroy(selectedAbilityParent.GetChild(i).gameObject); }
        selectedAbility=null;
    }

    //public void SetOmenIcon(Character chara,Ability.AbilityStatus abilityStatus)
    //{
    //    foreach (Battle_TurnOrderIcon turnOrderIcon in turnOrderIcons)
    //    {
    //        if (turnOrderIcon.GetCharacter() == chara)
    //        {
    //            turnOrderIcon.SetOmenIcon(abilityStatus);
    //            return;
    //        }
    //    }
    //    infoText.AddDebugText("error");
    //}
    public void SetSelectingAbility(bool f) { selectingAbility = f; }
    public void SetSelectingTarget(bool f) { selectingTarget = f; }
    public bool checkIfMyTurn(Character character)
    {
        if (inBattle && inRound && currentTurn.character == character) { return true; }
        return false;
    }
    
    public Ability GetSelectedAbility() { return selectedAbility; }
    public Character GetCurrntTurnChara() { return currentTurn.character; }
}
