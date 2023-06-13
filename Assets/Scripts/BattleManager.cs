using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{

    CharactersManager charactersManager;
    List<Character> turnOrder;
    int currentTurn;
    bool roundEnd;
    private void Start()
    {
        charactersManager = FindObjectOfType<CharactersManager>();
    }

    public void BattleStart()
    {
        //trigger
        RoundStart();
    }

    void RoundStart()
    {
        //trigger
        Debug.Log("ラウンド開始");

        DicideTurnOrder();
    }
    void DicideTurnOrder()
    {
        currentTurn = 0;
        turnOrder=charactersManager.GetExistingCharacters();//test
        turnOrder[currentTurn].MyTurnStart();
    }
    public void TurnEnd()
    {
        currentTurn++;
        if (currentTurn == turnOrder.Count) { RoundEnd(); }
        else { turnOrder[currentTurn].MyTurnStart(); }
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

    public Character GetCurrntTurnChara() { return turnOrder[currentTurn]; }
}
