using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaE_TrapRoom : RE_RandomEvents
{
    [SerializeField] int HPDec;
    [SerializeField] int EXP;
     List<REOptionParams> options;

    public override void StartRandomEvent()
    {
        options = GenPlayerSelects();
        for(int i = 0; i < players.Count; i++)
        {
            options[i].optionInfo_suffix += $"\n成功確率：{players[i].CharaStatus().CRITC}％";
        }

        expeditionManager.SetREOptionButtons(options);
    }

    public override void SelectOption(int index)
    {
        StartCoroutine(C(players[index]));
    }

    IEnumerator C(Character selected)
    {
        infoText.SwitchToLog();
        infoText.AddLogText($"{selected.CharaStatus().charaName}が解除中...");
        yield return new WaitForSeconds(1.5f);

        if (selected.CharaStatus().CRITC.Dice())
        {
            infoText.AddLogText("解除成功！");
            selected.GainEXP(expeditionManager.GetExpAmount(EXP));
        }
        else
        {
            infoText.AddLogText("罠が作動！");
            selected.DecreaseHP_Per(HPDec);
        }
        EndRoomEvent();
    }
}
