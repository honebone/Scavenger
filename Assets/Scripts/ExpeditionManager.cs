using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Rendering.Universal;
using System;
using System.Linq;

public class ExpeditionManager : MonoBehaviour
{
    [System.Serializable]
    public class Room
    {
      public  RoomEventData eventData;
        /// <summary>開始地点の上下2段ずつとかはtrue</summary>
        public bool empty;

        //ここからroomEvent関連
        public string eventName;
        public string eventInfo;
        public GameObject roomEventManager;
        public Sprite eventIcon;
        //ここまでroomEvent関連

        public bool blind;
        /// <summary>-1:false(locked) 0:false 1:true 2:true(locked)</summary>
        public int up;
        /// <summary>-1:false(locked) 0:false 1:true 2:true(locked)</summary>
        public int straight;
        /// <summary>-1:false(locked) 0:false 1:true 2:true(locked)</summary>
        public int down;

        public Vector2Int roomPos;
        /// <summary>-1:false(locked) 0:false 1:true 2:true(locked)</summary>
        public void SetBranch_OnInit(int u,int s,int d)
        {
            up = u;
            straight = s;
            down = d;
        }
        public void SetRoomEvent(RoomEventData data)
        {
            eventData = data;
            List<GameObject> eventM = new List<GameObject>(data.roomEventManager);
            if (data.debug) { roomEventManager = data.roomEventManager[0]; }
            else
            {
                if (data.randomEvent)
                {
                    //eventM.AddRange(Definer.generalRaEDataBase);

                    //ToDo 固有RaE追加
                    roomEventManager = Definer.RaEDataBase[Definer.inst.cp.RaEWeights.ChoiceWithWeight()].Choice();
                }
               else roomEventManager = eventM[UnityEngine.Random.Range(0, eventM.Count)];
            }

            eventName = roomEventManager.GetComponent<RoomEvent>().OverrideMapName(data.eventName);
            eventInfo = roomEventManager.GetComponent<RoomEvent>().OverrideMapInfo(data.eventInfo);

            eventIcon = data.eventIcon;
        }

        public void SetBranch_Up(int value)
        {
            if (!empty&&(up == 0 || up == 1)) { up = value; }
        }
        public void SetBranch_Down(int value)
        {
            if (!empty&&(down == 0 || down == 1)) { down = value; }
        }
    }
    [System.Serializable]
    public class PartyStatus
    {
        public float materialWeightMod;
        public float[] equipmentDropWeights = new float[] { 50, 35, 10, 4, 1 };
        public int turnOrderReveal = 3;

        //public int dropExpChance = 50;

        /// <summary>roomEvent終了時に特性追加確率</summary>
        public int getPerChance_endRE = 10;

        /// <summary>支給品の選択肢の数</summary>
        public int supplyOptions = 3;

        public int maxMadness = 5;
        public int madness;

        public float startTime;
        public float endTime;
        public int areaCount;
        public Vector2Int currentPos = new Vector2Int();
        public int killCount;
        public List<PersonalBattleReport> totalBattleReports = new List<PersonalBattleReport>();

        public void AddTBR(PersonalBattleReport add)
        {
            totalBattleReports.AddBR(add);
        }
        public void AddKillCount()
        {
            killCount++;
        }
    }
    [SerializeField]
    public PartyStatus partyStatus;

    [SerializeField] List<GameObject> MadnessPAPool;
    //[SerializeField] List<GameObject> RaE_epic;
    //[SerializeField] List<GameObject> RaE_good;
    //[SerializeField] List<GameObject> RaE_normal;
    //[SerializeField] List<GameObject> RaE_bad;

    //public StatusGrowth playerStatusGrowth;
    //public StatusGrowth enemyStatusGrowth;
    //[SerializeField] Character.CharaStatusMod enemyStatusGrowth;
    public int enemyLVL = 1;

    [SerializeField]
    AreaData areaDataForDebug;

    [SerializeField] Transform backgroundP;
    [SerializeField] Light2D globalLight;

    [SerializeField]
    Transform AreaManagerP;
    //[SerializeField]//test
    AreaManager currentAreaManger;
    RoomEvent currentRE;

    [SerializeField]
    GameObject REInfoPanel;
    [SerializeField]
    TextMeshProUGUI RETitle;
    [SerializeField]
    TextMeshProUGUI REInfo;

    [SerializeField]
    GameObject REOptionUI;
    [SerializeField]
    Transform REOptionButtonsP;
    [SerializeField]
    GameObject REOptionButton;

    [SerializeField]
    AudioClip SE_nextRoom;
    [SerializeField] AudioClip jingle_EnemyLVLUp;
    [SerializeField]
    Transform REManagerParent;

    public SpriteRenderer blackBack;


    [SerializeField] GameObject nextRoomButton;

    [SerializeField] TutorialData tutorial_expedition;
    [SerializeField] TutorialData tutorial_exp;
    [SerializeField] TutorialData tutorial_equipment;
    [SerializeField] TutorialData tutorial_passive;
    [SerializeField] TutorialData tutorial_personality;

    Definer definer;
    Map_MapPanel mapPanel;
    InfoText infoText;
    CharactersManager charactersManager;
    BattleManager battleManager;
    FadeOutUI fadeOutUI;
    SoundManager soundManager;
    LootPanel lootPanel;
    GameManager gameManager;
    GuideMessage guideMessage;
    SupplyManager supplyManager;
    MainMessage mainMessage;
    TutorialManager tutorialManager;
    Inventory inventory;
    RoomEndLogManager relManager;

    GameParams gp;
    CommonParams cp;

    bool inExpedition;
    bool inRoomEvent;
   public bool endlessMode;

    public List<CharacterData> deployedChara = new List<CharacterData>();
    AreaData currentArea;

    int addedMadness;
    List<GameObject> madnessPAs = new List<GameObject>();

    /// <summary>x:現在のレイヤー y:上下</summary>
    Room currentRoom;
    List<Map_LayerPanel> layers;

    public static ExpeditionManager inst;
    private void Awake()
    {
        inst = this;
    }

    private void Start()
    {
        definer = FindObjectOfType<Definer>();
        mapPanel = FindObjectOfType<Map_MapPanel>();
        infoText = FindObjectOfType<InfoText>();
        charactersManager = CharactersManager.inst;
        battleManager = BattleManager.inst;
        fadeOutUI = FindObjectOfType<FadeOutUI>();
        soundManager = SoundManager.instance;
        lootPanel = LootPanel.inst;
        gameManager = GameManager.instance;
        guideMessage = FindObjectOfType<GuideMessage>();
        supplyManager = FindObjectOfType<SupplyManager>();
        mainMessage = FindObjectOfType<MainMessage>();
        tutorialManager = FindObjectOfType<TutorialManager>();
        inventory = FindObjectOfType<Inventory>();
        relManager = RoomEndLogManager.inst;

        gp = GameManager.gameParams;
        cp = Definer.inst.cp;

        //for (int i = 0; i < Enum.GetNames(typeof(PA_Personality.PersonalityStatus.PersonalityType)).Length; i++)
        //{
        //    pers_pool.Add(new List<GameObject>());
        //}
        //for (int i = 0; i < 3; i++)
        //{
        //    pers_pool.Add(new List<GameObject>());
        //}
        //foreach (GameObject per in cp.perDataBase_randPool)
        //{
        //    PA_Personality.PersonalityStatus status = per.GetComponent<PA_Personality>().GetPersonalityStatus();
        //    PA_Personality.PersonalityStatus.PersonalityType perType = status.personalityType;
        //    int index = perTypeList.IndexOf(perType);
        //    if (index == -1) infoText.AddErrorText($"予期せぬ種類の特性がデータベースに存在{status.personalityName}");

        //    pers_pool[index].Add(per);
        //}
    }

    //========================[探索開始]==============================
    public void StartExpedition(AreaData firstArea)
    {
        inExpedition = true;

        //初期値設定
        enemyLVL = 1;
        inventory.AddCoin(gp.initialCoin);
        partyStatus.startTime = Time.time;
        charactersManager.GetExistingCharacters_All().ForEach(x =>
        {
            partyStatus.AddTBR(new PersonalBattleReport(x));
        });
        GameResultManager.inst.SetPartyStatus(partyStatus);

        //最初のエリアスタート
        StartArea(firstArea);
    }
    public void StartArea(AreaData area)
    {
        partyStatus.areaCount++;
        infoText.AddDebugText("探索開始");
        currentArea = area;

        SetBackground(currentArea.backgroundParams);

        soundManager.SetBGM_Normal(currentArea.BGM);

        var a = Instantiate(currentArea.areaManager, AreaManagerP);//managerの生成
        a.GetComponent<AreaManager>().Init(area);
        currentAreaManger = a.GetComponent<AreaManager>();

        StartCoroutine(StartAreaAnim());
    }

    IEnumerator StartAreaAnim()
    {
        fadeOutUI.FadeIn();
        yield return new WaitForSeconds(0.5f);
        mainMessage.SetMessage(string.Format("第{0}エリア  {1}", partyStatus.areaCount, currentArea.areaName));
        infoText.AddLogText(string.Format("△▽△▽△<<第{0}エリア {1}>>△▽△▽△", partyStatus.areaCount, currentArea.areaName));

        yield return new WaitForSeconds(2f);
        mainMessage.ResetMessage();

        yield return new WaitForSeconds(1.25f);
        CheckRoomEndEvent();
    }

    public void SetBackground(BackgroundParams backgroundParams)
    {
        for (int i = 0; i < backgroundP.transform.childCount; i++) { Destroy(backgroundP.transform.GetChild(i).gameObject); }//背景生成
        Instantiate(backgroundParams.obj, backgroundP);
        globalLight.intensity = backgroundParams.lightIntensity;
        globalLight.color = backgroundParams.lightColor;
    }

    public void NextArea(AreaData next)
    {
        inRoomEvent = false;
        StartCoroutine(EndAreaAnim(next));
    }
    IEnumerator EndAreaAnim(AreaData next)
    {
        fadeOutUI.FadeOut();
        yield return new WaitForSeconds(1f);
        StartArea(next);
    }

    public void SetLayers(List<Map_LayerPanel> l)
    {
        layers = l;

        partyStatus.currentPos = new Vector2Int(0, 2);
        currentRoom = GetRoom(partyStatus.currentPos);

        GetRoomButton(partyStatus.currentPos).SetState_currentPos();
        //SelectNextRoom();
    }

    public void AddMadness(int value)
    {
        addedMadness += value;
        infoText.AddLogText("狂気が満ちていく...".ColorStr(Definer.colorRef.affricted));
    }

    public string GetMadnessInfo(PA_Personality mad)
    {
        return $"{mad.GetPAName()}\n敵は{gp.madnessSpawnChance}％の確率で以下の能力を得る：\n{gp.madnessStatMod.GetInfo()}\n\nさらに、\n{mad.GetPAInfo(false)}\n";
    }

    void CheckRoomEndEvent()
    {
        if((partyStatus.areaCount > 1 && partyStatus.currentPos.x == 0) || partyStatus.currentPos.x == Mathf.FloorToInt(currentAreaManger.GetAreaLength() / 2f))//enemyLVLUP
        {
            relManager.Enqueue_EnemyLVL();
        }

        if (partyStatus.getPerChance_endRE.Dice())//特性追加
        {
            SetRandomPer_ToRandom();
        }

        if (partyStatus.madness < partyStatus.maxMadness)//狂気
        {
            for (int i = 0; i < addedMadness; i++)
            {
                GameObject add = MadnessPAPool.Choice();
                MadnessPAPool.Remove(add);
                madnessPAs.Add(add);
                partyStatus.madness++;
                relManager.Enqueue_Madness(partyStatus.madness, add);
                if (partyStatus.madness == partyStatus.maxMadness) break;
            }
        }
        addedMadness = 0;

        RoomEndLog();
    }

    void RoomEndLog()
    {
        if (relManager.HasLog())
        {
            relManager.StartREL();
        }
        else
        {
            SelectNextRoom();
        }
    }

    public string EnemyLVLUP()
    {
        enemyLVL++;
        return GameManager.gameParams.enemyStatusGrowth.GetInfo(enemyLVL);
    }
    public void SelectNextRoom()
    {
        if (teleport)
        {
            foreach (Map_LayerPanel layer in layers)
            {
                foreach (Map_RoomButton button in layer.GetRoomButtons()) { button.SetState_Selectable(); }
            }
        }
        else
        {
            if (currentRoom.up > 0) { GetRoomButton(new Vector2Int(partyStatus.currentPos.x + 1, partyStatus.currentPos.y + 1)).SetState_Selectable(); }
            if (currentRoom.straight > 0) { GetRoomButton(new Vector2Int(partyStatus.currentPos.x + 1, partyStatus.currentPos.y)).SetState_Selectable(); }
            if (currentRoom.down > 0) { GetRoomButton(new Vector2Int(partyStatus.currentPos.x + 1, partyStatus.currentPos.y - 1)).SetState_Selectable(); }
        }
        tutorialManager.SetTutorial(tutorial_expedition);
        guideMessage.SetGuideText("マップから次の階層へ移動可能");
        nextRoomButton.SetActive(true);

        if(tutorialManager.CompleteEssentials()) tutorialManager.SetTips();
        else if(inventory.GetExp() > 0 && !tutorialManager.CheckUnlocked(tutorial_exp)) tutorialManager.SetTutorial(tutorial_exp);
        else if(inventory.GetEquipments().Count > 0 && !tutorialManager.CheckUnlocked(tutorial_equipment)) tutorialManager.SetTutorial(tutorial_equipment);
    }
    public void GoToNextRoom(Vector2Int pos)
    {
        if (moveMode) { ToggleMoveMode(); }
        mapPanel.CloseMap();
        partyStatus.currentPos = pos;
        currentRoom = GetRoom(partyStatus.currentPos);
        soundManager.PlaySE(SE_nextRoom);

        nextRoomButton.SetActive(false);

        inRoomEvent = true;
        StartCoroutine(AnimationForNextRoom());
    }
    IEnumerator AnimationForNextRoom()
    {
        fadeOutUI.FadeOut();
        yield return new WaitForSeconds(0.85f);

        fadeOutUI.FadeIn();
        yield return new WaitForSeconds(0.5f);
        foreach (Map_LayerPanel layer in layers) { layer.ResetButtonsState(); }
        GetRoomButton(partyStatus.currentPos).SetState_currentPos();

        infoText.AddLogText(string.Format("==============<第{0}階層>==============", partyStatus.currentPos.x));

        //bool SANDMG=false;
        //foreach (Character chara in charactersManager.GetExistingCharacters_All())
        //{
        //    if (gp.SANDMGChanceOnRoom.Dice())
        //    {
        //        SANDMG = true;
        //        chara.SANDamage(gp.SANDMGOnRoom.Range());
        //    }
        //}
        //if(SANDMG) yield return new WaitForSeconds(1f);

        var r = Instantiate(currentRoom.roomEventManager, REManagerParent);
        r.GetComponent<RoomEvent>().Init(currentAreaManger.GetArea());
        currentRE = r.GetComponent<RoomEvent>();
    }

  

    public void EnterEndless()
    {
        endlessMode = true;
        currentRE.OnEnterEndless();
    }

    public void LogREName(string REName)
    {
        infoText.AddLogText(string.Format("～～{0}～～", REName));
        infoText.SwitchToLog();
    }

    //エリアの進行度に応じた経験値量を返す
    public int GetExpAmount(float _base) { return ((gameManager.gp.EXPBase * partyStatus.areaCount).ToInt() * _base).ToInt(); }

    bool moveMode;
    Character moveChara;
    /// <summary>部屋移動前の位置の編集</summary>
    /// 

    public void MoveButton()
    {
        if (Input.GetMouseButtonDown(1))
        {
            infoText.SetText_Old("ポジション変更", "キャラクターのポジションを変更する\n(イベント中は不可)");
        }
        if (Input.GetMouseButtonDown(0))
        {
            ToggleMoveMode();
        }
    }
    public void ToggleMoveMode()
    {
        if (!inRoomEvent)
        {
            moveMode = !moveMode;
            if (moveMode)
            {
                foreach(Character chara in charactersManager.GetExistingCharacters_All())
                {
                    if (!chara.CharaStatus().player) { infoText.AddErrorText("プレイヤーでないキャラが生きている"); }
                    chara.GetTargetButton().MoveMode_SelectableAsTarget();
                }
            }
            else
            {
                moveChara = null;
                for (int i = 0; i < 9; i++) { charactersManager.GetTargetButton(i).MoveMode_ResetAll(); }
            }
        }
        else { guideMessage.SetWaringText("イベント中のポジション変更不可"); }
    }
    public void MoveMode_SelectChara(Character chara)
    {
        moveChara = chara;
        for (int i = 0; i < 9; i++)
        {
            charactersManager.GetTargetButton(i).MoveMode_ResetAll();
            if (moveChara.CharaStatus().position != i)
            {
                charactersManager.GetTargetButton(i).MoveMode_SelectableAsMovePos();
            }
        }
    }
    public void MoveMode_SelectPos(int pos)
    {
        moveChara.GetTargetButton().ResetCharacter();

        if (charactersManager.CheckCharaExist(pos))
        {
            charactersManager.GetCharacterWithPos(pos).ChangePos(moveChara.CharaStatus().position);
        }
        moveChara.ChangePos(pos);
        ToggleMoveMode();
    }

    //==========================================[room event で呼ばれる関数]===========================================
    [System.Serializable]
    public struct BattleParams
    {
        public AudioClip bgm;
        public int lvlMod;
        public int additionalEq;
        public int additionalEq_epic;
        public int supplyPicksMod;
        public int expMod;
        public bool dontPlayBGMOnEnd;
    }
    public void Battle(List<AreaManager.EnemySet> w, GameObject fieldEffect)
    {
        Battle(w, fieldEffect, new BattleParams());
    }
    public void Battle(List<AreaManager.EnemySet> enemySet, GameObject fieldEffect,BattleParams battleParams)
    {
        //List<CharacterData> enemies = enemySet.GetEnemies();

        supplyManager.AddSupply_Eq(battleParams.additionalEq);
        supplyManager.AddSupply_Eq(battleParams.additionalEq_epic, ItemData.Rarity.epic);

        if (battleParams.supplyPicksMod > 0) supplyManager.AddPicks(battleParams.supplyPicksMod);
        lootPanel.AddExp(1+ battleParams.expMod);
        lootPanel.AddCoin(gp.coinPerBattle_base.Range());

        //for (int i = 0; i < 9; i++)
        //{
        //    if (enemies[i] != null) { charactersManager.SpawnEnemy(enemies[i], i + 9, true,enemyLVL+ battleParams.lvlMod); }
        //}
        if (currentArea)
        {
            if (battleParams.bgm != null) soundManager.StartBGM_Battle(battleParams.bgm);
            else if (currentArea.battleBGM.Count > 0) soundManager.StartBGM_Battle(currentArea.battleBGM.Choice());
        }
        else
        {
            infoText.AddDebugText("検知：DebugModeでの戦闘");
        }
       
        battleManager.BattleStart(enemySet,fieldEffect,battleParams);
    }
    

    public void SetREInfo(string title, string info)
    {
        REInfoPanel.SetActive(true);
        RETitle.text = title;
        REInfo.text = info;
    }
    public void EndREInfo()
    {
        ResetREInfo();
        currentRE.OnEndREInfo();
    }
    public void ResetREInfo()
    {
        REInfoPanel.SetActive(false);
        RETitle.text = "";
        REInfo.text = "";
    }
    public void SetREOptionButtons(List<RoomEvent.REOptionParams> optionParams)
    {
        REOptionUI.SetActive(true);
        for(int i=0; i < optionParams.Count; i++)
        {
            var o = Instantiate(REOptionButton, REOptionButtonsP);
            o.GetComponent<REOptionButton>().Init(optionParams[i], i, infoText,this);
        }
    }
    public void REOption_OnRClick(int index)
    {
        currentRE.OnRClick(index);
    }
    public void REOption_Select(int index)
    {
        for(int i = 0; i < REOptionButtonsP.childCount; i++) { Destroy(REOptionButtonsP.GetChild(i).gameObject); }
        ResetREInfo();
        REOptionUI.SetActive(false);
        currentRE.SelectOption(index);
    }

    public Definer.Item GetRandomEquipment()
    {
        Definer.Item equipment = new Definer.Item();
        equipment.Init(Definer.equipments[partyStatus.equipmentDropWeights.ChoiceWithWeight()].Choice());
        return equipment;
    }
    /// <summary>指定したレアリティからランダムに選ぶ</summary>
    public Definer.Item GetRandomEquipment_WithRarity(ItemData.Rarity rarity)
    {
        Definer.Item equipment = new Definer.Item();
        equipment.Init(Definer.equipments[(int)rarity].Choice());
        return equipment;
    }
    
    /// <summary>指定したレアリティ以上の装備品をランダムに選ぶ</summary>
    public Definer.Item GetRandomEquipment_WithGuarantee(ItemData.Rarity rarity)
    {
        List<float> weight = new List<float>(partyStatus.equipmentDropWeights);
        float buffer = 0;

        for (int i = 0; i < (int)rarity; i++)
        {
            buffer += weight[i];
            weight[i] = 0;
        }

        weight[(int)rarity] += buffer;

        Definer.Item equipment = new Definer.Item();
        equipment.Init(Definer.equipments[weight.ChoiceWithWeight()].Choice());
        return equipment;
    }

    //==========================================================================[Per]======================================================================
    ///// <summary>
    ///// ランダム取得時に含まれる特性(bad,good,awokenのみ)
    ///// </summary>
    //List<List<GameObject>> pers_pool= new List<List<GameObject>>();
    //ランダム取得の際に使用
    List<PA_Personality.PersonalityStatus.PersonalityType> perTypeList = new List<PA_Personality.PersonalityStatus.PersonalityType> { PA_Personality.PersonalityStatus.PersonalityType.bad,
    PA_Personality.PersonalityStatus.PersonalityType.good,PA_Personality.PersonalityStatus.PersonalityType.awoken};

    public void SetRandomPer_ToRandom(int amount = 1)
    {
        List<Character> pool = new List<Character>();
        foreach (Character c in charactersManager.GetExistingCharacters_All())
        {
            if (c.CharaStatus().playable) { pool.Add(c); }
        }
        Character chara = pool.Choice();

        SetPersonality(chara, GetPer_Random(chara, amount));
    }
    public void SetRandomPer_ToRandom_WithType(PA_Personality.PersonalityStatus.PersonalityType perType, int amount = 1)
    {
        List<Character> pool = new List<Character>();
        foreach (Character c in charactersManager.GetExistingCharacters_All())
        {
            if (c.CharaStatus().playable) { pool.Add(c); }
        }
        Character chara = pool.Choice();

        SetPersonality(chara, GetPer_Random_CertainType(chara, perType, amount));
    }
    public void SetRandomPer(Character chara, int amount = 1)
    {
        SetPersonality(chara, GetPer_Random(chara, amount));
    }
    public void SetRandomPer_WithType(Character chara, PA_Personality.PersonalityStatus.PersonalityType perType, int amount = 1)
    {
        SetPersonality(chara, GetPer_Random_CertainType(chara, perType, amount));
    }

    /// <summary>
    /// 指定したタイプのper
    /// </summary>
    /// <returns></returns>
    public List<GameObject> GetPer_Random_CertainType(PA_Personality.PersonalityStatus.PersonalityType perType,int amount=1)
    {
        int index=perTypeList.IndexOf(perType);
        if (index == -1) infoText.AddErrorText($"予期しないタイプの特性を取得しようとしています");

        return Definer.pers_pool[index].Sample(amount);
    }
    /// <summary>
    /// 指定したキャラが持っていない、指定したタイプのランダムなper
    /// </summary>
    /// <returns></returns>
    public List<GameObject> GetPer_Random_CertainType(Character chara,PA_Personality.PersonalityStatus.PersonalityType perType, int amount = 1)
    {
        int index = perTypeList.IndexOf(perType);
        if (index == -1) infoText.AddErrorText($"予期しないタイプの特性を取得しようとしています");

        return GetPerPool(chara)[index].Sample(amount);
    }
    public List<GameObject> GetPer_Random(int amount = 1)
    {
        return GetRandomPerFromPool(Definer.pers_pool,amount);
    }
    /// <summary>
    /// 指定したキャラが持っていない、ランダムなper
    /// </summary>
    /// <returns></returns>
    public List<GameObject> GetPer_Random(Character chara, int amount = 1)
    {
        return GetRandomPerFromPool(GetPerPool(chara),amount);
    }

    List<List<GameObject>> GetPerPool(Character chara)
    {
        List<List<GameObject>> pool = new List<List<GameObject>>();
        List<string> names = new List<string>();
        for(int i = 0; i < perTypeList.Count; i++)
        {
            names = chara.GetPers(perTypeList[i]).Select(p => p.GetPAName()).ToList();
            pool.Add(Definer.pers_pool[i].Where(p => !names.Contains(p.GetComponent<PA_Personality>().GetPAName())).ToList());
        }

        return pool;
    }
    /// <summary>
    /// 指定した数のランダムなperを重複なしで取得
    /// </summary>
    /// <param name="pool"></param>
    /// <param name="amount"></param>
    /// <returns></returns>
    List<GameObject> GetRandomPerFromPool(List<List<GameObject>> pool,int amount=1)
    {
        List<GameObject> list= new List<GameObject>();
        for(int i = 0; i < amount; i++)
        {
            int index = GameManager.gameParams.perWeights.ChoiceWithWeight();
            GameObject add = pool[index].Choice();
            if (add != null)
            {
                list.Add(add);
                pool[index].Remove(add);
            }
        }
        return list;
    }


    public void SetPersonality(Character target, GameObject personality)
    {
        relManager.Enqueue_AddPer(target, personality);
    }
    public void SetPersonality(Character target, List<GameObject> pers,bool immediate = false)
    {
       if(immediate) pers.ForEach(p=> target.AddPA_Personality(p,true));
       else pers.ForEach(p=> relManager.Enqueue_AddPer(target,p));
    }

    public void OnEndBattle(bool playBGM)
    {
        if (setting_ResetPos) charactersManager.ResetPlayerPos();
        if (playBGM) soundManager.PlayBGM_Normal();
        else soundManager.StopBGMs();
        //supplyManager.SetSupply_Eq(partyStatus.supplyOptions);
        currentRE.OnEndBattle();
    }
    public void OnEndShop() { currentRE.OnEndShop(); }

    public void OnEndLoot() { currentRE.OnEndLoot(); }
    public void OnEndSupply() { currentRE.OnEndSupply(); }

    //ここまでroom event で呼ばれる関数
    public void EndRoomEvent()
    {
        //あーだこーだ
        StartCoroutine(EndRoomEventC());
    }
    IEnumerator EndRoomEventC()
    {
        yield return new WaitForSeconds(1f);
        inRoomEvent = false;

        foreach (Character chara in ExpeditionRef.charactersManager.GetExistingCharacters_All())
        {
            if (gp.SANDMGChanceOnRoom.Dice())//SAN
            {
                chara.SANDamage(gp.SANDMGOnRoom.Range());
            }

            Character.CharacterStatus status = chara.CharaStatus();//回復
            int decreasedHP = status.maxHP - status.HP;
            chara.Heal((decreasedHP * GameManager.gameParams.RegenPercentOnRoomEnd / 100f).ToInt(), null);
        }
        yield return new WaitForSeconds(0.75f);
        
        CheckRoomEndEvent();
    }

    public void Defeat()
    {
        soundManager.StopBGMs();
        StartCoroutine(DefeatC());
    }
    IEnumerator DefeatC()
    {
        fadeOutUI.FadeOut_SetDuration(1f);
        yield return new WaitForSeconds(2f);
        GameResultManager.inst.SetResult(2);
    }
    public void EndExpediton()
    {
        gameManager.GoToResultScene(true);//test
    }

    //============================[デバッグ関連]========================================
    bool teleport;
    public void Debug_ToggleTeleport()
    {
        teleport = !teleport;
        if (teleport) { infoText.AddDebugText("テレポート：オン"); }
        else { infoText.AddDebugText("テレポート：オフ"); }
    }

    public void Debug_StartRE(AreaData area,GameObject room)
    {
        partyStatus.areaCount++;
        currentArea = area;

        SetBackground(currentArea.backgroundParams);

        var a = Instantiate(currentArea.areaManager, AreaManagerP);//managerの生成
        a.GetComponent<AreaManager>().Init(area);
        currentAreaManger = a.GetComponent<AreaManager>();

        var r = Instantiate(room, REManagerParent);
        r.GetComponent<RoomEvent>().Init(currentAreaManger.GetArea());
        currentRE = r.GetComponent<RoomEvent>();
    }

    //================================[チュートリアル]=======================================
    public void StartTutorial_Passive()
    {
        tutorialManager.SetTutorial(tutorial_passive);
    }
    public void StartTutorial_Personality() { tutorialManager.SetTutorial(tutorial_personality); }

    //====================================[Settings]========================================================
    public static bool setting_ResetPos;
    public void Setting_ResetPos(bool value) { setting_ResetPos = value; }

    public PartyStatus GetPartyStatus() { return partyStatus; }
    public Map_RoomButton GetRoomButton(Vector2Int pos) { return layers[pos.x].GetRoomButton(pos.y); }
    public Room GetRoom(Vector2Int pos) { return layers[pos.x].GetRoom(pos.y); }
    public List<Map_LayerPanel> GetLayers() { return layers; }
    public RoomEvent GetCurrentRE() { return currentRE; }
    public Vector2Int GetCurrentPos() { return partyStatus.currentPos; }
    public AreaManager GetAreaManager() { return currentAreaManger; }

    public bool CheckInRoomEvent() { return inRoomEvent; }
    public bool CheckInExpedition() { return inExpedition; }
    public List<GameObject> GetMadnessPA() { return madnessPAs; }
    public int EnemyLVL() { return enemyLVL; }  

}

[System.Serializable]
public class BackgroundParams
{
    public GameObject obj;
    public Color lightColor;
    public float lightIntensity;
}