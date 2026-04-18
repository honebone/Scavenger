using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Per_OnRoundEnd : Per_Master
{
    public override void OnRoundEnd()
    {
        ResetParamsOnRoundEnd();
        Activate();
    }
}
