using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StEIcon : MonoBehaviour
{
    [SerializeField]
    Image image;
    [SerializeField]
    Text stackText;
    public void Init(PA_StatusEffect.StatusEffectStatus status)
    {
        image.sprite = status.StEIcon;
        stackText.text = status.stack.ToString();
    }
    public void SetStackText(int stack)
    {
        stackText.text = stack.ToString();
    }
}
