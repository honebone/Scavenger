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
    SoundManager soundManager;

    bool revealed = false;
    Definer.Item item;
    public void Init(Definer.Item i, InfoText it,SoundManager sm)
    {
        item = i;
        infoText = it;
        soundManager = sm;
    }
    public void Reveal()
    {
        revealed = true;
        itemImage.sprite = item.data.sprite;
        frame.color = item.data.rarity.ToColor();
        amountText.text = item.amount.ToString();
    }

    public void OnMouseDown()
    {
        if (revealed)
        {
            if (Input.GetMouseButtonDown(1))
            {
                infoText.SetText(item.data.itemName.ColorStr(item.data.rarity.ToColor()), item.GetInfo());
            }
            //if (Input.GetMouseButtonDown(0))
            //{
            //    FindObjectOfType<LootPanel>().CreateOptionUI_Normal(transform.position, item);
            //}
        }
    }

    public Definer.Item GetItem() { return item; }
}
