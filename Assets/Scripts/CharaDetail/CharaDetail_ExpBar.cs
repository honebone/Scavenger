using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;


public class CharaDetail_ExpBar : MonoBehaviour
{
    [SerializeField] CharaDetailUI detailUI;
    [SerializeField] CharaDetail_LVLUp lvlup;
    [SerializeField] InfoText infoText;
    [SerializeField] MouseOverUI mouseOver;

    Tweener tweener;



    public void OnMouseDown()
    {
        if (Input.GetMouseButtonDown(1))
        {
            
        }
        if (Input.GetMouseButtonDown(0))
        {
            lvlup.GainEXP();
        }
    }

    public void OnMouseEnter()
    {
        if (tweener != null)
        {
            tweener.Kill(true);
        }
        tweener = transform.DOScale(new Vector3(1.1f, 1.1f, 1), 0.2f);

        //mouseOver.SetUI(string.Format("{0}\nƒhƒ‰ƒbƒO‚Å‘•”õ", item.data.itemName.ColorStr(item.data.rarity.ToColor())), true);
    }
    public void OnMouseExit()
    {
        mouseOver.ResetUI();
        if (tweener != null)
        {
            tweener.Kill(true);
        }
        tweener = transform.DOScale(new Vector3(1f, 1f, 1), 0.2f);
    }
}
