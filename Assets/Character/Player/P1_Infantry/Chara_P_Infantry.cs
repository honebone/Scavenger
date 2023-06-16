using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chara_P_Infantry : Character
{
    [SerializeField]
    Action.ActionStatus actionStatusTest;
   
    public override void OnTurnStart()
    {
        Enqueue(actionStatusTest);
        base.OnTurnStart();
    }
}
