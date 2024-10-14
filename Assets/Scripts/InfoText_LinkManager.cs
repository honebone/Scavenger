using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;



public class InfoText_LinkManager : MonoBehaviour, IPointerExitHandler, IPointerMoveHandler
{
    [SerializeField]
    TextMeshProUGUI infoText;

    Definer definer;
    MouseOverUI mouseOver;
    // Start is called before the first frame update
    void Start()
    {
        definer = ExpeditionRef.definer;
        mouseOver = ExpeditionRef.mouseover;

    }

    string currentLinkKey;
    public void OnPointerExit(PointerEventData eventData)
    {
        currentLinkKey = "";
        mouseOver.ResetUI();
    }

    public void OnPointerMove(PointerEventData eventData)
    {
        //この辺ほぼコピペ 意味不明
        var pos = Input.mousePosition;
        var canvas = infoText.canvas;
        var camera = canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : canvas.worldCamera;
        var index = TMP_TextUtilities.FindIntersectingLink(infoText, pos, camera);

        if (index == -1)
        {
            currentLinkKey = "";
            mouseOver.ResetUI();
            return;
        }
        var linkInfo = infoText.textInfo.linkInfo[index];
        string info = linkInfo.GetLinkID();
        if (info != currentLinkKey)
        {
            currentLinkKey = info;
            mouseOver.SetUI(definer.LinkKeyToStr(currentLinkKey));
        }
    }
}
