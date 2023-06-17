using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfoText : MonoBehaviour
{
    [SerializeField]
    Text nameText;
    [SerializeField]
    Text infoText;

    Character displayingChara;

    publicÅ@void SetText(string name,string info)
    {
        if (displayingChara != null)
        {
            displayingChara.GetCharacter_Object().SetSelectedIcon(false);
            displayingChara = null;
        }
            nameText.text = name;
        infoText.text = info;
    }
    public void SetCharaInfo(string name, string info,Character chara)
    {
        if (displayingChara != null)
        {
            displayingChara.GetCharacter_Object().SetSelectedIcon(false);
            displayingChara = null;
        }
        displayingChara = chara;
        nameText.text = name;
        infoText.text = info;
    }
}
