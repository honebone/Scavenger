using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class TotalDamageText : MonoBehaviour
{
    [SerializeField] Text damageText;
    int damage;
    Tweener tweener;
   
    public void SetText(int DMG)
    {
        damage += DMG;
        damageText.text = $"{damage} damage ";

        if (tweener != null) { tweener.Kill(true); }
        damageText.transform.localScale = Vector3.one * 2;
        tweener = damageText.transform.DOScale(Vector3.one, 0.2f);
    }
    public void ResetText()
    {
        damage = 0;
        damageText.text = "";
    }
}
