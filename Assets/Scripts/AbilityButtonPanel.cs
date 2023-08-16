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
    private void Start()
    {
        battleManager=FindObjectOfType<BattleManager>();
    }
    public void SetAbilityButtons(Ability.AbilityStatus[] abilitiesStatus,Character character)
    {
        for(int i = 0; i < parent.childCount; i++) { Destroy(parent.GetChild(i).gameObject); }
        for (int i = 0; i < abilitiesStatus.Length; i++) {
            var a = Instantiate(abilityButton, parent);
            a.GetComponent<AbilityButton>().Init(abilitiesStatus[i], battleManager, character);
        }
       
    }
}
