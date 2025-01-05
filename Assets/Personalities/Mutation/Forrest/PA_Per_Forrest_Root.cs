using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PA_Per_Forrest_Root : PA_Personality
{
    [SerializeField]
    Character.CharaStatusMod statusMod1;
    [SerializeField]
    Character.CharaStatusMod statusMod2;

    bool activated;

    public override void OnBattleStart()
    {
        activated = true;
        character.ModifyStatus(statusMod1, true);
    }

    public override void OnMoved(Action.OnMoveParams onMoveParams)
    {
        if (activated)
        {
            Log("‹­‰»Œø‰Ê‚ð‘rŽ¸");
            activated = false;
            character.ModifyStatus(statusMod1, false);
            character.ModifyStatus(statusMod2, true);
        }
    }

    public override void OnBattleEnd()
    {
        if (activated)
        {
            character.ModifyStatus(statusMod1, false);
        }
        else
        {
            character.ModifyStatus(statusMod2, false);
        }
    }
}
