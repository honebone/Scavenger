using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class CharaDetailUI : MonoBehaviour
{
    [SerializeField]
    GameObject UIpanel;

    [SerializeField] int maxParty = 4;
    [SerializeField] int maxEquipments;
    [Space(25), SerializeField]
    Transform equipmentP;
    [SerializeField]
    GameObject equipmentButton;
    [SerializeField]
    GameObject newEquipmentButton;

    [SerializeField] GameObject inventoryEqPanel;
    [SerializeField] CharaDetail_InventoryEq inventoryEq;
    [SerializeField] List<ChataDetail_CharaButton> charaButtons;

    [SerializeField] GameObject LVLUpPanel;
    [SerializeField] CharaDetail_LVLUp lvlup;

    [Space(25), SerializeField]
    Transform abilityP;
    [SerializeField]
    GameObject abilityButton;
    [SerializeField]
    TextMeshProUGUI abilityUpgradeInfo;

    [SerializeField] Definer.Item draggingItem;
    [SerializeField] Transform dragImageP;
    [SerializeField] GameObject dragImage;

    [SerializeField]
    TextMeshProUGUI expAmount;
    [SerializeField] TutorialData tutorial_unlockAbility;
    [SerializeField] TutorialData tutorial_equip;

    [SerializeField] GameObject warningPanel;

    [SerializeField] GraphicRaycaster raycaster;


    InfoText infoText;
    CharactersManager charactersManager;
    Inventory inventory;
    GuideMessage guideMessage;
    TutorialManager tutorialManager;

    Character displayingChara;
    Character.CharacterStatus status;

    bool selectingEquipment;
    bool empty;

    ChataDetail_CharaButton draggFrom;
    Definer.Item selectedEq;
    GameObject draggingImage;


    void Start()
    {
        infoText = FindObjectOfType<InfoText>();
        charactersManager = FindObjectOfType<CharactersManager>();
        inventory = FindObjectOfType<Inventory>();
        guideMessage = FindObjectOfType<GuideMessage>();
        tutorialManager = FindObjectOfType<TutorialManager>();
        displayingChara = null;
    }

    public void CharaDetailButton()
    {
        if (Input.GetMouseButtonDown(1))
        {
            infoText.SetText("キャラクター詳細", "キャラクターの装備品の確認や変更、アビリティの解放ができる");
        }
        if (Input.GetMouseButtonDown(0))
        {
            ToggleUI();
        }
    }
    public void ToggleUI()
    {
        if (!UIpanel.activeSelf)
        {
            if (charactersManager.GetExistingCharacters_All().Count > 0) { OpenUI(); }
        }
        else { CloseUI(); }
    }
    public void OpenUI()
    {
        UIpanel.SetActive(true);

        List<Character> characters = new List<Character>();
        foreach (Character chara in charactersManager.GetExistingCharacters_All())
        {
            Character.CharacterStatus status = chara.GetCharacterStatus();
            if (status.player && chara.CheckAlive()) { characters.Add(chara); }
        }

        for (int i = 0; i < characters.Count; i++)
        {
            charaButtons[i].SetChara(characters[i]);
        }

        if (characters.Count < maxParty)
        {
            for (int i = characters.Count; i < maxParty; i++)
            {
                charaButtons[i].ResetValue();
            }
        }

        if (!displayingChara || !displayingChara.CheckAlive())
        {
            charaButtons[0].SelectChara();
        }

        inventoryEq.SetButtons();//test

        if (inventory.GetExp() > 0) { tutorialManager.SetTutorial(tutorial_unlockAbility); }
        else if (inventory.GetEquipments().Count > 0) { tutorialManager.SetTutorial(tutorial_equip); }

       
    }
    public void CloseUI()
    {
        UIpanel.SetActive(false);

        ResetChara();
    }
    public void ChangeChara(Character chara)
    {
        EndSelectEquipment();

        displayingChara = chara;
        status = displayingChara.GetCharacterStatus();

        displayingChara.DisplayInfo();

        lvlup.SetValue();
        inventory.CloseOptionUI();

        //ToggleToLVLUp();
    }

    public void ResetChara()
    {
        EndSelectEquipment();
        ToggleToEquipment();

        displayingChara = null;
        status = new Character.CharacterStatus();


        lvlup.ResetValue();
        inventory.CloseOptionUI();
    }

    public void RestCharaButtonFrame()
    {
        foreach(ChataDetail_CharaButton charaButton in charaButtons)
        {
            charaButton.ResetFrameColor();
        }
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

        List<Character> characters = new List<Character>();
        foreach (Character chara in charactersManager.GetExistingCharacters_All())
        {
            Character.CharacterStatus status = chara.GetCharacterStatus();
            if (status.player && chara.CheckAlive()) { characters.Add(chara); }
        }

        for (int i = 0; i < characters.Count; i++)
        {
            charaButtons[i].SetChara(characters[i]);
        }

        if (characters.Count < maxParty)
        {
            for (int i = characters.Count; i < maxParty; i++)
            {
                charaButtons[i].ResetValue();
            }
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
                a.GetComponent<CharaDetail_AbilityButton>().Init(abilityStatus, 0, displayingChara, guideMessage, infoText, this, abilityUpgradeInfo);
            }
            else if (abilityStatus.abilityData.upgradeAbility != null)
            {
                Ability.AbilityStatus upgrade = new Ability.AbilityStatus(abilityStatus.abilityData.upgradeAbility, 0);
                var a = Instantiate(abilityButton, abilityP);
                a.GetComponent<CharaDetail_AbilityButton>().Init(upgrade, 1, displayingChara, guideMessage, infoText, this, abilityUpgradeInfo);
            }
        }
    }

    public void ToggleToEquipment()
    {
        warningPanel.SetActive(false);
        lvlup.ClosePanel();
        inventoryEq.SetButtons();
    }
    public void ToggleToLVLUp()
    {
        ResetDraggingItem();
        inventoryEq.ClosePanel();
        lvlup.OpenPanel();
        if (!displayingChara) { warningPanel.SetActive(true); }
    }

    public void ToggleToPersonality()
    {
        guideMessage.SetWaringText("この機能は未実装です...");
    }

    public void SetDraggingItem(Definer.Item item, ChataDetail_CharaButton draggChara)
    {
        ToggleToEquipment();
        draggingItem = item;
        draggFrom = draggChara;
        draggingImage = Instantiate(dragImage, dragImageP);
        draggingImage.GetComponent<Image>().sprite = item.data.sprite;
    }
    public void ResetDraggingItem()
    {
        draggingItem = new Definer.Item();
        draggFrom = null;
        Destroy(draggingImage);
    }

    // Update is called once per frame
    void Update()
    {
        if (draggingImage != null)
        {
            draggingImage.transform.position = Input.mousePosition;

            if (Input.GetMouseButtonUp(0))
            {
                EventSystem ev = EventSystem.current;
                PointerEventData ped = new PointerEventData(ev);
                ped.position = Input.mousePosition;
                List<RaycastResult> rr = new List<RaycastResult>();
                raycaster.Raycast(ped, rr);

                bool onSlot = false;

                foreach (RaycastResult result in rr)
                {
                    if (result.gameObject.GetComponent<CharaDetail_CharaEqButton>())//CharaEqButton上でボタン離したなら
                    {
                        CharaDetail_CharaEqButton charaEqButton = result.gameObject.GetComponent<CharaDetail_CharaEqButton>();
                        onSlot = true;

                        if (!charaEqButton.CheckLocked() && charaEqButton.GetCharaButton() != draggFrom)
                        {
                            if (charaEqButton.GetCharacter().CheckSameEquipment(draggingItem.data))
                            {
                                guideMessage.SetWaringText("同名の装備品は装備不可");
                            }
                            else
                            {
                                if (draggFrom == null)
                                {
                                    inventory.RemoveItem(draggingItem, 1);
                                }
                                else
                                {
                                    draggFrom.GetCharacter().UnequipItem(draggingItem, false);
                                    draggFrom.SetButtons();
                                }

                                charaEqButton.Equip(draggingItem);
                            }
                            
                        }
                        inventoryEq.SetButtons();
                        //result.gameObject.GetComponent<CharaDetail_CharaEqButton>().SetChara(draggingChara);
                        break;
                    }
                }


                if (!onSlot && draggFrom != null)
                {
                    draggFrom.GetCharacter().UnequipItem(draggingItem, false);
                    inventory.AddItem(draggingItem, 1, false);

                    draggFrom.SetButtons();
                    inventoryEq.SetButtons();
                }

                ResetDraggingItem();
            }
        }
    }

    public bool CheckSelectingEquipment() { return selectingEquipment; }
    public bool CheckEmpty() { return empty; }
    public Definer.Item GetSelectedEquipment() { return selectedEq; }
    public Character GetDisplayingChara() { return displayingChara; }
}
