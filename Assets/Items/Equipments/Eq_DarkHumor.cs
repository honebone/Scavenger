using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eq_DarkHumor : PA_Equipment
{
    [SerializeField]
    Action.ActionStatus actionStatus;

    bool activated;

    public override void OnBattleStart()
    {
        activated = false;
    }

    public override void OnSomeoneDied(Character died)
    {
        if (ExpeditionRef.battleManager.GetCurrntTurnChara() == died)
        {
            Enqueue_Self(actionStatus);
        }
    }

    public override string GetPAInfo_Base()
    {
        string s = equipmentStatus.GetInfo();
        s += actionStatus.GetInfo(false, new Character.CharacterStatus());
        return s;
    }

    public override string GetCurrentStateInfo()
    {
        return activated ? "”\—Í”­“®Ï‚İ" : "”\—Í”­“®‰Â”\";
    }
}
