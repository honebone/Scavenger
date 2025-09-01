using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharaDetail_InventoryEqButton : MonoBehaviour
{
    [SerializeField] Image itemImage;
    [SerializeField] Image frame;
    [SerializeField] Material glitched;

    public List<Sprite> frames;

    InfoText infoText;
    CharaDetailUI detailUI;
    MouseOverUI mouseOver;

    Definer.Item item;
    public void Init(Definer.Item i, InfoText it, CharaDetailUI d,MouseOverUI m,ScrollRect scrollRect)
    {
        item = i;
        infoText = it;
        detailUI = d;
        mouseOver = m;
        scroll = scrollRect;

        itemImage.sprite = item.data.sprite;
        frame.color = item.data.rarity.ToColor();
        if (item.data.rarity == ItemData.Rarity.madness)
        {
            itemImage.material = glitched;
        }

        int index = Mathf.Clamp((int)item.data.rarity, 1, 4);
        frame.sprite = frames[index];
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
        if (Input.GetMouseButtonDown(1))
        {
            infoText.SetText(item.data.itemName.ColorStr(item.data.rarity.ToColor()), item.GetInfo(false), item.GetInfo(true));
        }
        if (Input.GetMouseButtonDown(0))
        {
            if (!FindObjectOfType<ExpeditionManager>().CheckInRoomEvent())
            {
                mouseOver.ResetUI();
                detailUI.SetDraggingItem(item, null);
            }
            else
            {
                FindObjectOfType<GuideMessage>().SetWaringText("イベント中の装備変更不可");
            }
        }
    }

    public void OnMouseEnter()
    {
        p = true;
        mouseOver.SetUI($"{item.data.itemName.ColorStr(item.data.rarity.ToColor())}\nドラッグで装備\n{item.GetInfo(true)}", true);
    }
    public void OnMouseExit()
    {
        p = false;
        mouseOver.ResetUI();
    }

}
