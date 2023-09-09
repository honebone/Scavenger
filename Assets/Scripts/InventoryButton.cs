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

    InfoText infoText;

    Definer.Item item;
    public void Init(Definer.Item i,InfoText it)
    {
        item = i;
        infoText = it;

        itemImage.sprite = item.sprite;
        frame.color = item.rarity.ToColor();
        amountText.text = item.amount.ToString();
    }

    public void OnMouseDown()
    {
        if (Input.GetMouseButtonDown(1))
        {
            infoText.SetText(item.itemName.ColorStr(item.rarity.ToColor()), item.GetInfo());
        }
        //if (Input.GetMouseButtonDown(0))
        //{
        //    switch (parent)
        //    {
        //        case 0:
        //            FindObjectOfType<ExpeditionInventoryUI>().CreateOptionUI_Material(transform.position, material);
        //            break;
        //        case 1:
        //            FindObjectOfType<LootManager>().CreateOptionUI_Material(transform.position, material);
        //            break;
        //    }
        //}
    }
}
