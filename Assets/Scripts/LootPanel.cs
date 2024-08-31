using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootPanel : MonoBehaviour
{
    [SerializeField]//test
    List<Definer.Item> loots = new List<Definer.Item>();
    int expOrbs;
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
    [SerializeField] AudioClip SE_Exp;

    [SerializeField] TutorialData tutorial_loot;

    bool revealing;

    InfoText infoText;
    Inventory inventory;
    ExpeditionManager expeditionManager;
    SoundManager soundManager;
    TutorialManager tutorialManager;
    //[System.Serializable]
    //public class DropItem
    //{
    //    //public GameObject itemData;
    //    public ItemData dropItem;
    //    public int quantity;

    //    //public ItemData GetItemData() { return itemData.GetComponent<ItemObject>().GetItemData(); }
    //}
   
    [System.Serializable]
    public class LootStatus
    {
        [Header("x:min y:max")]
        public Vector2Int drawAttemptsRange;
        //public List<float> rarityWeightMod;

        public List<ItemData> uniqueDropPool;
        public List<IncludeTag> includeTags;
        public List<ItemData.MaterialTag> excludeTags;

        [Header("\n\n確定で落とす個数の範囲")]
        public Vector2Int dropEquipmentsRange;

        [System.Serializable]
        public class IncludeTag
        {
            public ItemData.MaterialTag tag;
            public int weight;
        }

       
    }
    private void Start()
    {
        infoText = FindObjectOfType<InfoText>();
        inventory = FindObjectOfType<Inventory>();
        expeditionManager = FindObjectOfType<ExpeditionManager>();
        soundManager = FindObjectOfType<SoundManager>();
        tutorialManager = FindObjectOfType<TutorialManager>();
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
        if (expOrbs > 0)
        {
            var eb = Instantiate(itemButton, content);
            eb.GetComponent<LootButton>().Init_AsExp(expOrbs, infoText, soundManager);
        }
    }
    IEnumerator Reveal()
    {
        yield return new WaitForSeconds(0.5f);
        foreach (LootButton lootButton in content.GetComponentsInChildren<LootButton>())
        {
            yield return new WaitForSeconds(0.25f);
            lootButton.Reveal();

            if (lootButton.GetIfExp())
            {
                soundManager.PlaySE(SE_Exp);
            }
            else
            {
                Definer.Item item = lootButton.GetItem();
                soundManager.PlaySE(Definer.soundRef.getItem[(int)item.data.rarity]);
                if (item.data.itemType == ItemData.ItemType.equipment) { soundManager.PlaySE(SE_Equipment); }
                if (item.data.rarity == ItemData.Rarity.epic) { yield return new WaitForSeconds(0.5f); }
                else if (item.data.rarity == ItemData.Rarity.legendary) { yield return new WaitForSeconds(1f); }
            }
           
        }
        revealing = false;
        tutorialManager.StartTutorial(tutorial_loot);
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
    public void DropItem_Loot(LootStatus status)
    {
        if (status.uniqueDropPool.Count > 0 || status.includeTags.Count > 0)
        {
            List<ItemData> lootDataBase = Definer.lootDataBase;
            //試行回数の決定
            int attempts = Random.Range(status.drawAttemptsRange.x, status.drawAttemptsRange.y + 1);

            for (int i = 0; i < attempts; i++)
            {
                List<ItemData> pool = new List<ItemData>(status.uniqueDropPool);

                if (status.includeTags.Count > 0)
                {
                    //ドロップするタグの決定
                    List<float> tagWeight = new List<float>();
                    foreach (LootStatus.IncludeTag include in status.includeTags)
                    {
                        tagWeight.Add(include.weight);
                    }

                    ItemData.MaterialTag tag = status.includeTags[tagWeight.ChoiceWithWeight()].tag;

                    //プールからexcludeを除外
                    foreach (ItemData dataBase in lootDataBase)
                    {
                        if (dataBase.materialTags.Contains(tag)) { pool.Add(dataBase); }//データベースから今回ドロップするタグを持つもののみを抽出
                    }

                    foreach (ItemData item in pool)
                    {
                        foreach (ItemData.MaterialTag materialTag in item.materialTags)
                        {
                            if (status.excludeTags.Contains(materialTag))//プールに除外するタグを含むアイテムがあるならそれを除外
                            {
                                pool.Remove(item);
                                break;
                            }
                        }
                    }
                }

                if (pool.Count > 0)
                {
                    //レアリティの決定
                    ItemData.Rarity rarity = (ItemData.Rarity)expeditionManager.GetPartyStatus().materialDropChance.ChoiceWithWeight();

                    //ドロップアイテムの決定
                    ItemData drop = pool[0];
                    bool found = false;
                    pool = pool.Shuffle();

                    for (int j = (int)rarity; j >= 0; j--)//決定されたレアリティのアイテムがpoolになかったら、レアリティを1つ下げる
                    {
                        foreach (ItemData item in pool)//シャッフルされたpoolの手前から見て、決定されたレアリティと同じものを見つける
                        {
                            if (item.rarity == (ItemData.Rarity)j)
                            {
                                drop = item;
                                found = true;
                                break;
                            }
                        }
                        if (found) { break; }
                    }

                    if (found)
                    {
                        //ドロップ量の決定
                        Definer.Item item = new Definer.Item();
                        item.Init(drop);
                        AddItem(item, Random.Range(drop.dropAmountRange.x, drop.dropAmountRange.y + 1));
                    }
                }
            }

            attempts = Random.Range(status.dropEquipmentsRange.x, status.dropEquipmentsRange.y + 1);//装備品ドロップ
            for (int i = 0; i < attempts; i++)
            {
                AddItem(expeditionManager.GetRandomEquipment(), 1);
            }
        }
    }
    public void AddItem(Definer.Item item, int amount)
    {
        item.amount = amount;
        loots.Add(item);
        //Sort();
    }
    public void AddExp(int amount)
    {
        expOrbs += amount * expeditionManager.GetExpAmount();
    }
    public void RemoveExp()
    {
        expOrbs = 0;
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
            if(expOrbs > 0)
            {
                inventory.AddExp(expOrbs, true);
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
        expOrbs = 0;
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
