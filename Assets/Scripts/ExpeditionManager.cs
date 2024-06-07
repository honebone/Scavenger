using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Rendering.Universal;

public class ExpeditionManager : MonoBehaviour
{
    [System.Serializable]
    public struct Room
    {
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
        public void SetRoomEvent(RoomEventData data)
        {
            eventName = data.eventName;
            eventInfo = data.eventInfo;
            if (data.debug) { roomEventManager = data.roomEventManager[0]; }
            else { roomEventManager = data.roomEventManager[Random.Range(0, data.roomEventManager.Count)]; }
            eventIcon = data.eventIcon;
        }
    }
    [System.Serializable]
    public class PartyStatus
    {
        public float[] materialDropChance=new float[] { 60, 30, 10, 5, 1 };
        public float[] equipmentDropWeights= new float[] { 50, 35, 10, 4, 1 };
        public int turnOrderReveal = 3;

        public int dropExpChance = 50;

        /// <summary>roomEvent終了時に特性追加確率</summary>
        public int getPerChance_endRE = 10;

        /// <summary>支給品の選択肢の数</summary>
        public int supplyOptions = 3;

    }
    [SerializeField]
    PartyStatus partyStatus;
    [SerializeField]
    AreaData areaDataForDebug;

    [SerializeField] Transform backgroundP;
    [SerializeField] Light2D globalLight;
    int areaCount;
    AreaData currentArea;

    /// <summary>x:現在のレイヤー y:上下</summary>
    [SerializeField]
    Vector2Int currentPos;
    Room currentRoom;
    List<Map_LayerPanel> layers;

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
    [SerializeField]
    Transform REManagerParent;

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

    bool inExpedition;
    bool inRoomEvent;

    private void Start()
    {
        definer = FindObjectOfType<Definer>();
        mapPanel = FindObjectOfType<Map_MapPanel>();
        infoText = FindObjectOfType<InfoText>();
        charactersManager = FindObjectOfType<CharactersManager>();
        battleManager = FindObjectOfType<BattleManager>();
        fadeOutUI = FindObjectOfType<FadeOutUI>();
        soundManager = FindObjectOfType<SoundManager>();
        lootPanel = FindObjectOfType<LootPanel>();
        gameManager = FindObjectOfType<GameManager>();
        guideMessage = FindObjectOfType<GuideMessage>();
        supplyManager = FindObjectOfType<SupplyManager>();
        mainMessage = FindObjectOfType<MainMessage>();
        tutorialManager = FindObjectOfType<TutorialManager>();
        inventory = FindObjectOfType<Inventory>();

        //StartArea(areaDataForDebug);//test
    }

    //========================[探索開始]==============================
    public void StartExpedition(AreaData firstArea)
    {
        inExpedition = true;
        StartArea(firstArea);
    }
    public void StartArea(AreaData area)
    {
        areaCount++;
        infoText.AddDebugText("探索開始");
        currentArea = area;

        for (int i = 0; i < backgroundP.transform.childCount; i++) { Destroy(backgroundP.transform.GetChild(i).gameObject); }//背景生成
        Instantiate(currentArea.background, backgroundP);
        globalLight.intensity = currentArea.GLightIntensity;
        globalLight.color = currentArea.GLightColor;

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
        mainMessage.SetMessage(string.Format("第{0}エリア  {1}", areaCount, currentArea.areaName));
        infoText.AddLogText(string.Format("△▽△▽△<<第{0}エリア {1}>>△▽△▽△", areaCount, currentArea.areaName));

        yield return new WaitForSeconds(2f);
        mainMessage.ResetMessage();

        yield return new WaitForSeconds(1.25f);
        SelectNextRoom();
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

        currentPos = new Vector2Int(0, 2);
        currentRoom = GetRoom(currentPos);

        GetRoomButton(currentPos).SetState_currentPos();
        //SelectNextRoom();
    }
    

    public void SelectNextRoom()
    {
        if (teleport)
        {
            foreach (Map_LayerPanel layer in layers)
            {
                foreach(Map_RoomButton button in layer.GetRoomButtons()) { button.SetState_Selectable(); }
            }
        }
        else
        {
            if (currentRoom.up > 0) { GetRoomButton(new Vector2Int(currentPos.x + 1, currentPos.y + 1)).SetState_Selectable(); }
            if (currentRoom.straight > 0) { GetRoomButton(new Vector2Int(currentPos.x + 1, currentPos.y)).SetState_Selectable(); }
            if (currentRoom.down > 0) { GetRoomButton(new Vector2Int(currentPos.x + 1, currentPos.y - 1)).SetState_Selectable(); }
        }
        tutorialManager.StartTutorial(tutorial_expedition);
        guideMessage.SetGuideText("マップから次の階層へ移動可能");
        nextRoomButton.SetActive(true);
        if (inventory.GetExp() > 0) { tutorialManager.StartTutorial(tutorial_exp); }
        else if (inventory.GetEquipments().Count > 0) { tutorialManager.StartTutorial(tutorial_equipment); }
    }
    public void GoToNextRoom(Vector2Int pos)
    {
        if (moveMode) { ToggleMoveMode(); }
        mapPanel.CloseMap();
        currentPos = pos;
        currentRoom = GetRoom(currentPos);
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
        GetRoomButton(currentPos).SetState_currentPos();

        infoText.AddLogText(string.Format("==============<第{0}階層>==============", currentPos.x));

        var r = Instantiate(currentRoom.roomEventManager, REManagerParent);
        r.GetComponent<RoomEvent>().Init(currentAreaManger.GetArea());
        currentRE = r.GetComponent<RoomEvent>();
    }
    public void LogREName(string REName)
    {
        infoText.AddLogText(string.Format("～～{0}～～", REName));
        infoText.SwitchToLog();
    }

    bool moveMode;
    Character moveChara;
    /// <summary>部屋移動前の位置の編集</summary>
    /// 

    public void MoveButton()
    {
        if (Input.GetMouseButtonDown(1))
        {
            infoText.SetText("ポジション変更", "キャラクターのポジションを変更する\n(イベント中は不可)");
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
                    if (!chara.GetCharacterStatus().player) { infoText.AddErrorText("プレイヤーでないキャラが生きている"); }
                    chara.GetCharacter_TargetButton().MoveMode_SelectableAsTarget();
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
            if (moveChara.GetCharacterStatus().position != i)
            {
                charactersManager.GetTargetButton(i).MoveMode_SelectableAsMovePos();
            }
        }
    }
    public void MoveMode_SelectPos(int pos)
    {
        moveChara.GetCharacter_TargetButton().ResetCharacter();

        if (charactersManager.CheckCharaExist(pos))
        {
            charactersManager.GetCharacterWithPos(pos).ChangePos(moveChara.GetCharacterStatus().position);
        }
        moveChara.ChangePos(pos);
        ToggleMoveMode();
    }

    //==========================================[room event で呼ばれる関数]===========================================
    [System.Serializable]
    public struct BattleParams
    {
       public AudioClip bgm;
    }
    public void Battle(AreaManager.EnemySet enemySet, GameObject fieldEffect,BattleParams battleParams)
    {
        List<CharacterData> enemies = enemySet.GetEnemies();
        for (int i = 0; i < 9; i++)
        {
            if (enemies[i] != null) { charactersManager.SpawnEnemy(enemies[i], i + 9, true); }
        }
        if (battleParams.bgm == null) { soundManager.StartBGM_Battle(currentArea.battleBGM.Choice()); }
        else { soundManager.StartBGM_Battle(battleParams.bgm); }
        battleManager.BattleStart(fieldEffect);
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
    public void SelectOption(int index)
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
    public void SetRandomPersonality(Character target) { 
        SetPersonality(target, definer.GetPersonalityDataBase().Choice());
    }
    public void SetPersonality_ToRandom(GameObject personality)
    {
        List<Character> pool = new List<Character>();
        foreach (Character c in charactersManager.GetExistingCharacters_All())
        {
            if (c.GetCharacterStatus().playable) { pool.Add(c); }
        }

        SetPersonality(pool.Choice(), personality);
    }
    public void SetRandomPersonality_ToRandom()
    {
        List<Character> pool = new List<Character>();
        foreach(Character c in charactersManager.GetExistingCharacters_All())
        {
            if (c.GetCharacterStatus().playable) { pool.Add(c); }
        }

        SetPersonality(pool.Choice(), definer.GetPersonalityDataBase().Choice());
    }

    public void SetPersonality(Character target, GameObject personality)
    {
        target.AddPA_Personality(personality, true);
    }


    public void OnEndBattle()
    {
        soundManager.PlayBGM_Normal();
        if (partyStatus.dropExpChance.Dice()) { lootPanel.AddExp(1); }
        supplyManager.SetSupply_Eq(partyStatus.supplyOptions);
        currentRE.OnEndBattle();
    }
    public void OnEndLoot() { currentRE.OnEndLoot(); }
    public void OnEndSupply() { currentRE.OnEndSupply(); }

    //ここまでroom event で呼ばれる関数
    public void EndRoomEvent()
    {
        //あーだこーだ
        inRoomEvent = false;
        if (partyStatus.getPerChance_endRE.Dice())
        {
            SetRandomPersonality_ToRandom();
        }
        SelectNextRoom();
    }

    public void Defeat()
    {
        soundManager.StopBGMs();
        StartCoroutine(DefeatC());
    }
    IEnumerator DefeatC()
    {
        fadeOutUI.FadeOut_SetDuration(1f);
        yield return new WaitForSeconds(1f);
        gameManager.GoToResultScene(false);//
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
    //================================[チュートリアル]=======================================
    public void StartTutorial_Passive()
    {
        tutorialManager.StartTutorial(tutorial_passive);
    }
    public void StartTutorial_Personality() { tutorialManager.StartTutorial(tutorial_personality); }


    public PartyStatus GetPartyStatus() { return partyStatus; }
    public Map_RoomButton GetRoomButton(Vector2Int pos) { return layers[pos.x].GetRoomButton(pos.y); }
    public Room GetRoom(Vector2Int pos) { return layers[pos.x].GetRoom(pos.y); }
    public RoomEvent GetCurrentRE() { return currentRE; }
    public AreaManager GetAreaManager() { return currentAreaManger; }

    public bool CheckInRoomEvent() { return inRoomEvent; }
    public bool CheckInExpedition() { return inExpedition; }

}
