using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class REL_AddPer : RoomEndLog
{
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI infoText;
    [SerializeField] TextMeshProUGUI errorText;

    public Image charaIcon;
    public Image perIcon;

    [SerializeField] CanvasGroup canvas_buttons;
    [SerializeField] Transform buttonsP;
    [SerializeField] GameObject perButton;
    List<REL_AddPerButton> buttons=new List<REL_AddPerButton>();

    Character character;
    GameObject personality;
    PA_Personality perS;
    List<PA_Personality.PersonalityStatus.PersonalityType> goodPers = new List<PA_Personality.PersonalityStatus.PersonalityType>() { PA_Personality.PersonalityStatus.PersonalityType.awoken
        , PA_Personality.PersonalityStatus.PersonalityType.good };

    PA_Personality selected;


    public void Init_AddPer(Character chara,GameObject per)
    {
        character = chara;
        personality = per;

        perS = personality.GetComponent<PA_Personality>();

        charaIcon.sprite = character.CharaStatus().characterData.spriteForUI;
        perIcon.sprite = Definer.inst.cp.perIcons[(int)perS.GetPersonalityStatus().personalityType];

        titleText.text = $"新たな特性<{perS.GetPAName()}>";
        infoText.text = perS.GetPAInfo();

        
    }
    public override void OnLogStart()
    {
        if (character.CheckSamePer(personality))
        {
            errorText.text = "同じ特性を所持しているためスキップ";

            manager.LogEnd();
        }
        else if (goodPers.Contains(perS.GetPerType()) && character.CheckGoodPersMax())
        {
            errorText.text = "良い特性が上限数に到達";
            canvas_buttons.alpha = 1;
            canvas_buttons.interactable = true;
            canvas_buttons.blocksRaycasts = true;

            character.AddPA_Personality(personality, true);
            character.GetPers(goodPers).ForEach(p =>
            {
                var b = Instantiate(perButton, buttonsP);
                b.GetComponent<REL_AddPerButton>().Init(p, this);
                buttons.Add(b.GetComponent<REL_AddPerButton>());
            });

            removing = true;
        }
        else if(perS.GetPerType()==PA_Personality.PersonalityStatus.PersonalityType.bad && character.CheckBadPersMax())
        {
            PA_Personality remove = character.GetPers(PA_Personality.PersonalityStatus.PersonalityType.bad).Choice();
            errorText.text = $"<{remove.GetPAName()}と入れ替え>";
            remove.Disable();

            character.AddPA_Personality(personality, true);

            manager.LogEnd();
            //canvas_buttons.alpha = 1;
            //canvas_buttons.interactable = true;
            //canvas_buttons.blocksRaycasts = true;

            //character.AddPA_Personality(personality, true);
            //character.GetPers(PA_Personality.PersonalityStatus.PersonalityType.bad).ForEach(p =>
            //{
            //    var b = Instantiate(perButton, buttonsP);
            //    b.GetComponent<REL_AddPerButton>().Init(p,this);
            //    buttons.Add(b.GetComponent<REL_AddPerButton>());
            //});

            //removing = true;
        }
        else
        {
            character.AddPA_Personality(personality, true);

            manager.LogEnd();
        }
    }

    bool removing;

    public void SelectPer(REL_AddPerButton button)
    {
        if (removing)
        {
            soundManager.PlaySE_Select();

            buttons.ForEach(b => { if (b != button) b.ResetSelected(); });
            selected = button.GetPer();
        }
    }

    public void Confirm()
    {
        if(selected!=null&& removing)
        {
            soundManager.PlaySE_Select();
            selected.Disable();
            removing = false;

            canvas_buttons.alpha = 0;
            canvas_buttons.interactable = false;
            canvas_buttons.blocksRaycasts = false;

            manager.LogEnd();
        }
    }
}
