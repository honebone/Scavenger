using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eq_OnMoved : Eq_Master
{
    [SerializeField,Header("ˆÚ“®‹——£•ª”­“®‚·‚é‚©")] bool eachRange;   
    public override void OnMoved(Action.OnMoveParams onMoveParams)
    {
        int active = eachRange ? onMoveParams.range : 1;
        for (int i = 0; i < active; i++) Activate();
    }
}
