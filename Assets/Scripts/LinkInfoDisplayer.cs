using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LinkInfoDisplayer : MonoBehaviour, IPointerExitHandler, IPointerMoveHandler
{
    [TextArea(3,10)]public string message = "これは必ずText Mesh proのアタッチされたオブジェクトにつけること！！";
    bool init;
    Canvas canvas;
    TextMeshProUGUI text;
    MouseOverUI mouseOver;

    // Start is called before the first frame update
    void Start()
    {
        Init();
    }
    void Init()
    {
        if (!init)
        {
            init = true;
            text = GetComponent<TextMeshProUGUI>();
            canvas = text.canvas;
            mouseOver = MouseOverUI.inst;
        }
    }

    string currentLinkKey;
    public void OnPointerExit(PointerEventData eventData)
    {
        if (!init) Init();
        currentLinkKey = "";
        mouseOver.ResetUI();
    }
    public void OnPointerMove(PointerEventData eventData)
    {
        if (!init) Init();
        //この辺ほぼコピペ 意味不明
        var pos = Input.mousePosition;
        var camera = canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : canvas.worldCamera;
        var index = TMP_TextUtilities.FindIntersectingLink(text, pos, camera);

        if (index == -1)
        {
            currentLinkKey = "";
            mouseOver.ResetUI();
            return;
        }
        var linkInfo = text.textInfo.linkInfo[index];
        string info = linkInfo.GetLinkID();
        if (info != currentLinkKey)
        {
            currentLinkKey = info;
            mouseOver.SetUI(Definer.inst.LinkKeyToStr(currentLinkKey));
        }
    }
}
