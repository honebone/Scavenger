using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfoText : MonoBehaviour
{
    [SerializeField]
    Text nameText;

    [SerializeField]
    GameObject infoPanel;
    [SerializeField]
    Text infoText;
    [SerializeField]
    Scrollbar infoTextScrollBar;

    [SerializeField]
    GameObject logPanel;
    [SerializeField]
    Text logText;

    Character displayingChara;
    Utility util;
    private void Start()
    {
        util=FindObjectOfType<Utility>();
    }

    public　void SetText(string name,string info)
    {
        if (displayingChara != null)
        {
            displayingChara.GetCharacter_Object().SetSelectedIcon(false);
            displayingChara = null;
        }
        SwitchToInfo();
        nameText.text = name;
        infoText.text = info;
        infoTextScrollBar.value = 1;
    }
    public void ResetText()
    {
        if (displayingChara != null)
        {
            displayingChara.GetCharacter_Object().SetSelectedIcon(false);
            displayingChara = null;
        }
        nameText.text = "";
        infoText.text = "";
        infoTextScrollBar.value = 1;
    }

    public void SetCharaInfo(string name, string info,Character chara)
    {
        if (displayingChara != null)
        {
            displayingChara.GetCharacter_Object().SetSelectedIcon(false);
            displayingChara = null;
        }
        SwitchToInfo();
        displayingChara = chara;
        nameText.text = name;
        infoText.text = info;
        infoTextScrollBar.value = 1;
    }

    public void SwitchToInfo()
    {
        logPanel.SetActive(false);
        infoPanel.SetActive(true);
    }
    public void SwitchToLog()
    {
        logPanel.SetActive(true);
        infoPanel.SetActive(false);
        //nameText.text = "ログ";
    }

    public void AddLogText(string log)
    {
        SwitchToLog();
        //nameText.text = "ログ";
        logText.text += "\n" + log;
    }
    public void AddDebugText(string debugLog)
    {
        SwitchToLog();
        //nameText.text = "ログ";
        logText.text += util.GetColoredText(Definer.colorRef.debug, string.Format("\n##デバッグ：{0}##", debugLog));
        print(debugLog);
    }
    public void AddErrorText(string errorLog)
    {
        SwitchToLog();
        //nameText.text = "ログ";
        logText.text += string.Format("\n##error!!：{0}##", errorLog).ColorStr(Color.red);
        print(errorLog);
    }
}
