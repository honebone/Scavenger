using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootPanel : MonoBehaviour
{
    [SerializeField]//test
    List<Definer.Item> loots = new List<Definer.Item>();
    [SerializeField]
    GameObject lootPanel;
    [SerializeField]
    Transform content;
    [SerializeField]
    Transform optionUIPanel;

    [SerializeField]
    GameObject itemButton;
    [SerializeField]
    GameObject optionPanel;


    InfoText infoText;
    Inventory inventory;
    ExpeditionManager expeditionManager;
    [System.Serializable]
    public class DropItem
    {
        public ItemObject dropItemData;
        public int quantity;

        public ItemData GetItemData() { return dropItemData.GetItemData(); }
    }
    private void Start()
    {
        infoText = FindObjectOfType<InfoText>();
        inventory = FindObjectOfType<Inventory>();
        expeditionManager = FindObjectOfType<ExpeditionManager>();
    }

    public void ToggleLootPanel()
    {
        if (lootPanel.activeSelf) { CloseLootPanel(); }
        else { OpenLootPanel(); }
    }
    public void OpenLootPanel()
    {
        SetButtons();
        if (!lootPanel.activeSelf) { lootPanel.SetActive(true); }
        inventory.OpenInventory();
    }
    public void CloseLootPanel()
    {
        if (lootPanel.activeSelf) { lootPanel.SetActive(false); }
        inventory.CloseInventory();
        expeditionManager.OnEndLoot();
    }
    public void SetButtons()
    {
        //CloseOptionUI();
        if (content.childCount != 0)
        {
            for (int i = 0; i < content.childCount; i++)
            {
                Destroy(content.GetChild(i).gameObject);
            }
        }
        foreach (Definer.Item item in loots)
        {
            int a = item.amount;
            while (a > 0)
            {
                Definer.Item i = new Definer.Item();
                i.Init(item.data);
                i.amount = Mathf.Min(i.data.amountPerStack, a);

                var ib = Instantiate(itemButton, content);
                ib.GetComponent<LootButton>().Init(i, infoText);

                a -= item.data.amountPerStack;
            }
        }
    }

    public void CreateOptionUI_Normal(Vector3 pos, Definer.Item item)
    {
        CloseOptionUI();
        pos.x += 135 * (1f * Screen.width / 1600);
        pos.y -= 70 * (1f * Screen.height / 900);
        var o = Instantiate(optionPanel, optionUIPanel);
        o.transform.position = pos;
        o.GetComponent<ItemOptionButton>().Init(item);
    }

    public void CloseOptionUI()
    {
        if (optionUIPanel.childCount != 0)
        {
            for (int i = 0; i < optionUIPanel.childCount; i++)
            {
                Destroy(optionUIPanel.GetChild(i).gameObject);
            }
        }
        //inventoryや納品等のoptionUIも閉じる
    }

    public void ItemDrop(DropItem dropItem)
    {
        float[] dropRate = new float[] { 60, 30, 10, 5, 1 };//test
        int dropQuantity = 0;
        for (int i = 0; i < dropItem.quantity; i++)
        {
            if (dropRate[(int)dropItem.GetItemData().rarity].Probability())
            {
                dropQuantity++;
            }
        }

        if (dropQuantity > 0)
        {
            Definer.Item item = new Definer.Item();
            item.Init(dropItem.GetItemData());
            AddItem(item, dropQuantity);
        }
    }
    public void AddItem(Definer.Item item, int amount)
    {
        item.amount = amount;
        //infoText.AddLogText(string.Format("●{0}x{1}を入手", item.itemName.ColorStr(item.rarity.ToColor()), amount.ToString()));
        for (int i = 0; i < loots.Count; i++)
        {
            if (loots[i].data == item.data)
            {
                Definer.Item replace = loots[i];
                replace.amount += amount;
                loots.RemoveAt(i);
                loots.Add(replace);

                Sort();
                return;
            }
        }
        loots.Add(item);
        Sort();
    }
    public void RemoveItem(Definer.Item remove, int amount)
    {
        if (loots.Count == 0)
        {
            infoText.AddErrorText("そもそも何も持ってません");
        }
        if (amount < 0)
        {
            infoText.AddErrorText("減らす数が負の値です");
            return;
        }

        for (int i = loots.Count - 1; i >= 0; i--)
        {
            if (loots[i].data == remove.data)
            {

                //infoText.AddLogText(string.Format("○{0}x{1}を失った", remove.itemName.ColorStr(remove.rarity.ToColor()), amount.ToString()));

                if (loots[i].amount == amount) { loots.RemoveAt(i); }//減らす量がスロットのamountと同じ
                else if (loots[i].amount < amount)//減らす量がamountより多い
                {
                    infoText.AddErrorText("現在所持している数よりも多くのアイテムを減らそうとしています");
                }
                else//減らす量がスロットのamountより小さい
                {
                    Definer.Item replace = loots[i];
                    replace.amount -= amount;
                    loots.RemoveAt(i);
                    loots.Add(replace);
                }

                Sort();
                return;
            }
        }

        infoText.AddErrorText("持っていないアイテムの数を減らそうとしてます");
    }
    public void ObtainAll()
    {
        foreach (Definer.Item item in loots)
        {
            inventory.AddItem(item, item.amount, true);
        }
        EndLoot();
    }
    public void EndLoot()
    {
        CloseLootPanel();
        ResetLoots();

        //expeditionManager.LootEnd();
    }
    public void ResetLoots()
    {
        loots = new List<Definer.Item>();
        SetButtons();
    }

    void Sort()
    {
        loots.Sort((a, b) => (int)b.data.rarity - (int)a.data.rarity);
        SetButtons();
    }

    public int GetItemAmount(ItemData itemData)
    {
        foreach (Definer.Item item in loots)
        {
            if (item.data == itemData) { return item.amount; }
        }
        return 0;
    }
}
