using System;
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

    public List<Sprite> frames;


    InfoText infoText;
    SoundManager soundManager;

    bool revealed = false;
    Definer.Item item;

    [Header("\n\nexpOrb"),SerializeField] Sprite expSprite;
    [SerializeField] Color expColor;
    [SerializeField] string expName;
    [SerializeField, TextArea(3, 10)] string expInfo;
    [Header("\n\nexpOrb"), SerializeField] Sprite coinSprite;
    [SerializeField] string coinName;
    [SerializeField, TextArea(3, 10)] string coinInfo;

    bool expOrb;
    int expAmount;
    bool coin;
    int coinAmount;
    public void Init(Definer.Item i, InfoText it, SoundManager sm)
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
    public void Init_AsCoin(int amount, InfoText it, SoundManager sm)
    {
        coin = true;
        coinAmount = amount;
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
            frame.sprite = frames[2];
        }
        else if (coin)
        {
            itemImage.sprite = coinSprite;
            frame.color = Definer.colorRef.coin;
            amountText.text = coinAmount.ToString();
            frame.sprite = frames[2];
        }
        else
        {
            itemImage.sprite = item.data.sprite;
            frame.color = item.data.rarity.ToColor();
            if (item.data.itemType != ItemData.ItemType.equipment) { amountText.text = item.amount.ToString(); }
            if (revealVE[(int)item.data.rarity] != null) { Instantiate(revealVE[(int)item.data.rarity], transform); }
            int index = Mathf.Clamp((int)item.data.rarity, 1, 4);
            frame.sprite = frames[index];
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
                    infoText.SetText_Old(expName.ColorStr(expColor), expInfo);
                }
                else if (coin)
                {
                    infoText.SetText_Old(coinName.ColorStr(Definer.colorRef.coin), coinInfo);
                }
                else
                {
                    infoText.SetText(item.data.itemName.ColorStr(item.data.rarity.ToColor()), item.GetInfo(false), item.GetInfo(true));
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
            else if (coin)
            {
                MouseOverUI.inst.SetUI(coinName.ColorStr(Definer.colorRef.coin),true);
            }
            else
            {
                MouseOverUI.inst.SetUI($"{item.data.itemName.ColorStr(item.data.rarity.ToColor())}\n{item.GetInfo(true)}", true);
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
    public bool GetIfCoin() { return coin; }
}
