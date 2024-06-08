using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryButton : MonoBehaviour
{
    [SerializeField]
    Text amountText;
    [SerializeField]
    Image itemImage;
    [SerializeField]
    Image frame;
    [SerializeField]
    Sprite[] frames;

   

    InfoText infoText;
    CharaDetailUI detailUI;

    Definer.Item item;
    public void Init(Definer.Item i,InfoText it,CharaDetailUI d)
    {
        item = i;
        infoText = it;
        detailUI = d;

        itemImage.sprite = item.data.sprite;
        frame.sprite = frames[(int)item.data.itemType];
        frame.color = item.data.rarity.ToColor();
        if(item.data.itemType != ItemData.ItemType.equipment) { amountText.text = item.amount.ToString(); }
        else { amountText.text = ""; }
    }

   

    public void OnMouseDown()
    {
        if (Input.GetMouseButtonDown(1))
        {
            infoText.SetText(item.data.itemName.ColorStr(item.data.rarity.ToColor()), item.GetInfo());
        }
        if (Input.GetMouseButtonDown(0))
        {
            //equipment tool / sell deliverÇÃÇ∆Ç´Ç∆Ç©Ç≈èÍçáï™ÇØ
            if (detailUI.CheckSelectingEquipment() && item.data.itemType == ItemData.ItemType.equipment)//ëïîıëIë
            {
                FindObjectOfType<Inventory>().CreateOptionUI_Equipment(transform.position, item);
            }
            else
            {
                FindObjectOfType<Inventory>().CreateOptionUI_Normal(transform.position, item);
            }
        }
    }

    public void OnMouseEnter()
    {
        FindObjectOfType<MouseOverUI>().SetUI(item.data.itemName.ColorStr(item.data.rarity.ToColor()), true);
    }
    public void OnMouseExit()
    {
        FindObjectOfType<MouseOverUI>().RestUI();
    }

}
