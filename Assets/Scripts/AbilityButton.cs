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
    GuideMessage guideMessage;

    bool available;
    bool deployMode;

    List<string> unavailableInfo;

    public void Init(Ability.AbilityStatus status,BattleManager bm,Character chara,CharactersManager cm,GuideMessage gm)
    {
        abilityStatus = status;
        battleManager = bm;
        character = chara;
        charactersManager = cm;
        guideMessage = gm;

        nameText.text = abilityStatus.abilityName;
        available = abilityStatus.instantiatedManager.CheckAvailable();
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

        unavailableInfo = new List<string>(abilityStatus.instantiatedManager.GetUnavailabeInfo());
    }
    public void Init_Deploy(Ability.AbilityStatus status)//出撃キャラ選択画面でのみ有効
    {
        deployMode = true;
        abilityStatus = status;

        nameText.text = abilityStatus.abilityName;
        if (abilityStatus.locked) { locked.enabled = true; }
        frame.sprite = frames[(int)abilityStatus.abilityType];
        frame.color = Definer.colorRef.abilityColors[(int)abilityStatus.abilityType];
    }

    public void OnMouseDown()
    {
        if (!deployMode)
        {
            battleManager.SetSelectedAbility(abilityStatus, character);
            FindObjectOfType<InfoText>().SetText(abilityStatus.abilityName.ColorStr(Definer.colorRef.abilityColors[(int)abilityStatus.abilityType]), abilityStatus.instantiatedManager.GetInfo(false), abilityStatus.instantiatedManager.GetInfo(true));
            charactersManager.ResetAllTargetIcons();
            if (battleManager.checkIfMyTurn(character) && BattleManager.selectingAbility && available) //自分のターン中かつアビリティ選択中なら、対象選択開始      
            {
                abilityStatus.instantiatedManager.StartSelectTarget();
            }
            else if (Input.GetMouseButtonUp(0))
            {
                //List<string> unavailableInfo = abilityStatus.instantiatedManager.GetUnavailabeInfo();
                foreach (string s in unavailableInfo)
                {
                    guideMessage.SetWaringText(s);
                }
            }
        }
        else
        {
            FindObjectOfType<InfoText>().SetText(abilityStatus.abilityName.ColorStr(Definer.colorRef.abilityColors[(int)abilityStatus.abilityType]), abilityStatus.GetInfo(false,new Character.CharacterStatus(),false), abilityStatus.GetInfo(false, new Character.CharacterStatus(), true));
        }

    }

    public void OnMouseEnter()
    {
        string availableInfo = "";
        if (!deployMode)
        {
            if (available && battleManager.checkIfMyTurn(character))
            {
                availableInfo = "発動可能".ColorStr(Definer.colorRef.emphasize);
            }
            else
            {
                availableInfo += "発動不可\n";
                foreach (string s in unavailableInfo)
                {
                    availableInfo += $"・{s}\n";
                }
                availableInfo = availableInfo.ColorStr(Color.red);
            }
        }
        MouseOverUI.inst.SetUI(availableInfo, true);
        
    }
    public void OnMouseExit()
    {
        FindObjectOfType<MouseOverUI>().ResetUI();
    }
}
