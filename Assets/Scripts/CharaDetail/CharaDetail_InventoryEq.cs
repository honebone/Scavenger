using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharaDetail_InventoryEq : MonoBehaviour
{
    //[SerializeField] GameObject panel;
    [SerializeField] Transform content;
    [SerializeField] GameObject equipmentButton;

     CharaDetailUI detailUI;
     InfoText infoText;
    MouseOverUI mouseOver;
    Inventory inventory;

    [SerializeField] ScrollRect scrollRect;

    private void Start()
    {
        inventory = Inventory.inst;
        infoText = InfoText.inst;
        mouseOver=MouseOverUI.inst;
        detailUI = CharaDetailUI.inst;
    }

    public void SetButtons()
    {
        //panel.SetActive(true);

        if (content.childCount != 0)
        {
            for (int i = 0; i < content.childCount; i++)
            {
                Destroy(content.GetChild(i).gameObject);
            }
        }

        foreach (Definer.Item item in inventory.GetEquipments())
        {
            for (int a = 0; a < item.amount; a++)
            {
                Definer.Item i = new Definer.Item();
                i.Init(item.data);
                i.amount = 1;

                var eb = Instantiate(equipmentButton, content);
                eb.GetComponent<CharaDetail_InventoryEqButton>().Init(i, infoText, detailUI, mouseOver);
                eb.GetComponent<ScrollManager>().SetScroll(scrollRect);
            }
        }
    }

    //public void ClosePanel()
    //{
    //    if (content.childCount != 0)
    //    {
    //        for (int i = 0; i < content.childCount; i++)
    //        {
    //            Destroy(content.GetChild(i).gameObject);
    //        }
    //    }
    //    panel.SetActive(false);
    //}
}
