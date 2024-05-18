using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuideMessage : MonoBehaviour
{
    [SerializeField] GameObject text;
    [SerializeField] Color color_guide;
    [SerializeField] Color color_warning;

   public void SetWaringText(string s)
    {
        var t = Instantiate(text, transform);
        t.GetComponent<GuideMessageText>().Init(s.ColorStr(color_warning));
    }
}
