using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class REOptionButton : MonoBehaviour
{
    [SerializeField]
    Text optionNameText;
    RoomEvent.REOptionParams optionParams;
    InfoText infoText;
    ExpeditionManager expeditionManager;
    MouseOverUI mouseOver;

    int index;
    public void Init(RoomEvent.REOptionParams op,int id,InfoText i,ExpeditionManager e)
    {
        optionParams = op;
        index = id;
        infoText = i;
        optionNameText.text = optionParams.available ? optionParams.optionName : optionParams.optionName.ColorStr(Color.red);
        expeditionManager = e;
        mouseOver = FindObjectOfType<MouseOverUI>();

    }
    public void OnMouseDown()
    {
        if (Input.GetMouseButtonDown(1))
        {
            infoText.SetText_Old(optionParams.optionName, optionParams.optionInfo);
            expeditionManager.REOption_OnRClick(index);
        }
        if (Input.GetMouseButtonDown(0) && optionParams.available)
        {
            mouseOver.ResetUI();
            expeditionManager.REOption_Select(index);
        }
    }

    public void OnMouseEnter()
    {
        infoText.SetText_Old(optionParams.optionName, optionParams.optionInfo);
        //mouseOver.SetUI("");
    }
    public void OnMouseExit()
    {
        //mouseOver.ResetUI();
    }
}
