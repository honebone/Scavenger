using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using TMPro;

public class REL_AddPerButton : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI nameText;
    PA_Personality per;
    [SerializeField] Image back;
    [SerializeField] Color col_selected;
    [SerializeField] Color col_unselected;

    REL_AddPer manager;
    public void Init(PA_Personality p,REL_AddPer m)
    {
        manager = m;
        if (p == null) InfoText.inst.AddErrorText("per is null");

        per=p;
        nameText.text = per.GetPAName();
    }

    public void ResetSelected() { back.color = col_unselected; }
    public void OnMouseDown()
    {
        InfoText.inst.SetText(per.GetPAName(), per.GetPAInfo(false), per.GetPAInfo(true));
        if(Input.GetMouseButtonDown(0))
        {
            back.color = col_selected;
            manager.SelectPer(this);
        }
    }
    public void OnMouseEnter()
    {
        MouseOverUI.inst.SetUI(per.GetPAInfo(true));
    }
    public void OnMouseExit()
    {
        MouseOverUI.inst.ResetUI();
    }

    public PA_Personality GetPer() { return per; }
}
