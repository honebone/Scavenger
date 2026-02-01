using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
using System.Linq;

public class Shop : MonoBehaviour
{
    public static Shop inst;
    private void Awake()
    {
        if (inst == null) { inst = this; }
    }

    [SerializeField] GameObject panel;
    [SerializeField] TextMeshProUGUI coinsText;
    [SerializeField] List<ShopItemButton> buttons;
    [SerializeField] List<AudioClip> SE_purchase;
    ShopParams shopParams;
    public  void StartShop(ShopParams _params)
    {
        panel.SetActive(true);

        shopParams = _params;
        if (shopParams.lineups.Count > buttons.Count)
        {
            InfoText.inst.AddErrorText("ショップのラインナップ数エラー");
            return;
        }

        GameParams gp=GameManager.gameParams;

        List<int> saleIndex= Enumerable.Range(0, shopParams.lineups.Count).ToList().Sample(gp.salePerShop);
        for(int i = 0;i< shopParams.lineups.Count;i++)
        {
            int price = gp.eqPrice[(int)shopParams.lineups[i]].Mul(gp.priceMul.Range()).Mul(shopParams.priceMul);
            bool sale = saleIndex.Contains(i);
            int salePrice = price.Mul(gp.saleMul);
            buttons[i].SetItem(ExpeditionManager.inst.GetRandomEquipment_WithGuarantee(shopParams.lineups[i]),price,sale,salePrice);
        }

        Refresh();
    }
    public void Purchase()
    {
        Refresh();
        SoundManager.instance.PlaySE(SE_purchase);
        buttons.ForEach(b=>b.Refresh());
    }

    public void Refresh()
    {
        coinsText.text = $"{"coin".ToSpr()}{Inventory.inst.GetCoin()}".ColorStr(Definer.colorRef.coin);
    }

    public void EndShop()
    {
        buttons.ForEach(b => b.ResetItem());
        panel.SetActive(false);
        ExpeditionManager.inst.OnEndShop();
        SoundManager.instance.PlaySE_Select();
    }
}

[System.Serializable]
public class ShopParams
{
    public List<ItemData.Rarity> lineups;
    public float priceMul = 100f;
}
