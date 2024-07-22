using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharaDetail_InventoryEqButton : MonoBehaviour
{
    [SerializeField] Image itemImage;
    [SerializeField] Image frame;

    InfoText infoText;
    CharaDetailUI detailUI;
    MouseOverUI mouseOver;

    Definer.Item item;
    public void Init(Definer.Item i, InfoText it, CharaDetailUI d,MouseOverUI m)
    {
        item = i;
        infoText = it;
        detailUI = d;
        mouseOver = m;

        itemImage.sprite = item.data.sprite;
        frame.color = item.data.rarity.ToColor();
    }



    public void OnMouseDown()
    {
        if (Input.GetMouseButtonDown(1))
        {
            infoText.SetText(item.data.itemName.ColorStr(item.data.rarity.ToColor()), item.GetInfo());
        }
        if (Input.GetMouseButtonDown(0))
        {
            if (!FindObjectOfType<ExpeditionManager>().CheckInRoomEvent())
            {
                mouseOver.ResetUI();
                detailUI.SetDraggingItem(item, null);
            }
            else
            {
                FindObjectOfType<GuideMessage>().SetWaringText("イベント中の装備変更不可");
            }
        }
    }

    public void OnMouseEnter()
    {
        mouseOver.SetUI(string.Format("{0}\nドラッグで装備", item.data.itemName.ColorStr(item.data.rarity.ToColor())), true);
    }
    public void OnMouseExit()
    {
        mouseOver.ResetUI();
    }

}
