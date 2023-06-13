using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chara_P_Infantry : Character
{
    [SerializeField]
    ActionData actionDataTest;
    [SerializeField]
    Action.ActionStatus actionStatus;
    private void Start()
    {
        actionStatus = new Action.ActionStatus();
        actionStatus.Init(actionDataTest);
    }
    public override void OnTurnStart()
    {
        Enqueue(actionStatus.actionObject, actionStatus);
        base.OnTurnStart();
    }
}
