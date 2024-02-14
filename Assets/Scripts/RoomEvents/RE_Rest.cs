using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RE_Rest : RoomEvent
{
    [SerializeField]
    List<RoomEvent.REOptionParams> options;
    public override void StartRoomEvent()
    {
        expeditionManager.LogREName("ãxëßÇÃ‚æâŒ");
       expeditionManager.SetREOptionButtons(options);
    }
    public override void SelectOption(int index)
    {
        infoText.AddDebugText("äÔèPîªíËäÆê¨Ç≥ÇπÇƒÇÀ");
        switch (index)
        {
            case 0:
                foreach(Character chara in characterManager.GetExistingCharacters_All())
                {
                    int maxHP = chara.GetCharacterStatus().maxHP;
                    chara.Heal(Mathf.RoundToInt(maxHP * 0.25f), null);
                    chara.SANHeal(15);
                }
                break;
            case 1:
                foreach(Character chara in characterManager.GetExistingCharacters_All())
                {
                    int maxHP = chara.GetCharacterStatus().maxHP;
                    //chara.Heal(Mathf.RoundToInt(maxHP * 0.25f), null);
                    chara.SANHeal(30);
                }
                break;
            case 2:
                foreach(Character chara in characterManager.GetExistingCharacters_All())
                {
                    int maxHP = chara.GetCharacterStatus().maxHP;
                    chara.Heal(Mathf.RoundToInt(maxHP * 0.5f), null);
                    chara.SANHeal(15);
                }
                break;
        }
        StartCoroutine(Delay());
    }
    IEnumerator Delay()
    {
        yield return new WaitForSeconds(1f);
        EndRoomEvent();
    }
}
