using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityButtonPanel : MonoBehaviour
{
    [SerializeField]
    GameObject abilityButton;
    [SerializeField]
    Transform parent;
    public void SetAbilityButtons(Ability.AbilityStatus[] abilitiesStatus)
    {
        for(int i = 0; i < parent.childCount; i++) { Destroy(parent.GetChild(i).gameObject); }
        foreach(Ability.AbilityStatus ability in abilitiesStatus)
        {
            var a = Instantiate(abilityButton, parent);
            a.GetComponent<AbilityButton>().Init(ability);
        }
    }
}
