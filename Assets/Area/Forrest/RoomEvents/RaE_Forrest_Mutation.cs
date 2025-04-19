using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaE_Forrest_Mutation : RoomEvent
{
    [SerializeField] List<GameObject> mutations;

    protected int choice = 0;
    /// <summary> 0:deploy 1:selectArea </summary>
    int phase;
    //GameObject eventManager;
    List<CharacterData> deployPool = new List<CharacterData>();

    GameObject selectedMutation;
    List<Character> characters = new List<Character>();

    public override void OnEndREInfo()
    { 

        phase = 0;
        SelectMutation();
    }
    void SelectMutation()
    {
        List<REOptionParams> list = new List<REOptionParams>();
        foreach (GameObject mutation in mutations)
        {
            PassiveAbility PA=mutation.GetComponent<PassiveAbility>();
            string title = PA.GetPAName();
            string info = $"îCà”ÇÃÉLÉÉÉâÇ™ì¡ê´<{PA.GetPAName()}>ÇìæÇÈ\n\n<{PA.GetPAName()}>\n{PA.GetPAInfo().ColorStr(Color.gray)}";
            REOptionParams option = new REOptionParams();
            option.optionName =title;
            option.optionInfo = info;
            list.Add(option);
        }
        expeditionManager.SetREOptionButtons(list);
    }
    void SelectChara()
    {
        List<REOptionParams> list = new List<REOptionParams>();
        characters = characterManager.GetExistingCharacters_All();
        foreach (Character chara in characters)
        {
            REOptionParams option = new REOptionParams();
            option.optionName = chara.CharaStatus().charaName;
            option.optionInfo = $"{chara.CharaStatus().charaName}Ç™{selectedMutation.GetComponent<PassiveAbility>().GetPAName()}ÇälìæÇ∑ÇÈ";
            list.Add(option);
        }
        expeditionManager.SetREOptionButtons(list);
    }
    
    public override void SelectOption(int index)
    {
        choice = index;

        if (phase == 0)
        {
            selectedMutation = mutations[choice];

            phase = 1;
            SelectChara();
        }
        else if (phase == 1)
        {
            expeditionManager.SetPersonality(characters[choice], selectedMutation);
            //characters[choice].AddPA_Personality(selectedMutation, true);
            EndRoomEvent();
        }
    }
}
