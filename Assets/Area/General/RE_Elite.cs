using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RE_Elite : RoomEvent
{
    public ExpeditionManager.BattleParams battleParams;
    [SerializeField] int rareChance;
    public override void StartRoomEvent()
    {
        List<AreaManager.EnemySet> waves = new List<AreaManager.EnemySet>();
        waves.Add(expeditionManager.GetNormalBattleEnemySet());

        expeditionManager.Battle(waves, currentArea.GetRandomFE(), battleParams);
    }
    public override void OnEndBattle()
    {
        supplyManager.AddSupply_Eq(partyStatus.supplyOptions);
        lootPanel.Loot();
    }

    GameObject per;
    PassiveAbility PA;
    public override void OnEndSupply()
    {
        PA_Personality.PersonalityStatus.PersonalityType type = rareChance.Dice() ? PA_Personality.PersonalityStatus.PersonalityType.awoken : PA_Personality.PersonalityStatus.PersonalityType.good;
        per = expeditionManager.GetPer_Random_CertainType(type)[0];
        PA = per.GetComponent<PassiveAbility>();
        string info = $"特性<{PA.GetPAName()}>を手に入れた！\nキャラ1体に付与することができる";
        expeditionManager.SetREInfo("報酬", info);
    }

    public override void OnEndREInfo()
    {
        List<REOptionParams> options = new List<REOptionParams>();
        characterManager.GetExistingCharacters_All().ForEach(p =>
        {
            REOptionParams option = new REOptionParams();
            option.optionName = p.CharaStatus().charaName;

            option.optionInfo = $"{p.CharaStatus().charaName}が特性<{PA.GetPAName()}>を得る\n\n{PA.GetPAName()}：\n{PA.GetPAInfo()}";

            options.Add(option);
        });

        REOptionParams option_exit;

        option_exit = new REOptionParams();
        option_exit.optionName = "スキップ";
        option_exit.optionInfo = "特性を得ずに進む";

        options.Add(option_exit);

        expeditionManager.SetREOptionButtons(options);
    }

    public override void SelectOption(int index)
    {
        if(index < characterManager.GetExistingCharacters_All().Count)
        {
            expeditionManager.SetPersonality(characterManager.GetExistingCharacters_All()[index], per);
        }
        EndRoomEvent();
    }
}
