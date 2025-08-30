using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuideMessage : MonoBehaviour
{
    public static GuideMessage inst;
    [SerializeField] GameObject text;
    [SerializeField] Color color_guide;
    [SerializeField] Color color_warning;

    private void Awake()
    {
        if(inst == null)inst = this;
    }

    public void SetWaringText(string s)
    {
        var t = Instantiate(text, transform);
        t.GetComponent<GuideMessageText>().Init(s.ColorStr(color_warning));
    }
    public void SetGuideText(string s)
    {
        var t = Instantiate(text, transform);
        t.GetComponent<GuideMessageText>().Init(s.ColorStr(color_guide));
    }
}
