using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RE_Loot : RoomEvent
{
    [SerializeField]
    List<RoomEvent.REOptionParams> options;

    [SerializeField]
    string eventName;
    [SerializeField, TextArea(3, 10)]
    string eventInfo;

    [SerializeField, Header("x:min y:max")]
    Vector2Int drawAttemptsRange;
    [SerializeField]
    List<LootPanel.DropItem> dropItems;

    [SerializeField,Header("ämíËÇ≈óéÇ∆Ç∑å¬êîÇÃîÕàÕ")]
    Vector2Int dropEquipmentsRange;
    [SerializeField]
    LootPanel.LootStatus lootStatus;

    public override void StartRoomEvent()
    {
        if (eventName != "") { expeditionManager.LogREName(eventName); }
        if (eventInfo != "") { infoText.AddLogText(eventInfo + "\n"); }
        infoText.SwitchToLog();

        lootPanel=FindObjectOfType<LootPanel>();

        expeditionManager.SetREOptionButtons(options);
    }
    public override void SelectOption(int index)
    {
        lootPanel.DropItem_Loot(lootStatus);
        lootPanel.Loot();
    }
    public override void OnEndLoot()
    {
        EndRoomEvent();
    }
}
