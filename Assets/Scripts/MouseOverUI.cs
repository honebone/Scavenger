using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MouseOverUI : MonoBehaviour
{
    [SerializeField] GameObject panel;
    [SerializeField] TextMeshProUGUI text;
    [SerializeField] float baseOffset = 0.17f;

    public void SetUI(string s,bool rightClickGuide=false)
    {
        text.text = "";
        if (rightClickGuide&&!SettingManager.infoOnMouseover) { text.text += "右クリックで詳細表示\n"; }
        text.text += s;
        if (text.text != "")
        {
            Vector3 offset = new Vector3(0, 0, -10);
            if (Input.mousePosition.x / Camera.main.pixelWidth < 0.5f) { offset.x = baseOffset * Camera.main.pixelWidth; }
            else { offset.x = -1 * baseOffset * Camera.main.pixelWidth; }
            panel.transform.position = Input.mousePosition + offset;
            //print(Input.mousePosition);
            //print(Camera.main.ScreenToWorldPoint(offset));
            panel.SetActive(true);
        }
    }
    public void SetUI_Simple(string s)
    {
        SetUI(s, false);
    }

    public void ResetUI()
    {
        text.text = "";
        panel.SetActive(false);
    }

    private void Update()
    {
        if (panel.activeSelf)
        {
            Vector3 offset = new Vector3(0, 0, -10);
            if (Input.mousePosition.x / Camera.main.pixelWidth < 0.5f) { offset.x = baseOffset * Camera.main.pixelWidth; }
            else { offset.x = -1 * baseOffset * Camera.main.pixelWidth; }
            panel.transform.position = Input.mousePosition + offset;
        }
    }
}
