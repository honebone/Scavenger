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
    [SerializeField]
    List<GameObject> revealVE;


    InfoText infoText;
    SoundManager soundManager;

    bool revealed = false;
    Definer.Item item;

    [Header("\n\nexpOrb"),SerializeField] Sprite expSprite;
    [SerializeField] Color expColor;
    [SerializeField] string expName;
    [SerializeField, TextArea(3, 10)] string expInfo;
    bool expOrb;
    int expAmount;
    public void Init(Definer.Item i, InfoText it,SoundManager sm)
    {
        item = i;
        infoText = it;
        soundManager = sm;
    }
    public void Init_AsExp(int amount,InfoText it, SoundManager sm)
    {
        expOrb = true;
        expAmount = amount;
        infoText = it;
        soundManager = sm;
    }
    public void Reveal()
    {
        revealed = true;

        if (expOrb)
        {
            itemImage.sprite = expSprite;
            frame.color = expColor;
            amountText.text=expAmount.ToString();
        }
        else
        {
            itemImage.sprite = item.data.sprite;
            frame.color = item.data.rarity.ToColor();
            if (item.data.itemType != ItemData.ItemType.equipment) { amountText.text = item.amount.ToString(); }
            if (revealVE[(int)item.data.rarity] != null) { Instantiate(revealVE[(int)item.data.rarity], transform); }
        }
    }

    public void OnMouseDown()
    {
        if (revealed)
        {
            if (Input.GetMouseButtonDown(1))
            {
                if (expOrb)
                {
                    infoText.SetText(expName.ColorStr(expColor), expInfo);
                }
                else
                {
                    infoText.SetText(item.data.itemName.ColorStr(item.data.rarity.ToColor()), item.GetInfo());
                }
            }
            //if (Input.GetMouseButtonDown(0))
            //{
            //    FindObjectOfType<LootPanel>().CreateOptionUI_Normal(transform.position, item);
            //}
        }
    }

    public void OnMouseEnter()
    {
        if (revealed)
        {
            if (expOrb)
            {
                FindObjectOfType<MouseOverUI>().SetUI(expName.ColorStr(expColor), true);
            }
            else
            {
                FindObjectOfType<MouseOverUI>().SetUI(item.data.itemName.ColorStr(item.data.rarity.ToColor()), true);
            }
        }
        else
        {
            ExpeditionRef.mouseover.SetUI("???");
        }
       
    }
    public void OnMouseExit()
    {
        FindObjectOfType<MouseOverUI>().ResetUI();
    }


    public Definer.Item GetItem() { return item; }
    public bool GetIfExp() { return expOrb; }
}
