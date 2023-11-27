using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PEIcon : MonoBehaviour
{
    [SerializeField]
    Image image;
    [SerializeField]
    Text stackText;
    public void Init(PositionEffect.PositionEffectStatus status)
    {
        image.sprite = status.PEIcon;
        stackText.text = status.stack.ToString();
    }
    public void SetStackText(int stack)
    {
        stackText.text = stack.ToString();
    }
}
