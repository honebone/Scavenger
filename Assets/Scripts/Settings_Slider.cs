using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Settings_Slider : SettingsOption
{
    [SerializeField,TextArea(3,10)] string info;
    [SerializeField] float defaultValue;
    [SerializeField] string unit;
    [SerializeField] Vector2 valueMul = new Vector2(0, 1);

    [SerializeField,Header("\n\n\n")] Slider slider;
    [SerializeField] TextMeshProUGUI valueText;

    /// <summary>スライダーの値ではなく実際の値</summary>
    float value;


    public override void Init(string k,Del del)
    {
        key = k;
        valueChanged = del;

        value = PlayerPrefs.GetFloat($"Settings_{key}", defaultValue);
        slider.value = (value - valueMul.x) / (valueMul.y - valueMul.x);
        SetValueText();
    }

    public void SetValueText()
    {
        valueText.text = $"{value:0.00}{unit}";
    }

    public void OnChangeValue()
    {
        value = valueMul.x + (valueMul.y - valueMul.x) * slider.value;
        SetValueText();
        //settingManager.SetValue(value,key);
        valueChanged(value, key);
    }

    ///// <summary>スライダーの値ではなく実際の値を入れる</summary>
    //public void SetValue(float value)
    //{
    //    slider.value = (value - valueMul.x) / (valueMul.y - valueMul.x);
    //}
   
    public override float GetValue()
    {
        return value;
    }

    public void OnMouseEnter()
    {
        if (info != "") { MouseOverUI.inst.SetUI(info); }
    }

    public void OnMouseExit()
    {
        if (info != "") { MouseOverUI.inst.ResetUI(); }
    }

}
