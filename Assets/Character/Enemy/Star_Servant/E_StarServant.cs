using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class E_StarServant : PA_Personality
{
    public Action.ActionStatus actionStatus;
    public ActionMod.ActionModStatus mod;

    public override void OnTurnStart(bool myTurn, int turnCount)
    {
        if (myTurn)
        {
            int row = character.CharaStatus().position.GetRow();
            var enemyDMG = charactersManager.GetExistingCharacters_All().Where(m => m.PlayerPos() != character.PlayerPos() && m.CharaStatus().position.GetRow() == row);
            if (enemyDMG.Count() > 3)
            {
                infoText.AddErrorText("“G”‚ÌŽæ“¾ƒGƒ‰[");
            }

            int stack = 3 - enemyDMG.Count();
            if (stack > 0)
            {
                Action.ActionStatus action = actionStatus;
                for (int i = 0; i < stack; i++)
                {
                    action = action.Modify(mod);
                }
                Enqueue_Self(action);
            }
        }
    }

    public override string GetPAInfo_Base()
    {
        string s = actionStatus.GetInfo() + "\n";
        return s;
    }
}
