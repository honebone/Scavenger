using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "ScriptableObjects/AreaData")]
public class AreaData : ScriptableObject
{
    public string areaName;

    public GameObject background;

    [Header("ランダム生成される層の数(スタート地点,ボス後を除く)")]
    public int minLength;
    public int maxLength;

    public int branchChance;
    public int blindChance;
    public List<AreaManager.Area_RoomEvent> roomEvents;
    public RoomEventData boss;
    public RoomEventData endArea;
    public List<AreaManager.EnemySet> normalBattlePool;
    //public FieldEffectWeight[] normalBattleFEPool;
    public int applyFEChance;
    public List<GameObject> normalBattleFEPool;
    //nextArea

    public List<int> GetREWeights()
    {
        List<int> weights = new List<int>();
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
