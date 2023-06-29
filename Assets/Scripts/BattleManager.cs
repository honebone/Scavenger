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

    CharactersManager charactersManager;
    Utility utility;
    InfoText infoText;

    int roundCount;

    List<Character> characterInTurnOrder;
    List<Battle_TurnOrderIcon> turnOrderIcons=new List<Battle_TurnOrderIcon>();
    int currentTurn;
    /// <summary>nextRoundButtonを押せるかどうか</summary>
    bool roundEnd;

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

        characterInTurnOrder=new List<Character>();
    }

    public void BattleStart()
    {
        inBattle = true;
        //trigger
        
        roundCount=0;
        RoundStart();
    }

    void RoundStart()
    {
        roundCount++;
        roundText.text=roundCount.ToString();
        infoText.AddLogText(string.Format("\n◇◇ラウンド{0}◇◇", roundCount));
        //trigger
        DicideTurnOrder();
    }
    void DicideTurnOrder()
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
        Debug.Log("ラウンド開始");
        yield return new WaitForSeconds(1f);
        inRound = true;
        characterInTurnOrder[currentTurn].MyTurnStart();
    }
    public void TurnEnd()
    {
        infoText.AddDebugText("ターン終了");
        Destroy(turnOrderIconParent.GetChild(0).gameObject);
        infoText.AddDebugText("破壊");
        turnOrderIcons.RemoveAt(0);
        for (int i = 0; i < Mathf.Min(turnOrderIcons.Count, 3); i++) { turnOrderIcons[i].Reveal(); }

        for (int i = currentTurn+1; i < characterInTurnOrder.Count; i++)//次の生きているキャラのターンを開始
        {
            if (characterInTurnOrder[i].CheckAlive())
            {
                currentTurn = i;
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
        roundEnd = true;
        Debug.Log("ラウンド終了");
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha2)) { BattleStart(); }//test
        if (Input.GetKeyDown(KeyCode.Space) && roundEnd)//test
        {
            roundEnd = false;
            RoundStart();
        }
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
    public void SetSelectingAbility(bool f) { selectingAbility = f; }
    public void SetSelectingTarget(bool f) { selectingTarget = f; }
    public bool checkIfMyTurn(Character character)
    {
        if (inBattle && inRound && characterInTurnOrder[currentTurn] == character) { return true; }
        return false;
    }

    public Character GetCurrntTurnChara() { return characterInTurnOrder[currentTurn]; }
}
