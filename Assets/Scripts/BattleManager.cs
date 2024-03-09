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

    CharactersManager charactersManager;
    Utility utility;
    InfoText infoText;
    MessageText messageText;
    ExpeditionManager expeditionManager;
    ActionQueueManager actionQueue;
    SoundManager soundManager;
    PositionManager[] positionManagers;
    ExpeditionManager.PartyStatus partyStatus;

    FieldEffect fieldEffect;
    int roundCount;

    List<Character> characterInTurnOrder;
    List<Battle_TurnOrderIcon> turnOrderIcons=new List<Battle_TurnOrderIcon>();
    int currentTurn;
    /// <summary>nextRoundButtonを押せるかどうか</summary>
    //bool roundEnd;

    public static bool inBattle;
    public static bool inRound;
    public static bool selectingAbility;
    public static bool selectingTarget;
    public static Ability selectedAbility;
    private void Start()
    {
        charactersManager = FindObjectOfType<CharactersManager>();
        utility =FindObjectOfType<Utility>();
        infoText = FindObjectOfType<InfoText>();
        messageText = FindObjectOfType<MessageText>();
        expeditionManager = FindObjectOfType<ExpeditionManager>();
        partyStatus = expeditionManager.GetPartyStatus();
        actionQueue = FindObjectOfType<ActionQueueManager>();
        soundManager = FindObjectOfType<SoundManager>();
        positionManagers = charactersManager.GetPositionManagers();

        characterInTurnOrder=new List<Character>();
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
        currentTurn = 0;
        characterInTurnOrder.Clear();
        List<Character> charas = new List<Character>(charactersManager.GetExistingCharacters_All());
        List<int> turns = new List<int>();
        List<float> ACT = new List<float>();

        foreach(Character chara in charas)
        {
            turns.Add(chara.GetCharacterStatus().turnPerRound);
            ACT.Add(chara.GetCharacterStatus().ACT);
            chara.SetTurnIcon();
        }
        for (int i = 0; turns.Count > 0; i++)
        {
            int a = utility.ChoiceWithWeight(ACT.ToArray());
            characterInTurnOrder.Add(charas[a]);
            var t = Instantiate(turnOrderIcon, turnOrderIconParent);
            t.GetComponent<Battle_TurnOrderIcon>().Init(charas[a], i < partyStatus.turnOrderReveal);
            turnOrderIcons.Add(t.GetComponent<Battle_TurnOrderIcon>());
            turns[a]--;
            if (turns[a] == 0)
            {
                charas.RemoveAt(a);
                turns.RemoveAt(a);
                ACT.RemoveAt(a);
            }
        }
        //foreach (Character chara in charactersManager.GetExistingCharacters_All())
        //{
        //    chara.SetOmen(); //このラウンド、ターンが1つ以上まわってくるキャラに予兆をセットさせる
        //}

        Trigger_TurnOrderDecide();
    }

    public void RemoveTurn(Character chara)
    {
        List<Battle_TurnOrderIcon> removeTurns = new List<Battle_TurnOrderIcon>();
        foreach (Battle_TurnOrderIcon turn in turnOrderIcons)
        {
            if (turn.GetCharacter() == chara) { removeTurns.Add(turn); }
        }
        foreach (Battle_TurnOrderIcon remove in removeTurns)
        {
            turnOrderIcons.Remove(remove);
            Destroy(remove.gameObject);
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
        characterInTurnOrder[currentTurn].MyTurnStart();
    }
    public void TurnEnd()
    {
        Destroy(turnOrderIconParent.GetChild(0).gameObject);
        turnOrderIcons.RemoveAt(0);
        for (int i = 0; i < Mathf.Min(turnOrderIcons.Count, 3); i++) { turnOrderIcons[i].Reveal(); }

        for (int i = currentTurn+1; i < characterInTurnOrder.Count; i++)//次の生きているキャラのターンを開始
        {
            if (characterInTurnOrder[i].CheckAlive())
            {
                currentTurn = i;

                //foreach (Character chara in charactersManager.GetExistingCharacters_All())
                //{
                //    chara.SetOmen(); //このラウンド、ターンが1つ以上まわってくるキャラに予兆をセットさせる
                //}

                characterInTurnOrder[i].MyTurnStart();
                return;
            }
        }
        RoundEnd();//ターンが回ってきていない生きているキャラがもういないならラウンド終了
        //if (currentTurn == characterInTurnOrder.Count) { RoundEnd(); }
        //else
        //{
        //    for (int i = currentTurn; i < characterInTurnOrder.Count; i++)
        //    {
        //        if (characterInTurnOrder[i].CheckAlive())
        //        {
        //            currentTurn = i;
        //            characterInTurnOrder[i].MyTurnStart();
        //        }
        //    }
        //}
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
        currentTurn = 0;
        inRound = false;
        inBattle = false;
        if (selectedAbility) { infoText.AddErrorText("アビリティ選択中に戦闘が終了しました"); }
        if (selectingTarget) { infoText.AddErrorText("対象選択中に戦闘が終了しました"); }

        if (fieldEffect != null)
        {
            Destroy(fieldEffectP.GetChild(0).gameObject);
            fieldEffect = null;
        }
        characterInTurnOrder = new List<Character>();
        turnOrderIcons = new List<Battle_TurnOrderIcon>();
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

        StartCoroutine(BattleEndAnim());
    }

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
        if(fieldEffect != null) { fieldEffect.OnBattleStart(); }
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
        if (fieldEffect != null) { fieldEffect.OnTurnStart(currentTurn + 1); }
        foreach (Character character in charactersManager.GetExistingCharacters_All())
        {
            character.OnTurnStart(checkIfMyTurn(character), currentTurn + 1);
        }
        foreach (PositionManager positionManager in positionManagers)
        {
            positionManager.OnTurnStart();
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
        {
            if (charactersManager.CheckCharaExist(i) && !charactersManager.GetCharacterWithPos(i).GetCharacterStatus().immovable)//移動不可でないキャラがポジションiに存在しているなら
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
        for (int i = 0; i < selectedAbilityParent.childCount; i++) { Destroy(selectedAbilityParent.GetChild(i).gameObject); }
        GameObject abilityManager;
        if (abilityStatus.abilityManager != null) { abilityManager = abilityStatus.abilityManager; }
        else { abilityManager=Definer.abilityManager_General; }
        var a = Instantiate(abilityManager, selectedAbilityParent);
        a.GetComponent<Ability>().Init(character, abilityStatus);
        selectedAbility = a.GetComponent<Ability>();
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
        if (inBattle && inRound && characterInTurnOrder[currentTurn] == character) { return true; }
        return false;
    }
    public bool CheckIfTurnRemain(Character character)
    {
        for (int i = currentTurn; i < characterInTurnOrder.Count; i++)
        {
            if (characterInTurnOrder[i] == character) { return true; }
        }
        return false;
    }

    public Character GetCurrntTurnChara() { return characterInTurnOrder[currentTurn]; }
}
