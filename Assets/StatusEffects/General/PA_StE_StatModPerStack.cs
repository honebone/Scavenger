using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PA_StE_StatModPerStack : PA_StatusEffect
{
    public Character.CharaStatusMod statModPerStack;

    public override void OnPAInit()
    {
        applyMod(StEStatus.stack);
    }

    public override void OnAddStack(int add)
    {
        applyMod(add);
    }

    public override void AtTheEnd()
    {
        applyMod(-StEStatus.stack);
    }

    void applyMod(int loop)
    {
        if (loop > 0)
        {
            for(int i = 0; i < loop; i++)
            {
                character.ModifyStatus(statModPerStack, true);
            }
        }
        else
        {
            for (int i = 0; i > loop; i--)
            {
                character.ModifyStatus(statModPerStack, false);
            }
        }
    }

    public override string GetAdditionalInfo()
    {
        return $"スタックごとに、\n{statModPerStack.GetInfo()}";
    }
}
