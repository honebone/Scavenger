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
    public void Init(Character.CharacterStatus status,InfoText it,DeployCharacterManager dc,MouseOverUI mo)
    {
        charaStatus = status;
        infoText = it;
        deployCharacterManager = dc;
        mouseOver = mo;

        charaNameText.text = charaStatus.charaName;
        charaImage.sprite = charaStatus.spriteForUI;
    }

    public void SetDragChara()
    {
        if (Input.GetMouseButtonDown(1))
        {
            string info = charaStatus.GetInfo();
            info += "\nÅûÅûì¡ê´ÅûÅû\n";
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

    public void OnMouseEnter()
    {
        mouseOver.SetUI("", true);
    }
    public void OnMouseExit()
    {
        mouseOver.ResetUI();
    }
}
