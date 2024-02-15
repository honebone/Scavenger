using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemOptionButton : MonoBehaviour
{
    [SerializeField]
    GameObject[] buttons;//0:obtain all 1:obtain 2:buy 3:sell 4:abandon 5:craft 6:deliver 7:buyall 8:sellall
    [SerializeField]
    GameObject selectAmountPanel;
    [SerializeField]
    Slider amountSlider;
    [SerializeField]
    Text amountText;
    [SerializeField, Header("0:inventory 1:loot 2:equip_equipment")]
    int mode;

    //[SerializeField]
    //GameObject pricePanel;
    //[SerializeField]
    //Text priceText;
    Definer.Item item;
    Character equipChara;
    bool hasEquipment;
    Definer.Item currentEquipment;

    int amount = 1;
    int maxAmount;
    /// <summary>0:abandon 1:loot 2:buy 3:sell</summary>
    int p;
    //bool canBuyAll = true;
    //bool canbuy = true;

    int buyPrice;
    int sellPrice;

    ExpeditionManager expeditionManager;
    Inventory inventory;
    LootPanel loot;
    private void Awake()
    {
        expeditionManager = FindObjectOfType<ExpeditionManager>();
        inventory = FindObjectOfType<Inventory>();
        loot = FindObjectOfType<LootPanel>();
    }

    /// <summary>
    /// modeは選択肢を決定する引数 (0:normal 1:loot)
    /// </summary>
    public void Init(Definer.Item i)
    {
        item = i;
        switch (mode)
        {
            case 0:
                maxAmount = inventory.GetItemAmount(item.data);
                break;
            case 1:
                maxAmount = loot.GetItemAmount(item.data);
                break;
        }
    }
    public void Init_Equipment(Definer.Item i,Character c,bool h,Definer.Item current)
    {
        item = i;
        equipChara = c;
        hasEquipment = h;
        currentEquipment = current;
        if (mode != 2) { FindObjectOfType<InfoText>().AddErrorText("modeの設定ミス"); }
    }
    void StartSelectAmount()
    {
        selectAmountPanel.SetActive(true);
        switch (p)
        {
            case 0:
                amountSlider.maxValue = inventory.GetItemAmount(item.data);
                break;
            case 1:
                amountSlider.maxValue = loot.GetItemAmount(item.data);
                break;
                //case 2:
                //    amountSlider.maxValue = Mathf.Clamp(Mathf.Floor(inventory.GetMaterialAmount(1) / buyPrice * 1f), 0, item.amount);
                //    break;
                //case 3:
                //    amountSlider.maxValue = item.amount;
                //    break;
        }
    }
    public void SetAmountBySlider()
    {
        amount = Mathf.RoundToInt(amountSlider.value);
        SetAmountText();
    }
    public void AddAmount()
    {
        if (amount < maxAmount)
        {
            amount++;
            amountSlider.value = amount;
            SetAmountText();
        }
    }
    public void SubtractAmount()
    {
        if (amount > 0)
        {
            amount--;
            amountSlider.value = amount;
            SetAmountText();
        }
    }
    public void WholeSlotAmount()
    {
        amount = item.amount;
        amountSlider.value = amount;
        SetAmountText();
    }
    public void ConfirmAmont()
    {
        switch (p)
        {
            case 0:
                inventory.RemoveItem(item, amount);
                break;
            case 1:
                inventory.AddItem(item, amount,true);
                loot.RemoveItem(item, amount);
                //FindObjectOfType<ExpeditionInventoryUI>().ToggleInventoryToMaterial();
                break;
            //case 2:
            //    inventory.AddItem(item, amount);
            //    inventory.RemoveItem(FindObjectOfType<Definer>().GetGold(), buyPrice * amount, true);
            //    lootManager.RemoveMaterialLoot(item, amount);
            //    break;
            //case 3:
            //    inventory.RemoveItem(item, amount);
            //    inventory.AddItem(FindObjectOfType<Definer>().GetGold(), sellPrice * amount);
            //    break;
        }
        CloseOptionnUI();
    }
    void SetAmountText()
    {
        amountText.text = "個数：" + amount.ToString();
    }


    public void AbandonAll()
    {
        inventory.RemoveItem(item, item.amount);
        CloseOptionnUI();
    }
    public void Abandon()
    {
        p = 0;
        StartSelectAmount();
    }
    public void ObtainAll()
    {
        inventory.AddItem(item, item.amount, true);
        loot.RemoveItem(item, item.amount);
        //FindObjectOfType<ExpeditionInventoryUI>().ToggleInventoryToMaterial();
        CloseOptionnUI();
    }
    public void Obtain()
    {
        p = 1;
        StartSelectAmount();
    }
    public void Equip()
    {
        if (hasEquipment) { equipChara.UnequipItem(currentEquipment); }
        inventory.RemoveItem(item, item.amount);
        equipChara.EquipItem(item);

        FindObjectOfType<CharaDetailUI>().EndSelectEquipment();
        FindObjectOfType<CharaDetailUI>().Refresh();
        inventory.CloseInventory();
        CloseOptionnUI();
    }
    //public void BuyAll()
    //{
    //    if (canBuyAll)
    //    {
    //        inventory.ObtainMaterial(item, item.amount);
    //        inventory.AbandonMaterial(FindObjectOfType<Definer>().GetGold(), buyPrice * item.amount, true);
    //        loot.RemoveMaterialLoot(item, item.amount);
    //        CloseOptionnUI();
    //    }
    //}
    //public void Buy()
    //{
    //    if (canbuy)
    //    {
    //        p = 2;
    //        StartSelectAmount();
    //    }
    //}
    //public void SellAll()
    //{
    //    inventory.AbandonMaterial(item, item.amount, true);
    //    inventory.ObtainMaterial(FindObjectOfType<Definer>().GetGold(), sellPrice * item.amount);
    //    CloseOptionnUI();
    //}
    //public void Sell()
    //{
    //    p = 3;
    //    StartSelectAmount();
    //}



    public void CloseOptionnUI()
    {      
        Destroy(gameObject);
    }
}
