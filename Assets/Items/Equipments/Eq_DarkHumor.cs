using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eq_DarkHumor : PA_Equipment
{
    [SerializeField]
    Action.ActionStatus actionStatus;
    [SerializeField] int maxRemain = 5;
    int remain;

    public override void OnBattleStart()
    {
        remain = maxRemain;
    }
    public override void OnRoundStart()
    {
        remain = maxRemain;
    }

    public override void OnSomeoneDied(Character died)
    {
        if (remain > 0 && ExpeditionRef.battleManager.GetCurrntTurnChara() == died && !died.CharaStatus().characterTags.Contains(CharacterData.CharacterTag.corpse))
        {
            remain--;
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
        return $"c‚è”­“®‰ñ”F{remain}/{maxRemain}";
    }
}
