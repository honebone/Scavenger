using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MouseOverUI : MonoBehaviour
{
    [SerializeField] GameObject panel;
    [SerializeField] TextMeshProUGUI text;
    [SerializeField] RectTransform rt;
    [SerializeField] float baseOffset_x;
    [SerializeField] float baseOffset_y;
    [SerializeField] GameObject sidePanel;
    [SerializeField] TextMeshProUGUI sideText;
    [SerializeField] RectTransform sideRT;
    [SerializeField] Vector2 baseSideOffset;

    Vector3 pos;

    Vector3 offset;
    Vector3 sideOffset;
    public static MouseOverUI inst;
    private void Awake()
    {
        inst = this;
    }

    public void SetUI(string s, bool rightClickGuide = false)
    {
        text.text = "";
        if (rightClickGuide && !SettingManager.infoOnMouseover) { text.text += "右クリックで詳細表示\n"; }
        text.text += s;
        if (text.text != "")
        {            
            panel.SetActive(true);
            SetUIPos();
        }
    }
    public void SetUI_Simple(string s)
    {
        SetUI(s, false);
    }

    public void SetSideUI(string s)
    {
        sideText.text = s;
        if(sideText.text != "")
        {
            sidePanel.SetActive(true);
            SetSideUIPos();
        }
    }

    public void ResetUI()
    {
        text.text = "";
        panel.SetActive(false);

        sideText.text = "";
        sidePanel.SetActive(false);
    }

    private void Update()
    {
        if(panel.activeSelf|| sidePanel.activeSelf)
        {
            pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            pos.z = 0;
        }

        SetUIPos();
        SetSideUIPos();
    }

    void SetUIPos()
    {
        if (panel.activeSelf)
        {
            offset = Vector3.zero;

            if (Input.mousePosition.x / Camera.main.pixelWidth < 0.5f) { offset.x = baseOffset_x; }
            else { offset.x = -1 * baseOffset_x; }

            if (Input.mousePosition.y / Camera.main.pixelHeight < 0.25f) { rt.pivot = new Vector2(0.5f, 0); }
            else if (Input.mousePosition.y / Camera.main.pixelHeight > 0.75f) { rt.pivot = new Vector2(0.5f, 1); }
            else { rt.pivot = new Vector2(0.5f, 0.5f); }

            panel.transform.position = pos + offset;
        }
    }
    void SetSideUIPos()
    {
        if (sidePanel.activeSelf)
        {
            sideOffset = Vector3.zero;

            if (Input.mousePosition.x / Camera.main.pixelWidth < 0.5f) { sideOffset.x = baseOffset_x + baseSideOffset.x; }
            else { sideOffset.x = -1 * (baseOffset_x + baseSideOffset.x); }

            if (Input.mousePosition.y / Camera.main.pixelHeight < 0.25f) { sideRT.pivot = new Vector2(0.5f, 0); }
            else if (Input.mousePosition.y / Camera.main.pixelHeight > 0.75f) { sideRT.pivot = new Vector2(0.5f, 1); }
            else { sideRT.pivot = new Vector2(0.5f, 0.5f); }

            sidePanel.transform.position = pos + sideOffset;
        }
    }
}
