using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CharaDetail_PerButton : MonoBehaviour
{
    [SerializeField] Image icon;
    [SerializeField] TextMeshProUGUI perName;
    [SerializeField] GameObject lockButton;
    [SerializeField] GameObject removeButton;
    PA_Personality per;

    List<PA_Personality.PersonalityStatus.PersonalityType> canLockTypes = new List<PA_Personality.PersonalityStatus.PersonalityType> { PA_Personality.PersonalityStatus.PersonalityType.good, PA_Personality.PersonalityStatus.PersonalityType.awoken ,PA_Personality.PersonalityStatus.PersonalityType.bad};
    List<PA_Personality.PersonalityStatus.PersonalityType> canRemoveTypes = new List<PA_Personality.PersonalityStatus.PersonalityType> { PA_Personality.PersonalityStatus.PersonalityType.good, PA_Personality.PersonalityStatus.PersonalityType.awoken };

    public void Init(PA_Personality p)
    {
        per = p;
        if (per == null) InfoText.inst.AddErrorText("per is null");
        perName.text = per.GetPAName();
        lockButton.SetActive(canLockTypes.Contains(per.GetPerType()));
        removeButton.SetActive(canRemoveTypes.Contains(per.GetPerType()));
        icon.sprite = Definer.inst.cp.perIcons[(int)per.GetPersonalityStatus().personalityType];
    }

    public void OnMouseDown()
    {
        InfoText.inst.SetText(per.GetPAName(),per.GetPAInfo(false), per.GetPAInfo(true));
    }
    public void OnMouseEnter()
    {
        MouseOverUI.inst.SetUI(per.GetPAInfo(true));
    }
    public void OnMouseExit()
    {
        MouseOverUI.inst.ResetUI();
    }
}
