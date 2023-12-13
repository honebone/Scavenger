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

    Definer.Item item;
    public void Init(Definer.Item i,InfoText it)
    {
        item = i;
        infoText = it;

        itemImage.sprite = item.sprite;
        frame.sprite = frames[(int)item.itemType];
        frame.color = item.rarity.ToColor();
        if(item.itemType!= ItemData.ItemType.equipment) { amountText.text = item.amount.ToString(); }
        else { amountText.text = ""; }
    }

    public void OnMouseDown()
    {
        if (Input.GetMouseButtonDown(1))
        {
            infoText.SetText(item.itemName.ColorStr(item.rarity.ToColor()), item.GetInfo());
        }
        if (Input.GetMouseButtonDown(0))
        {
            //equipment tool / sell deliverÇÃÇ∆Ç´Ç∆Ç©Ç≈èÍçáï™ÇØ
            FindObjectOfType<Inventory>().CreateOptionUI_Normal(transform.position, item);
        }
    }
    
}
