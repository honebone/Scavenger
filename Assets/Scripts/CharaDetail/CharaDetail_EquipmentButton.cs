using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharaDetail_EquipmentButton : MonoBehaviour
{


    //このスクリプトは用済み


    //Definer.Item equipment;

    //[SerializeField]
    //Image eqImage;
    //[SerializeField]
    //Image frame;
    //[SerializeField]
    //Image selected;
    //[SerializeField]
    //Text eqName;

    //[SerializeField, Header("新たに装備を追加するボタン")]
    //bool empty;

    //InfoText infoText;
    //CharaDetailUI detailUI;
    //public  void Init(Definer.Item e,InfoText i,CharaDetailUI d)
    //{
    //    equipment = e;
    //    infoText = i;
    //    detailUI = d;
    //    if (!empty)
    //    {
    //        eqImage.sprite = equipment.data.sprite;
    //        eqName.text = equipment.data.itemName.ColorStr(equipment.data.rarity.ToColor());
    //        frame.color = equipment.data.rarity.ToColor();
    //        if (detailUI.GetSelectedEquipment().createdManager == equipment.createdManager) { selected.enabled = true; }
    //    }
    //    else if (detailUI.CheckSelectingEquipment() && detailUI.CheckEmpty()) { selected.enabled = true; }
    //}

    //public void Unequip()
    //{
    //    if (!FindObjectOfType<ExpeditionManager>().CheckInRoomEvent())
    //    {
    //        detailUI.GetDisplayingChara().UnequipItem(equipment);
    //        detailUI.EndSelectEquipment();
    //        detailUI.Refresh();
    //    }
    //    else
    //    {
    //        FindObjectOfType<GuideMessage>().SetWaringText("イベント中の装備変更不可");
    //    }
    //}

    //public void OnMouseDown()
    //{
    //    if (Input.GetMouseButtonDown(1))
    //    {
    //        if (!empty)
    //        {
    //            infoText.SetText(equipment.data.itemName.ColorStr(equipment.data.rarity.ToColor()), equipment.GetInfo(false), equipment.GetInfo(true));
    //        }
    //    }
    //    if (Input.GetMouseButtonDown(0))
    //    {
    //        if (!FindObjectOfType<ExpeditionManager>().CheckInRoomEvent())
    //        {
    //            detailUI.StartSelectEquipment(empty, equipment);
    //        }
    //        else
    //        {
    //            FindObjectOfType<GuideMessage>().SetWaringText("イベント中の装備変更不可");
    //        }
    //    }
    //}
}
