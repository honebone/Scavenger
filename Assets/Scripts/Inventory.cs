using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Inventory : MonoBehaviour
{
    [SerializeField]//test
    List<Definer.Item> inventory=new List<Definer.Item> ();
    int expOrbs;


    [SerializeField]
    GameObject inventoryPanel;
    [SerializeField]
    Transform content;
    [SerializeField]
    Transform optionUIPanel;

    [SerializeField]
    TextMeshProUGUI expOrbText;

    [SerializeField]
    GameObject inventoryButton;
    [SerializeField]
    GameObject optionPanel_normal;
    [SerializeField]
    GameObject optionPanel_equipment;

    /// <summary>0:all 1:material 2:equipment 3:tool</summary>
    int sort;
    InfoText infoText;
    CharaDetailUI detailUI;
    private void Start()
    {
        infoText = FindObjectOfType<InfoText>();
        detailUI=FindObjectOfType<CharaDetailUI>();
    }
   
    public void ToggleInventory()
    {
        if (inventoryPanel.activeSelf) { CloseInventory(); }
        else { OpenInventory(); }
    }
    /// <summary>0:all 1:material 2:equipment 3:tool</summary>
    public void ChangeSort(int s)
    {
        sort = s;
        SetButtons();
    }
    public void OpenInventory()
    {
        SetButtons();
        if (!inventoryPanel.activeSelf) { inventoryPanel.SetActive(true); }
    }
    public void CloseInventory()
    {
        if (inventoryPanel.activeSelf) { inventoryPanel.SetActive(false); }
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
        ItemData.ItemType sortType=ItemData.ItemType.material;
        switch (sort)
        {
            case 2:
                sortType = ItemData.ItemType.equipment;
                break;
            case 3:
                sortType = ItemData.ItemType.tool;
                break;
        }
        foreach(Definer.Item item in inventory)
        {
            if (sort == 0 || item.data.itemType == sortType)
            {
                int a = item.amount;
                while (a > 0)
                {
                    Definer.Item i = new Definer.Item();
                    i.Init(item.data);
                    i.amount = Mathf.Min(i.data.amountPerStack, a);

                    var ib = Instantiate(inventoryButton, content);
                    ib.GetComponent<InventoryButton>().Init(i, infoText, detailUI);

                    a -= item.data.amountPerStack;
                }
            }
        }

        expOrbText.text = string.Format("経験のオーブ x{0}", expOrbs);
    }

    public void CreateOptionUI_Normal(Vector3 pos, Definer.Item item)
    {
        CloseOptionUI();
        pos.x += 135 * (1f * Screen.width / 1600);
        pos.y -= 70 * (1f * Screen.height / 900);
        var o = Instantiate(optionPanel_normal, optionUIPanel);
        o.transform.position = pos;
        o.GetComponent<ItemOptionButton>().Init(item);
    }
    public void CreateOptionUI_Equipment(Vector3 pos, Definer.Item item)
    {
        CloseOptionUI();
        pos.x += 135 * (1f * Screen.width / 1600);
        pos.y -= 70 * (1f * Screen.height / 900);
        var o = Instantiate(optionPanel_equipment, optionUIPanel);
        o.transform.position = pos;
        o.GetComponent<ItemOptionButton>().Init_Equipment(item, detailUI.GetDisplayingChara(), !detailUI.CheckEmpty(), detailUI.GetSelectedEquipment());
    }

    /// <summary>インベントリに限らず、選択肢UIを消去する際はこれを呼ぶ</summary>
    public void CloseOptionUI()
    {
        if (optionUIPanel.childCount != 0)
        {
            for (int i = 0; i < optionUIPanel.childCount; i++)
            {
                Destroy(optionUIPanel.GetChild(i).gameObject);
            }
        }
        detailUI.Refresh();
        //lootや納品等のoptionUIも閉じる
    }

    public void AddItem(Definer.Item item,int amount,bool note)
    {
        Definer.Item replace = new Definer.Item();

        if (note)
        {
            infoText.AddLogText(string.Format("●{0}x{1}を入手", item.data.itemName.ColorStr(item.data.rarity.ToColor()), amount.ToString()));
        }
        for (int i=0;i<inventory.Count;i++)
        {
            if (inventory[i].data == item.data)
            {
                replace = inventory[i];
                replace.amount += amount;
                inventory.RemoveAt(i);
                inventory.Add(replace);

                SortInventory();
                return;
            }
        }
        replace.Init(item.data);
        replace.amount = amount;
        inventory.Add(replace);
        SortInventory();
    }
   
    public void RemoveItem(Definer.Item remove,int amount)
    {
        if (inventory.Count == 0)
        {
            infoText.AddErrorText("そもそも何も持ってません");
        }
        if (amount < 0)
        {
            infoText.AddErrorText("減らす数が負の値です");
            return;
        }

        for (int i = inventory.Count - 1; i >= 0; i--)
        {
            if (inventory[i].data == remove.data)
            {
               
                infoText.AddLogText(string.Format("○{0}x{1}を失った", remove.data.itemName.ColorStr(remove.data.rarity.ToColor()), amount.ToString()));
                
                if (inventory[i].amount == amount) { inventory.RemoveAt(i); }//減らす量がスロットのamountと同じ
                else if (inventory[i].amount < amount)//減らす量がamountより多い
                {
                    infoText.AddErrorText("現在所持している数よりも多くのアイテムを減らそうとしています");
                }
                else//減らす量がスロットのamountより小さい
                {
                    Definer.Item replace = inventory[i];
                    replace.amount -= amount;
                    inventory.RemoveAt(i);
                    inventory.Add(replace);
                }

                SortInventory();
                return;
            }
        }

        infoText.AddErrorText("持っていないアイテムの数を減らそうとしてます");        
    }
    public void AddExp(int amount, bool note)
    {
        if (note)
        {
            infoText.AddLogText(string.Format("●{0}x{1}を入手", "経験のオーブ", amount.ToString()));
        }
        expOrbs += amount;
    } public void RemoveExp(int amount, bool note)
    {
        if (amount > expOrbs) { infoText.AddErrorText("減らす数所持数より多いです"); }
        if (note)
        {
            infoText.AddLogText(string.Format("○{0}x{1}を失った", "経験のオーブ", amount.ToString()));
        }
        expOrbs -= amount;
    }

    void SortInventory()
    {
        inventory.Sort((a, b) => (int)b.data.rarity - (int)a.data.rarity);
        SetButtons();
    }

    public int GetItemAmount(ItemData itemData)
    {
        foreach (Definer.Item item in inventory)
        {
            if (item.data == itemData) { return item.amount; }
        }
        return 0;
    }

    public List<Definer.Item> GetMaterials()
    {
        List<Definer.Item> materials = new List<Definer.Item>();
        foreach(Definer.Item material in inventory)
        {
            if (material.data.itemType == ItemData.ItemType.material) { materials.Add(material); }
        }
        return materials;
    }
}
