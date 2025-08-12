using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "gameParams")]

public class GameParams : ScriptableObject
{
    public float madnessSpawnChance;
    public Character.CharaStatusMod madnessStatMod;
}
