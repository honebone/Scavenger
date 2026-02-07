using System.Collections;
using System.Collections.Generic;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine;
using System.Linq;

public class DebugFunction : MonoBehaviour
{
    [SerializeField] bool debug;
    [SerializeField] bool skipDeployPhase;
    [SerializeField] bool skipTutorial;

    public int autoset_playerLVL;
    public int autoset_enemyLVL;

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
    List<ItemData> itemData;
    [SerializeField]
    int[] amount;
    [SerializeField]
    List<ItemData> eq_exclusive;
    [SerializeField]
    AreaManager.EnemySet enemySetTest;
    [SerializeField]
    GameObject FE;

    [SerializeField] AreaData areaData;
 
    [SerializeField]
    GameObject RE;
    [SerializeField]
    Transform REP;
    [SerializeField] ShopParams shopParams;

    [SerializeField] TutorialData tutorial;

    [SerializeField] AreaData tutorialArea;


    [SerializeField]
    GameObject personality;

    [SerializeField]
    GameObject debugPanel;

    public static int int1;



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

        if (debug) { FindObjectOfType<GameManager>().SetTutorialMode(!skipTutorial); }
        
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
                    if (playerList[i] != null) { charactersManager.SpawnPlayer(playerList[i], i, 1); }
                }
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                FindObjectOfType<ExpeditionManager>().Battle(new List<AreaManager.EnemySet> { enemySetTest }, FE, new ExpeditionManager.BattleParams());
            }

            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                if (expeditionManager.GetAreaManager() == null) { expeditionManager.StartExpedition(areaData); }
                else { expeditionManager.GetAreaManager().GenerateMap(); }
            }
            if (Input.GetKeyDown(KeyCode.Alpha4)) { FindObjectOfType<ExpeditionManager>().SelectNextRoom(); }
            if (Input.GetKeyDown(KeyCode.Alpha5))
            {
                expeditionManager.Debug_StartRE(areaData, RE);
            }

            if (Input.GetKeyDown(KeyCode.Alpha6))
            {
                List<GameObject> per = expeditionManager.GetPer_Random(3);
                expeditionManager.SetPersonality(charactersManager.GetExistingCharacters_All()[0], per,true);
                //charactersManager.GetExistingCharacters_All().ForEach(x => expeditionManager.SetRandomPer(x,10));
            }
            if (Input.GetKeyDown(KeyCode.Alpha7))
            {
                Inventory.inst.AddCoin(40);
               Shop.inst.StartShop(shopParams);
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
        FindObjectOfType<Inventory>().AddExp(100, true);
    }
    public void GetCoin()
    {
        inventory.AddCoin(100, true);
    }
    public void GetAllEquipments()
    {
        foreach (ItemData eqData in FindObjectOfType<Definer>().GetAllEquipments())
        {
            Definer.Item eq = new Definer.Item();
            eq.Init(eqData);
            inventory.AddItem(eq, 1, true);
        }
        foreach (ItemData eqData in eq_exclusive)
        {
            Definer.Item eq = new Definer.Item();
            eq.Init(eqData);
            inventory.AddItem(eq, 1, true);
        }
    }
    public void SpawnDebugger()
    {
        FindObjectOfType<CharactersManager>().SpawnPlayer(characterData[0], 7,1);
    }

    public void StartTutorial()
    {
        FindObjectOfType<CharactersManager>().SpawnPlayer(characterData[2], 8, 1);
        FindObjectOfType<CharactersManager>().SpawnPlayer(characterData[1], 4, 1);

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

    public void GetEq()
    {
        itemData.ForEach(i =>
        {
            inventory.AddItem(i);
        });
    }

    public void GetRandomEq()
    {
        inventory.AddItem(expeditionManager.GetRandomEquipment(), 1, true);
    }

    public void AutoSetPartyStat()
    {
        charactersManager.GetExistingCharacters_All().ForEach(chara =>
        {
            chara.LVLUp_Auto(autoset_playerLVL-1);
        });

        GetAllEquipments();

        if (expeditionManager.GetAreaManager() == null) { expeditionManager.StartExpedition(areaData); }
        else { expeditionManager.GetAreaManager().GenerateMap(); }

        for (int i = 0; i < autoset_enemyLVL - 1; i++) { expeditionManager.EnemyLVLUP(); }

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
