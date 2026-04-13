using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaE_Statue : RE_RandomEvents
{
    [SerializeField]
    List<RoomEvent.REOptionParams> options;
    [SerializeField]
    LootPanel.LootStatus loot;

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
                infoText.AddLogText("女神像に旅の安全を祈った");
                infoText.SwitchToLog();
                foreach (Character chara in characterManager.GetExistingCharacters_All())
                {
                    chara.SANHeal(Random.Range(2, 11));
                }

                yield return new WaitForSeconds(1f);
                EndRoomEvent();

                break;
            case 1:
                infoText.AddLogText("女神像に供えられているものを盗んだ\n生きるためとはいえ、罪悪感に苛まれた");
                infoText.SwitchToLog();

                foreach (Character chara in characterManager.GetExistingCharacters_All())
                {
                    chara.SANDamage(Random.Range(5, 11));
                }

                yield return new WaitForSeconds(1f);
                lootPanel.DropItem_Loot(loot);
                lootPanel.Loot();
                break;
        }
    }
}
