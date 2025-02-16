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

    [SerializeField] AudioClip battleEnd;
    [SerializeField] TutorialData tutorial_Battle;
    [SerializeField] TutorialData tutorial_ability;
    [SerializeField] TutorialData tutorial_corpse;
    [SerializeField] TutorialData tutorial_frontLine;

    CharactersManager charactersManager;
    InfoText infoText;
    MessageText messageText;
    ExpeditionManager expeditionManager;
    ActionQueueManager actionQueue;
    SoundManager soundManager;
    PositionManager[] positionManagers;
    ExpeditionManager.PartyStatus partyStatus;
    TutorialManager tutorialManager;
    [SerializeField] TotalDamageText totalDamageText;

    FieldEffect fieldEffect;
    int roundCount;

    [System.Serializable]
    public class Turn
    {
        public Character character;
        public Battle_TurnOrderIcon turnIcon;
        public string charaName;

        public Turn(Character chara, Battle_TurnOrderIcon icon)
        {
            character = chara;
            turnIcon = icon;
            charaName = character.CharaStatus().charaName;
        }
    }
    [SerializeField]
    List<Turn> turns;
    //List<Battle_TurnOrderIcon> turnOrderIcons=new List<Battle_TurnOrderIcon>();
    Turn currentTurn;
    int currentTurnCount;
    /// <summary>nextRoundButtonを押せるかどうか</summary>
    //bool roundEnd;

    public static BattleManager inst;

    public static bool inBattle;
    public static bool inRound;
    public static bool selectingAbility;
    public static bool selectingTarget;
    Ability selectedAbility;

    private void Awake()
    {
        inst = this;
    }
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
        AddBattleReport();//test
    }

   public void RoundStart()
    {
        //for (int i = 0; i < turnOrderIconParent.childCount; i++) { turnOrderIconParent.GetChild(i).GetComponent<Battle_TurnOrderIcon>().CheckDeadForDebug(); }

        roundCount++;
        roundText.text=roundCount.ToString();
        infoText.AddLogText(string.Format("\n◇◇ラウンド{0}◇◇", roundCount));
        Trigger_RoundStart();
    }

    class TurnOrderParams
    {
        public int ACT;
        public int turns;
        public int index;
        public Character chara;
    }

    public void DicideTurnOrder()
    {
        for (int i = 0; i < turnOrderIconParent.childCount; i++) { Destroy(turnOrderIconParent.GetChild(i).gameObject); }//tst

        currentTurn = null;
        currentTurnCount = 0;
        turns.Clear();
        List<Character> charas = new List<Character>();
        //List<int> turnsPerRound = new List<int>();//i番目のラウンド毎ターン数
        //List<int> ACT = new List<int>();//i番目のACT

        List<TurnOrderParams> turnOrderParamsList = new List<TurnOrderParams>();
        foreach(Character chara in new List<Character>(charactersManager.GetExistingCharacters_All()))
        {
            if (chara.CharaStatus().turnPerRound > 0)
            {
                charas.Add(chara);
            }
        }

        //foreach (Character chara in charas)
        //{
        //    turnsPerRound.Add(chara.GetCharacterStatus().turnPerRound);
        //    ACT.Add(chara.GetCharacterStatus().ACT);
        //    chara.SetTurnIcon();
        //}
        for(int j = 0; j < charas.Count; j++)
        {
            TurnOrderParams turnOrderParams = new TurnOrderParams();
            turnOrderParams.chara = charas[j];
            turnOrderParams.ACT = charas[j].CharaStatus().ACT;
            turnOrderParams.turns = charas[j].CharaStatus().turnPerRound;
            turnOrderParams.index = j;

            turnOrderParamsList.Add(turnOrderParams);

            charas[j].SetTurnIcon();
        }
        //int minACT = 1;
        //foreach (int act in ACT) { if (act < minACT) { minACT = act; } }
        //if (minACT <= 0)//ACTの最低値が0以下なら
        //{
        //    for (int i = 0; i < ACT.Count; i++) { ACT[i] += Mathf.Abs(minACT) + 1; }//|ACTの最低値|+1を全ACTに足す(=0以下のACTがなくなる)
        //}
        //for (int i = 0; turnsPerRound.Count > 0; i++)
        //{
          
        //    int index = ACT.ChoiceWithWeight();

        //    var t = Instantiate(turnOrderIcon, turnOrderIconParent);
        //    t.GetComponent<Battle_TurnOrderIcon>().Init(charas[index], i < partyStatus.turnOrderReveal + 1);
        //    //turnOrderIcons.Add(t.GetComponent<Battle_TurnOrderIcon>());
        //    turns.Add(new Turn(charas[index], t.GetComponent<Battle_TurnOrderIcon>()));
        //    turnsPerRound[index]--;

        //    if (turnsPerRound[index] == 0)
        //    {
        //        //infoText.AddDebugText("skip");
        //        charas.RemoveAt(index);
        //        turnsPerRound.RemoveAt(index);
        //        ACT.RemoveAt(index);
        //    }
        //    else if (turnsPerRound[index] < 0) { infoText.AddErrorText(""); }           
        //}

        for (int i = 0; turnOrderParamsList.Count > 0; i++)
        {
            //ACTの最大値を探す
            int maxACT = turnOrderParamsList[0].ACT;
            foreach (TurnOrderParams turn in turnOrderParamsList) { if (turn.ACT > maxACT) { maxACT = turn.ACT; } }

            //ACTが最大値と等しいキャラを探す
            List<TurnOrderParams> pool = new List<TurnOrderParams>();
            foreach(TurnOrderParams turn in turnOrderParamsList)
            {
                if (turn.ACT == maxACT) { pool.Add(turn); }
            }

            //探したキャラからランダムに選ぶ
            TurnOrderParams selected = pool.Choice();

            //var t = Instantiate(turnOrderIcon, turnOrderIconParent);
            //t.GetComponent<Battle_TurnOrderIcon>().Init(selected.chara, i < partyStatus.turnOrderReveal + 1);
            ////turnOrderIcons.Add(t.GetComponent<Battle_TurnOrderIcon>());
            //turns.Add(new Turn(selected.chara, t.GetComponent<Battle_TurnOrderIcon>()));
            AddTurn(selected.chara,i < partyStatus.turnOrderReveal + 1,1);
           selected.turns--;
            selected.ACT = (selected.ACT / 1.5f).ToInt();

            if (selected.turns == 0)
            {
                //infoText.AddDebugText("skip");
                turnOrderParamsList.Remove(selected);
            }
            else if (selected.turns  < 0) { infoText.AddErrorText(""); }
        }

        for (int i = 0; i < Mathf.Min(turns.Count, partyStatus.turnOrderReveal + 1); i++) { turns[i].turnIcon.Reveal(); }

        Trigger_TurnOrderDecide();
    }

    public void AddTurn(Character chara,bool reveal,int add)
    {
        if (chara.CheckAlive())
        {
            for(int i = 0; i < add; i++)
            {
                var t = Instantiate(turnOrderIcon, turnOrderIconParent);
                t.GetComponent<Battle_TurnOrderIcon>().Init(chara, reveal);
                turns.Add(new Turn(chara, t.GetComponent<Battle_TurnOrderIcon>()));
            }
        }
    }

    /// <summary>消滅時に呼ばれる</summary>
    public void RemoveTurn(Character chara)
    {
        if (currentTurn != null && currentTurn.character == chara) { currentTurn.turnIcon.RemoveTurnOrderIcon(); }

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
        turns.RemoveAt(0);

        currentTurn.character.MyTurnStart();
    }
    public void TurnEnd(int cause)
    {
        Trigger_TurnEnd();
    }

    public void NextTurn()
    {
        if (currentTurn.character.CheckAlive()) { currentTurn.turnIcon.RemoveTurnOrderIcon(); }
        if (turns.Count == 0) { RoundEnd(); }
        else
        {
            for (int i = 0; i < Mathf.Min(turns.Count, partyStatus.turnOrderReveal + 1); i++) { turns[i].turnIcon.Reveal(); }

            currentTurn = turns[0];
            currentTurnCount++;
            turns.RemoveAt(0);

            currentTurn.character.MyTurnStart();

        }
    }

    public void RoundEnd()
    {
        currentTurn = null;
        inRound = false;
        infoText.AddLogText(string.Format("\n◇◇ラウンド{0}終了◇◇", roundCount));
        Trigger_RoundEnd();
        //for (int i = 0; i < turnOrderIconParent.childCount; i++) { Destroy(turnOrderIconParent.GetChild(i).gameObject); }//tst
    }

    public void BattleEnd()
    {
        infoText.AddLogText("\n◇◇◇◇戦闘終了◇◇◇◇");
        currentTurnCount = 0;
        inRound = false;
        inBattle = false;
        if (selectedAbility) { infoText.AddErrorText("アビリティ選択中に戦闘が終了しました"); }
        if (selectingTarget) { infoText.AddErrorText("対象選択中に戦闘が終了しました"); }

        //test
        infoText.AddLogText("\n====戦闘レポート====");
        foreach (Character chara in charactersManager.GetExistingCharacters_All())
        {
            if (chara.CharaStatus().player)
            {
                string s = chara.CharaStatus().charaName + "\n";
                s += chara.GetBattleReport().Report();
                infoText.AddLogText(s+"\n");
                chara.ResetBattleReport();
            }
        }

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
            if (!chara.CharaStatus().player)
            {
                chara.Retreat();
            }
        }

        currentTurn = null;

        Trigger_BattleEnd();
    }


    public void EndTrigger_BattleEnd()
    {
        charactersManager.DestroyDead();
        StartCoroutine(BattleEndAnim());
    }

    IEnumerator BattleEndAnim()
    {
        yield return new WaitForSeconds(1f);
        soundManager.PlaySE(battleEnd);
        anim_battleIcon.SetTrigger("BattleEnd");
        battleText.text = "<color=#FFFF00>Victory!</color>";
        yield return new WaitForSeconds(1.5f);
        battleText.text = "";


        expeditionManager.OnEndBattle();
    }

    public void Trigger_OnSomeoneDamaged(Action.OnDamageParams onDamageParams)
    {
        //if (fieldEffect != null) { fieldEffect.OnBattleStart(); }
        foreach (Character character in charactersManager.GetExistingCharacters_All())
        {
            character.OnSomeoneDamaged(onDamageParams);
        }
        //foreach (PositionManager positionManager in positionManagers)
        //{
        //    positionManager.OnBattleStart();
        //}
    }
    public void Trigger_OnSomeoneMove(Action.OnMoveParams onMoveParams)
    {
        //if (fieldEffect != null) { fieldEffect.OnBattleStart(); }
        foreach (Character character in charactersManager.GetExistingCharacters_All())
        {
            character.OnSomeoneMove(onMoveParams);
        }
        //foreach (PositionManager positionManager in positionManagers)
        //{
        //    positionManager.OnBattleStart();
        //}
    }
    public void Trigger_OnSomeoneFocus(List<Action.OnFocusParams> focusParamsList)
    {
        //if (fieldEffect != null) { fieldEffect.OnBattleStart(); }
        foreach (Character character in charactersManager.GetExistingCharacters_All())
        {
            character.OnSomeoneFocus(focusParamsList);
        }
        //foreach (PositionManager positionManager in positionManagers)
        //{
        //    positionManager.OnBattleStart();
        //}
    }
    public void Trigger_OnSomeoneDied(Character died)
    {
        foreach (Character character in charactersManager.GetExistingCharacters_All())
        {
            character.OnSomeoneDied(died);
        }
    }
    public void Trigger_OnSomeoneApplyedStE(List<Action.OnApplyStEParams> onApplyStEParamsList)
    {
        foreach (Character character in charactersManager.GetExistingCharacters_All())
        {
            character.OnSomeoneApplyedStE(onApplyStEParamsList);
        }
    }

    public void Trigger_BattleStart()
    {
        tutorialManager.SetTutorial(tutorial_Battle);
        if (DebugFunction.instance.battleDebug)
        {
            infoText.AddDebugText("戦闘開始");
            if (currentTurn != null) { infoText.AddDebugText($"現在のターン：{currentTurn.character.CharaStatus().charaName}"); }
            else { infoText.AddDebugText("現在のターン：なし"); }
        }

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
        if (DebugFunction.instance.battleDebug)
        {
            infoText.AddDebugText("ラウンド開始");
            if (currentTurn != null) { infoText.AddDebugText($"現在のターン：{currentTurn.character.CharaStatus().charaName}"); }
            else { infoText.AddDebugText("現在のターン：なし"); }
        }

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
        if (DebugFunction.instance.battleDebug)
        {
            infoText.AddDebugText("ターン開始");
            if (currentTurn != null) { infoText.AddDebugText($"現在のターン：{currentTurn.character.CharaStatus().charaName}"); }
            else { infoText.AddDebugText("現在のターン：なし"); }
        }

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
        if (DebugFunction.instance.battleDebug)
        {
            infoText.AddDebugText("ターン順決定");
            if (currentTurn != null) { infoText.AddDebugText($"現在のターン：{currentTurn.character.CharaStatus().charaName}"); }
            else { infoText.AddDebugText("現在のターン：なし"); }
        }
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

    public void Trigger_TurnEnd()
    {
        if (!CheckCurrentTurnAlive()) infoText.AddDebugText("Triggerチェック：ターン中のキャラの死亡を確認");
        foreach (Character character in charactersManager.GetExistingCharacters_All())
        {
            character.OnTurnEnd(checkIfMyTurn(character), currentTurnCount + 1, !CheckCurrentTurnAlive());
        }
        foreach (PositionManager positionManager in positionManagers)
        {
            positionManager.OnTurnEnd(currentTurn.character, currentTurnCount + 1,!CheckCurrentTurnAlive());
        }
        actionQueue.StartResolve(4);
    }

    public void Trigger_RoundEnd()
    {
        if (DebugFunction.instance.battleDebug)
        {
            infoText.AddDebugText("ラウンド終了");
            if (currentTurn != null) { infoText.AddDebugText($"現在のターン：{currentTurn.character.CharaStatus().charaName}"); }
            else { infoText.AddDebugText("現在のターン：なし"); }
        }

        if (fieldEffect != null) { fieldEffect.OnRoundEnd(); }
        foreach (Character character in new List<Character>(charactersManager.GetExistingCharacters_All()))
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
        if (DebugFunction.instance.battleDebug)
        {
            infoText.AddDebugText("戦闘終了");
            if (currentTurn != null) { infoText.AddDebugText($"現在のターン：{currentTurn.character.CharaStatus().charaName}"); }
            else { infoText.AddDebugText("現在のターン：なし"); }
        }

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
        bool f = false;
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
                colmun = charactersManager.GetCharacterWithPos(i).CharaStatus().position.GetColumn();
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
            actionQueue.Enqueue(actionStatus,0);
            f = true;
        }
        else
        {
            actionStatus.moveForword = 1;
            if (emptyFront && !emptyMid)//前列にキャラがいない　かつ　中列にキャラが1体でもいるなら
            {
                condition.mid = true;
                actionStatus.actionTargets = charactersManager.SearchCharaWithCondition(condition);
                actionStatus.targetInfo = string.Format("中列の{0}全て", s);
                actionQueue.Enqueue(actionStatus,0);
                f = true;
            }
            if ((emptyFront||emptyMid) && !emptyBack)//中、前列のいずれかが開いている　かつ　後列にキャラが1体でもいるなら
            {
                condition.mid = false;
                condition.back = true;
                actionStatus.actionTargets = charactersManager.SearchCharaWithCondition(condition);
                actionStatus.targetInfo = string.Format("後列の{0}全て", s);
                actionQueue.Enqueue(actionStatus,0);
                f = true;
            }

        }

        if (f) { tutorialManager.SetTutorial(tutorial_frontLine);//infoText.AddDebugText("ok"); 
        }
    }

    //======================================[戦闘レポート関連]===============================
    [SerializeField] List<BattleReport> battleReports=new List<BattleReport>();

    public void AddBattleReport()
    {
        BattleReport report = new BattleReport();
        int count = 0;
        int enmCount = 0;
        foreach(Character character in charactersManager.GetExistingCharacters_All())
        {
            Character.CharacterStatus status = character.CharaStatus();
            if (status.player)
            {
                count++;
                report.playerTotalMaxHP += status.maxHP;
                report.playerTotalATK_INT += status.ATK + status.INT;
                report.playerAveLVL += status.level;
            }
            else if (!status.position.IsPlayerPos())
            {
                enmCount++;
                report.enemyAveLVL += status.level;
            }
        }
        report.playerTotalATK_INT = (report.playerTotalATK_INT / 2f).ToInt();
        report.playerAveMaxHP = (report.playerTotalMaxHP / count * 1f).ToInt();
        report.playerAveATK_INT = (report.playerTotalATK_INT / count * 1f).ToInt();
        report.playerAveLVL /= count;
        report.enemyAveLVL /= enmCount;
        battleReports.Add(report);

        string maxHP = "playerAveMaxHP\n";
        string ATK = "playerTotalATK_INT\n";
        string playerLVL = "playerAveLVL\n";
        string enemyLVL = "enemyAveLVL\n";
        for (int i = 0; i < battleReports.Count; i++)
        {
            maxHP += $"{battleReports[i].playerAveMaxHP}\n";
            ATK += $"{battleReports[i].playerTotalATK_INT}\n";
            playerLVL += $"{battleReports[i].playerAveLVL}\n";
            enemyLVL += $"{battleReports[i].enemyAveLVL}\n";
        }
        Debug.Log(maxHP);
        Debug.Log(ATK);
        Debug.Log(playerLVL);
        Debug.Log(enemyLVL);
    }

    public void SetTotalDamageText(int damage) { totalDamageText.SetText(damage); }

    //===============================================================[Settings]=================================================================
    public static bool displayInfoOnTS;

    public void Settings_DisplayInfoOnTS(bool value) { displayInfoOnTS = value; }

    public void StartTutorial_Ability() { tutorialManager.SetTutorial(tutorial_ability); }
    public void StartTutorial_Corpse() { tutorialManager.SetTutorial(tutorial_corpse); }

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
    public bool CheckCurrentTurnAlive()
    {
        return currentTurn.character.CheckAlive();
    }
    
    public Ability GetSelectedAbility() { return selectedAbility; }
    public Character GetCurrntTurnChara() { return (currentTurn == null) ? null : currentTurn.character; }
}

[System.Serializable]
public class BattleReport
{
    public int playerTotalATK_INT;
    public int playerTotalMaxHP;
    public int playerAveATK_INT;
    public int playerAveMaxHP;
    public float playerAveLVL;
    public float enemyAveLVL;
}