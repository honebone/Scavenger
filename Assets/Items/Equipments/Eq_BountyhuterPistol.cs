using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eq_BountyhuterPistol : PA_Equipment
{
    [SerializeField] Action.ActionStatus actionStatus;
    [SerializeField] CharactersManager.SearchCharaCondition condition;
    [SerializeField] int maxCount;
    [SerializeField] int ATKPerCOunt;
    int count;

    public override void OnConsumeFocus(Action.ActionParams actionParams)
    {
        if (count < maxCount)
        {
            count++;
            Log($"カウント増加({count})");
            character.AddATK(0, ATKPerCOunt);
        }
    }

    public override void OnTurnStart(bool myTurn, int turnCount)
    {
        if (myTurn)
        {
            Enqueue_SearchTarget(actionStatus, condition, 1);
        }
    }

    public override void OnBattleEnd()
    {
        character.AddATK(0, -ATKPerCOunt * count);
        count =0;
    }

    public override string GetPAInfo_Base()
    {
        string s = equipmentStatus.GetInfo();
        s += "\n" + actionStatus.GetInfo(false, new Character.CharacterStatus());
        return s;
    }
    public override string GetCurrentStateInfo()
    {
        return $"現在のカウント：{count}/{maxCount}";
    }
}
