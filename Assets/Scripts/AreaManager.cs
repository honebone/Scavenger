using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AreaManager : MonoBehaviour
{
    //[System.Serializable]
    //public struct Area
    //{
    //    public string areaName;

    //    public GameObject background;

    //    [Header("ÉâÉìÉ_ÉÄê∂ê¨Ç≥ÇÍÇÈëwÇÃêî(ÉXÉ^Å[Égínì_,É{ÉXå„ÇèúÇ≠)")]
    //    public int minLength;
    //    public int maxLength;

    //    public int branchChance;
    //    public int blindChance;
    //    public Area_RoomEvent[] roomEvents;
    //    public RoomEventData boss;
    //    public RoomEventData endArea;
    //    public List<EnemySet> normalBattlePool;
    //    //public FieldEffectWeight[] normalBattleFEPool;
    //    public int applyFEChance;
    //    public List<GameObject> normalBattleFEPool;
    //    //nextArea

    //    public List<int> GetREWeights()
    //    {
    //        List<int> weights = new List<int>();
    //        foreach(Area_RoomEvent roomEvent in roomEvents) { weights.Add(roomEvent.weight); }
    //        return weights;
    //    }
    //    public EnemySet GetRandomEnemySet()
    //    {
    //        //List<int> weights = new List<int>();
    //        //foreach (EnemySet battle in normalBattlePool) { weights.Add(battle.weight); }
    //        return normalBattlePool.Choice();
    //    }
    //    public GameObject GetRandomFE()
    //    {
    //        //List<int> weights = new List<int>();
    //        //foreach (FieldEffectWeight FE in normalBattleFEPool) { weights.Add(FE.weight); }
    //        //return normalBattleFEPool[weights.ChoiceWithWeight()].fieldEffect;
    //        if (applyFEChance.Dice())
    //        {
    //            return normalBattleFEPool.Choice();
    //        }
    //        else { return null; }
    //    }
    //}
    [System.Serializable]
    public struct Area_RoomEvent
    {
        public RoomEventData roomEvent;
        public int weight;
    }
    [System.Serializable]
    public struct EnemySet
    {
        //public int weight;
        //public CharacterData[] enemies;
        public bool dontShuffleFront;
        public CharacterData upperFront;
        public CharacterData centerFront;
        public CharacterData lowerFront;
        [Header("\n")]
        public bool dontShuffleMid;
        public CharacterData upperMid;
        public CharacterData centerMid;
        public CharacterData lowerMid;
        [Header("\n")]
        public bool dontShuffleBack;
        public CharacterData upperBack;
        public CharacterData centerBack;
        public CharacterData lowerBack;

        public List<CharacterData> GetEnemies()
        {
            List<CharacterData> enemies = new List<CharacterData>();
            List<CharacterData> buffer = new List<CharacterData>();
            buffer.Add(upperFront);
            buffer.Add(centerFront);
            buffer.Add(lowerFront);
            if (!dontShuffleFront) { buffer = buffer.Shuffle(); }
            enemies.AddRange(buffer);

            buffer = new List<CharacterData>();
            buffer.Add(upperMid);
            buffer.Add(centerMid);
            buffer.Add(lowerMid);
            if (!dontShuffleMid) { buffer = buffer.Shuffle(); }
            enemies.AddRange(buffer);

            buffer = new List<CharacterData>();
            buffer.Add(upperBack);
            buffer.Add(centerBack);
            buffer.Add(lowerBack);
            if (!dontShuffleBack) { buffer = buffer.Shuffle(); }
            enemies.AddRange(buffer);

            return enemies;
        }
    }
    [System.Serializable]
    public struct FieldEffectWeight
    {
        public int weight;
        public GameObject fieldEffect;
    }
    //[SerializeField] Area area;//test
    [SerializeField] protected AreaData areaData;
    //[SerializeField] GameObject content;
    //[SerializeField] ScrollRect mapScroll;
    //[SerializeField] GameObject layerPanel;

    protected ExpeditionManager.Room[] layer;
    protected List<List<ExpeditionManager.Room>> areaRooms = new List<List<ExpeditionManager.Room>>();

    //List<Map_LayerPanel> layers;
    protected ExpeditionManager expeditionManager;
    protected InfoText infoText;
    protected Map_MapPanel map;

    int areaLength;
    //private void Start()
    //{
    //    expeditionManager = FindObjectOfType<ExpeditionManager>();
    //    infoText = FindObjectOfType<InfoText>();
    //    map = FindObjectOfType<Map_MapPanel>();
    //    layer = new ExpeditionManager.Room[5];
    //}

    public void Init(AreaData area)
    {
        areaData = area;

        expeditionManager = FindObjectOfType<ExpeditionManager>();
        infoText = FindObjectOfType<InfoText>();
        map = FindObjectOfType<Map_MapPanel>();
        layer = new ExpeditionManager.Room[5];
        for(int i = 0; i < layer.Length; i++)
        {
            layer[i] = new ExpeditionManager.Room();
        }

        GenerateMap();
    }
    int layerCount;

    public virtual void GenerateMap()
    {
        map.ResetMap();

        areaLength = Random.Range(areaData.minLength, areaData.maxLength + 1);
        layerCount = 0;

        for (int i = 0; i < 5; i++) { layer[i].empty = true; }//0ëwñ⁄
        layer[2].empty = false;
        layer[2].up = 2;
        layer[2].straight = 1;
        layer[2].down = 2;
        layer[2].SetRoomEvent(areaData.start);
        SetLayer();

        layer[0].empty = true;//1ëwñ⁄
        layer[1] = SetRoom(true);
        layer[1].down = 2;
        layer[2] = SetRoom(true);
        layer[3] = SetRoom(true);
        layer[3].up = 2;
        layer[4].empty = true;
        SetLayer();

        for (int j = 2; j < areaLength - 2; j++)//2Å`(length-3)ëwñ⁄Ç‹Ç≈
        {
            for (int i = 0; i < 5; i++) { layer[i] = SetRoom(true); }
            layer[0].down = -1;
            layer[4].up = -1;
            SetLayer();
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
        SetLayer();

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
        SetLayer();

        for (int i = 0; i < 5; i++) { layer[i].empty = true; }//lengthëwñ⁄
        layer[2].empty = false;
        layer[2] = SetRoom(false, true);
        layer[2].up = -1;
        layer[2].straight = 2;
        layer[2].down = -1;
        layer[2].SetRoomEvent(areaData.boss);
        SetLayer();

        for (int i = 0; i < 5; i++) { layer[i].empty = true; }//length+1ëwñ⁄(ÉGÉäÉAÇÃèIí[)
        layer[2].empty = false;
        layer[2].up = -1;
        layer[2].straight = -1;
        layer[2].down = -1;
        layer[2] = SetRoom(false,true);
        layer[2].SetRoomEvent(areaData.endArea);
        SetLayer();

        map.EndGenerateMap();
    }
   
    void SetLayer()
    {
        map.SetLayerPanel(layer, layerCount);
        areaRooms.Add(new List<ExpeditionManager.Room>(layer));
        layerCount++;
        for (int i = 0; i < layer.Length; i++)
        {
            layer[i] = new ExpeditionManager.Room();
        }
    }

    public ExpeditionManager.Room SetRoom(bool setEventRandomly,bool noBlind=false)
    {
        ExpeditionManager.Room room = new ExpeditionManager.Room();
        if (areaData.branchChance.Dice()){ room.up = 1; }
        room.straight = 1;
        if (areaData.branchChance.Dice()) { room.down = 1; }
        room.blind = !noBlind && areaData.blindChance.Dice();
        if (setEventRandomly)
        {
            room.SetRoomEvent(areaData.roomEvents[areaData.GetREWeights().ChoiceWithWeight()].roomEvent);
        }
        //room.blind = !room.eventData.noBlind && areaData.blindChance.Dice();

        return room;
    }

    public void AddBranch(int chance)
    {
        List<Map_LayerPanel> layers = expeditionManager.GetLayers();
        for (int i = expeditionManager.GetCurrentPos().x; i < areaLength; i++)
        {
            foreach(Map_RoomButton roomButton in layers[i].GetRoomButtons())
            {
                ExpeditionManager.Room room = roomButton.GetRoom();
                if (room.up == 0 && chance.Dice())
                {
                    roomButton.SetBranchUp(1);
                }
                if (room.down == 0 && chance.Dice())
                {
                    roomButton.SetBranchDown(1);
                }
            }
        }
    }
    public void Reveal(int chance)
    {
        List<Map_LayerPanel> layers = expeditionManager.GetLayers();
        for (int i = expeditionManager.GetCurrentPos().x; i < areaLength; i++)
        {
            foreach (Map_RoomButton roomButton in layers[i].GetRoomButtons())
            {
                ExpeditionManager.Room room = roomButton.GetRoom();
                if (room.blind && chance.Dice())
                {
                    roomButton.SetBlind(false);
                }
            }
        }
    }

    public AreaData GetArea() { return areaData; }
    public int GetAreaLength() { return areaLength; }
}
