using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Settings_Slider : MonoBehaviour
{
    [SerializeField] Slider slider;
    [SerializeField] TextMeshProUGUI valueText;
    [SerializeField] string unit;
    [SerializeField] Vector2 valueMul = new Vector2(0, 1);
    // Start is called before the first frame update
    void Start()
    {
        SetValueText();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetValueText()
    {
        valueText.text = $"{GetValue():0.00}{unit}";
    }
    public float GetValue()
    {
        return valueMul.x+(valueMul.y-valueMul.x)*slider.value;
    }
   
}
