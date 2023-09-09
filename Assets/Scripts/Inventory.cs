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
    GameObject inventoryButton;

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
            var i = Instantiate(inventoryButton, content);
            i.GetComponent<InventoryButton>().Init(item,infoText);
        }
    }


    public void AddItem(Definer.Item item)
    {
        infoText.AddLogText(string.Format("●{0}x{1}を入手", item.itemName.ColorStr(item.rarity.ToColor()), item.amount.ToString()));
        for (int i=0;i<inventory.Count;i++)
        {
            if (inventory[i].itemData == item.itemData)
            {
                Definer.Item replace = inventory[i];
                replace.amount += item.amount;
                inventory.RemoveAt(i);
                inventory.Add(replace);
                return;
            }
        }
        inventory.Add(item);
    }
    public void RemoveItem(Definer.Item remove,bool note)
    {
        if (inventory.Count == 0)
        {
            infoText.AddErrorText("そもそも何も持ってません");
        }
        if (remove.amount < 0)
        {
            infoText.AddErrorText("減らす数が負の値です");
            return;
        }

        for (int i = inventory.Count - 1; i >= 0; i--)
        {
            if (inventory[i].itemData == remove.itemData)
            {
                if (note)
                {
                    infoText.AddLogText(string.Format("○{0}x{1}を失った", remove.itemName.ColorStr(remove.rarity.ToColor()), remove.amount.ToString()));
                }
                if (inventory[i].amount == remove.amount) { inventory.RemoveAt(i); }//減らす量がスロットのamountと同じ
                else if (inventory[i].amount < remove.amount)//減らす量がamountより多い
                {
                    infoText.AddErrorText("現在所持している数よりも多くのアイテムを減らそうとしています");
                }
                else////減らす量がスロットのamountより小さい
                {
                    Definer.Item replace = inventory[i];
                    replace.amount -= remove.amount;
                    inventory.RemoveAt(i);
                    inventory.Add(replace);
                }
                return;
            }
        }

        infoText.AddErrorText("持っていないアイテムの数を減らそうとしてます");        
    }
}
