using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaManager : MonoBehaviour
{
    [System.Serializable]
    public struct Area
    {
        public string areaName;

        public GameObject background;

        [Header("ƒ‰ƒ“ƒ_ƒ€¶¬‚³‚ê‚é‘w‚Ì”(ƒXƒ^[ƒg’n“_,ƒ{ƒXŒã‚ğœ‚­)")]
        public int minLength;
        public int maxLength;

        public int branchChance;
        public int blindChance;
        //roomEvent
        //nextArea
    }
    [SerializeField]
    Area area;
    [SerializeField]
    GameObject content;
    [SerializeField]
    GameObject layerPanel;

    ExpeditionManager.Room[] layer;

    List<Map_LayerPanel> layers;
    ExpeditionManager expeditionManager;
    Utility util;
    private void Start()
    {
        expeditionManager = FindObjectOfType<ExpeditionManager>();
        util = FindObjectOfType<Utility>();
        layer = new ExpeditionManager.Room[5];
    }

    public virtual void GenerateMap()
    {
        int length = Random.Range(area.minLength, area.maxLength + 1);
        int layerCount = 0;
        layers = new List<Map_LayerPanel>();

        for (int i = 0; i < 5; i++) { layer[i].empty = true; }//0‘w–Ú
        layer[2].empty = false;
        layer[2].up = 2;
        layer[2].straight = 1;
        layer[2].down = 2;
        //layer[2]‚ÌroomEvent‚ğŠJn’n“_‚Ì“z‚É
        SetLayerPanel(layer, layerCount);
        layerCount++;

        layer[0].empty = true;//1‘w–Ú
        layer[1] = SetRoom();
        layer[1].down = 2;
        layer[2] = SetRoom();
        layer[3] = SetRoom();
        layer[3].up = 2;
        layer[4].empty = true;
        SetLayerPanel(layer, layerCount);
        layerCount++;

        for(int j = 2; j < length - 2; j++)//2`(length-3)‘w–Ú‚Ü‚Å
        {
            for (int i = 0; i < 5; i++) { layer[i] = SetRoom(); }
            layer[0].down = -1;
            layer[4].up = -1;
            SetLayerPanel(layer, layerCount);
            layerCount++;
        }

        for (int i = 0; i < 5; i++) { layer[i] = SetRoom(); }//length-2‘w–Ú
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

        layer[0].empty = true;//length-1‘w–Ú
        layer[1] = SetRoom();
        layer[1].up = 2;
        layer[1].straight = -1;
        layer[1].down = -1;
        layer[2] = SetRoom();
        layer[2].up = -1;
        layer[2].straight = 2;
        layer[2].down = -1;
        layer[3] = SetRoom();
        layer[3].up = -1;
        layer[3].straight = -1;
        layer[3].down = 2;
        layer[4].empty = true;
        SetLayerPanel(layer, layerCount);
        layerCount++;

        for (int i = 0; i < 5; i++) { layer[i].empty = true; }//length‘w–Ú
        layer[2].empty = false;
        layer[2].up = -1;
        layer[2].straight = 1;
        layer[2].down = -1;
        //layer[2]‚ÌroomEvent‚ğŠJn’n“_‚Ì“z‚É
        SetLayerPanel(layer, layerCount);

        expeditionManager.SetLayers(layers);
    }
    void SetLayerPanel(ExpeditionManager.Room[] l,int lc)
    {
        var lp = Instantiate(layerPanel, content.transform);
        lp.GetComponent<Map_LayerPanel>().Init(l, lc,expeditionManager);
        layers.Add(lp.GetComponent<Map_LayerPanel>());
    }
    ExpeditionManager.Room SetRoom()
    {
        ExpeditionManager.Room room = new ExpeditionManager.Room();
        if (util.Probability(area.branchChance)){ room.up = 1; }
        room.straight = 1;
        if (util.Probability(area.branchChance)) { room.down = 1; }
        if (util.Probability(area.blindChance)) { room.blind = true; }
        //roomEventİ’è

        return room;
    }
}
