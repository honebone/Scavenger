using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

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
    TextMeshProUGUI logText;

   // Character displayingChara;

    CharactersManager charactersManager;
    TutorialManager tutorialManager;

    int logCount;
    List<string> logs = new List<string>();
    [SerializeField] int maxLogs;
    [SerializeField] int deleteLogs;
    private void Start()
    {
        charactersManager = FindObjectOfType<CharactersManager>();
        tutorialManager = FindObjectOfType<TutorialManager>();
    }

    public　void SetText(string name,string info)
    {
        //if (displayingChara != null)
        //{
        //    displayingChara.GetCharacter_Object().SetSelectedIcon(false);
        //    displayingChara = null;
        //}
        charactersManager.ReseAlltSelectedIcons();
        SwitchToInfo();
        nameText.text = name;
        infoText.text = info;
        infoTextScrollBar.value = 1;
    }
    public void ResetText()
    {
        //if (displayingChara != null)
        //{
        //    displayingChara.GetCharacter_Object().SetSelectedIcon(false);
        //    displayingChara = null;
        //}
        charactersManager.ReseAlltSelectedIcons();
        nameText.text = "";
        infoText.text = "";
        infoTextScrollBar.value = 1;
    }

    public void SetCharaInfo(string name, string info,Character chara)
    {
        //if (displayingChara != null)
        //{
        //    displayingChara.GetCharacter_Object().SetSelectedIcon(false);
        //    displayingChara = null;
        //}
        charactersManager.ReseAlltSelectedIcons();
        SwitchToInfo();
        //displayingChara = chara;
        nameText.text = name;
        infoText.text = info;
        infoTextScrollBar.value = 1;
    }

    public void InfoButton()
    {
        if (Input.GetMouseButtonDown(1))
        {
            SetText("ログ", "ログを確認する");
        }
        if (Input.GetMouseButtonDown(0))
        {
            TogglePanel();
        }
    }

    public void TogglePanel()
    {
        if (infoPanel.activeSelf)
        {
            infoPanel.SetActive(false);
            logPanel.SetActive(true);
        }
        else
        {
            infoPanel.SetActive(true);
            logPanel.SetActive(false);
        }
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
        logs.Add(log);
        logCount++;
        if (logCount >= maxLogs)
        {
            for(int i = 0; i < deleteLogs; i++)
            {
                logs.RemoveAt(0);
            }
            logCount -= deleteLogs;
        }

        string totalLog = "";
        foreach(string l in logs)
        {
            totalLog += "\n"+l;
        }

        logText.text = totalLog;
    }
    public void AddDebugText(string debugLog)
    {
        //SwitchToLog();
        ////nameText.text = "ログ";
        //AddLogText(string.Format("\n##デバッグ：{0}##", debugLog).ColorStr(Definer.colorRef.debug));
        
        //print(debugLog);
    }
    public void AddErrorText(string errorLog)
    {
        SwitchToLog();
        //nameText.text = "ログ";
        AddLogText(string.Format("\n##error!!：{0}##", errorLog).ColorStr(Color.red));
        print(errorLog);
    }
}
