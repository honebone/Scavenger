using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomEvent : MonoBehaviour
{
    protected AreaManager.Area currentArea;
    protected ExpeditionManager expeditionManager;
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
        characterManager=FindObjectOfType<CharactersManager>();
        infoText = FindObjectOfType<InfoText>();
        StartRoomEvent();
    }
    public virtual void StartRoomEvent() { }
    public virtual void SelectOption(int index) { }
    public virtual void OnEndBattle() { }
    public virtual void OnEndLoot() { }

    public  void EndRoomEvent()
    {
        expeditionManager.EndRoomEvent();
        Destroy(this.gameObject);
    }
}
