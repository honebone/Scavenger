using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Eq_Katana : PA_Equipment
{
    [SerializeField] int _ATKPerCount;
    [SerializeField] int _maxCount;
    int _count;
    public override void OnDamage(List<Action.OnDamageParams> onDamageParamsList)
    {
        onDamageParamsList.ForEach(x =>
        {
            if (x.CRIT && _count < _maxCount)
            {
                _count++;
                character.AddATK(0, _ATKPerCount);
                Log($"{"ATK".ToSpr_withName()}+{_ATKPerCount}％ (+{_count * _ATKPerCount}％)");
            }
        });
    }

    public override void OnBattleEnd()
    {
        character.AddATK(0, -_count * _ATKPerCount);
        _count = 0;
    }

    public override string GetCurrentStateInfo()
    {
        return $"カウント：{_count}/{_maxCount} ({"ATK".ToSpr_withName()}+{_count * _ATKPerCount}％)";
    }
}
