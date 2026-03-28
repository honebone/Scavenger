using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Per_Defaulter : PA_Personality
{
    [SerializeField] int coin;
    public override void OnBattleEnd()
    {
        Log($"-<sprite name=coin><color=#D83ECE>{coin}</color>");
        Inventory.inst.RemoveCoin(coin);
    }
}
