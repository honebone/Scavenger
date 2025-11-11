using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CPD_EqButton : MonoBehaviour
{
    [SerializeField] Image itemImage;
    [SerializeField] Image frame;

    public List<Sprite> frames;
    public Color lockedColor;

    InfoText infoText;
    MouseOverUI mouseOver;

    ItemData item;
    bool unlocked;
    public CPD_EqButton Init(ItemData i, ScrollRect scrollRect,bool l)
    {
        item = i;
        infoText = InfoText.inst;
        mouseOver = MouseOverUI.inst;
        scroll = scrollRect;
        unlocked = l;

        itemImage.sprite = item.sprite;
        if (!unlocked) itemImage.color = Color.black;
        frame.color = unlocked ? item.rarity.ToColor() : lockedColor;
        //if (item.data.rarity == ItemData.Rarity.madness)
        //{
        //    itemImage.material = glitched;
        //}

        int index = Mathf.Clamp((int)item.rarity, 1, 4);
        frame.sprite = frames[index];
        return this;
    }

    public void Unlock()
    {
        unlocked = true;
         itemImage.color = Color.white;
        frame.color = item.rarity.ToColor();
    }

    [SerializeField]
    float scrollSpeed = 0.1f;
    ScrollRect scroll;
    float wheel;
    bool p;

    private void Update()
    {
        if (p)
        {
            wheel += Input.mouseScrollDelta.y;
            if (wheel != 0)
            {
                scroll.verticalNormalizedPosition += wheel * scrollSpeed;
                wheel = 0;
            }
        }
    }

    public void OnMouseDown()
    {
        //if (Input.GetMouseButtonDown(1))
        //{
        //    infoText.SetText(item.data.itemName.ColorStr(item.data.rarity.ToColor()), item.GetInfo(false), item.GetInfo(true));
        //}
        //if (Input.GetMouseButtonDown(0))
        //{
        //    if (!FindObjectOfType<ExpeditionManager>().CheckInRoomEvent())
        //    {
        //        mouseOver.ResetUI();
        //        detailUI.SetDraggingItem(item, null);
        //    }
        //    else
        //    {
        //        FindObjectOfType<GuideMessage>().SetWaringText("イベント中の装備変更不可");
        //    }
        //}
        if(unlocked) infoText.SetText(item.itemName.ColorStr(item.rarity.ToColor()), item.GetInfo(false), item.GetInfo(true));
        else infoText.SetText_Old("???".ColorStr(Color.gray), "プレイ中に発見するとアンロック\n(取得する必要はない)".ColorStr(Color.gray));
    }

    public void OnMouseEnter()
    {
        p = true;
        if(unlocked) mouseOver.SetUI($"{item.itemName.ColorStr(item.rarity.ToColor())}\n{item.GetInfo(true)}");
        else mouseOver.SetUI("???".ColorStr(Color.gray));
    }
    public void OnMouseExit()
    {
        p = false;
        mouseOver.ResetUI();
    }

}
