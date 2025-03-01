using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StE_RgKnight : PA_StatusEffect
{
    [SerializeField]
    int goalPercent;
    public int ACT;
    int goalDMG;
    int currentDMG;

    bool f;
    public override void OnPAInit()
    {
        goalDMG = Mathf.RoundToInt(character.CharaStatus().maxHP * goalPercent / 100f);
        character.AddACT(ACT);
    }

    //public override void OnDamaged(Action.OnDamageParams onDamageParams)
    //{
    //    currentDMG += onDamageParams.totalDMG;
    //    if(currentDMG >= goalDMG) { Disable(); }
    //}

    public override void OnDecreasedHP(int value)
    {
        currentDMG += value;
        if (currentDMG >= goalDMG) { Disable(); }
    }

    public override void OnActivateAbility(List<Action.ActionResult> actionResultsList)
    {
        if (!f) { f = true; }
        else { Disable(); }
    }

    public override void AtTheEnd()
    {
        character.AddACT(-ACT);
    }

    public override string GetCurrentStateInfo()
    {
        return string.Format("å∏è≠ÇµÇΩHPÅF{0}/{1}", currentDMG, goalDMG);
    }
}
