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
                    int maxHP = chara.GetCharacterStatus().maxHP;
                    chara.Heal(Mathf.RoundToInt(maxHP * 0.25f), null);
                    chara.SANHeal(10);
                }
                yield return new WaitForSeconds(1f);
                EndRoomEvent();
                break;
            case 1:
                foreach (Character chara in characterManager.GetExistingCharacters_All())
                {
                    //int maxHP = chara.GetCharacterStatus().maxHP;
                    //chara.Heal(Mathf.RoundToInt(maxHP * 0.25f), null);
                    chara.SANHeal(20);
                }
                yield return new WaitForSeconds(1f);
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
                    expeditionManager.Battle(currentArea.GetRandomEnemySet(), umbushed, new ExpeditionManager.BattleParams());
                }
                else
                {
                    foreach (Character chara in characterManager.GetExistingCharacters_All())
                    {
                        int maxHP = chara.GetCharacterStatus().maxHP;
                        chara.Heal(Mathf.RoundToInt(maxHP * 0.5f), null);
                        chara.SANHeal(15);
                    }
                    yield return new WaitForSeconds(1f);
                    EndRoomEvent();
                }
                break;
        }
    }
   
}
