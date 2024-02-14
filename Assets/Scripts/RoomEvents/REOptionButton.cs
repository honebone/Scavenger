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
    int index;
    public void Init(RoomEvent.REOptionParams op,int id,InfoText i,ExpeditionManager e)
    {
        optionParams = op;
        index = id;
        infoText = i;
        optionNameText.text = optionParams.optionName;
        expeditionManager = e;
    }
    public void OnMouseDown()
    {
        if (Input.GetMouseButtonDown(1))
        {
            infoText.SetText(optionParams.optionName, optionParams.optionInfo);
        }
        if (Input.GetMouseButtonDown(0))
        {
            expeditionManager.SelectOption(index);
        }
    }

}
