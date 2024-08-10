using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharaDetail_InventoryEq : MonoBehaviour
{
    [SerializeField] GameObject panel;
    [SerializeField] Transform content;
    [SerializeField] GameObject equipmentButton;

    [SerializeField] CharaDetailUI detailUI;
    [SerializeField] InfoText infoText;
    [SerializeField] MouseOverUI mouseOver;
    [SerializeField] Inventory inventory;

    [SerializeField] ScrollRect scrollRect;


    public void SetButtons()
    {
        panel.SetActive(true);

        if (content.childCount != 0)
        {
            for (int i = 0; i < content.childCount; i++)
            {
                Destroy(content.GetChild(i).gameObject);
            }
        }
      
        foreach (Definer.Item item in inventory.GetEquipments())
        {
            for(int a = 0; a < item.amount; a++)
            {
                Definer.Item i = new Definer.Item();
                i.Init(item.data);
                i.amount = 1;

                var eb = Instantiate(equipmentButton, content);
                eb.GetComponent<CharaDetail_InventoryEqButton>().Init(i, infoText, detailUI, mouseOver, scrollRect);
            }
        }
    }

    public void ClosePanel()
    {
        if (content.childCount != 0)
        {
            for (int i = 0; i < content.childCount; i++)
            {
                Destroy(content.GetChild(i).gameObject);
            }
        }
        panel.SetActive(false);
    }
}
