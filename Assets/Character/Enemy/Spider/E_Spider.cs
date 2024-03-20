using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E_Spider : Character
{
    [SerializeField]
    CharactersManager.SearchCharaCondition condition;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public override Ability.AbilityStatus SelectAbility_Random()
    {
        List<Ability.AbilityStatus> abilities = new List<Ability.AbilityStatus>(GetAvailableAbilitiesStatus(true));
        if (charactersManager.SearchCharaWithCondition(condition).Count > 0)//’wå‚Ì‘ƒ‚ª•t—^‚³‚ê‚Ä‚¢‚é“G‚ª‚¢‚é‚È‚ç
        {
            foreach(Character character in charactersManager.SearchCharaWithCondition(condition))
            {
                infoText.AddDebugText(string.Format("{0},{1}", character.GetCharacterStatus().charaName, character.GetCharacterStatus().position.PosIntToStr()));
            }
            abilities.Add(charaStatus.abilitiesStatus[2]);//èŒJ‚èŠñ‚¹‚ğƒv[ƒ‹‚É’Ç‰Á
        }
        return ChoiceAbilityWithWeight(abilities);
    }
}
