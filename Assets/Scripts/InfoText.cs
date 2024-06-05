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

    [SerializeField] TutorialData tutorial_info1;
    [SerializeField] TutorialData tutorial_info2;

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
        if (tutorialManager.CheckUnlocked(tutorial_info1)) { tutorialManager.StartTutorial(tutorial_info2); }
        charactersManager.ReseAlltSelectedIcons();
        SwitchToInfo();
        //displayingChara = chara;
        nameText.text = name;
        infoText.text = info;
        infoTextScrollBar.value = 1;
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
        if (logCount >= maxLogs)
        {
            AddDebugText("最大ログ数に到達");
        }
        else
        {
            logCount++;
            logText.text += "\n" + logCount.ToString().ColorStr(Definer.colorRef.currentState) + log;
        }
    }
    public void AddDebugText(string debugLog)
    {
        SwitchToLog();
        //nameText.text = "ログ";
        logText.text += string.Format("\n##デバッグ：{0}##", debugLog).ColorStr(Definer.colorRef.debug);
        
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
