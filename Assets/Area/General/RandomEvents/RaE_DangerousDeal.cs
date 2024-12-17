using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaE_DangerousDeal : RE_RandomEvents
{
   [SerializeField] List<RoomEvent.REOptionParams> options;

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
                ExpeditionRef.loot.AddItem(Definer.equipments[(int)ItemData.Rarity.epic].Choice(), 1);
                ExpeditionRef.loot.AddItem(Definer.equipments[(int)ItemData.Rarity.epic].Choice(), 1);
                foreach (Character c in characterManager.GetExistingCharacters_All())
                {
                    if (c.CharaStatus().playable)
                    {
                        expeditionManager.SetPersonality(c, Definer.personalities[(int)PA_Personality.PersonalityStatus.PersonalityType.bad].Choice());
                    }
                }
                yield return new WaitForSeconds(1f);
                ExpeditionRef.loot.Loot();
                break;
            case 1:
                ExpeditionRef.loot.AddItem(Definer.equipments[(int)ItemData.Rarity.legendary].Choice(), 1);
                foreach (Character c in characterManager.GetExistingCharacters_All())
                {
                    if (c.CharaStatus().playable)
                    {
                        expeditionManager.SetPersonality(c, Definer.personalities[(int)PA_Personality.PersonalityStatus.PersonalityType.bad].Choice());
                        expeditionManager.SetPersonality(c, Definer.personalities[(int)PA_Personality.PersonalityStatus.PersonalityType.bad].Choice());
                    }
                }
                yield return new WaitForSeconds(1f);
                ExpeditionRef.loot.Loot();
                break;
            default:
                yield return new WaitForSeconds(0.5f);
                EndRoomEvent();
                break;
        }
    }
}
