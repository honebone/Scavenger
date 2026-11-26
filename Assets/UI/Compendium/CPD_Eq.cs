using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class CPD_Eq : CPDElement
{
    List<CPD_EqButton> eqButtons = new List<CPD_EqButton>();
    List<ItemData> eqs = new List<ItemData>();

    public override void Init()
    {
        var list = new List<ItemData>(Definer.inst.GetAllEquipments());
        eqs = list.OrderBy(x => x.rarity).ToList();
        eqs.ForEach(x =>
        {
            var b = Instantiate(buttonPrefab, buttonsP);
            int state = PlayerPrefs.GetFloat($"unlock_{x.name}", 0).ToInt();
            unlockStates.Add(state);
            eqButtons.Add(b.GetComponent<CPD_EqButton>().Init(x, scroll, state != 0));
        });
    }

    public void UnlockEq(ItemData unlock)
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

        if (unlockStates[index] == 0)
        {
            GameManager.instance.SetSaveData($"unlock_{unlock.name}", 1);
            unlockStates[index] = 1;
            eqButtons[index].Unlock();

            GameManager.instance.SendScoreborad(2, unlockStates.Count(x => x != 0));
        }

    }
}
