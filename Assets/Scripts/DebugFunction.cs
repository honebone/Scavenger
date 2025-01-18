using System.Collections;
using System.Collections.Generic;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine;

public class DebugFunction : MonoBehaviour
{
    [SerializeField] bool debug;
    [SerializeField] bool skipDeployPhase;
    [SerializeField] bool skipTutorial;
    public bool battleDebug;

    [SerializeField] Volume volume;
    [SerializeField] Vector2 aberrationValueRange;
    [SerializeField] Vector2 aberrationDurationRange;

    bool glitch;
    Vignette vig;
    ChromaticAberration aberration;
    BagPostProcessVolume bag;

    [SerializeField]
    CharacterData[] characterData;
    [SerializeField] AreaManager.EnemySet players;

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

    List<Definer.Item> equTest;


    CharactersManager charactersManager;
    ExpeditionManager expeditionManager;
    BattleManager battleManager;
    Inventory inventory;
    InfoText infoText;

    [SerializeField] List<Vector2Int> rel;

    public static DebugFunction instance;

    private void Start()
    {
        instance = this;
        charactersManager = FindObjectOfType<CharactersManager>();
        expeditionManager = FindObjectOfType<ExpeditionManager>();
        battleManager = FindObjectOfType<BattleManager>();
        inventory = FindObjectOfType<Inventory>();
        infoText = FindObjectOfType<InfoText>();
        items = new Definer.Item[itemData.Length];

        if (debug) { FindObjectOfType<GameManager>().SetTutorialMode(!skipTutorial); }

        for (int i = 0; i < itemData.Length; i++)
        {
            items[i].Init(itemData[i]);
            items[i].amount = amount[i];
        }

        //for (int i = 0; i < 6; i++)
        //{
        //    for (int j = 0; j < 3; j++)
        //    {
        //        int pos = new Vector2Int(i, j).ToPosInt();
        //        string s = $"{pos}:";
        //        foreach(int neighbor in pos.RelativePosToAbsolute(rel,true)) { s += $"{neighbor},"; }
        //        Debug.Log(s);
        //    }
        //}

        
    }
    // Update is called once per frame
    void Update()
    {
        if (debug)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                //FindObjectOfType<CharactersManager>().SpawnPlayer(characterData[4], 6);
                //FindObjectOfType<CharactersManager>().SpawnPlayer(characterData[1], 4);
                //FindObjectOfType<CharactersManager>().SpawnPlayer(characterData[2], 8);
                //FindObjectOfType<CharactersManager>().SpawnPlayer(characterData[3], 0);

                List<CharacterData> playerList = players.GetEnemies();
                for (int i = 0; i < 9; i++)
                {
                    if (playerList[i] != null) { charactersManager.SpawnPlayer(playerList[i], i); }
                }
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
                foreach (ItemData eqData in FindObjectOfType<Definer>().GetAllEquipments())
                {
                    Definer.Item eq = new Definer.Item();
                    eq.Init(eqData);
                    SupplyManager.inst.AddItem(eq, 1);
                }

            }

            if (Input.GetKeyDown(KeyCode.Alpha6))
            {
                equTest = Inventory.inst.GetEquipments_WithEquipped();
            }
            if (Input.GetKeyDown(KeyCode.Alpha7))
            {
                expeditionManager.EnemyLVLUp();
            }
            if (Input.GetKeyDown(KeyCode.Space)) { debugPanel.SetActive(!debugPanel.activeSelf); }
        }
    }

    IEnumerator TestC()
    {
        vig.active = true;
        vig.intensity.value = 0.1f;
        for (int i = 0; i < 20; i++)
        {
            vig.intensity.value += 0.02f;
            yield return new WaitForSeconds(0.05f);
        }

        for (int i = 0; i < 20; i++)
        {
            vig.intensity.value -= 0.02f;
            yield return new WaitForSeconds(0.05f);
        }
        yield return new WaitForSeconds(0.5f);
        if (glitch) StartCoroutine(TestC());
    }

    IEnumerator TestC2()
    {
        aberration.active = true;
        aberration.intensity.value = Random.Range(aberrationValueRange.x, aberrationValueRange.y);
        yield return new WaitForSeconds(Random.Range(aberrationDurationRange.x, aberrationDurationRange.y));
        if (glitch) StartCoroutine(TestC2());
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
    public bool CheckSkipTutorial() { return skipTutorial; }

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

    public void ToggleGlitch()
    {
        glitch = !glitch;
        if (glitch)
        {
            if (volume.profile.TryGet<Vignette>(out vig)) StartCoroutine(TestC());
            if (volume.profile.TryGet<ChromaticAberration>(out aberration)) StartCoroutine(TestC2());
            if (volume.profile.TryGet<BagPostProcessVolume>(out bag)) bag.active = true;
        }
        else
        {
            vig.active = false;
            aberration.active = false;
            bag.active = false;
        }
    }

}
