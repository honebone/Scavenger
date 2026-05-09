using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Per_MagicAuthority : PA_Personality
{
    [SerializeField] private int magicReq = 3;
    int count;
    public override void OnBattleStart()
    {
        count = character.GetEquipments().Count(e => e.PATags.Contains(PATag.魔術)) / magicReq;
        character.AddTurnPerRound(count);
        Log($"ラウンドごとターン数+{count}");
    }

    public override void OnBattleEnd()
    {
        if (count > 0)
        {
            character.AddTurnPerRound(-count);
            count = 0;
        }
    }
}
