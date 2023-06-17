using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Character_TargetButton : MonoBehaviour
{
    [SerializeField]
    Image targetIcon;
    [SerializeField]
    GameObject button;
    Character character;

    bool selectableAsTarget;

    List<int> targetGroup = new List<int>();
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
}
