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
        
    Character.CharacterStatus charaStatus;
    // Start is called before the first frame update
    void Start()
    {
        deployCharacterManager = FindObjectOfType<DeployCharacterManager>();
        infoText=FindObjectOfType<InfoText>();
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
                infoText.SetText(charaStatus.charaName, charaStatus.GetInfo());
            }
            if (Input.GetMouseButtonDown(0))
            {
                deployCharacterManager.SetDraggingChara(charaStatus);
            }
        }
    }

    public Character.CharacterStatus GetCharacterStatus() { return charaStatus; }
}
