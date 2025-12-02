using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using TMPro;

public class GameResult_Eq : MonoBehaviour
{
    [SerializeField] Image itemImage;
    [SerializeField] Image frame;

    public List<Sprite> frames;

    MouseOverUI mouseOver;

    Definer.Item item;
    public void Init(Definer.Item i)
    {
        item = i;
        mouseOver = MouseOverUI.inst;

        itemImage.sprite = item.data.sprite;
        frame.color = item.data.rarity.ToColor();

        int index = Mathf.Clamp((int)item.data.rarity, 1, 4);
        frame.sprite = frames[index];
    }

    public void OnMouseEnter()
    {
        mouseOver.SetUI($"{item.data.itemName.ColorStr(item.data.rarity.ToColor())}\n{item.GetInfo(true)}", true);
    }
    public void OnMouseExit()
    {
        mouseOver.ResetUI();
    }
}
