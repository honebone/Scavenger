using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LVLUp_AbilityButton : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI upgradeName;
    [SerializeField] TextMeshProUGUI upgradeInfo;

    Color color;
    string upgradeStr;
    LVLUpManager.LVLUpParams lvlUpParams;

    [SerializeField] MouseOverUI mouseOver;

    public void SetUpgrade(LVLUpManager.LVLUpParams lvlUp)
    {
        lvlUpParams = lvlUp;
        color = Definer.colorRef.abilityColors[(int)lvlUpParams.abilityStatus.abilityType];
        if (!lvlUpParams.upgrade)

        {
            upgradeStr = string.Format("解放：{0}", lvlUpParams.abilityStatus.abilityName.ColorStr(color));
            upgradeName.text = upgradeStr;
            upgradeInfo.text = "アビリティを解放し、使用可能にする";
        }
        else
        {
            upgradeStr = string.Format("強化：{0}", lvlUpParams.abilityStatus.abilityName.ColorStr(color));
            upgradeName.text = upgradeStr;
            upgradeInfo.text = lvlUpParams.abilityStatus.abilityData.upgradeInfo;
        }
    }

    public void OnMouseDown()
    {

        if (Input.GetMouseButtonDown(1))
        {
           
        }

        if (Input.GetMouseButtonDown(0))
        {
         
        }
    }
    public void OnMouseEnter()
    {
        mouseOver.SetUI(upgradeStr, true);
    }
    public void OnMouseExit()
    {
        mouseOver.ResetUI();
    }
}
