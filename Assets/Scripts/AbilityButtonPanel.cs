using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityButtonPanel : MonoBehaviour
{
    [SerializeField]
    GameObject abilityButton;
    [SerializeField]
    Transform parent;

    BattleManager battleManager;
    CharactersManager charactersManager;
    GuideMessage guideMessage;
    private void Start()
    {
        battleManager=FindObjectOfType<BattleManager>();
        charactersManager = FindObjectOfType<CharactersManager>();
        guideMessage = FindObjectOfType<GuideMessage>();
    }
    public void SetAbilityButtons(Ability.AbilityStatus[] abilitiesStatus,Character character)
    {
        for(int i = 0; i < parent.childCount; i++) { Destroy(parent.GetChild(i).gameObject); }
        for (int i = 0; i < abilitiesStatus.Length; i++) {
            var a = Instantiate(abilityButton, parent);
            a.GetComponent<AbilityButton>().Init(abilitiesStatus[i], battleManager, character, charactersManager,guideMessage);
        }
    }
    public void SetAbilityButtons_Deploy(Ability.AbilityStatus[] abilitiesStatus)
    {
        for(int i = 0; i < parent.childCount; i++) { Destroy(parent.GetChild(i).gameObject); }
        for (int i = 0; i < abilitiesStatus.Length; i++) {
            var a = Instantiate(abilityButton, parent);
            a.GetComponent<AbilityButton>().Init_Deploy(abilitiesStatus[i]);
        }
    }
}
