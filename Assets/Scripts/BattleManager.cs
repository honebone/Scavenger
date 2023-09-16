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
    Text roundText;

    [SerializeField]
    GameObject turnOrderIcon;

    [SerializeField]
    AudioClip SE_battleStart;

    CharactersManager charactersManager;
    Utility utility;
    InfoText infoText;
    MessageText messageText;
    ExpeditionManager expeditionManager;
    ActionQueueManager actionQueue;
    SoundManager soundManager;

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
        actionQueue = FindObjectOfType<ActionQueueManager>();
        soundManager = FindObjectOfType<SoundManager>();

        characterInTurnOrder=new List<Character>();
    }

    public void BattleStart()
    {
        infoText.AddLogText("\n◇◇◇◇戦闘開始◇◇◇◇");
        inBattle = true;
        soundManager.PlaySE(SE_battleStart);
        
        roundCount=0;
        StartCoroutine(BattleStartAnim());
    }
    IEnumerator BattleStartAnim()
    {
        yield return new WaitForSeconds(1f);
        foreach (Character character in charactersManager.GetExistingCharacters_All())
        {
            character.OnBattleStart();
        }
        actionQueue.StartResolve(0);
    }

   public void RoundStart()
    {
        roundCount++;
        roundText.text=roundCount.ToString();
        infoText.AddLogText(string.Format("\n◇◇ラウンド{0}◇◇", roundCount));
        //trigger
        DicideTurnOrder();
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
            t.GetComponent<Battle_TurnOrderIcon>().Init(charas[a], i < 3);
            turnOrderIcons.Add(t.GetComponent<Battle_TurnOrderIcon>());
            turns[a]--;
            if (turns[a] == 0)
            {
                charas.RemoveAt(a);
                turns.RemoveAt(a);
                ACT.RemoveAt(a);
            }
        }
        foreach (Character chara in charactersManager.GetExistingCharacters_All())
        {
            chara.SetOmen(); //このラウンド、ターンが1つ以上まわってくるキャラに予兆をセットさせる
        }

        StartCoroutine(RoundStartEffect());
    }

    public void RemoveTurn(Character chara)
    {
        for (int i = 0; i < turnOrderIcons.Count; i++)
        {
            if (turnOrderIcons[i].GetCharacter() == chara)
            {
                //characterInTurnOrder.RemoveAt(i);
                turnOrderIcons.RemoveAt(i);
                Destroy(turnOrderIconParent.GetChild(i).gameObject);
                //if (i <= currentTurn) { currentTurn--; }
            }
        }
        //foreach(Battle_TurnOrderIcon turnOrderIcon in turnOrderIcons) { turnOrderIcon.RemoveTurnOrderIcon(chara); }
    }



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

                foreach (Character chara in charactersManager.GetExistingCharacters_All())
                {
                    chara.SetOmen(); //このラウンド、ターンが1つ以上まわってくるキャラに予兆をセットさせる
                }

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
        //trigger
        inRound = false;
        //roundEnd = true;
        Debug.Log("ラウンド終了");
        RoundStart();
    }

    public void BattleEnd()
    {
        infoText.AddLogText("\n◇◇◇◇戦闘終了◇◇◇◇");
        currentTurn = 0;
        inRound = false;
        inBattle = false;
        if (selectedAbility) { infoText.AddDebugText("アビリティ選択中に戦闘が終了しました"); }
        if (selectingTarget) { infoText.AddDebugText("対象選択中に戦闘が終了しました"); }

        characterInTurnOrder = new List<Character>();
        turnOrderIcons = new List<Battle_TurnOrderIcon>();
        for (int i = 0; i < turnOrderIconParent.childCount; i++) { Destroy(turnOrderIconParent.GetChild(i).gameObject); }

        //各キャラクターにも知らせる
        //generatedCharaから死亡しているキャラを消去
        expeditionManager.OnEndBattle();
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

    public void SetOmenIcon(Character chara,Ability.AbilityStatus abilityStatus)
    {
        foreach (Battle_TurnOrderIcon turnOrderIcon in turnOrderIcons)
        {
            if (turnOrderIcon.GetCharacter() == chara)
            {
                turnOrderIcon.SetOmenIcon(abilityStatus);
                return;
            }
        }
        infoText.AddDebugText("error");
    }
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
