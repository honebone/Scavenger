using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Per_OnBattleStart : Per_Master
{
    public override void OnBattleStart()
    {
        ResetParams();
        Activate();
    }
}
