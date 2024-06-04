using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StE_RgKnight : PA_StatusEffect
{
    [SerializeField]
    int goalPercent;
    int goalDMG;
    int currentDMG;

    bool f;
    public override void OnPAInit()
    {
        goalDMG = Mathf.RoundToInt(character.GetCharacterStatus().maxHP * goalPercent / 100f);
    }

    public override void OnDamaged(int DMG, Character attacker)
    {
        currentDMG += DMG;
        if(currentDMG >= goalDMG) { Disable(); }
    }

    public override void OnActivateAbility()
    {
        if (!f) { f = true; }
        else { Disable(); }
    }

    public override string GetAdditionalInfo()
    {
        return string.Format("現在の被ダメージ：{0}/{1}", currentDMG, goalDMG);
    }
}
