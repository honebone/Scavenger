using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField]//test
    List<Definer.Item> inventory=new List<Definer.Item> ();
    [SerializeField]
    GameObject inventoryPanel;
    [SerializeField]
    Transform content;
    [SerializeField]
    Transform optionUIPanel;

    [SerializeField]
    GameObject inventoryButton;
    [SerializeField]
    GameObject optionPanel_normal;



    InfoText infoText;
    private void Start()
    {
        infoText = FindObjectOfType<InfoText>();
    }
   
    public void ToggleInventory()
    {
        if (inventoryPanel.activeSelf) { CloseInventory(); }
        else { OpenInventory(); }
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
        foreach(Definer.Item item in inventory)
        {
            int a = item.amount;
            while(a > 0)
            {
                Definer.Item i = new Definer.Item();
                i.Init(item.itemData);
                i.amount = Mathf.Min(i.amountPerStack, a);

                var ib = Instantiate(inventoryButton, content);                
                ib.GetComponent<InventoryButton>().Init(i, infoText);

                a -= item.amountPerStack;
            }
        }
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

    public void CloseOptionUI()
    {
        if (optionUIPanel.childCount != 0)
        {
            for (int i = 0; i < optionUIPanel.childCount; i++)
            {
                Destroy(optionUIPanel.GetChild(i).gameObject);
            }
        }
        //lootや納品等のoptionUIも閉じる
    }

    public void AddItem(Definer.Item item,int amount)
    {
        Definer.Item replace = new Definer.Item();

        infoText.AddLogText(string.Format("●{0}x{1}を入手", item.itemName.ColorStr(item.rarity.ToColor()), amount.ToString()));
        for (int i=0;i<inventory.Count;i++)
        {
            if (inventory[i].itemData == item.itemData)
            {
                replace = inventory[i];
                replace.amount += amount;
                inventory.RemoveAt(i);
                inventory.Add(replace);

                SortInventory();
                return;
            }
        }
        replace.Init(item.itemData);
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
            if (inventory[i].itemData == remove.itemData)
            {
               
                infoText.AddLogText(string.Format("○{0}x{1}を失った", remove.itemName.ColorStr(remove.rarity.ToColor()), amount.ToString()));
                
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

    void SortInventory()
    {
        inventory.Sort((a, b) => (int)b.rarity - (int)a.rarity);
        SetButtons();
    }

    public int GetItemAmount(ItemData itemData)
    {
        foreach (Definer.Item item in inventory)
        {
            if (item.itemData == itemData) { return item.amount; }
        }
        return 0;
    }
}
