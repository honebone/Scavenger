using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharaDetail_CharaEqButton : MonoBehaviour
{
    [SerializeField] Image itemImage;
    [SerializeField] Image frame;
    [SerializeField] Image lockedImage;
    [SerializeField] Color lockedColor;

    InfoText infoText;
    CharaDetailUI detailUI;
    ChataDetail_CharaButton charaButton;
    MouseOverUI mouseOver;
    Character character;

    bool empty = true;
    bool locked = true;
    int unlockLvl;
    Definer.Item item;

   public void Init_Empty(bool l, InfoText it, CharaDetailUI d, MouseOverUI m,ChataDetail_CharaButton cb,int ul=0)
    {
        infoText = it;
        detailUI = d;
        mouseOver = m;
        charaButton = cb;
        character = charaButton.GetCharacter();
        locked = l;
        unlockLvl = ul;

        if (locked)
        {
            lockedImage.enabled = true;
            frame.color = lockedColor;
        }
    }

    public void Init_Equipped(Definer.Item i, InfoText it, CharaDetailUI d, MouseOverUI m, ChataDetail_CharaButton cb)
    {
        infoText = it;
        detailUI = d;
        mouseOver = m;
        charaButton = cb;
        character = charaButton.GetCharacter();
        item = i;

        empty = false;
        locked = false;
        itemImage.enabled = true;
        itemImage.sprite = item.data.sprite;
        frame.color = item.data.rarity.ToColor();
    }

    public void Equip(Definer.Item equip)
    {
        if (!empty) { character.UnequipItem(item); }
        character.EquipItem(equip);

        charaButton.SetButtons();
    }

    public void OnMouseDown()
    {
        if (Input.GetMouseButtonDown(1))
        {
            if (!empty)
            {
                infoText.SetText(item.data.itemName.ColorStr(item.data.rarity.ToColor()), item.GetInfo(false), item.GetInfo(true));
            }
        }
        if (Input.GetMouseButtonDown(0))
        {
            if (!empty)
            {
                if (!FindObjectOfType<ExpeditionManager>().CheckInRoomEvent())
                {
                    mouseOver.ResetUI();
                    detailUI.SetDraggingItem(item, charaButton);
                    Utils_VE.inst.SpawnVE_UISmoke();
                }
                else
                {
                    FindObjectOfType<GuideMessage>().SetWaringText("イベント中の装備変更不可");
                }
            }
        }
    }

    public void OnMouseEnter()
    {
        if (!empty)
        {
            mouseOver.SetUI($"{item.data.itemName.ColorStr(item.data.rarity.ToColor())}\n{item.GetInfo(true)}", true);
        }
        else if (locked)
        {
            mouseOver.SetUI($"LVL {unlockLvl}でアンロック", false);
        }
    }
    public void OnMouseExit()
    {
        mouseOver.ResetUI();
    }

    public bool CheckLocked() { return locked; }
    public Character GetCharacter() { return character; }
    public ChataDetail_CharaButton GetCharaButton() { return charaButton; }
}
