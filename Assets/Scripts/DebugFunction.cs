using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugFunction : MonoBehaviour
{
    [SerializeField] bool debug;
    [SerializeField] bool skipDeployPhase;

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

    [SerializeField] AreaData areaData;
 
    [SerializeField]
    GameObject RE;
    [SerializeField]
    Transform REP;

    [SerializeField] TutorialData tutorial;

    [SerializeField] AreaData tutorialArea;


    [SerializeField]
    GameObject personality;

    [SerializeField]
    GameObject debugPanel;

    CharactersManager charactersManager;
    ExpeditionManager expeditionManager;
    BattleManager battleManager;
    Inventory inventory;
    InfoText infoText;

    private void Start()
    {
        charactersManager = FindObjectOfType<CharactersManager>();
        expeditionManager = FindObjectOfType<ExpeditionManager>();
        battleManager = FindObjectOfType<BattleManager>();
        inventory = FindObjectOfType<Inventory>();
        infoText = FindObjectOfType<InfoText>();
        items = new Definer.Item[itemData.Length];
        for (int i = 0; i < itemData.Length; i++)
        {
            items[i].Init(itemData[i]);
            items[i].amount = amount[i];
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (debug)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                //FindObjectOfType<CharactersManager>().SpawnPlayer(characterData[4], 6);
                FindObjectOfType<CharactersManager>().SpawnPlayer(characterData[1], 4);
                FindObjectOfType<CharactersManager>().SpawnPlayer(characterData[2], 8);
                FindObjectOfType<CharactersManager>().SpawnPlayer(characterData[3], 0);
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                FindObjectOfType<ExpeditionManager>().Battle(enemySetTest, FE, new ExpeditionManager.BattleParams());
            }

            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                if (expeditionManager.GetAreaManager() == null) { expeditionManager.StartArea(areaData); }
                else { expeditionManager.GetAreaManager().GenerateMap(); }
            }
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
                FindObjectOfType<LVLUpManager>().LVLUp(charactersManager.GetExistingCharacters_All()[0]);
            }
            if (Input.GetKeyDown(KeyCode.Alpha7))
            {
                expeditionManager.EnemyLVLUp();
            }
            if (Input.GetKeyDown(KeyCode.Space)) { debugPanel.SetActive(!debugPanel.activeSelf); }
        }
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
    public void SpawnDebugger()
    {
        FindObjectOfType<CharactersManager>().SpawnPlayer(characterData[0], 7);
    }

    public void StartTutorial()
    {
        FindObjectOfType<CharactersManager>().SpawnPlayer(characterData[2], 8);
        FindObjectOfType<CharactersManager>().SpawnPlayer(characterData[1], 4);

        if (expeditionManager.GetAreaManager() == null) { expeditionManager.StartArea(tutorialArea); }
        else { expeditionManager.GetAreaManager().GenerateMap(); }
    }
    public bool CheckDebugMode() { return debug; }

    public bool CheckSkipDeploy() { return skipDeployPhase; }
    public void UnlockEqSlot()
    {
        foreach (Character character in charactersManager.GetExistingCharacters_All())
        {
            character.Debug_UnlockEqSlots();
        }
    }
    public void UnlockAbility_All()
    {
        foreach(Character character in charactersManager.GetExistingCharacters_All())
        {
            character.UnlockAbility_All();
        }
    }
    public void UpgradeAbility_All()
    {
        foreach (Character character in charactersManager.GetExistingCharacters_All())
        {
            character.UpgradeAbility_All();
        }
    }

}
