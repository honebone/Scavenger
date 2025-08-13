using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eq_ConquerorMace : PA_Equipment
{
    [SerializeField] Action.ActionStatus actionStatus;
    [SerializeField] List<Vector2Int> neigbors;
    [SerializeField] int ATKRatio;
    [SerializeField] int maxATKRatio;
    [SerializeField] int HPRatio;
    [SerializeField] int maxHPRatio;

    int maxATK;
    int maxHP;

    int ATK;
    int HP;

    public override void OnBattleStart()
    {
        maxATK = Mathf.FloorToInt(character.CharaStatus().BaseATK() * maxATKRatio / 100f);
        maxHP = Mathf.FloorToInt(character.CharaStatus().BaseHP() * maxHPRatio / 100f);
    }


    public override void OnKill(List<Action.OnKillParams> onKillParamsList)
    {
        List<Character> pool = new List<Character>();
        foreach (Action.OnKillParams onKillParams in onKillParamsList)
        {
            Character.CharacterStatus status = onKillParams.target.CharaStatus();
            if (!status.Obstacle())
            {
                List<int> neigbor = onKillParams.target.CharaStatus().position.RelPosToAbs(neigbors);
                pool.AddRangeWithNoOverlap(charactersManager.GetCharactersWithPos(neigbor));

                if (ATK < maxATK)
                {
                    int atk = Mathf.FloorToInt(status.ATK * ATKRatio / 100f);
                    atk = (ATK + atk >= maxATK) ? maxATK - ATK : atk;

                    ATK += atk;
                    Log($"{"ATK".ToSpr_withName()}+{atk} ({ATK})");
                    character.AddATK(0, 0, atk);

                }

                if (HP < maxHP)
                {
                    int hp = Mathf.FloorToInt(status.maxHP * HPRatio / 100f);
                    hp = (HP + hp >= maxHP) ? maxHP - HP : hp;

                    HP += hp;
                    Log($"{"maxHP".ToSpr_withName()}+{hp} ({HP})");
                    character.AddMaxHP(0, 0, true, hp);
                }
            }
        }
        if (pool.Count > 0)
        {
            Enqueue(actionStatus, true, pool);
        }
    }

    public override void OnBattleEnd()
    {
        character.AddATK(0, 0, -ATK);
        character.AddMaxHP(0, 0, true, -HP);

        maxATK = 0;
        maxHP = 0;

        ATK = 0;
        HP = 0;
    }

    public override string GetPAInfo_Base()
    {
        string s = equipmentStatus.GetInfo();
        s += "\n" + actionStatus.GetInfo(false, new Character.CharacterStatus());
        return s;
    }

    public override string GetCurrentStateInfo()
    {
        return $"ATKëùâ¡ó ÅF{ATK}/{maxATK}\nmaxHPëùâ¡ó ÅF{HP}/{maxHP}";
    }
}
