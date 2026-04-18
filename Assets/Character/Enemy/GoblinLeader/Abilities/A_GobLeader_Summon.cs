using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class A_GobLeader_Summon : Ability
{
    List<int> list = new List<int>() { 9, 10, 11, 12, 13, 14, 15, 16, 17 };
    List<int> empty = new List<int>();

    public override List<List<int>> GetTargetPool(int index)
    {

        if (index == 0)
        {
            empty = charactersManager.GetEmptyPos(list);
            return new List<List<int>>() { new List<int>(empty.Sample(3)) };
        }
        else return base.GetTargetPool(index);
    }
}
