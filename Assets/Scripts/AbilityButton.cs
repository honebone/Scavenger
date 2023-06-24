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
    Text nameText;
    [SerializeField]
    Text remainText;
    [SerializeField]
    Text cooldownText;

    Ability.AbilityStatus abilityStatus;
    BattleManager battleManager;
    Character character;

    public void Init(Ability.AbilityStatus status,BattleManager bm,Character chara)
    {
        abilityStatus = status;
        battleManager = bm;
        character = chara;

        nameText.text = abilityStatus.abilityName;
        if (abilityStatus.cooldown > 0) { cooldownText.text = abilityStatus.cooldown.ToString(); }
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
        FindObjectOfType<InfoText>().SetText(abilityStatus.abilityName, BattleManager.selectedAbility.GetInfo());
        if (battleManager.checkIfMyTurn(character)&&BattleManager.selectingAbility) { BattleManager.selectedAbility.StartSelectTarget(); } //自分のターン中かつアビリティ選択中なら、対象選択開始      
    }
}
