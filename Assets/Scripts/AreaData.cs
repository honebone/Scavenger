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
    public AudioClip BGM;
    public RoomEventData start;
    public RoomEventData halfway;
    public RoomEventData boss;
    public RoomEventData endArea;
    public List<AreaManager.EnemySet> normalBattlePool_tier1;
    public List<AreaManager.EnemySet> normalBattlePool_tier2;

    public List<AudioClip> battleBGM;
    public List<GameObject> uniqueRaE;
    public int applyFEChance;
    public List<GameObject> normalBattleFEPool;

    public GameObject GetRandomFE()
    {
        if (applyFEChance.Dice())
        {
            return normalBattleFEPool.Choice();
        }
        else { return null; }
    }
}
