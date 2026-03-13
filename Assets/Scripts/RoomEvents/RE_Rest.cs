using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class RE_Rest : RoomEvent
{
    [SerializeField]
    List<RoomEvent.REOptionParams> options;
    List<RoomEvent.REOptionParams> options2;
    int choice;

    [SerializeField]
    GameObject umbushed;

    [SerializeField] float healRatio_rest = 0.2f;
    [SerializeField] int SANHeal_rest = 10;
    [SerializeField] int SANHeal_banquet = 20;
    [SerializeField] float healRatio_sleep = 0.5f;
    [SerializeField] int SANHeal_sleep = 15;
    [SerializeField] int SANHeal_meditate = 20;
    [SerializeField, Header("x,yの範囲")] Vector2Int RemovePer;
    [SerializeField] float AddExp_train;

    int phase;
    int mode = 0;
    List<Character> pool = new List<Character>();

    public override void OnEndREInfo()
    {
        pool = new List<Character>();
        foreach (Character c in characterManager.GetExistingCharacters_All())
        {
            if (c.CharaStatus().playable) { pool.Add(c); }
        }

        List<REOptionParams> ops = new List<REOptionParams>(options);
        REOptionParams train = new REOptionParams();
        train.optionName = "訓練する";
        train.optionInfo = $"キャラを1体選ぶ\nそのキャラは{"EXP".ColorStr(Definer.colorRef.expOrb)}を{expeditionManager.GetExpAmount(AddExp_train)}得る";
        ops.Add(train);
        expeditionManager.SetREOptionButtons(ops);
    }
    public override void SelectOption(int index)
    {
        if (phase == 0)
        {
            phase++;
            choice = index;
            StartCoroutine(Consequence());
        }
        else if (phase == 1)
        {
            if (index >= pool.Count) InfoText.inst.AddErrorText($"選択したインデックスが想定外です:{index}");
            Character selected = pool[index];
            if (mode == 1)
            {
                Meditate(selected);
            } else if (mode == 2)
            {
                Train(selected);
            }
            else InfoText.inst.AddErrorText($"選択したモードが想定外です:{mode}");

            EndRoomEvent();
        }
    }
    IEnumerator Consequence()
    {
        switch (choice)
        {
            case 0:
                foreach (Character chara in characterManager.GetExistingCharacters_All())
                {
                    int maxHP = chara.CharaStatus().maxHP;
                    chara.Heal(Mathf.RoundToInt(maxHP * healRatio_rest), null);
                    chara.SANHeal(SANHeal_rest);
                }
                EndRoomEvent();
                break;
            case 1:
                foreach (Character chara in characterManager.GetExistingCharacters_All())
                {
                    chara.SANHeal(SANHeal_banquet);
                }
                EndRoomEvent();
                break;
            case 2:
                for (int i = 0; i < 3; i++)
                {
                    yield return new WaitForSeconds(0.5f);
                    infoText.AddLogText("");
                }
                if (20.Dice())
                {
                    expeditionManager.Battle(new List<AreaManager.EnemySet> { currentArea.GetRandomEnemySet() }, umbushed, new ExpeditionManager.BattleParams());
                }
                else
                {
                    foreach (Character chara in characterManager.GetExistingCharacters_All())
                    {
                        int maxHP = chara.CharaStatus().maxHP;
                        chara.Heal(Mathf.RoundToInt(maxHP * healRatio_sleep), null);
                        chara.SANHeal(SANHeal_sleep);
                    }
                    EndRoomEvent();
                }
                break;
            case 3:
                mode = 1;
                options2 = new List<REOptionParams>();
                foreach (Character character in pool)
                {
                    Character.CharacterStatus status = character.CharaStatus();
                    REOptionParams option = new REOptionParams();
                    option.optionName = $"{status.charaName}に瞑想させる";
                    options2.Add(option);
                }

                expeditionManager.SetREOptionButtons(options2);
                break;
            case 4:
                mode = 2;
                options2 = new List<REOptionParams>();
                foreach (Character character in pool)
                {
                    Character.CharacterStatus status = character.CharaStatus();
                    REOptionParams option = new REOptionParams();
                    option.optionName = $"{status.charaName}に訓練させる";
                    option.optionInfo = $"取得{"EXP".ToSpr_withLink()}：{expeditionManager.GetExpAmount(AddExp_train)}\n現在の{character.CharaStatus().GetExpInfo()}";
                    options2.Add(option);
                }

                expeditionManager.SetREOptionButtons(options2);
                break;
        }
    }

   void Meditate(Character chara)
    {
        chara.RemovePer_Random(RemovePer.Range(), PA_Personality.PersonalityStatus.PersonalityType.bad);
        chara.SANHeal(SANHeal_meditate);
    }
    void Train(Character chara)
    {
        chara.GainEXP(expeditionManager.GetExpAmount(AddExp_train));
    }
}
