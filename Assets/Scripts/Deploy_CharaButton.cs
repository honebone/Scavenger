using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Deploy_CharaButton : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI charaNameText;
    [SerializeField] Image charaImage;


    InfoText infoText;
    DeployCharacterManager deployCharacterManager;
    MouseOverUI mouseOver;

    Character.CharacterStatus charaStatus;
    public void Init(Character.CharacterStatus status,InfoText it,DeployCharacterManager dc,MouseOverUI mo, ScrollRect s)
    {
        charaStatus = status;
        infoText = it;
        deployCharacterManager = dc;
        mouseOver = mo;
        scroll = s;

        charaNameText.text = charaStatus.charaName;
        charaImage.sprite = charaStatus.spriteForUI;
    }

    public void SetDragChara()
    {
        if (Input.GetMouseButtonDown(1))
        {
            string info = string.Format("\n\"{0}\"\n\n", charaStatus.characterData.introduction).ColorStr(Definer.colorRef.emphasize);
            info += string.Format("使用難易度：{0}\n得意なポジション：{1}\n\n", charaStatus.characterData.difficulty, charaStatus.characterData.preferredPos);
            info += charaStatus.GetInfo();
            info += "\n◇◇特性◇◇\n";
            foreach (GameObject obj in charaStatus.passiveAbilities)
            {
                PassiveAbility pa = obj.GetComponent<PassiveAbility>();
                info += string.Format("<{0}>\n{1}\n", pa.GetPAName(), pa.GetPAInfo());
            }
            infoText.SetText(charaStatus.charaName, info);
            FindObjectOfType<AbilityButtonPanel>().SetAbilityButtons_Deploy(charaStatus.abilitiesStatus);
            deployCharacterManager.StartTutorial_Info();
        }
        if (Input.GetMouseButtonDown(0))
        {
            deployCharacterManager.SetDraggingChara(charaStatus);
        }
    }
    [SerializeField]
    float scrollSpeed = 0.1f;
    ScrollRect scroll;
    float wheel;
    bool p;
    private void Update()
    {
        if (p)
        {
            wheel += Input.mouseScrollDelta.y;
            if (wheel != 0)
            {
                scroll.verticalNormalizedPosition += wheel * scrollSpeed;
                wheel = 0;
            }
        }
    }
    public void OnMouseEnter()
    {
        p = true;
        mouseOver.SetUI("", true);
    }
    public void OnMouseExit()
    {
        p = false;
        mouseOver.ResetUI();
    }
}
