using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SupplyManager : MonoBehaviour
{
    [SerializeField]//test
    List<Definer.Item> supplies = new List<Definer.Item>();
    [SerializeField]
    GameObject supplyPanel;
    [SerializeField]
    Transform content;
    //[SerializeField]
    //Transform optionUIPanel;

    [SerializeField]
    GameObject itemButton;
    //[SerializeField]
    //GameObject optionPanel;

    [SerializeField]
    AudioClip SE_Equipment;

    bool revealing;

    InfoText infoText;
    Inventory inventory;
    ExpeditionManager expeditionManager;
    ExpeditionManager.PartyStatus partyStatus;
    SoundManager soundManager;
  
   
    
    private void Start()
    {
        infoText = FindObjectOfType<InfoText>();
        inventory = FindObjectOfType<Inventory>();
        expeditionManager = FindObjectOfType<ExpeditionManager>();
        partyStatus = expeditionManager.GetPartyStatus();
        soundManager = FindObjectOfType<SoundManager>();
    }

    public void StartSupply()
    {
        SetButtons();
        if (!supplyPanel.activeSelf)
        {
            supplyPanel.SetActive(true);
            revealing = true;
            StartCoroutine(Reveal());
        }
        //inventory.OpenInventory();
    }
    public void CloseSupplyPanel()
    {
        if (supplyPanel.activeSelf) { supplyPanel.SetActive(false); }
        //inventory.CloseInventory();
        expeditionManager.OnEndSupply();
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
        foreach (Definer.Item item in supplies)
        {
            int a = item.amount;
            while (a > 0)
            {
                Definer.Item i = new Definer.Item();
                i.Init(item.data);
                i.amount = Mathf.Min(i.data.amountPerStack, a);

                var ib = Instantiate(itemButton, content);
                ib.GetComponent<SupplyButton>().Init(i, infoText, this);

                a -= item.data.amountPerStack;
            }
        }
    }
    IEnumerator Reveal()
    {
        yield return new WaitForSeconds(0.5f);
        foreach (SupplyButton supplyButton in content.GetComponentsInChildren<SupplyButton>())
        {
            yield return new WaitForSeconds(0.25f);
            supplyButton.Reveal();

            Definer.Item item = supplyButton.GetItem();
            soundManager.PlaySE(Definer.soundRef.getItem[(int)item.data.rarity]);
            if (item.data.itemType == ItemData.ItemType.equipment) { soundManager.PlaySE(SE_Equipment); }
            if (item.data.rarity == ItemData.Rarity.epic) { yield return new WaitForSeconds(0.5f); }
            else if (item.data.rarity == ItemData.Rarity.legendary) { yield return new WaitForSeconds(1f); }
        }
        revealing = false;
    }

    //public void CreateOptionUI_Normal(Vector3 pos, Definer.Item item)
    //{
    //    CloseOptionUI();
    //    pos.x += 135 * (1f * Screen.width / 1600);
    //    pos.y -= 70 * (1f * Screen.height / 900);
    //    var o = Instantiate(optionPanel, optionUIPanel);
    //    o.transform.position = pos;
    //    o.GetComponent<ItemOptionButton>().Init(item);
    //}
    public void SelectItem(Definer.Item item)
    {
        inventory.AddItem(item, item.amount, true);
        soundManager.PlaySE(SE_Equipment);
        //繰り返すか判定
        EndSupply();
    }

    //public void CloseOptionUI()
    //{
    //    if (optionUIPanel.childCount != 0)
    //    {
    //        for (int i = 0; i < optionUIPanel.childCount; i++)
    //        {
    //            Destroy(optionUIPanel.GetChild(i).gameObject);
    //        }
    //    }
    //    //inventoryや納品等のoptionUIも閉じる
    //}

  public void SetSupply_Eq(int choices)
    {
        for(int i=0;i<choices; i++)
        {
            AddItem(expeditionManager.GetRandomEquipment(), 1);
        }
    }
    public void AddItem(Definer.Item item, int amount)
    {
        item.amount = amount;
        supplies.Add(item);
    }
    public void RemoveItem(Definer.Item remove, int amount)
    {
        if (supplies.Count == 0)
        {
            infoText.AddErrorText("そもそも何も持ってません");
        }
        if (amount < 0)
        {
            infoText.AddErrorText("減らす数が負の値です");
            return;
        }

        for (int i = supplies.Count - 1; i >= 0; i--)
        {
            if (supplies[i].data == remove.data && supplies[i].amount == remove.amount)
            {
                supplies.RemoveAt(i);

                Sort();
                return;
            }
        }

        infoText.AddErrorText("持っていないアイテムの数を減らそうとしてます");
    }
   
    public void EndSupply()
    {
        if (!revealing)
        {
            CloseSupplyPanel();
            ResetSupplies();
        }
    }
    public void ResetSupplies()
    {
        supplies = new List<Definer.Item>();
        SetButtons();
    }

    void Sort()
    {
        //loots.Sort((a, b) => (int)b.data.rarity - (int)a.data.rarity);
        SetButtons();
    }

    //public int GetItemAmount(ItemData itemData)
    //{
    //    foreach (Definer.Item item in supplies)
    //    {
    //        if (item.data == itemData) { return item.amount; }
    //    }
    //    return 0;
    //}
}
