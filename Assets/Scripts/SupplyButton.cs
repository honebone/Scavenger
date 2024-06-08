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


    InfoText infoText;
    SupplyManager supplyManager;

    bool revealed = false;
    Definer.Item item;
    public void Init(Definer.Item i, InfoText it, SupplyManager sm)
    {
        item = i;
        infoText = it;
        supplyManager = sm;
    }
    public void Reveal()
    {
        revealed = true;
        itemImage.sprite = item.data.sprite;
        frame.color = item.data.rarity.ToColor();
        if (item.data.itemType != ItemData.ItemType.equipment) { amountText.text = item.amount.ToString(); }
    }

    public void OnMouseDown()
    {
        if (revealed)
        {
            if (Input.GetMouseButtonDown(1))
            {
                infoText.SetText(item.data.itemName.ColorStr(item.data.rarity.ToColor()), item.GetInfo());
            }
            if (Input.GetMouseButtonDown(0))
            {
                supplyManager.SelectItem(item);
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

    public Definer.Item GetItem() { return item; }
}
