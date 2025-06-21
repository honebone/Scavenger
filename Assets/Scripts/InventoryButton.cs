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

    [SerializeField]
    float scrollSpeed = 0.1f;
    ScrollRect scroll;
    float wheel;
    bool p;

    InfoText infoText;
    CharaDetailUI detailUI;

    Definer.Item item;
    public void Init(Definer.Item i,InfoText it,CharaDetailUI d, ScrollRect scrollRect)
    {
        item = i;
        infoText = it;
        detailUI = d;
        scroll = scrollRect;

        itemImage.sprite = item.data.sprite;
        frame.sprite = frames[(int)item.data.itemType];
        frame.color = item.data.rarity.ToColor();
        if(item.data.itemType != ItemData.ItemType.equipment) { amountText.text = item.amount.ToString(); }
        else { amountText.text = ""; }
    }

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

    public void OnMouseDown()
    {
        infoText.SetText(item.data.itemName.ColorStr(item.data.rarity.ToColor()), item.GetInfo(false), item.GetInfo(true));
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
        p = true;
        MouseOverUI.inst.SetUI($"{item.data.itemName.ColorStr(item.data.rarity.ToColor())}\n{item.GetInfo(true)}", true);
    }
    public void OnMouseExit()
    {
        p = false;
        FindObjectOfType<MouseOverUI>().ResetUI();
    }

}
