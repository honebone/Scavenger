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

    [SerializeField,Header("\n\nマップ上の情報上書き")] bool overrideMapName;
    [SerializeField] string mapName;
    [SerializeField] bool overrideMapInfo;
    [SerializeField,TextArea(5,15)] string mapInfo;

    protected AreaData currentArea;
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
        [Header("expを自動計算")]
        public float expMul;
        [TextArea(3, 10)]
        public string optionInfo_suffix;

        public bool available = true;

        public string GetInfo()
        {
            string expInfo = expMul == 0 ? "" : ExpeditionManager.inst.GetExpAmount(expMul).ToString();
            return $"{optionInfo}{expInfo}{optionInfo_suffix}";
        }
        public REOptionParams()
        {
            optionName = "";
            optionInfo = "";
            expMul = 0;
            optionInfo_suffix = "";
            available = true;
        }
        public REOptionParams(REOptionParams copy)
        {
            optionName = copy.optionName;
            optionInfo = copy.optionInfo;
            expMul = copy.expMul;
            optionInfo_suffix = copy.optionInfo_suffix;
            available = copy.available;
        }
    }
    public void Init(AreaData area)
    {
        currentArea = area;
        expeditionManager=FindObjectOfType<ExpeditionManager>();
        partyStatus = expeditionManager.GetPartyStatus();
        lootPanel = FindObjectOfType<LootPanel>();
        supplyManager = FindObjectOfType<SupplyManager>();
        characterManager=FindObjectOfType<CharactersManager>();
        infoText = FindObjectOfType<InfoText>();

        string _name = REName;
        string info = "";
        if (this as RE_RandomEvents != null)//醜いダウンキャスト　致し方なし
        {
            RE_RandomEvents rae=this as RE_RandomEvents;
            info += $"{rae.GetRarityStr()}イベント";
            if (rae.GetRarity() == RE_RandomEvents.Rarity.legendary) info += $"({Definer.inst.cp.RaEWeights[(int)RE_RandomEvents.Rarity.legendary]}％！)";

            _name= _name.ColorStr(rae.GetRarity().ToColor());
        }
        info += $"{Extentions.NL(info, 2)}{REInfo}";

        if (_name != "") { expeditionManager.LogREName(_name); }
        if (REInfo != "") { infoText.AddLogText(info + "\n"); }
        if (showREInfoOnStart)
        {
            expeditionManager.SetREInfo(_name, info);
        }

        StartRoomEvent();
    }
    public string OverrideMapName(string _name)
    {
        return overrideMapName ? mapName : _name;
    }
    public string OverrideMapInfo(string _info)
    {
        return overrideMapInfo ? mapInfo : _info;
    }

    public virtual void StartRoomEvent() { }
    public virtual void OnEndREInfo() { }
    /// <summary>optionを右クリック時に呼ばれる</summary>
    public virtual void OnRClick(int index) { }
    public virtual void SelectOption(int index) { }
    /// <summary>デフォルトではルート開始</summary>
    public virtual void OnEndBattle()
    {
        supplyManager.AddSupply_Eq(partyStatus.supplyOptions);
        lootPanel.Loot();
    }
    /// <summary>デフォルトではイベント終了</summary>
    public virtual void OnEndLoot()
    {
        if (supplyManager.CheckHasSupply()) { supplyManager.StartSupply(); }
        else { EndRoomEvent(); }
    }
    public virtual void OnEnterEndless()
    {

    }
    public virtual void OnEndSupply() { EndRoomEvent(); }

    public  void EndRoomEvent()
    {
        expeditionManager.EndRoomEvent();
        Destroy(this.gameObject);
    }
}
