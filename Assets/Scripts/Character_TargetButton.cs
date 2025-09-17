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

    [SerializeField] Transform damageTextP;
    [SerializeField] GameObject damageText;

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
            if (character != null) {character.DisplayInfo(); }
            else { infoText.SetCharaInfo("空きスペース", positionManager.GetPEInfo()); }
            //SetSelectedIcon(true);
        }
        if (Input.GetMouseButtonDown(0))
        {
            if (selectableAsTarget) { battleManager.GetSelectedAbility().SelectTarget(targetGroup); }
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
            Character.CharacterStatus status = character.CharaStatus();
            string s = status.charaName + "\n";
            if (status.shield > 0)
            {
                s += $"{"HP".ToSpr_withName()}：{status.HP}+{"shield".ToSpr()}{status.shield.ToString().ColorStr(Definer.colorRef.shield)} ({status.GetHPPercent():0.0}％)";
            }
            else { s += $"{"HP".ToSpr_withName()}：{status.HP} ({status.GetHPPercent():0.0}％)"; }
            if (status.player)
            {
                s += $"\n{"SAN".ToSpr_withName()}：{status.SAN}";
            }

            if (status.lifetime > 0)
            {
                int lifetimeDMG = Mathf.CeilToInt(1f*status.BaseHP() / status.lifetime);
                s += $"\n寿命による{"HP".ToSpr_withName()}減少：" + $"{lifetimeDMG}/ラウンド".ColorStr(Definer.colorRef.decreaseHP);
            }

            foreach (GameObject DoT in Definer.DoTDataBase)
            {
                int DMGNextTurn = character.GetDoTDMG(DoT, false);
                int DMGTotal = character.GetDoTDMG(DoT, true);
                if (DMGTotal > 0)
                {
                    string StEName = DoT.GetComponent<PA_StatusEffect>().GetStatusEffectStatus().ToLinkKey();
                    s += $"\n{StEName}：次ターン{DMGNextTurn.ToString().ColorStr(Definer.colorRef.decreaseHP)}(計{DMGTotal.ToString().ColorStr(Definer.colorRef.decreaseHP)})";
                }
            }

            if (character.GetPACurrentStateInfo() != "")
            {
                s += "\n\n" + character.GetPACurrentStateInfo().ColorStr(Definer.colorRef.emphasize);
            }

            mouseOver.SetUI(s, true);

            if (SettingManager.infoOnMouseover)
            {
                character.DisplayInfo();
            }
        }
    }
    public void OnMouseExit()
    {
        if (selectableAsTarget || selectableAsMoveTarget || selectableAsMoveToPos) { charactersManager.ResetAllActionInvolvedIcons(); }
        mouseOver.ResetUI();
        highlightedIcon.enabled = false;
    }
}
