using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class AbilityButton : MonoBehaviour
{
    [SerializeField]
    Sprite[] frames;

    [SerializeField]
    Image frame;
    [SerializeField]
    Image locked;
    [SerializeField]
    Text nameText;
    [SerializeField]
    Text remainText;
    [SerializeField]
    Text cooldownText;

    Ability.AbilityStatus abilityStatus;
    BattleManager battleManager;
    Character character;
    CharactersManager charactersManager;

    bool available;

    public void Init(Ability.AbilityStatus status,BattleManager bm,Character chara,CharactersManager cm)
    {
        abilityStatus = status;
        battleManager = bm;
        character = chara;
        charactersManager = cm;

        nameText.text = abilityStatus.abilityName;
        available = abilityStatus.CheckAvailable(character,cm);
        if (abilityStatus.locked) { locked.enabled = true; }
        if (!available) { nameText.color = Color.red; }
        if (abilityStatus.cooldown > 0) { 
            cooldownText.text = abilityStatus.cooldown.ToString(); 
        
        }
        if (abilityStatus.hasRemain)
        {
            remainText.text = abilityStatus.remain.ToString();
            if (abilityStatus.remain == 0) { remainText.color = Color.red; }
        }
        frame.sprite = frames[(int)abilityStatus.abilityType];
        frame.color = Definer.colorRef.abilityColors[(int)abilityStatus.abilityType];
    }

    public void OnMouseDown()
    {
        battleManager.SetSelectedAbility(abilityStatus, character);
        FindObjectOfType<InfoText>().SetText(abilityStatus.abilityName.ColorStr(Definer.colorRef.abilityColors[(int)abilityStatus.abilityType]), BattleManager.selectedAbility.GetInfo());
        charactersManager.ResetAllTargetIcons();
        if (battleManager.checkIfMyTurn(character) && BattleManager.selectingAbility&&available) //自分のターン中かつアビリティ選択中なら、対象選択開始      
        {
            BattleManager.selectedAbility.StartSelectTarget();
        }
    }
}
