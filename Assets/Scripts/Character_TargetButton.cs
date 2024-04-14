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
    [SerializeField]
    Image selectedIcon;
    [SerializeField]
    Image highlightedIcon;
    [SerializeField]
    Image emptyIcon;
    [SerializeField]
    Transform actionIconsP;
    [SerializeField]
    GameObject actionIcon;
    Character character;
    CharactersManager charactersManager;
    InfoText infoText;
    PositionManager positionManager;

    bool selectableAsTarget;

    List<int> targetGroup = new List<int>();

    private void Start()
    {
        charactersManager = FindObjectOfType<CharactersManager>();
        infoText = FindObjectOfType<InfoText>();
        positionManager = GetComponent<PositionManager>();
    }
    public void SetCharacter(Character chara)
    {
        //button.SetActive(true);
        character = chara;
        positionManager.SetCharacter(chara);
        emptyIcon.enabled = false;
    }
    public void ResetCharacter()
    {
        //button.SetActive(false);
        character = null;
        positionManager.ResetCharacter();
        emptyIcon.enabled = true;
    }

    public void SetTargetIcon(List<int> tg)
    {
        selectableAsTarget = true;
        //button.SetActive(true);
        targetGroup = tg;
        targetIcon.enabled = true;
    }
    public void ResetTargetIcon()
    {
        selectableAsTarget = false;
       // if (character == null) { button.SetActive(false); }
        targetGroup.Clear();
        targetIcon.enabled = false;
    }
    public void SetActionIcon(Sprite sprite)
    {
        var a = Instantiate(actionIcon, actionIconsP);
        a.GetComponent<ActionIcon>().Init(sprite);
      
    }

    /// <summary> </summary>
    /// <param name="owner">falseなら対象と判断</param>
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
    public void SetSelectedIcon(bool set)
    {
        selectedIcon.enabled = set;
    }

    public PositionManager GetPositionManager() { return positionManager; }

    public void OnMouseDown()
    {
        if (Input.GetMouseButtonDown(1))
        {
            if (character != null) {character.DisplayInfo(); }
            else { infoText.SetCharaInfo("空きスペース", positionManager.GetPEInfo(), null); }
            //SetSelectedIcon(true);
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
        else
        {
            highlightedIcon.enabled = true;
        }
    }
    public void OnMouseExit()
    {
        if (selectableAsTarget) { charactersManager.ResetAllActionInvolvedIcons(); }
        highlightedIcon.enabled = false;
    }
}
