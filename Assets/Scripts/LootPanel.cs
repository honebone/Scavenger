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

    [SerializeField]
    AudioClip SE_Equipment;

    bool revealing;

    InfoText infoText;
    Inventory inventory;
    ExpeditionManager expeditionManager;
    SoundManager soundManager;
    [System.Serializable]
    public class DropItem
    {
        //public GameObject itemData;
        public ItemData dropItem;
        public int quantity;

        //public ItemData GetItemData() { return itemData.GetComponent<ItemObject>().GetItemData(); }
    }
    [System.Serializable]
    public class DropItemFromPreset
    {
        public LootPresetData preset;
        public Vector2Int attemptsRange;
    }
    [System.Serializable]
    public class LootStatus
    {
        [Header("x:min y:max")]
        public Vector2Int drawAttemptsRange;

        public List<DropItem> dropItems;
        public List<DropItemFromPreset> dropItemFromPresets;
        [Header("確定で落とす個数の範囲")]
        public Vector2Int dropEquipmentsRange;
    }
    private void Start()
    {
        infoText = FindObjectOfType<InfoText>();
        inventory = FindObjectOfType<Inventory>();
        expeditionManager = FindObjectOfType<ExpeditionManager>();
        soundManager = FindObjectOfType<SoundManager>();
    }

    public void ToggleLootPanel()
    {
        if (lootPanel.activeSelf) { CloseLootPanel(); }
        else { Loot(); }
    }
    public void Loot()
    {
        SetButtons();
        if (!lootPanel.activeSelf)
        {
            lootPanel.SetActive(true);
            revealing = true;
            StartCoroutine(Reveal());
        }
        //inventory.OpenInventory();
    }
    public void CloseLootPanel()
    {
        if (lootPanel.activeSelf) { lootPanel.SetActive(false); }
        //inventory.CloseInventory();
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
                ib.GetComponent<LootButton>().Init(i, infoText,soundManager);

                a -= item.data.amountPerStack;
            }
        }
    }
    IEnumerator Reveal()
    {
        yield return new WaitForSeconds(0.5f);
        foreach (LootButton lootButton in content.GetComponentsInChildren<LootButton>())
        {
            yield return new WaitForSeconds(0.25f);
            lootButton.Reveal();

            Definer.Item item = lootButton.GetItem();
            soundManager.PlaySE(Definer.soundRef.getItem[(int)item.data.rarity]);
            if (item.data.itemType == ItemData.ItemType.equipment) { soundManager.PlaySE(SE_Equipment); }
            if (item.data.rarity == ItemData.Rarity.epic) { yield return new WaitForSeconds(0.5f); }
            else if (item.data.rarity == ItemData.Rarity.legendary) { yield return new WaitForSeconds(1f); }
        }
        revealing = false;
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

    /// <summary>キャラ死亡時のドロップ処理</summary>
    public void DropItem_Enemy(DropItem dropItem)
    {
        float[] dropRate = new float[] { 60, 30, 10, 5, 1 };//test
        int dropQuantity = 0;
        for (int i = 0; i < dropItem.quantity; i++)
        {
            if (dropRate[(int)dropItem.dropItem.rarity].Probability())
            {
                dropQuantity++;
            }
        }

        if (dropQuantity > 0)
        {
            Definer.Item item = new Definer.Item();
            item.Init(dropItem.dropItem);
            AddItem(item, dropQuantity);
        }
    }
    public void DropItem_Loot(LootStatus status)
    {
        //Attempts回だけdropItemsからアイテムを1つ選び、そのアイテムのレアリティに応じた確率でルートに追加
        int attempts = Random.Range(status.drawAttemptsRange.x, status.drawAttemptsRange.y + 1);
        //float[] materialDropChance = expeditionManager.GetPartyStatus().materialDropChance;

        for (int i = 0; i < attempts; i++)
        {
            DropItem_Enemy(status.dropItems.Choice());
        }

        foreach(DropItemFromPreset preset in status.dropItemFromPresets)
        {
            attempts= Random.Range(preset.attemptsRange.x, preset.attemptsRange.y + 1);
            for (int i = 0; i < attempts; i++)
            {
                DropItem_Enemy(preset.preset.lootItems.Choice());
            }
        }
        attempts = Random.Range(status.dropEquipmentsRange.x, status.dropEquipmentsRange.y + 1);
        for (int i = 0; i < attempts; i++)
        {
            AddItem(expeditionManager.GetRandomEquipment(), 1);
        }
    }
    public void AddItem(Definer.Item item, int amount)
    {
        item.amount = amount;
        loots.Add(item);
        //Sort();
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
            if (loots[i].data == remove.data && loots[i].amount == remove.amount)
            {

                //if (loots[i].amount == amount) { loots.RemoveAt(i); }//減らす量がスロットのamountと同じ
                //else if (loots[i].amount < amount)//減らす量がamountより多い
                //{
                //    infoText.AddErrorText("現在所持している数よりも多くのアイテムを減らそうとしています");
                //}
                //else//減らす量がスロットのamountより小さい
                //{
                //    Definer.Item replace = loots[i];
                //    replace.amount -= amount;
                //    loots.RemoveAt(i);
                //    loots.Add(replace);
                //}

                loots.RemoveAt(i);

                Sort();
                return;
            }
        }

        infoText.AddErrorText("持っていないアイテムの数を減らそうとしてます");
    }
    public void ObtainAll()
    {
        if (!revealing)
        {
            foreach (Definer.Item item in loots)
            {
                inventory.AddItem(item, item.amount, true);
            }
            EndLoot();
        }
    }
    public void EndLoot()
    {
        if (!revealing)
        {
            CloseLootPanel();
            ResetLoots();
        }
    }
    public void ResetLoots()
    {
        loots = new List<Definer.Item>();
        SetButtons();
    }

    void Sort()
    {
        //loots.Sort((a, b) => (int)b.data.rarity - (int)a.data.rarity);
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
