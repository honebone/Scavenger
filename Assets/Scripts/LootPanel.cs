using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class LootPanel : MonoBehaviour
{
    [SerializeField] List<float> MaterialWeightModRatio;
    [SerializeField] List<float> materialRarityWeight;

    [SerializeField]//test
    List<Definer.Item> loots = new List<Definer.Item>();
    int expOrbs;
    int coins;
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

    public static LootPanel inst;
   
    [System.Serializable]
    public class LootStatus
    {
        [Header("\n\n確定で落とす個数の範囲")]
        public Vector2Int dropEquipmentsRange;
        public int guarantee_rare;
        public int guarantee_epic;
        public int guarantee_legendary;

        public Vector2Int coin;
    }
    private void Awake()
    {
        inst = this;
    }
    private void Start()
    {
        infoText = FindObjectOfType<InfoText>();
        inventory = FindObjectOfType<Inventory>();
        expeditionManager = FindObjectOfType<ExpeditionManager>();
        soundManager = FindObjectOfType<SoundManager>();
        tutorialManager = FindObjectOfType<TutorialManager>();

        //List<float> list = new List<float>();
        //for (int i = 0; i < 31; i++)
        //{
        //    string s=i.ToString()+"：";
        //    list = GetMaterialRarityWeight(i);
        //    foreach (float f in list)
        //    {
        //        s += $",{((f / list.Sum()) * 100f).ToString(".00")}";
        //    }
        //    s += "\n";
        //    Debug.Log(s);
        //}
    }

    public void ToggleLootPanel()
    {
        if (lootPanel.activeSelf) { CloseLootPanel(); }
        else { Loot(); }
    }
    /// <summary>ルートを開始する </summary>
    public void Loot()
    {
        SetButtons();
        loots.ForEach(x =>
        {
            if (x.data && x.data.itemType == ItemData.ItemType.equipment) Compendium.inst.UnllickEq(x.data);
        });
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
        if (coins > 0)
        {
            var eb = Instantiate(itemButton, content);
            eb.GetComponent<LootButton>().Init_AsCoin(coins, infoText, soundManager);
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
                soundManager.PlaySE(Definer.soundRef.expOrb);
            }
            else if (lootButton.GetIfCoin())
            {
                soundManager.PlaySE(Definer.soundRef.coin);
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
        tutorialManager.SetTutorial(tutorial_loot);
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
        int equipments = Random.Range(status.dropEquipmentsRange.x, status.dropEquipmentsRange.y + 1);//装備品ドロップ
        for (int i = 0; i < equipments; i++)
        {
            AddItem(expeditionManager.GetRandomEquipment(), 1);
        }

        for(int i = 0; i < status.guarantee_rare;i++)
        {
            AddItem(expeditionManager.GetRandomEquipment_WithRarity(ItemData.Rarity.rare), 1);
        }
        for (int i = 0; i < status.guarantee_epic; i++)
        {
            AddItem(expeditionManager.GetRandomEquipment_WithRarity(ItemData.Rarity.epic), 1);
        }
        for (int i = 0; i < status.guarantee_legendary; i++)
        {
            AddItem(expeditionManager.GetRandomEquipment_WithRarity(ItemData.Rarity.legendary), 1);
        }
        if (status.coin.y > 0) { AddCoin(status.coin.Range()); }
    }
    public void AddItem(Definer.Item item, int amount=1)
    {
        if (item.data.itemType == ItemData.ItemType.equipment)
        {
            item.amount = amount;
            loots.Add(item);
        }
        
        //Sort();
    }
    public void AddItem(ItemData itemData, int amount = 1)
    {
        if (itemData.itemType == ItemData.ItemType.equipment)
        {
            Definer.Item item = new Definer.Item();
            item.Init(itemData);
            item.amount = amount;
            loots.Add(item);
        }
        //Sort();
    }

    public void AddItem(List<ItemData> items)
    {
        items.ForEach(item => { AddItem(item); });
    }
    /// <summary>
    /// エリアごとの量を自動計算
    /// </summary>
    /// <param name="amount"></param>
    public void AddExp(float amount)
    {
        expOrbs += expeditionManager.GetExpAmount(amount);
    }
    public void RemoveExp()
    {
        expOrbs = 0;
    }
    public void AddCoin(int amount)
    {
        coins += amount;
    }
    public void RemoveCoin()
    {
        coins = 0;
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
            SoundManager.instance.PlaySE_Select();
            foreach (Definer.Item item in loots)
            {
                inventory.AddItem(item, item.amount, true);
            }
            if(expOrbs > 0)
            {
                inventory.AddExp(expOrbs, true);
            }
            if (coins > 0)
            {
                inventory.AddCoin(coins, true);
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
        coins = 0;
        SetButtons();
    }

    public bool CheckHasLoot()
    {
        return loots.Count > 0 || expOrbs > 0 || coins > 0;
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

    public List<float> GetMaterialRarityWeight(float exMod)
    {
        List<float> weight = new List<float>(materialRarityWeight);
        float mod = expeditionManager.GetPartyStatus().materialWeightMod + exMod;
        for (int i = 0; i < materialRarityWeight.Count; i++)
        {
            weight[i] += MaterialWeightModRatio[i] * mod;
        }
        return weight;
    }
}
