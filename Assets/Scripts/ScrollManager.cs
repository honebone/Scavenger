using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollManager : MonoBehaviour
{
    public ScrollRect scroll;
    [Header("Scroll Sensitivityと一致させるべき")] public float scrollSpeed = 100f;
    [Header("カーソルを合わせて居なくてもスクロールするか")] public bool alwaysScroll;
    public bool horizontal;

    RectTransform content;
    float wheel;
    bool p;

    private void Start()
    {
        if (scroll != null) content = scroll.content;
    }

    /// <summary>
    /// UI要素が動的に生成される場合に
    /// </summary>
    /// <param name="s"></param>
    public void SetScroll(ScrollRect s)
    {
        scroll = s;
        content = scroll.content;
    }
    private void Update()
    {
        if ((p || alwaysScroll) && content != null)
        {
            wheel += Input.mouseScrollDelta.y;
            if (wheel != 0)
            {
                Vector2 pos = content.anchoredPosition;
                if (horizontal) pos.x -= wheel * scrollSpeed;
                else pos.y -= wheel * scrollSpeed;
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
