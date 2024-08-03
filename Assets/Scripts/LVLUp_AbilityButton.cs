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

    [SerializeField] LVLUpManager lvlUpManger;
    [SerializeField] InfoText infoText;
    [SerializeField] MouseOverUI mouseOver;

    public void SetUpgrade(LVLUpManager.LVLUpParams lvlUp)
    {
        lvlUpParams = lvlUp;
        color = Definer.colorRef.abilityColors[(int)lvlUpParams.abilityStatus.abilityType];
        if (!lvlUpParams.upgrade)

        {
            //upgradeStr = string.Format("解放：{0}", lvlUpParams.abilityStatus.abilityName.ColorStr(color));
            upgradeStr = $"解放：{lvlUpParams.abilityStatus.abilityName.ColorStr(color)}";
            upgradeName.text = upgradeStr;
            upgradeInfo.text = "<<アビリティを解放>>";
        }
        else
        {
            upgradeStr = string.Format("強化：{0}", lvlUpParams.abilityStatus.abilityName.ColorStr(color));
            upgradeName.text = upgradeStr;
            upgradeInfo.text = "<<アビリティを強化>>\n\n";
            upgradeInfo.text += lvlUpParams.abilityStatus.abilityData.upgradeInfo;
        }
    }

    public void OnMouseDown()
    {

        if (Input.GetMouseButtonDown(1))
        {
            infoText.SetText(lvlUpParams.abilityStatus.abilityName.ColorStr(color), lvlUpParams.abilityStatus.GetInfo(false, new Character.CharacterStatus()));
        }

        if (Input.GetMouseButtonDown(0))
        {
            if (!lvlUpParams.upgrade) { lvlUpParams.abilityStatus.Unlock(); }
            else { lvlUpParams.character.UpgradeAbility(lvlUpParams.abilityStatus.abilityData); }
            lvlUpParams.character.LVLUp();

            lvlUpManger.EndSelectUpgrade();
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
