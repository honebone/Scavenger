using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AreaManager : MonoBehaviour
{
    [System.Serializable]
    public struct Area
    {
        public string areaName;

        public GameObject background;

        [Header("ÉâÉìÉ_ÉÄê∂ê¨Ç≥ÇÍÇÈëwÇÃêî(ÉXÉ^Å[Égínì_,É{ÉXå„ÇèúÇ≠)")]
        public int minLength;
        public int maxLength;

        public int branchChance;
        public int blindChance;
        public Area_RoomEvent[] roomEvents;
        public RoomEventData boss;
        public RoomEventData endArea;
        public EnemySet[] normalBattlePool;
        //public FieldEffectWeight[] normalBattleFEPool;
        public int applyFEChance;
        public List<GameObject> normalBattleFEPool;
        //nextArea

        public List<int> GetREWeights()
        {
            List<int> weights = new List<int>();
            foreach(Area_RoomEvent roomEvent in roomEvents) { weights.Add(roomEvent.weight); }
            return weights;
        }
        public EnemySet GetRandomEnemySet()
        {
            List<int> weights = new List<int>();
            foreach (EnemySet battle in normalBattlePool) { weights.Add(battle.weight); }
            return normalBattlePool[weights.ChoiceWithWeight()];
        }
        public GameObject GetRandomFE()
        {
            //List<int> weights = new List<int>();
            //foreach (FieldEffectWeight FE in normalBattleFEPool) { weights.Add(FE.weight); }
            //return normalBattleFEPool[weights.ChoiceWithWeight()].fieldEffect;
            if (applyFEChance.Dice())
            {
                return normalBattleFEPool.Choice();
            }
            else { return null; }
        }
    }
    [System.Serializable]
    public struct Area_RoomEvent
    {
        public RoomEventData roomEvent;
        public int weight;
    }
    [System.Serializable]
    public struct EnemySet
    {
        public int weight;
        public CharacterData[] enemies;
    }
    [System.Serializable]
    public struct FieldEffectWeight
    {
        public int weight;
        public GameObject fieldEffect;
    }
    [SerializeField]
    Area area;//test
    [SerializeField]
    GameObject content;
    [SerializeField]
    ScrollRect mapScroll;
    [SerializeField]
    GameObject layerPanel;

    ExpeditionManager.Room[] layer;

    List<Map_LayerPanel> layers;
    ExpeditionManager expeditionManager;
    InfoText infoText;
    Utility util;
    private void Start()
    {
        expeditionManager = FindObjectOfType<ExpeditionManager>();
        infoText = FindObjectOfType<InfoText>();
        util = FindObjectOfType<Utility>();
        layer = new ExpeditionManager.Room[5];
    }

    public virtual void GenerateMap()
    {
        for (int i = 0; i < content.transform.childCount; i++) { Destroy(content.transform.GetChild(i).gameObject); }

        int length = Random.Range(area.minLength, area.maxLength + 1);
        int layerCount = 0;
        layers = new List<Map_LayerPanel>();

        for (int i = 0; i < 5; i++) { layer[i].empty = true; }//0ëwñ⁄
        layer[2].empty = false;
        layer[2].up = 2;
        layer[2].straight = 1;
        layer[2].down = 2;
        //layer[2]ÇÃroomEventÇäJénínì_ÇÃìzÇ…
        SetLayerPanel(layer, layerCount);
        layerCount++;

        layer[0].empty = true;//1ëwñ⁄
        layer[1] = SetRoom(true);
        layer[1].down = 2;
        layer[2] = SetRoom(true);
        layer[3] = SetRoom(true);
        layer[3].up = 2;
        layer[4].empty = true;
        SetLayerPanel(layer, layerCount);
        layerCount++;

        for(int j = 2; j < length - 2; j++)//2Å`(length-3)ëwñ⁄Ç‹Ç≈
        {
            for (int i = 0; i < 5; i++) { layer[i] = SetRoom(true); }
            layer[0].down = -1;
            layer[4].up = -1;
            SetLayerPanel(layer, layerCount);
            layerCount++;
        }

        for (int i = 0; i < 5; i++) { layer[i] = SetRoom(true); }//length-2ëwñ⁄
        layer[0].down = -1;
        layer[0].straight = -1;
        layer[0].up = 2;
        layer[1].down = -1;
        layer[3].up = -1;
        layer[4].down = 2;
        layer[4].straight = -1;
        layer[4].up = -1;
        SetLayerPanel(layer, layerCount);
        layerCount++;

        layer[0].empty = true;//length-1ëwñ⁄
        layer[1] = SetRoom(true);
        layer[1].up = 2;
        layer[1].straight = -1;
        layer[1].down = -1;
        layer[2] = SetRoom(true);
        layer[2].up = -1;
        layer[2].straight = 2;
        layer[2].down = -1;
        layer[3] = SetRoom(true);
        layer[3].up = -1;
        layer[3].straight = -1;
        layer[3].down = 2;
        layer[4].empty = true;
        SetLayerPanel(layer, layerCount);
        layerCount++;

        for (int i = 0; i < 5; i++) { layer[i].empty = true; }//lengthëwñ⁄
        layer[2].empty = false;
        layer[2] = SetRoom(false);
        layer[2].up = -1;
        layer[2].straight = 2;
        layer[2].down = -1;
        layer[2].SetRoomEvent(area.boss);
        SetLayerPanel(layer, layerCount);
        layerCount++;

        for (int i = 0; i < 5; i++) { layer[i].empty = true; }//length+1ëwñ⁄(ÉGÉäÉAÇÃèIí[)
        layer[2].empty = false;
        layer[2].up = -1;
        layer[2].straight = -1;
        layer[2].down = -1;
        layer[2] = SetRoom(false);
        layer[2].SetRoomEvent(area.endArea);
        SetLayerPanel(layer, layerCount);

        expeditionManager.SetLayers(layers);
    }
    void SetLayerPanel(ExpeditionManager.Room[] l,int lc)
    {
        var lp = Instantiate(layerPanel, content.transform);
        lp.GetComponent<Map_LayerPanel>().Init(l, lc,expeditionManager,infoText,mapScroll);
        layers.Add(lp.GetComponent<Map_LayerPanel>());
    }
    ExpeditionManager.Room SetRoom(bool setEventRandomly)
    {
        ExpeditionManager.Room room = new ExpeditionManager.Room();
        if (area.branchChance.Dice()){ room.up = 1; }
        room.straight = 1;
        if (area.branchChance.Dice()) { room.down = 1; }
        if (area.blindChance.Dice()) { room.blind = true; }
        if (setEventRandomly)
        {
            room.SetRoomEvent(area.roomEvents[area.GetREWeights().ChoiceWithWeight()].roomEvent);
        }

        return room;
    }
    public Area GetArea() { return area; }
}
