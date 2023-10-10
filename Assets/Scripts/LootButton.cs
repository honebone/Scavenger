using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class LootButton : MonoBehaviour
{
    [SerializeField]
    Text amountText;
    [SerializeField]
    Image itemImage;
    [SerializeField]
    Image frame;


    InfoText infoText;

    Definer.Item item;
    public void Init(Definer.Item i, InfoText it)
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
        if (Input.GetMouseButtonDown(0))
        {
            FindObjectOfType<LootPanel>().CreateOptionUI_Normal(transform.position, item);
        }
    }
}
