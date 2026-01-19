using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ChataDetail_CharaButton : MonoBehaviour
{
    [SerializeField] InfoText infoText;
    [SerializeField] CharaDetailUI detailUI;
    [SerializeField] MouseOverUI mouseOver;
    [SerializeField] Image charaImage;
    [SerializeField] Image frame;
    public TextMeshProUGUI lvlText;

    [SerializeField] Transform equipmentsP;
    [SerializeField] GameObject eqButton;
    public ParticleSystem canLVLUP;

    Character character;
    Character.CharacterStatus status;
    int expCount;
    int expReq;

    Inventory inventory;
    LVLUpManager LVLUpManager;

    private void Start()
    {
        inventory = Inventory.inst;
        LVLUpManager = LVLUpManager.inst;
    }

    public void SetChara(Character chara)
    {
        character = chara;
        charaImage.sprite = character.CharaStatus().spriteForUI;
        charaImage.color = Color.white;
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

       Refresh();
    }

    public void Refresh()
    {
        status = character.CharaStatus();
        expCount = Inventory.inst.GetExp();
        expReq = Mathf.Max(status.GetNextExp() - status.exp, 0);
        if (status.level < GameManager.gameParams.maxLVL && expCount >= expReq)
        {
            frame.color = Definer.colorRef.expOrb;
            canLVLUP.Play();
        }
        else
        {
            frame.color = Color.white;
            canLVLUP.Stop();
        }

        lvlText.text=$"LVL {status.level}";

        foreach (Definer.Item item in status.equipments)
        {
            var e = Instantiate(eqButton, equipmentsP);
            e.GetComponent<CharaDetail_CharaEqButton>().Init_Equipped(item, infoText, detailUI, mouseOver, this);
        }
        for (int i = 0; i < status.equipmentSlots - status.equipments.Count; i++)
        {
            var n = Instantiate(eqButton, equipmentsP);
            n.GetComponent<CharaDetail_CharaEqButton>().Init_Empty(false, infoText, detailUI, mouseOver, this);
        }
        List<int> unlickEqSlotLvl = GameManager.gameParams.unlockEqSlotLVL;

        for (int i = status.equipmentSlots - 4; i < 4; i++)
        {
            var l = Instantiate(eqButton, equipmentsP);
            l.GetComponent<CharaDetail_CharaEqButton>().Init_Empty(true, infoText, detailUI, mouseOver, this, unlickEqSlotLvl[i]);
        }
    }
    public void ResetValue()
    {
        character = null;
        charaImage.color = Color.clear;
        if (equipmentsP.childCount != 0)
        {
            for (int i = 0; i < equipmentsP.childCount; i++)
            {
                Destroy(equipmentsP.GetChild(i).gameObject);
            }
        }
    }

    public void ResetFrameColor()
    {
        frame.color = Color.white;
    }

    public void SelectChara()
    {
        //detailUI.RestCharaButtonFrame();
        //frame.color = Color.green;
        detailUI.ChangeChara(character);
    }

    public void OnMouseDown()
    {
        if (character != null)
        {
            if (Input.GetMouseButtonDown(1)) SelectChara();
            if (Input.GetMouseButtonDown(0) && status.level < GameManager.gameParams.maxLVL && expCount >= expReq && !LVLUpManager.GetInLVLUp())
            {
                if (!ExpeditionManager.inst.CheckInRoomEvent())
                {
                    if (inventory.GetExp() >= expReq)
                    {
                        if (expReq > 0)
                        {
                            inventory.AddExp(-expReq, false);
                            character.GainEXP(expReq);
                        }
                        
                        LVLUpManager.inst.LVLUp(character);
                    }
                    else
                    {
                        infoText.AddWarningText("経験のオーブ数の不一致");
                    }
                }
                else
                {
                    GuideMessage.inst.SetWaringText("イベント中のLVL UP不可");
                }
            }
        }
    }

    public void OnMouseEnter()
    {
        string s = "右クリックでキャラ詳細\n\n";
        if (status.level >= GameManager.gameParams.maxLVL)
        {
            s += "LVL 最大".ColorStr(Definer.colorRef.emphasize);
        }
        else
        {
            s += $"次のLVL UPに必要なexp：{expReq}\n";
            if (expCount >= expReq)
            {
                s += "LVL UP 可能\n左クリックでLVL UP!".ColorStr(Definer.colorRef.expOrb);
            }
            else
            {
                s += "経験のオーブが足りない\n".ColorStr(Color.red);

            }
        }
            
        mouseOver.SetUI(s, false);
    }
    public void OnMouseExit()
    {
        mouseOver.ResetUI();
    }

    public Character GetCharacter() { return character; }
}
