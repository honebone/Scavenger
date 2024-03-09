using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomEvent : MonoBehaviour
{
    protected AreaManager.Area currentArea;
    protected ExpeditionManager expeditionManager;
    protected LootPanel lootPanel;
    protected CharactersManager characterManager;
    protected InfoText infoText;
    [System.Serializable]
    public class REOptionParams
    {
        public string optionName;
        [TextArea(3, 10)]
        public string optionInfo;
    }
    public void Init(AreaManager.Area area)
    {
        currentArea = area;
        expeditionManager=FindObjectOfType<ExpeditionManager>();
        lootPanel = FindObjectOfType<LootPanel>();
        characterManager=FindObjectOfType<CharactersManager>();
        infoText = FindObjectOfType<InfoText>();
        StartRoomEvent();
    }
    public virtual void StartRoomEvent() { }
    public virtual void SelectOption(int index) { }
    /// <summary>デフォルトではルート開始</summary>
    public virtual void OnEndBattle() { lootPanel.Loot(); }
    /// <summary>デフォルトではイベント終了</summary>
    public virtual void OnEndLoot() { EndRoomEvent(); }

    public  void EndRoomEvent()
    {
        expeditionManager.EndRoomEvent();
        Destroy(this.gameObject);
    }
}
