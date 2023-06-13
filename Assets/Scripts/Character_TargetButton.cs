using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Character_TargetButton : MonoBehaviour
{
    [SerializeField]
    Image targetIcon;
    Character character;
   public void SetCharacter(Character chara) { character = chara;}
    public void ResetCharacter() { character = null; }

    public void OnMouseDown()
    {
        if (Input.GetMouseButtonDown(1))
        {
            if (character != null) { character.DisplayInfo(); }
        }
        if (Input.GetMouseButtonDown(0))
        {
            
        }
    }
}
