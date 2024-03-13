using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaE_TreasureBox : RE_RandomEvents
{
    [SerializeField]
    List<RoomEvent.REOptionParams> options;
    [SerializeField]
    LootPanel.LootStatus lootStatus;
    [SerializeField]
    AreaManager.EnemySet mimic;
    [SerializeField]
    GameObject umbushed;
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
                infoText.AddLogText("宝箱を開けた");
                infoText.SwitchToLog();
                for (int i = 0; i < 3; i++)
                {
                    yield return new WaitForSeconds(0.5f);
                    infoText.AddLogText("");
                }
                lootPanel.DropItem_Loot(lootStatus);
                if (105.Probability())
                {
                    infoText.AddLogText("ミミックだ!!");
                    infoText.SwitchToLog();
                    yield return new WaitForSeconds(1f);
                    expeditionManager.Battle(mimic, umbushed);
                }
                else
                {
                    lootPanel.Loot();
                }
                break;
            case 1:
                infoText.AddLogText("嫌な予感がする...\n宝箱を無視して先に進んだ");
                infoText.SwitchToLog();
                yield return new WaitForSeconds(1.5f);
                EndRoomEvent();
                break;
        }
    }
}
