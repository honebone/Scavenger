using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using TMPro;

public class Compendium : MonoBehaviour
{
    public static Compendium inst;

    public CanvasGroup canvas;
    public Transform eqButtonsP;
    public GameObject eqButton;
    public ScrollRect scroll;
    public TextMeshProUGUI unlocksText;

    List<ItemData> eqs = new List<ItemData>();
    List<int> eqUnlockStates = new List<int>();
    List<CPD_EqButton> eqButtons = new List<CPD_EqButton>();
    bool active;

    private void Awake()
    {
        if (inst == null) inst = this;
    }

    void Start()
    {
        var list = new List<ItemData>(Definer.inst.GetAllEquipments());
        eqs = list.OrderBy(x => x.rarity).ToList();
        eqs.ForEach(x =>
        {
            var b = Instantiate(eqButton, eqButtonsP);
            int state = PlayerPrefs.GetFloat($"unlock_{x.name}", 0).ToInt();
            eqUnlockStates.Add(state);
            eqButtons.Add(b.GetComponent<CPD_EqButton>().Init(x, scroll, state != 0));
        });

        unlocksText.text = $"{eqUnlockStates.Count(x => x != 0)}/{eqUnlockStates.Count()}";
    }

    public void ToggleUI()
    {
        if (active)  Close();
        else Open();
        SoundManager.instance.PlaySE_Select();
    }
    public void Open()
    {
        canvas.alpha = 1.0f;
        canvas.interactable = true;
        canvas.blocksRaycasts = true;
        active = true;
    }
    public void Close()
    {
        canvas.alpha = 0f;
        canvas.interactable = false;
        canvas.blocksRaycasts = false;
        active = false;
    }

    public void UnllickEq(ItemData unlock)
    {
        if (unlock.itemType != ItemData.ItemType.equipment)
        {
            InfoText.inst.AddWarningText("装備品でないアイテムの図鑑設定をしようとしています");
            return;
        }
        int index = eqs.IndexOf(unlock);
        if (index == -1)
        {
            InfoText.inst.AddErrorText("図鑑に存在しないアイテム");
            return;
        }

        if (eqUnlockStates[index] == 0)
        {
            GameManager.instance.SetSaveData($"unlock_{unlock.name}", 1);
            eqUnlockStates[index] = 1;
            eqButtons[index].Unlock();
        }

        unlocksText.text = $"{eqUnlockStates.Count(x => x != 0)}/{eqUnlockStates.Count()}";
    }

   

    // Update is called once per frame
    void Update()
    {
        
    }
}
