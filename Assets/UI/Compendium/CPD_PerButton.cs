using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CPD_PerButton : MonoBehaviour
{
    [SerializeField] Image itemImage;
    [SerializeField] Image frame;

    public List<Sprite> frames;
    public Color lockedColor;

    InfoText infoText;
    MouseOverUI mouseOver;

    PA_Personality per;
    bool unlocked;
    public CPD_PerButton Init(PA_Personality p, ScrollRect scrollRect, bool l)
    {
        per = p;
        infoText = InfoText.inst;
        mouseOver = MouseOverUI.inst;
        scroll = scrollRect;
        unlocked = l;

        itemImage.sprite = Definer.inst.cp.perIcons[(int)per.GetPerType()];
        if (!unlocked) itemImage.color = Color.black;
        frame.color = unlocked ? Definer.colorRef.personalityColors[(int)per.GetPerType()] : lockedColor;

        int index = Mathf.Clamp((int)per.GetPerType(), 0, frames.Count - 1);
        frame.sprite = frames[index];
        return this;
    }

    public void Unlock()
    {
        unlocked = true;
        itemImage.color = Color.white;
        frame.color = Definer.colorRef.personalityColors[(int)per.GetPerType()];
    }

    [SerializeField]
    float scrollSpeed = 0.1f;
    ScrollRect scroll;
    float wheel;
    bool p;

    private void Update()
    {
        if (p)
        {
            wheel += Input.mouseScrollDelta.y;
            if (wheel != 0)
            {
                scroll.verticalNormalizedPosition += wheel * scrollSpeed;
                wheel = 0;
            }
        }
    }

    public void OnMouseDown()
    {
        if (unlocked) infoText.SetText(per.GetPAName(), per.GetPAInfo(false), per.GetPAInfo(true));
        else infoText.SetText_Old("???".ColorStr(Color.gray), "プレイ中に獲得するとアンロック".ColorStr(Color.gray));
    }

    public void OnMouseEnter()
    {
        SoundManager.instance.PlaySE_MO();

        p = true;
        if (unlocked) mouseOver.SetUI($"{per.GetPAName()}\n{per.GetPAInfo(true)}");
        else mouseOver.SetUI("???".ColorStr(Color.gray));
    }
    public void OnMouseExit()
    {
        p = false;
        mouseOver.ResetUI();
    }
}
