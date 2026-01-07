using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "ScriptableObjects/AreaData")]
public class AreaData : ScriptableObject
{
    public string areaName;
    [TextArea(3, 10)] public string areaInfo;

    public int tier;
    public BackgroundParams backgroundParams;
    public GameObject areaManager;

    //[Header("ランダム生成される層の数(スタート地点,ボス後を除く)")]
    //public int minLength;
    //public int maxLength;

    //public int branchChance;
    //public int blindChance;
    public AudioClip BGM;
    public List<AreaManager.Area_RoomEvent> roomEvents;
    public RoomEventData start;
    public RoomEventData halfway;
    public RoomEventData boss;
    public RoomEventData endArea;
    public List<AreaManager.EnemySet> normalBattlePool;
    public List<AudioClip> battleBGM;
    //public FieldEffectWeight[] normalBattleFEPool;
    public int applyFEChance;
    public List<GameObject> normalBattleFEPool;
    //nextArea

    public List<float> GetREWeights()
    {
        List<float> weights = new List<float>();
        foreach (AreaManager.Area_RoomEvent roomEvent in roomEvents) { weights.Add(roomEvent.weight); }
        return weights;
    }
    public AreaManager.EnemySet GetRandomEnemySet()
    {
       
        return normalBattlePool.Choice();
    }
    public GameObject GetRandomFE()
    {
        if (applyFEChance.Dice())
        {
            return normalBattleFEPool.Choice();
        }
        else { return null; }
    }
}
