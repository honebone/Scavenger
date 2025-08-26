using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Deploy_PositionButton : MonoBehaviour
{
    [SerializeField] Image charaImage;
    [SerializeField] Sprite empty;
    [SerializeField] Animator anim;

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
            infoText.SetText(charaStatus.charaName, charaStatus.characterData.GetInfo(false), charaStatus.characterData.GetInfo(true));
            FindObjectOfType<AbilityButtonPanel>().SetAbilityButtons_Deploy(charaStatus.abilitiesStatus);

            if (Input.GetMouseButtonDown(0))
            {
                deployCharacterManager.SetDraggingChara(charaStatus);
            }
        }
    }

    public void SetAnim(bool set) { anim.SetBool("Enabled", set); }

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
