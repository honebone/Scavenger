using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugFunction : MonoBehaviour
{

    [SerializeField]
    CharacterData[] characterData;

    [SerializeField]
    ItemData[] itemData;
    [SerializeField]
    int[] amount;
    Definer.Item[] items;
    [SerializeField]
    ItemData Eq;
    [SerializeField]
    AreaManager.EnemySet enemySetTest;
    [SerializeField]
    GameObject FE;

    [SerializeField]
    GameObject RE;
    [SerializeField]
    Transform REP;

    [SerializeField]
    GameObject personality;

    [SerializeField]
    GameObject debugPanel;

    CharactersManager charactersManager;
    ExpeditionManager expeditionManager;
    BattleManager battleManager;
    Inventory inventory;

    private void Start()
    {
        charactersManager = FindObjectOfType<CharactersManager>();
        expeditionManager = FindObjectOfType<ExpeditionManager>();
        battleManager = FindObjectOfType<BattleManager>();
        inventory = FindObjectOfType<Inventory>();
        items = new Definer.Item[itemData.Length];
        for (int i = 0; i < itemData.Length; i++)
        {
            items[i].Init(itemData[i]);
            items[i].amount = amount[i];
        }
        //for(int i = 0; i < 18; i++)
        //{
        //    print(string.Format("{0}:{1}",i,i.GetCurrentColumn()));
        //}

    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            //FindObjectOfType<CharactersManager>().SpawnPlayer(characterData[0], 7);
            FindObjectOfType<CharactersManager>().SpawnPlayer(characterData[4], 6);
            FindObjectOfType<CharactersManager>().SpawnPlayer(characterData[1], 4);
            FindObjectOfType<CharactersManager>().SpawnPlayer(characterData[2], 8);
            FindObjectOfType<CharactersManager>().SpawnPlayer(characterData[3], 0);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            FindObjectOfType<ExpeditionManager>().Battle(enemySetTest, FE);
        }

        if (Input.GetKeyDown(KeyCode.Alpha3)) { FindObjectOfType<AreaManager>().GenerateMap(); }
        if (Input.GetKeyDown(KeyCode.Alpha4)) { FindObjectOfType<ExpeditionManager>().SelectNextRoom(); }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            foreach (Definer.Item item in items)
            {
                FindObjectOfType<Inventory>().AddItem(item, item.amount, true);
            }

        }
        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            List<Definer.Item> equipments = new List<Definer.Item>();
            int[] rarityCount = new int[5];
            for (int i = 0; i < 100; i++)
            {
                equipments.Add(expeditionManager.GetRandomEquipment());
                rarityCount[(int)equipments[i].data.rarity]++;
                print(equipments[i].data.itemName);
            }
            foreach (int j in rarityCount) { print(j); }
        }
        if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            //expeditionManager.SetPersonality_ToRandom(personality);
            FindObjectOfType<GuideMessage>().SetWaringText("ƒeƒXƒg");
        }
        if (Input.GetKeyDown(KeyCode.Space)) { debugPanel.SetActive(!debugPanel.activeSelf); }
    }

    public void GainExp()
    {
        FindObjectOfType<Inventory>().AddExp(99, true);
    }
    public void GetAllEquipments()
    {
        foreach (ItemData eqData in FindObjectOfType<Definer>().GetAllEquipments())
        {
            Definer.Item eq = new Definer.Item();
            eq.Init(eqData);
            inventory.AddItem(eq, 1, true);
        }
    }
}
