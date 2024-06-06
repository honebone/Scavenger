using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class DeployCharacterManager : MonoBehaviour
{
    [SerializeField] GameObject deployCharaButton;
    [SerializeField] Transform deployCharaP;

    [SerializeField] GameObject dragImage;
    [SerializeField] Transform dragImageP;

    [SerializeField] List<Deploy_PositionButton> positionButtons;

    [SerializeField] GraphicRaycaster raycaster;

    Definer definer;
    InfoText infoText;
    CharactersManager charactersManager;
    FadeOutUI fadeOutUI;
    SoundManager soundManager;
    GameManager gameManager;
    GuideMessage guideMessage;
    TutorialManager tutorialManager;
    ExpeditionManager expeditionManager;

    Character.CharacterStatus draggingChara;
    GameObject draggingImage;

    bool deploy;
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
    }

    public void StartDeploy()
    {
        deploy = true;
        foreach(CharacterData data in definer.GetPlayerDataBase())
        {
            Character.CharacterStatus status = new Character.CharacterStatus();
            status.Init(data, 0);
            var c = Instantiate(deployCharaButton, deployCharaP);
            c.GetComponent<Deploy_CharaButton>().Init(status, infoText, this);
        }
    }

    public void SetDraggingChara(Character.CharacterStatus character)
    {
        draggingChara = character;
        draggingImage = Instantiate(dragImage, dragImageP);
        draggingImage.GetComponent<Image>().sprite = draggingChara.spriteForUI;
    }

    // Update is called once per frame
    void Update()
    {
        if (deploy)
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
}
