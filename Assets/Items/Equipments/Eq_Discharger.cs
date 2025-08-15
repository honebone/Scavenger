using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eq_Discharger : PA_Equipment
{
    [SerializeField] List<Vector2Int> neigbors;
    [SerializeField] Action.ActionStatus attack;
    [SerializeField] int chargeOnBS;
    [SerializeField] int chargeOnTS;
    [SerializeField] int maxCharge;
    int charge;
    List<Character> attackedChara=new List<Character>();

    public override void OnBattleStart()
    {
        charge += chargeOnBS;
    }

    public override void OnDamage(List<Action.OnDamageParams> onDamageParamsList)
    {
        if (charge > 0)
        {
            List<Character> targetPool = new List<Character>();
            foreach (Action.OnDamageParams onDamageParams in onDamageParamsList)
            {
                if (onDamageParams.totalDMG > 0)
                {
                    List<Character> neigborsChara = charactersManager.GetCharactersWithPos(onDamageParams.target.CharaStatus().position.RelPosToAbs(neigbors));
                    //attackedChara.AddRangeWithNoOverlap(neigborsChara);
                    foreach (Character c in neigborsChara)
                    {
                        if (!attackedChara.Contains(c))
                        {
                            targetPool.Add(c);
                            //attackedChara.Add(c);
                        }
                    }
                }
            }

            if (targetPool.Count > 0)
            {
                Character target = targetPool.Choice();
                attackedChara.Add(target);
                charge--;
                Enqueue(attack, true, new List<Character> { target}, 1);
            }
        }
    }
    public override void OnTurnStart(bool myTurn, int turnCount)
    {
        attackedChara.Clear();
        if (myTurn)
        {
            charge += chargeOnTS;
            charge = charge.Limit(maxCharge);
            Log($"チャージ増加({charge})");
        }
    }

    public override void OnBattleEnd()
    {
        charge = 0;
        attackedChara.Clear();
    }
    public override string GetPAInfo_Base()
    {
        string s = equipmentStatus.GetInfo();
        s += "\n"+attack.GetInfo(false, new Character.CharacterStatus());
        return s;
    }
    public override string GetCurrentStateInfo()
    {
        return $"現在のチャージ：{charge}/{maxCharge}";
    }
}
