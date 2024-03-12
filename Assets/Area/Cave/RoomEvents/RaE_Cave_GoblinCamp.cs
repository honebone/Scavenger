using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaE_Cave_GoblinCamp : RE_RandomEvents
{
    [SerializeField]
    List<RoomEvent.REOptionParams> options;
    [SerializeField]
    AreaManager.EnemySet enemySet;
    [SerializeField]
    LootPanel.LootStatus loot;
    [SerializeField]
    GameObject surprise;
    [SerializeField]
    GameObject siege;
    public override void StartRandomEvent()
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
                lootPanel.DropItem_Loot(loot);
                if (40.Probability()) { expeditionManager.Battle(enemySet, surprise); }
                else { expeditionManager.Battle(enemySet, null); }
                break;
            case 1:
                infoText.AddLogText("å©í£ÇËÇ…å©Ç¬Ç©ÇÁÇ»Ç¢ÇÊÇ§Ç…ÇµÇ»Ç™ÇÁÅAñÏâcínÇ…Ç‡ÇÆÇËÇ±ÇÒÇæ");
                infoText.SwitchToLog();
                for (int i = 0; i < 3; i++)
                {
                    yield return new WaitForSeconds(0.5f);
                    infoText.AddLogText("");
                }
                lootPanel.DropItem_Loot(loot);
                if (25.Probability())
                {
                    infoText.AddLogText("ñ⁄ÇäoÇ‹ÇµÇΩÉSÉuÉäÉìÇ∆ñ⁄Ç™çáÇ¡ÇΩ!!");
                    infoText.SwitchToLog();
                    expeditionManager.Battle(enemySet, siege);
                }
                else
                {
                    infoText.AddLogText("â◊ï®ÇîqéÿÇ∑ÇÈÇ±Ç∆Ç…ê¨å˜ÇµÇΩ");
                    yield return new WaitForSeconds(1f);
                    lootPanel.Loot();
                }
                break;
            case 2:
                infoText.AddLogText("ñÏâcínÇîÇØÇƒêÊÇ…êiÇÒÇæ");
                infoText.SwitchToLog();
                yield return new WaitForSeconds(1.5f);
                EndRoomEvent();
                break;
        }
    }
}
