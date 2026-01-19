using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class REL_AddPer : RoomEndLog
{
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI infoText;
    public CanvasGroup titleCanvas;
    public CanvasGroup infoCanvas;

    public Image charaIcon;
    public Image perIcon;

    Character character;
    GameObject personality;

    public void Init_AddPer(Character chara,GameObject per)
    {
        character = chara;
        personality = per;

        PA_Personality perS = personality.GetComponent<PA_Personality>();

        charaIcon.sprite = character.CharaStatus().characterData.spriteForUI;
        perIcon.sprite = Definer.inst.cp.perIcons[(int)perS.GetPersonalityStatus().personalityType];

        titleText.text = $"êVÇΩÇ»ì¡ê´<{perS.GetPAName()}>";
        infoText.text = perS.GetPAInfo();
    }
    public override void OnLogStart()
    {
        if (CharactersManager.inst.GetExistingCharacters_All().Contains(character))
        {
            character.AddPA_Personality(personality,true);
        }
        manager.LogEnd();
    }
}
