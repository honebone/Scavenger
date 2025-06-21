using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SupplyButton : MonoBehaviour
{
    [SerializeField]
    Text amountText;
    [SerializeField]
    Image itemImage;
    [SerializeField]
    Image frame;
    [SerializeField]
    List<GameObject> revealVE;


    InfoText infoText;
    SupplyManager supplyManager;
    MouseOverUI mouseOver;

    bool revealed = false;
    Definer.Item item;
    public void Init(Definer.Item i, InfoText it, SupplyManager sm)
    {
        item = i;
        infoText = it;
        supplyManager = sm;
        mouseOver = FindObjectOfType<MouseOverUI>();
    }
    public void Reveal()
    {
        revealed = true;
        itemImage.sprite = item.data.sprite;
        frame.color = item.data.rarity.ToColor();
        if (item.data.itemType != ItemData.ItemType.equipment) { amountText.text = item.amount.ToString(); }
        if (revealVE[(int)item.data.rarity] != null) { Instantiate(revealVE[(int)item.data.rarity], transform); }
    }

    public void OnMouseDown()
    {
        if (revealed)
        {
            if (Input.GetMouseButtonDown(1))
            {
                infoText.SetText(item.data.itemName.ColorStr(item.data.rarity.ToColor()), item.GetInfo(false), item.GetInfo(true));
            }
            if (Input.GetMouseButtonDown(0))
            {
                mouseOver.ResetUI();
                supplyManager.SelectItem(item);
            }
        }
    }

    public void OnMouseEnter()
    {
        if (revealed)
        {
            mouseOver.SetUI($"{item.data.itemName.ColorStr(item.data.rarity.ToColor())}\n{item.GetInfo(true)}", true);
        }
        else
        {
            ExpeditionRef.mouseover.SetUI("???");
        }

    }
    public void OnMouseExit()
    {
       mouseOver.ResetUI();
    }

    public Definer.Item GetItem() { return item; }
}
