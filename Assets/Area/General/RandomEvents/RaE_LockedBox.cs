using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaE_LockedBox : RE_RandomEvents
{
    [SerializeField] REOptionParams breach;
    [SerializeField] LootPanel.LootStatus loot_breach;
    [SerializeField] LootPanel.LootStatus loot_pick;
    float chance;

    public override void StartRandomEvent()
    {
        float maxCRITC = 0;
        foreach (Character chara in characterManager.GetExistingCharacters_All())
        {
            float CRITC = chara.GetCharacterStatus().CRITC;
            maxCRITC = Mathf.Max(CRITC, maxCRITC);
        }
        chance = maxCRITC * 2;

        REOptionParams pick = new REOptionParams();
        pick.optionName = "開錠する";
        pick.optionInfo = "パーティで最もCRIT率の多いキャラの、[CRIT率]x2の確率で成功し、装備品を6-8個手に入れる\n";
        pick.optionInfo += string.Format("成功確率：{0}％", maxCRITC * 2);
        expeditionManager.SetREOptionButtons(new List<REOptionParams> { breach, pick });
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
                infoText.AddLogText("大きく振りかぶって、宝箱の蓋を吹き飛ばした");
                infoText.SwitchToLog();

                yield return new WaitForSeconds(1f);
                lootPanel.DropItem_Loot(loot_breach);
                lootPanel.Loot();
                break;
            case 1:
                if (chance.Dice())
                {
                    infoText.AddLogText("鍵を開けるのに成功した！");
                    infoText.SwitchToLog();

                    yield return new WaitForSeconds(1f);
                    lootPanel.DropItem_Loot(loot_pick);
                    lootPanel.Loot();
                }
                else
                {
                    infoText.AddLogText("鍵は開かず、中身を手に入れることができなかった...");
                    infoText.SwitchToLog();

                    yield return new WaitForSeconds(1f);
                    EndRoomEvent();
                }
                               
                break;
        }
    }
}
