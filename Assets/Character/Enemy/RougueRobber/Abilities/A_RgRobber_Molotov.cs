using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class A_RgRobber_Molotov : Ability
{
    List<int> list = new List<int>() { 0, 1, 2, 3, 4, 5, 6, 7, 8 };

    public override List<List<int>> GetTargetPool(int index)
    {

        return new List<List<int>>() { new List<int>(list.Sample(3)) };
    }
}
