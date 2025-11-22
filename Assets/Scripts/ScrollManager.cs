using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollManager : MonoBehaviour
{
    public ScrollRect scroll;
    public float scrollSpeed = 100f;
    public bool alwaysScroll;

    RectTransform content;
    float wheel;
    bool p;

    private void Start()
    {
        if (scroll != null) content = scroll.content;
    }

    /// <summary>
    /// UIóvëfÇ™ìÆìIÇ…ê∂ê¨Ç≥ÇÍÇÈèÍçáÇ…
    /// </summary>
    /// <param name="s"></param>
    public void SetScroll(ScrollRect s)
    {
        scroll = s;
        content = scroll.content;
    }
    private void Update()
    {
        if (p || alwaysScroll)
        {
            wheel += Input.mouseScrollDelta.y;
            if (wheel != 0)
            {
                Vector2 pos = content.anchoredPosition;
                pos.y -= wheel * scrollSpeed;
                content.anchoredPosition = pos;
                wheel = 0;
            }
        }
    }

    public void OnMouseEnter()
    {
        p = true;
    }
    public void OnMouseExit()
    {
        p = false;
    }
}
