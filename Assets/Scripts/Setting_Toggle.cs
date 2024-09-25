using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Setting_Toggle : MonoBehaviour
{
    [SerializeField] Toggle toggle;


    

    public void SetValue(bool on)
    {
        toggle.isOn = on;
    }
    
    public bool GetValue()
    {
        return toggle.isOn;
    }
}
