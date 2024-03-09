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
        //drawAttempts回だけdropItemsからアイテムを1つ選び、そのアイテムのレアリティに応じた確率でルートに追加
        int drawAttempts = Random.Range(drawAttemptsRange.x, drawAttemptsRange.y + 1);
        lootPanel.DropItem_Loot(drawAttempts, dropItems);
        lootPanel.Loot();
    }
    public override void OnEndLoot()
    {
        EndRoomEvent();
    }
}
