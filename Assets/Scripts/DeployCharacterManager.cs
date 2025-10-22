using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using System.Linq;

public class DeployCharacterManager : MonoBehaviour
{
    [SerializeField] int maxParty = 3;
    [SerializeField] GameObject panel;
    [SerializeField] ScrollRect scroll;
    [SerializeField] GameObject deployCharaButton;
    [SerializeField] Transform deployCharaP;

    [SerializeField] GameObject dragImage;
    [SerializeField] Transform dragImageP;

    [SerializeField] List<Deploy_PositionButton> positionButtons;
    [SerializeField] TextMeshProUGUI embarkText;

    public CanvasGroup warningPanel;
    public TextMeshProUGUI warningText;

    [SerializeField] GraphicRaycaster raycaster;

    [SerializeField] AreaData area_tutorial;//test
    [SerializeField] AreaData area_cave;//test

    [SerializeField] TutorialData tutroial_deploy;
    [SerializeField] TutorialData tutroial_embark;

    [SerializeField] CharacterData infantry;
    [SerializeField] CharacterData hunter;

    [SerializeField] Animator anim;

    [SerializeField] AudioClip pickChara;
    [SerializeField] AudioClip setChara;

    Definer definer;
    InfoText infoText;
    CharactersManager charactersManager;
    FadeOutUI fadeOutUI;
    SoundManager soundManager;
    GameManager gameManager;
    GuideMessage guideMessage;
    TutorialManager tutorialManager;
    ExpeditionManager expeditionManager;
    MouseOverUI mouseOver;

    Character.CharacterStatus draggingChara;
    Vector3 dragImagePos;
    GameObject draggingImage;

    bool tutorial;
    bool deploy;
    bool hasWarning;
    string warningInfo;
    bool canEmbark;
    void Start()
    {
        definer = FindObjectOfType<Definer>();
        infoText = FindObjectOfType<InfoText>();
        charactersManager = FindObjectOfType<CharactersManager>();
        fadeOutUI = FindObjectOfType<FadeOutUI>();
        soundManager = FindObjectOfType<SoundManager>();
        gameManager = FindObjectOfType<GameManager>();
        guideMessage = FindObjectOfType<GuideMessage>();
        tutorialManager = FindObjectOfType<TutorialManager>();
        expeditionManager = FindObjectOfType<ExpeditionManager>();
        mouseOver = FindObjectOfType<MouseOverUI>();

        if (FindObjectOfType<DebugFunction>().CheckSkipDeploy()) { fadeOutUI.FadeIn(); }
        else
        {
            StartCoroutine(Delay());
        }
    }
    IEnumerator Delay()
    {
        yield return new WaitForSeconds(1f);
        tutorial = gameManager.DoTutorial();
        Debug.Log(tutorial);
        //tutorial = true;
        StartDeploy();

        if (tutorial)
        {
            //Character.CharacterStatus inf = new Character.CharacterStatus();
            //Character.CharacterStatus hun = new Character.CharacterStatus();
            //inf.Init(infantry, 0);
            //hun.Init(hunter, 0);
            //positionButtons[4].SetChara(hun);
            //positionButtons[6].SetChara(inf);

            yield return new WaitForSeconds(1f);
            tutorialManager.SetTutorial(tutroial_deploy);
            //yield return new WaitForSeconds(0.5f);
            //tutorialManager.StartTutorial(tutroial_embark);
            CheckParty();
        }
    }

    List<Deploy_CharaButton> charaButtons = new List<Deploy_CharaButton>();
    public float animSpeed;
    public void StartDeploy()
    {
        panel.SetActive(true);
        deploy = true;
        foreach (CharacterData data in definer.GetPlayerDataBase())
        {
            Character.CharacterStatus status = new Character.CharacterStatus();
            status.Init(data);
            var c = Instantiate(deployCharaButton, deployCharaP);
            Deploy_CharaButton button = c.GetComponent<Deploy_CharaButton>();
            charaButtons.Add(button);
            button.Init(status, infoText, this, mouseOver, scroll);
        }
        StartCoroutine(CharaButtonAnim());
        CheckParty();
        //if (tutorial) { canEmbark = false; }
    }

    IEnumerator CharaButtonAnim()
    {
        foreach (Deploy_CharaButton button in charaButtons)
        {
            button.Anim();
            yield return new WaitForSeconds(animSpeed);
        }
    }

    public void SetDraggingChara(Character.CharacterStatus character)
    {
        soundManager.PlaySE(pickChara);
        //if (!tutorial)
        //{
        //    draggingChara = character;
        //    draggingImage = Instantiate(dragImage, dragImageP);
        //    draggingImage.GetComponent<Image>().sprite = draggingChara.spriteForUI;
        //}
        //else { guideMessage.SetWaringText("チュートリアル中は編成の変更不可"); }

        List<int> i = new List<int>();
        if (character.characterData.preferBack) i.AddRange(new List<int>() { 0, 1, 2 });
        if (character.characterData.preferMid) i.AddRange(new List<int>() { 3, 4, 5 });
        if (character.characterData.preferFront) i.AddRange(new List<int>() { 6, 7, 8 });

        foreach (int index in i) { positionButtons[index].SetAnim(true); }

        draggingChara = character;
        draggingImage = Instantiate(dragImage, dragImageP);
        draggingImage.GetComponent<Image>().sprite = draggingChara.spriteForUI;
    }

    void ResetGrid()
    {
        foreach (Deploy_PositionButton grid in positionButtons) { grid.SetAnim(false); }
    }

    // Update is called once per frame
    void Update()
    {
        if (deploy)//&& !tutorial
        {
            if (draggingImage != null)
            {
                dragImagePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                dragImagePos.z = 0;
                draggingImage.transform.position = dragImagePos;

                if (Input.GetMouseButtonUp(0))
                {
                    EventSystem ev = EventSystem.current;
                    PointerEventData ped = new PointerEventData(ev);
                    ped.position = Input.mousePosition;
                    List<RaycastResult> rr = new List<RaycastResult>();
                    raycaster.Raycast(ped, rr);

                    CheckDouble(draggingChara);
                    foreach (RaycastResult result in rr)
                    {
                        if (result.gameObject.GetComponent<Deploy_PositionButton>())
                        {
                            result.gameObject.GetComponent<Deploy_PositionButton>().SetChara(draggingChara);
                            soundManager.PlaySE(setChara);
                            tutorialManager.SetTutorial("deployTips");
                            break;
                        }
                    }

                    draggingChara = new Character.CharacterStatus();
                    Destroy(draggingImage);
                    ResetGrid();

                    CheckParty();
                }
            }
        }
    }

    void CheckDouble(Character.CharacterStatus check)
    {
        foreach (Deploy_PositionButton button in positionButtons)
        {
            if (button.GetCharacterStatus().characterData == check.characterData) { button.ResetChara(); }
        }
    }

    void CheckParty()
    {
        int count = 0;
        //canEmbark = false;
        foreach (Deploy_PositionButton button in positionButtons)
        {
            if (button.GetCharacterStatus().characterData != null) { count++; }
        }

        if (count == maxParty) tutorialManager.SetTutorial("Embark");

        if (count > maxParty || count == 0)//誰もいないor最大編成数を上回る
        {
            if (canEmbark) { anim.SetTrigger("Flip"); }
            canEmbark = false;
            embarkText.text = $"出撃({count}/{maxParty})".ColorStr(Color.red);
        }
        else
        {
            bool emptyFront = true;
            bool emptyMid = true;
            bool noTank = true;
            bool noDPS = true;
            bool noHealer = true;
            string preferColWarningText = "";
            for (int i = 0; i < 9; i++)
            {
                int col = i.GetColumn();
                CharacterData data = positionButtons[i].GetCharacterStatus().characterData;
                if (data)
                {
                    if (col == 0) emptyFront = false;
                    if (col == 1) emptyMid = false;

                    List<string> role = new List<string>(data.mainRole);
                    if (role.Contains("タンク")) noTank = false;
                    if (role.Contains("攻撃")) noDPS = false;
                    if (role.Contains("回復")) noHealer = false;

                    if ((col == 0 && !data.preferFront) || (col == 1 && !data.preferMid) || (col == 2 && !data.preferBack))
                    {
                        string preferCol = "";
                        bool f = false;
                        if (data.preferFront)
                        {
                            preferCol += "前";
                            f = true;
                        }
                        if (data.preferMid)
                        {
                            preferCol += f ? ",中" : "中";
                            f = true;
                        }
                        if (data.preferBack)
                        {
                            preferCol += f ? ",後" : "後";
                        }

                        string currentCol = col == 0 ? "前" : col == 1 ? "中" : "後";
                        preferColWarningText += $"・{data.charaName}が{currentCol}列にいる(得意な列：{preferCol}列)\n";
                    }
                }
            }

            warningInfo = "";
            hasWarning = false;

            if (emptyFront) { warningInfo += "・前列に誰もいない\n"; hasWarning = true; }
            if (emptyMid) { warningInfo += "・中列に誰もいない\n"; hasWarning = true; }
            if (count < maxParty) { warningInfo += $"・編成人数が{maxParty}人未満\n"; hasWarning = true; }
            if (noTank) { warningInfo += "・タンク役がいない\n"; hasWarning = true; }
            if (noDPS) { warningInfo += "・攻撃役がいない\n"; hasWarning = true; }
            if (noHealer) { warningInfo += "・回復役がいない\n"; hasWarning = true; }
            if (preferColWarningText != "") { warningInfo += preferColWarningText; hasWarning = true; }

            if (!canEmbark) { anim.SetTrigger("Flip"); }
            canEmbark = true;
            embarkText.text = hasWarning ? $"! 出撃({count}/{maxParty})".ColorStr(Color.yellow) : string.Format("出撃({0}/{1})", count, maxParty);
        }
    }
    public void TryEmbark()
    {
        if(canEmbark)
        {
            if (hasWarning)
            {
                warningPanel.alpha = 1;
                warningPanel.interactable = true;
                warningPanel.blocksRaycasts = true;

                warningText.text = $"この編成には問題があります！\n{warningInfo}";
            }
            else { Embark(); }
        }
    }

    public void CancelEmbark()
    {
        warningPanel.alpha = 0;
        warningPanel.interactable = false;
        warningPanel.blocksRaycasts = false;

        warningText.text = "";
    }

    public void Embark()
    {
        panel.SetActive(false);
        deploy = false;

        warningPanel.alpha = 0;
        warningPanel.interactable = false;
        warningPanel.blocksRaycasts = false;

        warningText.text = "";

        for (int i = 0; i < 9; i++)
        {
            if (positionButtons[i].GetCharacterStatus().characterData)
            {
                charactersManager.SpawnPlayer(positionButtons[i].GetCharacterStatus().characterData, i, 1);
                expeditionManager.deployedChara.Add(positionButtons[i].GetCharacterStatus().characterData);
            }
        }

        //if (tutorial) { expeditionManager.StartExpedition(area_tutorial); }
        //else { expeditionManager.StartExpedition(area_cave); }
        expeditionManager.StartExpedition(area_cave);//test
    }
}
