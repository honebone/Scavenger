using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharaDetailUI : MonoBehaviour
{
    [SerializeField]
    GameObject UIpanel;

    [SerializeField]
    Image charaImage;

    [Space(25), SerializeField]
    Transform equipmentP;
    [SerializeField]
    GameObject equipmentButton;
    [SerializeField]
    GameObject newEquipmentButton;

    InfoText infoText;
    CharactersManager charactersManager;
    Inventory inventory;

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
            displayingChara.DisplayInfo();
        }
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
        var n = Instantiate(newEquipmentButton, equipmentP);
        n.GetComponent<CharaDetail_EquipmentButton>().Init(new Definer.Item(), infoText, this);
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
