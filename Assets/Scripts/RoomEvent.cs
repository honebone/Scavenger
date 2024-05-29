using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomEvent : MonoBehaviour
{
    [SerializeField]
    bool showREInfoOnStart;
    [SerializeField]
    string REName;
    [SerializeField, TextArea(3, 10)]
    string REInfo;
    protected AreaManager.Area currentArea;
    protected ExpeditionManager expeditionManager;
    protected ExpeditionManager.PartyStatus partyStatus;
    protected LootPanel lootPanel;
    protected SupplyManager supplyManager;
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
        partyStatus = expeditionManager.GetPartyStatus();
        lootPanel = FindObjectOfType<LootPanel>();
        supplyManager = FindObjectOfType<SupplyManager>();
        characterManager=FindObjectOfType<CharactersManager>();
        infoText = FindObjectOfType<InfoText>();

        if (REName != "") { expeditionManager.LogREName(REName); }
        if (REInfo != "") { infoText.AddLogText(REInfo + "\n"); }
        if (showREInfoOnStart)
        {
            expeditionManager.SetREInfo(REName, REInfo);
        }

        StartRoomEvent();
    }
    public virtual void StartRoomEvent() { }
    public virtual void OnEndREInfo() { }
    public virtual void SelectOption(int index) { }
    /// <summary>デフォルトではルート開始</summary>
    public virtual void OnEndBattle() { lootPanel.Loot(); }
    /// <summary>デフォルトではイベント終了</summary>
    public virtual void OnEndLoot()
    {
        if (supplyManager.CheckHasSupply()) { supplyManager.StartSupply(); }
        else { EndRoomEvent(); }
    }
    public virtual void OnEndSupply() { EndRoomEvent(); }

    public  void EndRoomEvent()
    {
        expeditionManager.EndRoomEvent();
        Destroy(this.gameObject);
    }
}
