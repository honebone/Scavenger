using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Deploy_PositionButton : MonoBehaviour
{
    [SerializeField] Image charaImage;
    [SerializeField] Sprite empty;

    DeployCharacterManager deployCharacterManager;
    InfoText infoText;
    MouseOverUI mouseOver;
        
    Character.CharacterStatus charaStatus;
    // Start is called before the first frame update
    void Start()
    {
        deployCharacterManager = FindObjectOfType<DeployCharacterManager>();
        infoText=FindObjectOfType<InfoText>();
        mouseOver = FindObjectOfType<MouseOverUI>();
    }

    public void SetChara(Character.CharacterStatus status)
    {
        charaStatus = status;
        charaImage.sprite = charaStatus.spriteForUI;
    } 
    public void ResetChara()
    {
        charaStatus = new Character.CharacterStatus();
        charaImage.sprite = empty;
    }

    public void SetDragChara()
    {

        if (charaStatus.characterData != null)
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
    }

    public void OnMouseEnter()
    {
        if (charaStatus.characterData != null)
        {

            mouseOver.SetUI("", true);
        }
    }
    public void OnMouseExit()
    {
        if (charaStatus.characterData != null)
        {
            mouseOver.ResetUI();
        }
    }

    public Character.CharacterStatus GetCharacterStatus() { return charaStatus; }
}
