using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using static ExpeditionManager;

public class AreaManager : MonoBehaviour
{
    [System.Serializable]
    public struct Area_RoomEvent
    {
        public RoomEventData roomEvent;
        public float weight;
        public int garanteed;
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

        [Header("\n")]
        public List<CharacterData> atRandom;

        public List<CharacterData> GetEnemies()
        {
            List<CharacterData> enemies = new List<CharacterData>();
            List<CharacterData> buffer = new List<CharacterData>();
            buffer.Add(lowerFront);
            buffer.Add(centerFront);
            buffer.Add(upperFront);
            if (!dontShuffleFront) { buffer = buffer.Shuffle(); }
            enemies.AddRange(buffer);

            buffer = new List<CharacterData>();
            buffer.Add(lowerMid);
            buffer.Add(centerMid);
            buffer.Add(upperMid);
            if (!dontShuffleMid) { buffer = buffer.Shuffle(); }
            enemies.AddRange(buffer);

            buffer = new List<CharacterData>();
            buffer.Add(lowerBack);
            buffer.Add(centerBack);
            buffer.Add(upperBack);
            if (!dontShuffleBack) { buffer = buffer.Shuffle(); }
            enemies.AddRange(buffer);

            List<int> empty;
            foreach(CharacterData random in atRandom)
            {
                empty = new List<int>();
                for(int i = 0; i < 9; i++)
                {
                    if (enemies[i] == null) { empty.Add(i); }
                }
                if (empty.Count > 0) { enemies[empty.Choice()] = random; }
            }

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

    //protected ExpeditionManager.Room[] layer;
    List<ExpeditionManager.Room> randRooms = new List<ExpeditionManager.Room>();
    protected List<List<ExpeditionManager.Room>> layers = new List<List<ExpeditionManager.Room>>();

    protected ExpeditionManager expeditionManager;
    protected InfoText infoText;
    protected Map_MapPanel map;

  protected  int areaLength;

    public void Init(AreaData area)
    {
        areaData = area;

        expeditionManager = FindObjectOfType<ExpeditionManager>();
        infoText = FindObjectOfType<InfoText>();
        map = FindObjectOfType<Map_MapPanel>();
        //layer = new ExpeditionManager.Room[5];
        //for(int i = 0; i < layer.Length; i++)
        //{
        //    layer[i] = new ExpeditionManager.Room();
        //}

        GenerateMap();
    }

    public virtual void GenerateMap()
    {
        map.ResetMap();

        areaLength = GameManager.gameParams.areaLength;

        //for (int i = 0; i < 5; i++) { layer[i].empty = true; }//0層目
        //layer[2].empty = false;
        //layer[2].up = 2;
        //layer[2].straight = 1;
        //layer[2].down = 2;
        //layer[2].SetRoomEvent(areaData.start);
        //SetLayer();

        //layer[0].empty = true;//1層目
        //layer[1] = SetRoom(true);
        //layer[1].down = 2;
        //layer[2] = SetRoom(true);
        //layer[3] = SetRoom(true);
        //layer[3].up = 2;
        //layer[4].empty = true;
        //SetLayer();

        //for (int j = 2; j < areaLength - 2; j++)//2～(length-3)層目まで
        //{
        //    if (areaData.halfway != null && j == Mathf.CeilToInt(areaLength / 2f))
        //    {
        //        for (int i = 0; i < 5; i++)
        //        {
        //            layer[i].straight = 2;
        //            layer[i].SetRoomEvent(areaData.halfway);
        //        }
        //    }
        //    else { for (int i = 0; i < 5; i++) { layer[i] = SetRoom(true); } }
        //    //for (int i = 0; i < 5; i++) { layer[i] = SetRoom(true); }
        //    layer[0].down = -1;
        //    layer[4].up = -1;
        //    SetLayer();
        //}

        //for (int i = 0; i < 5; i++) { layer[i] = SetRoom(true); }//length-2層目
        //layer[0].down = -1;
        //layer[0].straight = -1;
        //layer[0].up = 2;
        //layer[1].down = -1;
        //layer[3].up = -1;
        //layer[4].down = 2;
        //layer[4].straight = -1;
        //layer[4].up = -1;
        //SetLayer();

        //layer[0].empty = true;//length-1層目
        //layer[1] = SetRoom(true);
        //layer[1].up = 2;
        //layer[1].straight = -1;
        //layer[1].down = -1;
        //layer[2] = SetRoom(true);
        //layer[2].up = -1;
        //layer[2].straight = 2;
        //layer[2].down = -1;
        //layer[3] = SetRoom(true);
        //layer[3].up = -1;
        //layer[3].straight = -1;
        //layer[3].down = 2;
        //layer[4].empty = true;
        //SetLayer();

        //for (int i = 0; i < 5; i++) { layer[i].empty = true; }//length層目 休憩
        //layer[2].empty = false;
        //layer[2] = SetRoom(false, true);
        //layer[2].up = -1;
        //layer[2].straight = 2;
        //layer[2].down = -1;
        //layer[2].SetRoomEvent(Definer.inst.cp.roomRef.rest);
        //SetLayer();

        //for (int i = 0; i < 5; i++) { layer[i].empty = true; }//length+1層目 ボス
        //layer[2].empty = false;
        //layer[2] = SetRoom(false, true);
        //layer[2].up = -1;
        //layer[2].straight = 2;
        //layer[2].down = -1;
        //layer[2].SetRoomEvent(areaData.boss);
        //SetLayer();

        //for (int i = 0; i < 5; i++) { layer[i].empty = true; }//length+2層目(エリアの終端)
        //layer[2].empty = false;
        //layer[2].up = -1;
        //layer[2].straight = -1;
        //layer[2].down = -1;
        //layer[2] = SetRoom(false,true);
        //layer[2].SetRoomEvent(areaData.endArea);
        //SetLayer();

        for(int x = 0; x <= areaLength + 2; x++) { MakeLayer(x); }

        GenRoomEvent();
        map.SetMap(layers);

        map.EndGenerateMap();
    }

    void MakeLayer(int x)
    {
        List<ExpeditionManager.Room> layer = new List<ExpeditionManager.Room>();
        for (int i = 0; i < 5; i++)
        {
            layer.Add(new ExpeditionManager.Room());
        }

       
        if (x == 0)//スタート
        {
            layer[0].empty = true;
            layer[1].empty = true;

            layer[2].SetBranch_OnInit(2, 1, 2);
            layer[2].SetRoomEvent(areaData.start);

            layer[3].empty = true;
            layer[4].empty = true;
        }
        else if (x == 1)
        {
            layer[0].empty = true;

            layer[1].SetBranch_OnInit(0, 0, 2);
            randRooms.Add(layer[1]);
            randRooms.Add(layer[2]);
            layer[3].SetBranch_OnInit(2, 0, 0);
            randRooms.Add(layer[3]);

            layer[4].empty = true;
        }
        else if (2 <= x && x <= areaLength - 3)
        {
            if (areaData.halfway != null && x == Mathf.CeilToInt(areaLength / 2f))
            {
                for (int i = 0; i < 5; i++)
                {
                    layer[i].SetRoomEvent(areaData.halfway);
                }
            }

            layer[0].SetBranch_OnInit(0, 0, -1);
            layer[4].SetBranch_OnInit(-1, 0, 0);
            randRooms.AddRange(layer);
        }
        else if (x == areaLength - 2)
        {
            layer[0].SetBranch_OnInit(2, -1, -1);
            layer[1].SetBranch_OnInit(0, 0, -1);
            layer[3].SetBranch_OnInit(-1, 0, 0);
            layer[4].SetBranch_OnInit(-1, -1, 2);

            randRooms.AddRange(layer);
        }
        else if (x == areaLength - 1)
        {
            layer[0].empty = true;

            layer[1].SetBranch_OnInit(2, -1, -1);
            randRooms.Add(layer[1]);
            layer[2].SetBranch_OnInit(-1, 2, -1);
            randRooms.Add(layer[2]);
            layer[3].SetBranch_OnInit(-1, -1, 2);
            randRooms.Add(layer[3]);

            layer[4].empty = true;
        }
        else if (x == areaLength)
        {
            layer[0].empty = true;
            layer[1].empty = true;

            layer[2].SetBranch_OnInit(-1, 2, -1);
            layer[2].SetRoomEvent(Definer.inst.cp.roomRef.rest);

            layer[3].empty = true;
            layer[4].empty = true;
        }
        else if (x == areaLength+1)
        {
            layer[0].empty = true;
            layer[1].empty = true;

            layer[2].SetBranch_OnInit(-1, 2, -1);
            layer[2].SetRoomEvent(areaData.boss);

            layer[3].empty = true;
            layer[4].empty = true;
        }
        else if (x == areaLength + 2)
        {
            layer[0].empty = true;
            layer[1].empty = true;

            layer[2].SetBranch_OnInit(-1, 2, -1);
            layer[2].SetRoomEvent(areaData.endArea);

            layer[3].empty = true;
            layer[4].empty = true;
        }

        layers.Add(layer);
    }

    void GenRoomEvent()
    {
        GameParams gp = GameManager.gameParams;
        List<Room> TBDRooms = randRooms.Where(r => r.eventData == null).ToList();//まだ内容が決まっていない部屋を抽出

        List<Area_RoomEvent> garanteed = new List<Area_RoomEvent>();
        gp.roomEvents.ForEach(e =>
        {
            for (int i = 0; i < e.garanteed; i++)
            {
                garanteed.Add(e);
            }
        });//各イベントの最低保障を確保

        if (garanteed.Count > TBDRooms.Count) infoText.AddWarningText("各イベントの最低保証の合計が、部屋数を超過");
        List<float> weights = gp.roomEvents.Select(w => w.weight).ToList();
        TBDRooms.Shuffle().ForEach(e =>
        {
            if (garanteed.Count > 0)
            {
                e.SetRoomEvent(garanteed[garanteed.Count - 1].roomEvent);
                garanteed.RemoveAt(garanteed.Count - 1);
            }
            else
            {
                e.SetRoomEvent(gp.roomEvents[weights.ChoiceWithWeight()].roomEvent);
            }
        });

        List<bool> branchs = new List<bool>();
        List<bool> blinds = new List<bool>();
        randRooms.ForEach(r =>
        {
            if (r.up == 0 ) branchs.Add(false);
            if (r.down == 0) branchs.Add(false);

            if (!r.eventData.noBlind) blinds.Add(false);
        });

        //infoText.AddDebugText($"branchs:{branchs.Count}/{branchs.Count.Mul(gp.branchChance)}");
        //infoText.AddDebugText($"blinds:{blinds.Count}/{blinds.Count.Mul(gp.blindChance)}");

        for(int i = 0;i< branchs.Count.Mul(gp.branchChance); i++) { branchs[i] = true; }
        for(int i = 0;i< blinds.Count.Mul(gp.blindChance); i++) { blinds[i] = true; }

        branchs = branchs.Shuffle();
        blinds = blinds.Shuffle();

        int x = 0;
        int y=0;
        randRooms.ForEach(r =>
        {
            if (r.up == 0)
            {
                r.up = branchs[x] ? 1 : 0;
                x++;
            }
            if (r.straight == 0) r.straight = 1;
            if (r.down == 0)
            {
                r.down = branchs[x] ? 1 : 0;
                x++;
            }

            if (!r.eventData.noBlind)
            {
                r.blind = blinds[y];
                y++;
            }
        });

        //randRooms.ForEach(r =>
        //{
        //    r.blind = !r.eventData.noBlind && gp.blindChance.Dice();
        //    if (r.up == 0 && gp.branchChance.Dice()) r.up = 1;
        //    if (r.straight == 0) r.straight = 1;//ここは決定
        //    if (r.down == 0 && gp.branchChance.Dice()) r.down = 1;
        //});
    }

    //void SetLayer()
    //{
    //    map.SetLayerPanel(layer, layerCount);
    //    layers.Add(new List<ExpeditionManager.Room>(layer));
    //    layerCount++;
    //    for (int i = 0; i < layer.Length; i++)
    //    {
    //        layer[i] = new ExpeditionManager.Room();
    //    }
    //}


    //public ExpeditionManager.Room SetRoom(bool setEventRandomly,bool noBlind=false)
    //{
    //    ExpeditionManager.Room room = new ExpeditionManager.Room();
    //    if (GameManager.gameParams.branchChance.Dice()){ room.up = 1; }
    //    room.straight = 1;
    //    if (GameManager.gameParams.branchChance.Dice()) { room.down = 1; }
    //    room.blind = !noBlind && GameManager.gameParams.blindChance.Dice();
    //    if (setEventRandomly)
    //    {
    //        room.SetRoomEvent(areaData.roomEvents[areaData.GetREWeights().ChoiceWithWeight()].roomEvent);
    //    }
    //    //room.blind = !room.eventData.noBlind && areaData.blindChance.Dice();

    //    return room;
    //}

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
