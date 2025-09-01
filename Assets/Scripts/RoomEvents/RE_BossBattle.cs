using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RE_BossBattle : RoomEvent
{
    [SerializeField]
    List<RoomEvent.REOptionParams> options;
    [SerializeField]
    AreaManager.EnemySet boss;
    [SerializeField]
    GameObject fieldEffect;
    [SerializeField] ExpeditionManager.BattleParams battleParams;

    protected int choice = 0;
    //GameObject eventManager;
   
    public override void OnEndREInfo()
    {
        expeditionManager.SetREOptionButtons(options);
    }
    public override void SelectOption(int index)
    {
        choice = index;
        StartCoroutine(Consequence());
    }
    IEnumerator Consequence()
    {
        switch (choice)
        {
            case 0:
                for (int i = 0; i < 3; i++)
                {
                    yield return new WaitForSeconds(0.5f);
                    infoText.AddLogText("");
                }
                lootPanel.AddExp(5);
                if(fieldEffect != null) { expeditionManager.Battle(new List<AreaManager.EnemySet> { boss }, fieldEffect, new ExpeditionManager.BattleParams()); }
                else { expeditionManager.Battle(new List<AreaManager.EnemySet> { boss }, null, battleParams); }
                break;
            case 1:
                infoText.AddLogText("êÊÇ…êiÇÒÇæ");
                infoText.SwitchToLog();
                yield return new WaitForSeconds(1.5f);
                EndRoomEvent();
                break;
        }
    }

    public override void OnEndBattle()
    {
        supplyManager.AddSupply_Eq(partyStatus.supplyOptions,ItemData.Rarity.epic);
        lootPanel.Loot();
    }
}
