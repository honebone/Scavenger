using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "gameParams")]

public class GameParams : ScriptableObject
{
    [Header("\n\n\nStatus")]
    public int maxLVL = 10;
    [Header("\n\n\nLevel Up\nEXPBase x エリア数が基本のEXP量")]
    public List<int> EXP_reqs;
    public float EXPBase;
    public float EXP_Boss = 5;
    public List<int> unlockEqSlotLVL;
    public StatusGrowth playerStatusGrowth;
    public StatusGrowth enemyStatusGrowth;
    [Header("\n\n\nMadness")] public float madnessSpawnChance;
    public Character.CharaStatusMod madnessStatMod;
    [Header("精神崩壊時にHPが[HPDecOnAffrict]%減少")] public int HPDecOnAffrict;
    public int SANDMGChanceOnRoom;
    public Vector2Int SANDMGOnRoom;
    public int RegenPercentOnRoomEnd;
    public int SANPenaltyOnDie;
    [Header("\n\n\nマップ生成")]
    public int areaLength;
    public int branchChance;
    public int blindChance;
    [Header("\n\n\n特性")]
    public List<int> perWeights;
    public int maxPer_good = 4;
    public int maxPer_bad = 4;
}
