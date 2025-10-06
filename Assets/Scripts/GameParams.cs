using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "gameParams")]

public class GameParams : ScriptableObject
{
    [Header("\n\n\nStatus")]
    public int maxLVL = 10;
    [Header("\n\n\nLevel Up")]
    public List<int> unlockEqSlotLVL;
    [Header("\n\n\nMadness")] public float madnessSpawnChance;
    public Character.CharaStatusMod madnessStatMod;
   [Header("¸_•ö‰ó‚ÉHP‚ª[HPDecOnAffrict]%Œ¸­")] public int HPDecOnAffrict;
    public int SANDMGChanceOnRoom;
    public Vector2Int SANDMGOnRoom;
}
