using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Character_TargetButton : MonoBehaviour
{
    [SerializeField]
    Image targetIcon;
    [SerializeField]
    Image actionOwnerIcon;
    [SerializeField]
    Image actionTargetIcon;
    [SerializeField]
    GameObject button;
    Character character;
    CharactersManager charactersManager;

    bool selectableAsTarget;

    List<int> targetGroup = new List<int>();

    private void Start()
    {
        charactersManager = FindObjectOfType<CharactersManager>();
    }
    public void SetCharacter(Character chara)
    {
        button.SetActive(true);
        character = chara;
    }
    public void ResetCharacter()
    {
        button.SetActive(false);
        character = null;
    }

    public void SetTargetIcon(List<int> tg)
    {
        selectableAsTarget = true;
        button.SetActive(true);
        targetGroup = tg;
        targetIcon.enabled = true;
    }
    public void ResetTargetIcon()
    {
        selectableAsTarget = false;
        if (character == null) { button.SetActive(false); }
        targetGroup.Clear();
        targetIcon.enabled = false;
    }

    /// <summary> </summary>
    /// <param name="owner">falseÇ»ÇÁëŒè€Ç∆îªíf</param>
    public void SetActionInvolvedIcon(bool owner)
    {
        if (owner) { actionOwnerIcon.enabled = true; }
        else { actionTargetIcon.enabled = true; }
    }
    public void ResetActionInvolvedIcon()
    {
        actionOwnerIcon.enabled = false; 
        actionTargetIcon.enabled = false; 
    }

    public void OnMouseDown()
    {
        if (Input.GetMouseButtonDown(1))
        {
            if (character != null) {character.DisplayInfo(); }
        }
        if (Input.GetMouseButtonDown(0))
        {
            if (selectableAsTarget) { BattleManager.selectedAbility.SelectTarget(targetGroup); }
        }
    }
    public void OnMouseOver()
    {
        if (selectableAsTarget)
        {
            foreach (int pos in targetGroup)
            {
                if (charactersManager.CheckCharaExist(pos))
                {
                    charactersManager.GetCharacterWithPos(pos).GetCharacter_TargetButton().SetActionInvolvedIcon(false);
                }
                else
                {
                    charactersManager.GetTargetButton(pos).SetActionInvolvedIcon(false);
                }
            }
        }
    }
    public void OnMouseExit()
    {
        if (selectableAsTarget) { charactersManager.ResetAllActionInvolvedIcons(); }
    }
}
