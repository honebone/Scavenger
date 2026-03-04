using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Per_SurvivalInstinct : PA_Personality
{
    [SerializeField] int HPTH;
    [SerializeField] int EVD;

    bool active;
    public override void OnDecreasedHP(int value)
    {
        if (character.CharaStatus().GetHPPercent() <= HPTH) { SetActive(true); }
    }
    public override void OnHealed(Character healer, Action.OnHealParams onHealParams)
    {
        if (character.CharaStatus().GetHPPercent() > HPTH) { SetActive(false); }
    }
    public override void OnBattleEnd()
    {
        SetActive(false);
    }
    void SetActive(bool set)
    {
        if (set && !active)
        {
            active = true;
            character.AddEVD(EVD);
            Log("発動！");
        }
        else if (!set && active)
        {
            active = false;
            character.AddEVD(-EVD);
            Log("効果リセット");
        }
    }

    public override string GetCurrentStateInfo()
    {
        return active ? "効果発動中" : "効果未発動";
    }
}
