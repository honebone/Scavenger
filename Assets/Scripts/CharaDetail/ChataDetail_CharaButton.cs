using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChataDetail_CharaButton : MonoBehaviour
{
    [SerializeField] InfoText infoText;
    [SerializeField] CharaDetailUI detailUI;
    [SerializeField] MouseOverUI mouseOver;
    [SerializeField] Image charaImage;
    [SerializeField] Image frame;

    [SerializeField] Transform equipmentsP;
    [SerializeField] GameObject eqButton;

    Character character;

    public void SetChara(Character chara)
    {
        character = chara;
        charaImage.sprite = character.GetCharacterStatus().spriteForUI;
        SetButtons();
    }

    public void SetButtons()
    {
        if (equipmentsP.childCount != 0)
        {
            for (int i = 0; i < equipmentsP.childCount; i++)
            {
                Destroy(equipmentsP.GetChild(i).gameObject);
            }
        }

        Character.CharacterStatus status = character.GetCharacterStatus();

        foreach (Definer.Item item in status.equipments)
        {
            var e = Instantiate(eqButton, equipmentsP);
            e.GetComponent<CharaDetail_CharaEqButton>().Init_Equipped(item, infoText, detailUI, mouseOver,this);
        }
        for (int i = 0; i < status.equipmentSlots - status.equipments.Count; i++)
        {
            var n = Instantiate(eqButton, equipmentsP);
            n.GetComponent<CharaDetail_CharaEqButton>().Init_Empty(false, infoText, detailUI, mouseOver,this);
        }
        for (int i = 0; i < 8 - status.equipmentSlots; i++)
        {
            var l = Instantiate(eqButton, equipmentsP);
            l.GetComponent<CharaDetail_CharaEqButton>().Init_Empty(true, infoText, detailUI, mouseOver,this);
        }
    }

    public void ResetFrameColor()
    {
        frame.color = Color.white;
    }

    public void SelectChara()
    {
        detailUI.RestCharaButtonFrame();
        frame.color = Color.green;
        detailUI.ChangeChara(character);
    }

    public void OnMouseDown()
    {
        SelectChara();
    }

    public void OnMouseEnter()
    {
        mouseOver.SetUI("クリックでキャラ詳細", false);
    }
    public void OnMouseExit()
    {
        mouseOver.ResetUI();
    }

    public Character GetCharacter() { return character; }
}
