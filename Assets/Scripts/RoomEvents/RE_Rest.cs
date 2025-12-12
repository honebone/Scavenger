using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class RE_Rest : RoomEvent
{
    [SerializeField]
    List<RoomEvent.REOptionParams> options;
    List<RoomEvent.REOptionParams> options2;
    int choice;
    int choice2;

    [SerializeField]
    GameObject umbushed;

    [SerializeField] float healRatio_rest = 0.2f;
    [SerializeField] int SANHeal_rest = 10;
    [SerializeField] int SANHeal_banquet = 20;
    [SerializeField] float healRatio_sleep = 0.5f;
    [SerializeField] int SANHeal_sleep = 15;

    int phase;
    List<Character> pool=new List<Character>();

    public override void OnEndREInfo()
    {
        pool = new List<Character>();
        foreach (Character c in characterManager.GetExistingCharacters_All())
        {
            if (c.CharaStatus().playable) { pool.Add(c); }
        }

        expeditionManager.SetREOptionButtons(options);
    }
    public override void SelectOption(int index)
    {
        if(phase == 0)
        {
            phase++;
            choice = index;
            StartCoroutine(Consequence());
        }
        else if(phase == 1)
        {
            choice2=index;
            StartCoroutine(Consequence2());
        }
        
        
    }
    IEnumerator Consequence()
    {
        switch (choice)
        {
            case 0:
                foreach (Character chara in characterManager.GetExistingCharacters_All())
                {
                    int maxHP = chara.CharaStatus().maxHP;
                    chara.Heal(Mathf.RoundToInt(maxHP * healRatio_rest), null);
                    chara.SANHeal(SANHeal_rest);
                }
                EndRoomEvent();
                break;
            case 1:
                foreach (Character chara in characterManager.GetExistingCharacters_All())
                {
                    chara.SANHeal(SANHeal_banquet);
                }
                EndRoomEvent();
                break;
            case 2:
                for (int i = 0; i < 3; i++)
                {
                    yield return new WaitForSeconds(0.5f);
                    infoText.AddLogText("");
                }
                if (20.Dice())
                {
                    expeditionManager.Battle(new List<AreaManager.EnemySet> { currentArea.GetRandomEnemySet() }, umbushed, new ExpeditionManager.BattleParams());
                }
                else
                {
                    foreach (Character chara in characterManager.GetExistingCharacters_All())
                    {
                        int maxHP = chara.CharaStatus().maxHP;
                        chara.Heal(Mathf.RoundToInt(maxHP * healRatio_sleep), null);
                        chara.SANHeal(SANHeal_sleep);
                    }
                    EndRoomEvent();
                }
                break;
            case 3:
                

                options2 = new List<REOptionParams>();
                foreach (Character character in pool)
                {
                    Character.CharacterStatus status = character.CharaStatus();
                    REOptionParams option = new REOptionParams();
                    option.optionName = string.Format("{0}Ç…ì«Ç‹ÇπÇÈ", status.charaName);
                    option.optionInfo = "ÉâÉìÉ_ÉÄÇ»1-3å¬ÇÃì¡ê´ÇìæÇÈ";
                    options.Add(option);
                }

                expeditionManager.SetREOptionButtons(options);
                break;
        }
    }
    IEnumerator Consequence2()
    {
        switch (choice2)
        {
            case 0:
                foreach (Character chara in characterManager.GetExistingCharacters_All())
                {
                    int maxHP = chara.CharaStatus().maxHP;
                    chara.Heal(Mathf.RoundToInt(maxHP * healRatio_rest), null);
                    chara.SANHeal(SANHeal_rest);
                }
                EndRoomEvent();
                break;
            case 1:
                foreach (Character chara in characterManager.GetExistingCharacters_All())
                {
                    chara.SANHeal(SANHeal_banquet);
                }
                EndRoomEvent();
                break;
            case 2:
                for (int i = 0; i < 3; i++)
                {
                    yield return new WaitForSeconds(0.5f);
                    infoText.AddLogText("");
                }
                if (20.Dice())
                {
                    expeditionManager.Battle(new List<AreaManager.EnemySet> { currentArea.GetRandomEnemySet() }, umbushed, new ExpeditionManager.BattleParams());
                }
                else
                {
                    foreach (Character chara in characterManager.GetExistingCharacters_All())
                    {
                        int maxHP = chara.CharaStatus().maxHP;
                        chara.Heal(Mathf.RoundToInt(maxHP * healRatio_sleep), null);
                        chara.SANHeal(SANHeal_sleep);
                    }
                    EndRoomEvent();
                }
                break;
        }
    }
}
