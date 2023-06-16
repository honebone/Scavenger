using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    [SerializeField]
    Transform selectedAbilityParent;

    CharactersManager charactersManager;
    Utility utility;

    List<Character> CharacterInTurnOrder;
    int currentTurn;
    bool roundEnd;

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
        //trigger
        RoundStart();
    }

    void RoundStart()
    {
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
        while(turns.Count > 0)
        {
            int a = utility.ChoiceWithWeight(ACT.ToArray());
            CharacterInTurnOrder.Add(charas[a]);
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

    public Character GetCurrntTurnChara() { return CharacterInTurnOrder[currentTurn]; }
}
