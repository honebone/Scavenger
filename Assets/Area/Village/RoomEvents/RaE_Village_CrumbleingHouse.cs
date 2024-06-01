using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaE_Village_CrumbleingHouse : RE_RandomEvents
{
    [SerializeField] REOptionParams exit;
    [SerializeField] float defaultChance;
    [SerializeField] float mul;
    [SerializeField] LootPanel.LootStatus lootStatus;

    float crumbleChance;
    string lootLog = "納屋の中へ入り、使えそうなものを探した";
    string exitLog = "瓦礫の下敷きになるのは御免だ\n納屋には入らず先に進んだ";

    bool firstLoot = true;

    public override void OnEndREInfo()
    {
        if (firstLoot)
        {
            firstLoot = false;
            StartRandomEvent();
        }
        else
        {
            List<REOptionParams> options = new List<REOptionParams>();
            crumbleChance *= mul;

            lootLog = "\nもう少しだけ...";
            exitLog = "\nそろそろ外へ出たほうがよさそうだ";

            REOptionParams loot = new REOptionParams();
            loot.optionName = "漁り続ける";
            loot.optionInfo = string.Format("装備品を得ることができる\n{0}％の確率で納屋が倒壊し、全員がHPを20-30％失う", crumbleChance);

            REOptionParams stop = new REOptionParams();
            stop.optionName = "納屋から出る";
            stop.optionInfo = "納屋から出る";

            options.Add(loot);
            options.Add(stop);

            expeditionManager.SetREOptionButtons(options);
        }
    }


    public override void StartRandomEvent()
    {
        List<REOptionParams> options = new List<REOptionParams>();
        crumbleChance = defaultChance;

        REOptionParams loot = new REOptionParams();
        loot.optionName = "納屋を漁る";
        loot.optionInfo = string.Format("装備品を得ることができる\n{0}％の確率で納屋が倒壊し、全員がHPを20-30％失う",crumbleChance);

        options.Add(loot);
        options.Add(exit);

        expeditionManager.SetREOptionButtons(options);
    }

    public override void OnEndLoot()
    {
        expeditionManager.SetREInfo("倒壊寸前の納屋", "有用な品をいくつか頂戴したが、まだ探していないところはたくさんあるようだ\nしかし、建物中からミシミシという音が鳴っている");
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
                infoText.AddLogText(lootLog);
                infoText.SwitchToLog();

                yield return new WaitForSeconds(1.5f);
                if (crumbleChance.Dice())
                {
                    infoText.AddLogText("納屋が倒壊し、瓦礫の下敷きになった！！");
                    infoText.SwitchToLog();
                    yield return new WaitForSeconds(1f);
                    foreach (Character chara in characterManager.GetExistingCharacters_All())
                    {
                        int maxHP = chara.GetCharacterStatus().maxHP;
                        float value = Random.Range(0.2f, 0.3f);
                        chara.DecreaseHP(Mathf.RoundToInt(maxHP * value));
                    }
                    yield return new WaitForSeconds(1.5f);
                    EndRoomEvent();
                }
                else
                {
                    lootPanel.DropItem_Loot(lootStatus);
                    lootPanel.Loot();
                }
                break;
            case 1:
                infoText.AddLogText(exitLog);
                infoText.SwitchToLog();
                yield return new WaitForSeconds(1.5f);
                EndRoomEvent();
                break;
        }
    }
}
