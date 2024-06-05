using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CharaDetailUI : MonoBehaviour
{
    [SerializeField]
    GameObject UIpanel;

    [SerializeField]
    Image charaImage;

    [SerializeField] int maxEquipments;
    [Space(25), SerializeField]
    Transform equipmentP;
    [SerializeField]
    GameObject equipmentButton;
    [SerializeField]
    GameObject newEquipmentButton;

    [Space(25), SerializeField]
    Transform abilityP;
    [SerializeField]
    GameObject abilityButton;
    [SerializeField]
    TextMeshProUGUI abilityUpgradeInfo;

    [SerializeField]
    TextMeshProUGUI expAmount;
    [SerializeField] TutorialData tutorial_unlockAbility;
    [SerializeField] TutorialData tutorial_equip;


    InfoText infoText;
    CharactersManager charactersManager;
    Inventory inventory;
    GuideMessage guideMessage;
    TutorialManager tutorialManager;

    Character displayingChara;
    Character.CharacterStatus status;

    bool selectingEquipment;
    bool empty;
    Definer.Item selectedEq;


    void Start()
    {
        infoText = FindObjectOfType<InfoText>();
        charactersManager = FindObjectOfType<CharactersManager>();
        inventory = FindObjectOfType<Inventory>();
        guideMessage = FindObjectOfType<GuideMessage>();
        tutorialManager = FindObjectOfType<TutorialManager>();
        displayingChara = null;
    }

    void Update()
    {
        
    }
    public void ToggleUI()
    {
        if (!UIpanel.activeSelf) { OpenUI(); }
        else { CloseUI(); }
    }
    public void OpenUI()
    {
        UIpanel.SetActive(true);
        if(displayingChara == null||!displayingChara.CheckAlive())//表示中のキャラがいないか死んでいるなら
        {
            ChangeChara(charactersManager.GetExistingCharacters_All()[0]);
        }
        if (inventory.GetExp() > 0) { tutorialManager.StartTutorial(tutorial_unlockAbility); }
        else if (inventory.GetEquipments().Count > 0) { tutorialManager.StartTutorial(tutorial_equip); }
    }
    public void CloseUI()
    {
        UIpanel.SetActive(false);

        displayingChara = null;
        inventory.CloseOptionUI();
        EndSelectEquipment();
    }
    public void ChangeChara(Character chara)
    {
        EndSelectEquipment();

        displayingChara = chara;
        status = displayingChara.GetCharacterStatus();

        charaImage.sprite = status.spriteForUI;
        displayingChara.DisplayInfo();

        inventory.CloseOptionUI();
    }
    public void Refresh()
    {
        if(displayingChara != null)
        {
            status = displayingChara.GetCharacterStatus();

            SetEquipmnetButtons();
            SetAbilityButtons();
            abilityUpgradeInfo.text = "";
            displayingChara.DisplayInfo();
        }
        expAmount.text = string.Format("経験のオーブ{0}個",inventory.GetExp());
    }
    public void SetEquipmnetButtons()
    {
        if (equipmentP.childCount != 0)
        {
            for (int i = 0; i < equipmentP.childCount; i++)
            {
                Destroy(equipmentP.GetChild(i).gameObject);
            }
        }
        foreach (Definer.Item item in status.equipments)
        {
            var e = Instantiate(equipmentButton, equipmentP);
            e.GetComponent<CharaDetail_EquipmentButton>().Init(item, infoText,this);
        }
        for (int i = 0; i < maxEquipments - status.equipments.Count; i++)
        {
            var n = Instantiate(newEquipmentButton, equipmentP);
            n.GetComponent<CharaDetail_EquipmentButton>().Init(new Definer.Item(), infoText, this);
        }
       
    }

    public void StartSelectEquipment(bool e,Definer.Item selected)
    {
        selectingEquipment = true;
        empty = e;
        selectedEq = selected;

        inventory.CloseOptionUI();
        inventory.ChangeSort(2);
        inventory.OpenInventory();
    }
    public void EndSelectEquipment()
    {
        selectingEquipment = false;
        selectedEq = new Definer.Item();
    }

    public void SetAbilityButtons()
    {
        if (abilityP.childCount != 0)
        {
            for (int i = 0; i < abilityP.childCount; i++)
            {
                Destroy(abilityP.GetChild(i).gameObject);
            }
        }
        foreach (Ability.AbilityStatus abilityStatus in status.abilitiesStatus)
        {
            if (abilityStatus.locked)
            {
                var a = Instantiate(abilityButton, abilityP);
                a.GetComponent<CharaDetail_AbilityButton>().Init(abilityStatus, displayingChara, guideMessage, infoText,this,abilityUpgradeInfo);
            }
            //アップグレード
        }
    }

    public void NextChara()
    {
        List<Character> exist = charactersManager.GetExistingCharacters_All();
        if (displayingChara == null || !displayingChara.CheckAlive())//表示中のキャラがいないか死んでいるなら
        {
            ChangeChara(exist[0]);
        }
        else
        {
            int index = 0;
            for (int i = 0; i < exist.Count; i++)
            {
                if (displayingChara == exist[i])
                {
                    index = i;
                    break;
                }
            }
            if (index == exist.Count - 1) { ChangeChara(exist[0]); }
            else { ChangeChara(exist[index + 1]); }
        }

    }
    public void PrevChara()
    {
        List<Character> exist = charactersManager.GetExistingCharacters_All();
        if (displayingChara == null || !displayingChara.CheckAlive())//表示中のキャラがいないか死んでいるなら
        {
            ChangeChara(exist[0]);
        }
        else
        {
            int index = 0;
            for (int i = 0; i < exist.Count; i++)
            {
                if (displayingChara == exist[i])
                {
                    index = i;
                    break;
                }
            }
            if (index == 0) { ChangeChara(exist[exist.Count - 1]); }
            else { ChangeChara(exist[index - 1]); }
        }
    }

    

    public bool CheckSelectingEquipment() { return selectingEquipment; }
    public bool CheckEmpty() { return empty; }
    public Definer.Item GetSelectedEquipment() { return selectedEq; }
    public Character GetDisplayingChara() { return displayingChara; }
}
