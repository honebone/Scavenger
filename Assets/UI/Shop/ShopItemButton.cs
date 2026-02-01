using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Xml.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ShopItemButton : MonoBehaviour
{
    [SerializeField] CanvasGroup canvas;
    [SerializeField] TextMeshProUGUI priceText;
    [SerializeField] Image itemImage;
    [SerializeField] Image frame;
    [SerializeField] List<GameObject> revealVE;

    public List<Sprite> frames;

    bool sale;
    int price;
    int price_sale;

    Definer.Item item;
    ItemData data;

    public void SetItem(Definer.Item _item,int _price,bool _sale,int _price_sale)
    {
        item= _item;
        data = _item.data;

        price = _price;
        sale = _sale;
        price_sale = _price_sale;

        Refresh();
        Utils_VE.inst.SpawnVE(revealVE[(int)data.rarity], transform.position);

        canvas.alpha = 1;
        canvas.interactable= true;
        canvas.blocksRaycasts = true;
    }

    public void ResetItem()
    {
        item = new Definer.Item();
        data=null;
        canvas.alpha = 0;
        canvas.interactable = false;
        canvas.blocksRaycasts = false;
    }
    public void Refresh()
    {
        if (data != null)
        {
            itemImage.sprite = data.sprite;
            frame.color = data.rarity.ToColor();
            int index = Mathf.Clamp((int)data.rarity, 1, 4);
            frame.sprite = frames[index];

            if (sale) priceText.text = $"{"coin".ToSpr()}{$"<s>{price}</s>".ColorStr(Definer.colorRef.failed_unavailable)}{price_sale}";
            else priceText.text = $"{"coin".ToSpr()}{price}";
            priceText.color = CanPurchase() ? Definer.colorRef.coin : Color.red;
        }
    }

    public void OnMouseDown()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if(CanPurchase())
            {
                Utils_VE.inst.SpawnVE_UISmoke();
                Inventory.inst.AddItem(item, 1, true);
                Inventory.inst.RemoveCoin(GetPrice());
                Shop.inst.Purchase();
                ResetItem();
            }
            else
            {
                GuideMessage.inst.SetWaringText($"{"coin".ToSpr_withName()}•s‘«");
            }
        }
        if (Input.GetMouseButtonDown(1))
        {
            InfoText.inst.SetText(data.itemName.ColorStr(data.rarity.ToColor()), data.GetInfo(false), data.GetInfo(true));
        }

    }

    public void OnMouseEnter()
    {
        MouseOverUI.inst.SetUI($"{data.itemName.ColorStr(data.rarity.ToColor())}\n{data.GetInfo(true)}", true);
    }
    public void OnMouseExit()
    {
        MouseOverUI.inst.ResetUI();
    }
    int GetPrice() { return sale ? price_sale : price; }
    bool CanPurchase()
    {
        return Inventory.inst.GetCoin() >= GetPrice();
    }
}
