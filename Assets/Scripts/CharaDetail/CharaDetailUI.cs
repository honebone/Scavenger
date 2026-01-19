using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class CharaDetailUI : MonoBehaviour
{
    public static CharaDetailUI inst;
    [SerializeField]
    GameObject UIpanel;

    [SerializeField] int maxParty = 4;
    [SerializeField] int maxEquipments;

    [SerializeField] CharaDetail_InventoryEq inventoryEq;
    [SerializeField] List<ChataDetail_CharaButton> charaButtons;

    [SerializeField] Definer.Item draggingItem;
    [SerializeField] Transform dragImageP;
    [SerializeField] GameObject dragImage;

    [SerializeField] List<CanvasGroup> canvasList_right;

    [SerializeField] List<GameObject> VE_rarity;
    [SerializeField] List<AudioClip> SE_grab;
    [SerializeField] List<AudioClip> SE_equip;

    public TextMeshProUGUI expAmount;
    [SerializeField] TutorialData tutorial_equip;
    [SerializeField] TutorialData tutorial_exp;
    [SerializeField] TutorialData tutorial_equipment;

    //[SerializeField] GameObject warningPanel;

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
    Vector3 dragImagePos;
    GameObject draggingImage;

    private void Awake()
    {
        if (inst == null) inst = this;
    }

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
            infoText.SetText_Old("キャラクター詳細", "キャラクターの装備品の確認や変更、アビリティの解放ができる");
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
        SoundManager.instance.PlaySE_Select();

        UIpanel.SetActive(true);

        //List<Character> characters = new List<Character>();
        //foreach (Character chara in charactersManager.GetExistingCharacters_All())
        //{
        //    Character.CharacterStatus status = chara.CharaStatus();
        //    if (status.player && chara.CheckAlive()) { characters.Add(chara); }
        //}

        //for (int i = 0; i < characters.Count; i++)
        //{
        //    charaButtons[i].SetChara(characters[i]);
        //}

        //if (characters.Count < maxParty)
        //{
        //    for (int i = characters.Count; i < maxParty; i++)
        //    {
        //        charaButtons[i].ResetValue();
        //    }
        //}



        //expAmount.text = inventory.GetExp().ToString();
        Refresh();

        //if (!displayingChara || !displayingChara.CheckAlive())
        //{
        //    charaButtons[0].SelectChara();
        //}
        ChangeChara(charaButtons[0].GetCharacter());

        inventoryEq.SetButtons();

        if (inventory.GetExp() > 0 && tutorialManager.CheckUnlocked(tutorial_exp)) { tutorialManager.SetTutorial("LVLUP"); }
        else if (inventory.GetEquipments().Count > 0 && tutorialManager.CheckUnlocked(tutorial_equipment)) { tutorialManager.SetTutorial(tutorial_equip); }


    }
    public void CloseUI()
    {
        SoundManager.instance.PlaySE_Select();
        UIpanel.SetActive(false);

        //ResetChara();
    }

    public void NextChara()
    {
        SoundManager.instance.PlaySE_Select();

        for (int i=0;i<charaButtons.Count;i++)
        {
            if (CheckButton(i) && charaButtons[i].GetCharacter() == displayingChara)
            {
                if(i< charaButtons.Count - 1 && CheckButton(i + 1))
                {
                    ChangeChara(charaButtons[i + 1].GetCharacter());
                    return;
                }
            } 
        }
        ChangeChara(charaButtons[0].GetCharacter());
    }
    public void PrevChara()
    {
        SoundManager.instance.PlaySE_Select();
        int last = -1;
        for (int i = charaButtons.Count-1; i >= 0; i--)
        {
            if (CheckButton(i))
            {
                if (last == -1) last = i;
                if (charaButtons[i].GetCharacter() == displayingChara)
                {
                    if (i > 0 && CheckButton(i - 1))
                    {
                        ChangeChara(charaButtons[i - 1].GetCharacter());
                        return;
                    }
                }
            }
        }
        ChangeChara(charaButtons[last].GetCharacter());
    }
    bool CheckButton(int i) { return charaButtons[i] != null && charaButtons[i].GetCharacter() != null; }

    public void ChangeChara(Character chara)
    {
        displayingChara = chara;
        status = displayingChara.CharaStatus();
        displayingChara.DisplayInfo();

        CharaDetail_Pers.inst.SetChara(chara);
        //inventory.CloseOptionUI();
    }

    //public void ResetChara()
    //{
    //    //EndSelectEquipment();
    //    //ToggleToEquipment();

    //    displayingChara = null;
    //    status = new Character.CharacterStatus();


    //    //lvlup.ResetValue();
    //    inventory.CloseOptionUI();
    //}

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
            status = displayingChara.CharaStatus();
            displayingChara.DisplayInfo();
        }

        List<Character> characters = new List<Character>();
        foreach (Character chara in charactersManager.GetExistingCharacters_All())
        {
            Character.CharacterStatus status = chara.CharaStatus();
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

        expAmount.text = inventory.GetExp().ToString();
    }

    public void SetDraggingItem(Definer.Item item, ChataDetail_CharaButton draggChara)
    {
        draggingItem = item;
        draggFrom = draggChara;
        draggingImage = Instantiate(dragImage, dragImageP);
        draggingImage.GetComponent<Image>().sprite = item.data.sprite;
        SoundManager.instance.PlaySE_Random(SE_grab);
    }
    public void ResetDraggingItem()
    {
        draggingItem = new Definer.Item();
        draggFrom = null;
        Destroy(draggingImage);
    }

    public void SwapRightUI(int index)
    {
        for(int i = 0;i< canvasList_right.Count; i++)
        {
            if (i != index)
            {
                canvasList_right[i].alpha = 0;
                canvasList_right[i].interactable = false;
                canvasList_right[i].blocksRaycasts = false;
            }
        }
        canvasList_right[index].alpha = 1;
        canvasList_right[index].interactable = true;
        canvasList_right[index].blocksRaycasts = true;

        SoundManager.instance.PlaySE_Select();
    }

    // Update is called once per frame
    void Update()
    {
        if (draggingImage != null)//保持中のアイテムがあるなら
        {
            dragImagePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            dragImagePos.z = 0;
            draggingImage.transform.position = dragImagePos;

            if (Input.GetMouseButtonUp(0))//左クリックを話した時
            {
                EventSystem ev = EventSystem.current;
                PointerEventData ped = new PointerEventData(ev);
                ped.position = Input.mousePosition;
                List<RaycastResult> rr = new List<RaycastResult>();
                raycaster.Raycast(ped, rr);

                bool onSlot = false;

                foreach (RaycastResult result in rr)//カーソルに重なってる前オブジェクトに対して
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
                                    inventory.RemoveItem(draggingItem, 1, false);
                                }
                                else
                                {
                                    draggFrom.GetCharacter().UnequipItem(draggingItem, false);
                                    draggFrom.SetButtons();
                                }
                                Utils_VE.inst.SpawnVE_MousePos(VE_rarity[(int)draggingItem.data.rarity]);
                                SoundManager.instance.PlaySE(SE_equip);
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
