using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Setting_Toggle : SettingsOption
{
    [SerializeField, TextArea(3, 10)] string info;
    [SerializeField] bool defaultValue;

    [SerializeField, Header("\n\n\n")] Toggle toggle;

    bool value;
    bool initialized;

    public override void Init(string k,Del del)
    {
        initialized = true;
        key = k;
        valueChanged = del;

        value = PlayerPrefs.GetFloat($"Settings_{key}", defaultValue ? 1 : 0) == 1;
        toggle.isOn = value;
    }

    public void OnChangeValue()
    {
        if (initialized)
        {
            value = toggle.isOn;
            //settingManager.SetValue(value ? 1 : 0, key);
            valueChanged(value ? 1 : 0, key);

            //InfoText.inst.AddDebugText($"{key} toggeled to {value}");
            Debug.Log($"{key} toggeled to {value}");
        }        
    }

    public override float GetValue()
    {
        return value ? 1 : 0;
    }

    public void OnMouseEnter()
    {
        if (info != "") { MouseOverUI.inst.SetUI(info); }
    }

    public void OnMouseExit()
    {
        if (info != "") { MouseOverUI.inst.ResetUI(); }
    }

    public void OnMouseDown()
    {
        //toggle.isOn = !toggle.isOn;
        //value = toggle.isOn;
        //settingManager.SetValue(value ? 1 : 0, key);

        //InfoText.inst.AddDebugText($"{key} toggeled to {value}");
    }
}
