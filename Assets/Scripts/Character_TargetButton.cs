using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
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

    [SerializeField] Transform damageTextP;
    [SerializeField] GameObject damageText;
    [SerializeField] TextMeshProUGUI intentText;

    Character character;
    CharactersManager charactersManager;
    InfoText infoText;
    PositionManager positionManager;
    BattleManager battleManager;
    ExpeditionManager expeditionManager;
    MouseOverUI mouseOver;

    int position;

    bool selectableAsTarget;

    //イベント前の移動関係
    bool selectableAsMoveTarget;
    bool selectableAsMoveToPos;

    List<int> targetGroup = new List<int>();

    bool revealIntent;

    private void Start()
    {
        charactersManager = FindObjectOfType<CharactersManager>();
        infoText = FindObjectOfType<InfoText>();
        battleManager = FindObjectOfType<BattleManager>();
        positionManager = GetComponent<PositionManager>();
        expeditionManager = FindObjectOfType<ExpeditionManager>();
        mouseOver = FindObjectOfType<MouseOverUI>();
    }
    public void SetPosition(int pos) { position = pos; }
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
        revealIntent = false;
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

    public void MoveMode_SelectableAsTarget()
    {
        selectableAsMoveTarget = true;
        targetIcon.enabled = true;
       
    }
    public void MoveMode_SelectableAsMovePos()
    {
        selectableAsMoveToPos = true;
        targetIcon.enabled = true;
    }
    public void MoveMode_ResetAll()
    {
        selectableAsMoveTarget = false;
        selectableAsMoveToPos = false;
        targetIcon.enabled = false;
        actionTargetIcon.enabled = false;
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

    public void SetIntent()
    {
        Ability.AbilityStatus intent = character.SetIntent();
        intentText.text = $"{GetSuffix(intent.abilityType)}{intent.abilityName.ColorStr(intent.abilityType.ToColor())}";

        string GetSuffix(AbilityData.AbilityType type)
        {
            switch (intent.abilityType)
            {
                case AbilityData.AbilityType.attack:
                    return "ATK".ToSpr();
                case AbilityData.AbilityType.buff:
                    return "buff".ToSpr();
                case AbilityData.AbilityType.debuff:
                    return "debuff".ToSpr();
                case AbilityData.AbilityType.heal:
                    return "heal".ToSpr();
                case AbilityData.AbilityType.summon:
                    return "summon".ToSpr();
                case AbilityData.AbilityType.move:
                    return "move".ToSpr();
                case AbilityData.AbilityType.pass:
                    return "×".ColorStr(AbilityData.AbilityType.pass.ToColor());
                default:
                    return "";
            }
        }

        revealIntent = true;
    }
    public void ResetIntentText()
    {
        intentText.text = "";
        revealIntent = false;
    }

    public PositionManager GetPositionManager() { return positionManager; }

    //====================[DamageText]==========================================

    public void SetDamageText(string text, Color color)
    {
        var d = Instantiate(damageText, damageTextP);
        d.GetComponent<DamageText>().Init(text, color,position.IsPlayerPos());
    }


    public void OnMouseDown()
    {
        if (Input.GetMouseButtonDown(1))
        {
            SoundManager.instance.PlaySE_Info();
            if (character != null) {character.DisplayInfo(); }
            else { infoText.SetCharaInfo("空きスペース", positionManager.GetPEInfo()); }
            //SetSelectedIcon(true);
        }
        if (Input.GetMouseButtonDown(0))
        {
            if (selectableAsTarget)
            {
                if (battleManager.GetSelectedAbility() == null)
                {
                    infoText.AddErrorText("アビリティが選択されていない状態で、対象が指定できてしまっています");
                    return;
                }
                battleManager.GetSelectedAbility().SelectTarget(targetGroup);
            }
            else if (selectableAsMoveTarget) { expeditionManager.MoveMode_SelectChara(character); }
            else if (selectableAsMoveToPos) { expeditionManager.MoveMode_SelectPos(position); }
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
                    charactersManager.GetCharacterWithPos(pos).GetTargetButton().SetActionInvolvedIcon(false);
                }
                else
                {
                    charactersManager.GetTargetButton(pos).SetActionInvolvedIcon(false);
                }
            }
        }
        else if (selectableAsMoveTarget || selectableAsMoveToPos)
        {
            actionTargetIcon.enabled = true;
        }
        else
        {
            highlightedIcon.enabled = true;
        }
        if (character == null)
        {
            mouseOver.SetUI("空きスペース", false);
            if (SettingManager.infoOnMouseover) { infoText.SetCharaInfo("空きスペース", positionManager.GetPEInfo()); }
        }
        else
        {
            character.SetMouseOver(revealIntent);
        }
    }
    public void OnMouseExit()
    {
        if (selectableAsTarget || selectableAsMoveTarget || selectableAsMoveToPos) { charactersManager.ResetAllActionInvolvedIcons(); }
        mouseOver.ResetUI();
        highlightedIcon.enabled = false;
    }
}
