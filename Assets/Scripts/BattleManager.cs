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

    int roundCount;

    List<Character> CharacterInTurnOrder;
    List<Battle_TurnOrderIcon> TurnOrderIcons=new List<Battle_TurnOrderIcon>();
    int currentTurn;
    bool roundEnd;

    public static bool inBattle;
    public static bool selectingAbility;
    public static bool selectingTarget;
    public static Ability selectedAbility;
    private void Start()
    {
        charactersManager = FindObjectOfType<CharactersManager>();
        utility =FindObjectOfType<Utility>();

        CharacterInTurnOrder=new List<Character>();
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
        //trigger
        DicideTurnOrder();
    }
    void DicideTurnOrder()
    {
        currentTurn = 0;
        CharacterInTurnOrder.Clear();
        List<Character> charas = new List<Character>(charactersManager.GetExistingCharacters());
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
            CharacterInTurnOrder.Add(charas[a]);
            var t = Instantiate(turnOrderIcon, turnOrderIconParent);
            t.GetComponent<Battle_TurnOrderIcon>().Init(charas[a], i < 3);
            TurnOrderIcons.Add(t.GetComponent<Battle_TurnOrderIcon>());
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
    IEnumerator RoundStartEffect()
    {
        Debug.Log("ラウンド開始");
        yield return new WaitForSeconds(1f);
        CharacterInTurnOrder[currentTurn].MyTurnStart();
    }
    public void TurnEnd()
    {
        currentTurn++;
        Destroy(turnOrderIconParent.GetChild(0).gameObject);
        TurnOrderIcons.RemoveAt(0);
        for(int i = 0; i < Mathf.Min(TurnOrderIcons.Count, 3); i++) { TurnOrderIcons[i].Reveal(); }
        if (currentTurn == CharacterInTurnOrder.Count) { RoundEnd(); }
        else { CharacterInTurnOrder[currentTurn].MyTurnStart(); }
    }

    public void RoundEnd()
    {
        //trigger
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
        var a = Instantiate(abilityStatus.abilityManager, selectedAbilityParent);
        a.GetComponent<Ability>().Init(character, abilityStatus);
        selectedAbility = a.GetComponent<Ability>();
        selectingTarget=true;
    }
    /// <summary>アビリティの対象選択が終了したときに呼ぶ </summary>
    public void ResetSelectedAbility()
    {
        selectingTarget = false;
        for (int i = 0; i < selectedAbilityParent.childCount; i++) { Destroy(selectedAbilityParent.GetChild(i).gameObject); }
        selectedAbility=null;
    }
    public bool checkIfMyTurn(Character character)
    {
        if (inBattle && CharacterInTurnOrder[currentTurn] == character) { return true; }
        return false;
    }

    public Character GetCurrntTurnChara() { return CharacterInTurnOrder[currentTurn]; }
}
