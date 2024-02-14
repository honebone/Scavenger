using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
            roomEventManager = data.roomEventManager;
            eventIcon = data.eventIcon;
        }
    }

    /// <summary>x:現在のレイヤー y:上下</summary>
    [SerializeField]
    Vector2Int currentPos;
    Room currentRoom;
    List<Map_LayerPanel> layers;

    [SerializeField]//test
    AreaManager currentAreaManger;
    RoomEvent currentRE;

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

    Map_MapPanel mapPanel;
    InfoText infoText;
    CharactersManager charactersManager;
    BattleManager battleManager;
    FadeOutUI fadeOutUI;
    SoundManager soundManager;

    private void Start()
    {
        mapPanel = FindObjectOfType<Map_MapPanel>();
        infoText = FindObjectOfType<InfoText>();
        charactersManager = FindObjectOfType<CharactersManager>();
        battleManager = FindObjectOfType<BattleManager>();
        fadeOutUI = FindObjectOfType<FadeOutUI>();
        soundManager = FindObjectOfType<SoundManager>();
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
        if (currentRoom.up > 0) { GetRoomButton(new Vector2Int(currentPos.x + 1, currentPos.y + 1)).SetState_Selectable(); }
        if (currentRoom.straight > 0) { GetRoomButton(new Vector2Int(currentPos.x + 1, currentPos.y)).SetState_Selectable(); }
        if (currentRoom.down > 0) { GetRoomButton(new Vector2Int(currentPos.x + 1, currentPos.y - 1)).SetState_Selectable(); }
        mapPanel.OpenMap();
    }
    public void GoToNextRoom(Vector2Int pos)
    {
        mapPanel.CloseMap();
        currentPos = pos;
        currentRoom = GetRoom(currentPos);
        soundManager.PlaySE(SE_nextRoom);

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
    }

    //ここからroom event で呼ばれる関数
    public void Battle(AreaManager.EnemySet enemySet)
    {
        if (enemySet.enemies.Length != 9) { infoText.AddErrorText("敵配置の配列数が間違ています"); }
        for(int i = 0; i < 9; i++)
        {
            if(enemySet.enemies[i] != null) { charactersManager.SpawnEnemy(enemySet.enemies[i], i+9,true); }         
        }
        battleManager.BattleStart();

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
        REOptionUI.SetActive(false);
        currentRE.SelectOption(index);
    }


    public void OnEndBattle() { currentRE.OnEndBattle(); }
    public void OnEndLoot() { currentRE.OnEndLoot(); }
    //ここまでroom event で呼ばれる関数
    public void EndRoomEvent()
    {
        //あーだこーだ
        SelectNextRoom();
    }

    public Map_RoomButton GetRoomButton(Vector2Int pos) { return layers[pos.x].GetRoomButton(pos.y); }
    public Room GetRoom(Vector2Int pos) { return layers[pos.x].GetRoom(pos.y); }
    public RoomEvent GetCurrentRE() { return currentRE; }
}
