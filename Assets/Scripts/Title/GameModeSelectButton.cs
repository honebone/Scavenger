using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameModeSelectButton : MonoBehaviour
{
    [SerializeField] Image frame;
    [SerializeField] TextMeshProUGUI text;

    public void Deactivate()
    {
        frame.color = Color.white;
        text.color = Color.white;
    }
    public void Activate()
    {
        frame.color = Definer.colorRef.emphasize;
        text.color = Definer.colorRef.emphasize;
    }
}
