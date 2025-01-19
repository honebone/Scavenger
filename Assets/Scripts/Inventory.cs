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

    public static Inventory inst;

    /// <summary>0:all 1:material 2:equipment 3:tool</summary>
    int sort;
    InfoText infoText;
    CharaDetailUI detailUI;

    private void Awake()
    {
        inst = this;
    }
    private void Start()
    {
        infoText = FindObjectOfType<InfoText>();
        detailUI=FindObjectOfType<CharaDetailUI>();
    }

    public void InventoryButton()
    {
        if (Input.GetMouseButtonDown(1))
        {
            infoText.SetText("インベントリ", "所持しているアイテムを確認する");
        }
        if (Input.GetMouseButtonDown(0))
        {
            ToggleInventory();
        }
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
   
    public void RemoveItem(Definer.Item remove,int amount,bool note = true)
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
               
               if(note) infoText.AddLogText(string.Format("○{0}x{1}を失った", remove.data.itemName.ColorStr(remove.data.rarity.ToColor()), amount.ToString()));
                
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

    /// <summary>装備品を消去　インベントリにないなら、装備中のアイテムを外す</summary>
    public void RemoveEq_WithEquipped(Definer.Item remove, bool note = true)
    {
        for (int i = inventory.Count - 1; i >= 0; i--)
        {
            if (inventory[i].data == remove.data)
            {

                if (note) infoText.AddLogText(string.Format("○{0}を失った", remove.data.itemName.ColorStr(remove.data.rarity.ToColor())));

                if (inventory[i].amount == 1) { inventory.RemoveAt(i); }
                else if (inventory[i].amount < 1)//アイテム数が0以下　本来あり得ないが一応
                {
                    infoText.AddErrorText("アイテムの数が異常です");
                }
                else//減らす量がスロットのamountより小さい
                {
                    Definer.Item replace = inventory[i];
                    replace.amount --;
                    inventory.RemoveAt(i);
                    inventory.Add(replace);
                }

                SortInventory();
                return;
            }
        }

        foreach (Character chara in CharactersManager.inst.GetExistingCharacters_All())
        {
            foreach(Definer.Item equipped in chara.CharaStatus().equipments)
            {
                if (equipped.data == remove.data)
                {
                    chara.UnequipItem(equipped, false);
                    return;
                }
            }
        }

        infoText.AddErrorText("持っていない装備品を減らそうとしてます");
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
    public List<Definer.Item> GetEquipments()
    {
        List<Definer.Item> equipments = new List<Definer.Item>();
        foreach(Definer.Item equipment in inventory)
        {
            if (equipment.data.itemType == ItemData.ItemType.equipment) { equipments.Add(equipment); }
        }
        return equipments;
    }

    /// <summary>キャラに装備中のアイテムも含めて返す</summary>
    public List<Definer.Item> GetEquipments_WithEquipped()
    {
        List<Definer.Item> list = new List<Definer.Item>(GetEquipments());
        List<Definer.Item> equipped = new List<Definer.Item>();
        foreach(Character chara in CharactersManager.inst.GetExistingCharacters_All())
        {
            equipped.AddRange(chara.CharaStatus().equipments);
        }

        foreach(Definer.Item item in equipped)
        {
            Definer.Item replace = new Definer.Item();
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].data == item.data)
                {
                    replace = list[i];
                    replace.amount ++;
                    list.RemoveAt(i);
                    list.Add(replace);

                    continue;
                }
            }

            //見つからなければ新たに追加
            replace.Init(item.data);
            replace.amount = 1;
            list.Add(replace);
        }

        return list;
    }
    //public List<List<Definer.Item>> GetEquipments_ByRarity(bool includeEquipped = false)
    //{

    //}

    public int GetExp() { return expOrbs; }
}
