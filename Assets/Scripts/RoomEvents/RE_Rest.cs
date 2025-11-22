using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RE_Rest : RoomEvent
{
    [SerializeField]
    List<RoomEvent.REOptionParams> options;
    int choice;

    [SerializeField]
    GameObject umbushed;

    [SerializeField] float healRatio_rest = 0.2f;
    [SerializeField] int SANHeal_rest = 10;
    [SerializeField] int SANHeal_banquet = 20;
    [SerializeField] float healRatio_sleep = 0.5f;
    [SerializeField] int SANHeal_sleep = 15;

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
