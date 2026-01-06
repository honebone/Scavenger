using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using System.Runtime.InteropServices.WindowsRuntime;

public class InfoText : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI nameText;

    [SerializeField]
    GameObject infoPanel;
    [SerializeField]
    TextMeshProUGUI infoText;
    [SerializeField]
    Scrollbar infoTextScrollBar;

    [SerializeField]
    GameObject logPanel;
    [SerializeField]
    TextMeshProUGUI logText;

    // Character displayingChara;

    CharactersManager charactersManager;
    TutorialManager tutorialManager;
    DebugFunction debugFunction;
    MouseOverUI mouseOver;
    public static InfoText inst;

    int logCount;
    List<string> logs = new List<string>();
    [SerializeField] int maxLogs;
    [SerializeField] int deleteLogs;

    public float toggleDur;
    public Image toggleKnob;
    public Image toggleImage;
    public TextMeshProUGUI detailToggleText;
    public Color enabledColor;

    bool showSimple = true;
    string detailInfo;
    string simpleInfo;

    Sequence sequence;

    private void Awake()
    {
        inst = this;
    }
    private void Start()
    {
        charactersManager = FindObjectOfType<CharactersManager>();
        tutorialManager = FindObjectOfType<TutorialManager>();
        debugFunction = FindObjectOfType<DebugFunction>();
        mouseOver = ExpeditionRef.mouseover;
    }

    public void SetText_Old(string name, string info)
    {
        //if (displayingChara != null)
        //{
        //    displayingChara.GetCharacter_Object().SetSelectedIcon(false);
        //    displayingChara = null;
        //}
        charactersManager.ReseAlltSelectedIcons();
        SwitchToInfo();

        detailInfo = info;
        simpleInfo = info;

        nameText.text = name;
        infoText.text = info;
        infoTextScrollBar.value = 1;
    }

    public void SetText(string name, string detailed, string simple = null)
    {
        nameText.text = name;
        detailInfo = detailed;
        simpleInfo = simple;
        infoText.text = showSimple ? simpleInfo : detailInfo;

        SwitchToInfo();

        if (simple != null && detailed != simple && tutorialManager.CompleteEssentials()) tutorialManager.SetTutorial("tips_simpleInfo", true);
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

    public void SetCharaInfo(string name, string info, string simple = null)
    {
        //if (displayingChara != null)
        //{
        //    displayingChara.GetCharacter_Object().SetSelectedIcon(false);
        //    displayingChara = null;
        //}
        charactersManager.ReseAlltSelectedIcons();
        SwitchToInfo();
        //displayingChara = chara;

        detailInfo = info;
        simpleInfo = simple == null ? info : simple;

        nameText.text = name;
        infoText.text = showSimple ? simpleInfo : detailInfo;
        infoTextScrollBar.value = 1;

        if (simple != null && info != simple && tutorialManager.CompleteEssentials()) tutorialManager.SetTutorial("tips_simpleInfo", true);
    }

    public void ToggleInfoType()
    {
        SoundManager.instance.PlaySE_Select();
        if (sequence != null) sequence.Kill();
        sequence = DOTween.Sequence();
        if (showSimple)
        {
            showSimple = false;
            sequence.Append(toggleKnob.rectTransform.DOLocalMoveX(-15, toggleDur));
            sequence.Join(detailToggleText.DOColor(Definer.colorRef.failed_unavailable, toggleDur));
            sequence.Join(toggleImage.DOColor(Definer.colorRef.failed_unavailable, toggleDur));
        }
        else
        {
            showSimple = true;
            sequence.Append(toggleKnob.rectTransform.DOLocalMoveX(15, toggleDur));
            sequence.Join(detailToggleText.DOColor(enabledColor, toggleDur));
            sequence.Join(toggleImage.DOColor(enabledColor, toggleDur));

        }
        infoText.text = showSimple ? simpleInfo : detailInfo;
        sequence.Play().SetUpdate(true);
    }

    public void InfoButton()
    {
        if (Input.GetMouseButtonDown(1))
        {
            SetText_Old("ログ", "ログを確認する");
        }
        if (Input.GetMouseButtonDown(0))
        {
            TogglePanel();
        }
    }

    public void TogglePanel()
    {
        SoundManager.instance.PlaySE_Select();
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

    private StringBuilder stringBuilder = new StringBuilder();

    // 更新が必要かどうかのフラグ
    private bool isDirty = false;

    // 更新頻度を制限するためのタイマー
    private float updateTimer = 0f;
    private const float UPDATE_INTERVAL = 0.1f;

    //public void AddLogText(string log)
    //{
    //    logs.Add(log);
    //    logCount++;
    //    if (logCount >= maxLogs)
    //    {
    //        for (int i = 0; i < deleteLogs; i++)
    //        {
    //            logs.RemoveAt(0);
    //        }
    //        logCount -= deleteLogs;
    //    }

    //    string totalLog = "";
    //    foreach (string l in logs)
    //    {
    //        totalLog += "\n" + l;
    //    }

    //    logText.text = totalLog;
    //}

    public void AddLogText(string log)
    {
        // ログを追加
        logs.Add(log);
        logCount++;

        // 最大ログ数を超えたら古いログを削除
        if (logCount >= maxLogs)
        {
            logs.RemoveRange(0, deleteLogs);
            logCount -= deleteLogs;
        }

        // 更新フラグを立てる
        isDirty = true;
    }

    private void Update()
    {
        // 更新が必要で、かつ更新間隔を過ぎている場合のみ更新
        if (isDirty)
        {
            updateTimer += Time.deltaTime;
            if (updateTimer >= UPDATE_INTERVAL)
            {
                UpdateLogText();
                updateTimer = 0f;
                isDirty = false;
            }
        }
    }

    private void UpdateLogText()
    {
        // StringBuilderをクリア
        stringBuilder.Clear();

        // ログを結合
        foreach (string log in logs)
        {
            stringBuilder.Append("\n").Append(log);
        }

        // テキストを更新
        logText.text = stringBuilder.ToString();
    }

    public void AddDebugText(string debugLog)
    {
        if (debugFunction.CheckDebugMode())
        {
            SwitchToLog();
            //nameText.text = "ログ";
            AddLogText(string.Format("\n##デバッグ：{0}##", debugLog).ColorStr(Definer.colorRef.debug));

            print(debugLog);
        }

    }
    public void AddErrorText(string errorLog)
    {
        SwitchToLog();
        //nameText.text = "ログ";
        AddLogText(string.Format("\n##error!!：{0}##", errorLog).ColorStr(Color.red));
        print(errorLog);
    }
    public void AddWarningText(string errorLog)
    {
        SwitchToLog();
        //nameText.text = "ログ";
        string message = "深刻なエラーではありませんが、想定されていない挙動です\n";
        message += "この警告文をコメント欄で報告していただけると非常にありがたいです";
        AddLogText(string.Format("\n##warning!：{0}\n{1}##", errorLog, message).ColorStr(Color.yellow));
        print(errorLog);
    }

    public bool IsSimple() { return showSimple; }
}
