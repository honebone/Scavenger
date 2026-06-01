using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class O_Beehive_PA : PA_Personality
{
    [SerializeField] int countReq;
    [SerializeField]
    Action.ActionStatus actionStatus;
    List<int> list = new List<int>() { 9, 10, 11, 12, 13, 14, 15, 16, 17 };
    List<int> empty = new List<int>();
    int count;

    public override void OnRoundStart()
    {
        count = 0;
        Log("カウントリセット");
    }

    public override void OnDamaged(Action.OnDamageParams onDamageParams)
    {
        if (onDamageParams.totalDMG > 0)
        {
            count++;
            LogCount(count);
            if (count == countReq)
            {
                count = 0;
                Action.ActionStatus action = actionStatus;
                actionStatus.actionOwner = character;
                empty = charactersManager.GetEmptyPos(list);
                action.actionTargetsInt = empty;
                int spawnCount = (50.Dice()) ? 2 : 1;
                character.Enqueue(action, false, new List<Character>(), spawnCount);
            }
        }
    }

    public override string GetPAInfo_Base()
    {
        return actionStatus.GetInfo();
    }

    public override string GetCurrentStateInfo()
    {
        return $"カウント：{count}/{countReq}";
    }
}
