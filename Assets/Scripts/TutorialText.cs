using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class TutorialText : MonoBehaviour, IPointerExitHandler, IPointerMoveHandler
{
    [SerializeField] GameObject panel;
    [SerializeField] TextMeshProUGUI titleText;
    [SerializeField] TextMeshProUGUI tutorialText;
    MouseOverUI mouseOver;
    Definer definer;


    public void SetText(TutorialData.Tutorial tutorial)
    {
        mouseOver = MouseOverUI.inst;
        definer = Definer.inst;
        panel.SetActive(true);
        titleText.text = tutorial.title;
        tutorialText.text = tutorial.tutorialText;
    }
    
    public void ResetText()
    {
        titleText.text = "";
        tutorialText.text = "";
        panel.SetActive(false);

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
        var canvas = tutorialText.canvas;
        var camera = canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : canvas.worldCamera;
        var index = TMP_TextUtilities.FindIntersectingLink(tutorialText, pos, camera);

        if (index == -1)
        {
            currentLinkKey = "";
            mouseOver.ResetUI();
            return;
        }
        var linkInfo = tutorialText.textInfo.linkInfo[index];
        string info = linkInfo.GetLinkID();
        if (info != currentLinkKey)
        {
            currentLinkKey = info;
            mouseOver.SetUI(definer.LinkKeyToStr(currentLinkKey));
        }
    }
}
