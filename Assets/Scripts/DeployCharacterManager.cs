using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

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

    [SerializeField] GraphicRaycaster raycaster;

    [SerializeField] AreaData area_tutorial;//test
    [SerializeField] AreaData area_cave;//test

    [SerializeField] TutorialData tutroial_info;
    [SerializeField] TutorialData tutroial_deploy;
    [SerializeField] TutorialData tutroial_embark;

    [SerializeField] CharacterData infantry;
    [SerializeField] CharacterData hunter;

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
    GameObject draggingImage;

    bool tutorial;
    bool deploy;
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
        expeditionManager= FindObjectOfType<ExpeditionManager>();
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
        tutorial = gameManager.CheckIfTutorialArea();
        Debug.Log(tutorial);
        //tutorial = true;
        StartDeploy();

        if (tutorial)
        {
            Character.CharacterStatus inf = new Character.CharacterStatus();
            Character.CharacterStatus hun = new Character.CharacterStatus();
            inf.Init(infantry, 0);
            hun.Init(hunter, 0);
            positionButtons[4].SetChara(hun);
            positionButtons[6].SetChara(inf);

            yield return new WaitForSeconds(1f);
            tutorialManager.StartTutorial(tutroial_deploy);
            //yield return new WaitForSeconds(0.5f);
            //tutorialManager.StartTutorial(tutroial_embark);
            CheckParty();
        }
    }

    public void StartDeploy()
    {
        panel.SetActive(true);
        deploy = true;
        foreach(CharacterData data in definer.GetPlayerDataBase())
        {
            Character.CharacterStatus status = new Character.CharacterStatus();
            status.Init(data, 0);
            var c = Instantiate(deployCharaButton, deployCharaP);
            c.GetComponent<Deploy_CharaButton>().Init(status, infoText, this, mouseOver, scroll);
        }
        CheckParty();
        if (tutorial) { canEmbark = false; }
    }

    public void SetDraggingChara(Character.CharacterStatus character)
    {
        if (!tutorial)
        {
            draggingChara = character;
            draggingImage = Instantiate(dragImage, dragImageP);
            draggingImage.GetComponent<Image>().sprite = draggingChara.spriteForUI;
        }
        else { guideMessage.SetWaringText("チュートリアル中は編成の変更不可"); }
    }

    // Update is called once per frame
    void Update()
    {
        if (deploy&&!tutorial)
        {
            if (draggingImage != null)
            {
                draggingImage.transform.position = Input.mousePosition;

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
                            break;
                        }
                    }

                    draggingChara = new Character.CharacterStatus();
                    Destroy(draggingImage);

                    CheckParty();
                }
            }
        }
    }

    void CheckDouble(Character.CharacterStatus check)
    {
        foreach(Deploy_PositionButton button in positionButtons)
        {
            if (button.GetCharacterStatus().characterData == check.characterData) { button.ResetChara(); }
        }
    }

    void CheckParty()
    {
        int count = 0;
        canEmbark = false;
        foreach (Deploy_PositionButton button in positionButtons)
        {
            if (button.GetCharacterStatus().characterData != null) { count++; }
        }
        if (count > maxParty) { embarkText.text = string.Format("編成できるキャラクターは{0}体まで", maxParty).ColorStr(Color.red); }
        else if (count == 0) { embarkText.text = "キャラを編成".ColorStr(Color.red); }
        else
        {
            canEmbark = true;
            embarkText.text = string.Format("出撃({0}/{1})", count, maxParty);
        }
    }

    public void Embark()
    {
        if (canEmbark)
        {
            if (tutorialManager.CheckUnlocked(tutroial_info))
            {
                panel.SetActive(false);
                deploy = false;

                for (int i = 0; i < 9; i++)
                {
                    if (positionButtons[i].GetCharacterStatus().characterData)
                    {
                        charactersManager.SpawnPlayer(positionButtons[i].GetCharacterStatus().characterData, i);
                    }
                }

                if (tutorial) { expeditionManager.StartExpedition(area_tutorial); }
                else { expeditionManager.StartExpedition(area_cave); }
            }
            else
            {
                guideMessage.SetWaringText("まずはキャラクターの詳細をチェックしよう");
            }
        }
    }

    public void StartTutorial_Info()
    {
        if (tutorialManager.CheckUnlocked(tutroial_deploy)) { tutorialManager.StartTutorial(tutroial_info); }
    }
}
