using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Battle_TurnOrderIcon : MonoBehaviour
{
    [SerializeField]
    Image charaSprite;
    //[SerializeField]
    //Image sign;
    [SerializeField]
    Image frame;

    [SerializeField]
    Sprite unrevealedIcon;
    [SerializeField]
    Sprite[] signIcons;

    Character character;

    Tweener tweener;

    bool revealed;
    bool destroyed;
    public void Init(Character chara, bool reveal)
    {
        character = chara;
        if (reveal)
        {
           Reveal();
        }
        else
        {
            charaSprite.enabled = false;
            frame.color = Color.grey;
        }
    }
    //public void SetOmenIcon(Ability.AbilityStatus o)
    //{
    //    omen = o;
    //    omenSet = true;
    //    if (revealed) { sign.sprite = signIcons[(int)omen.abilityType]; }
        
    //}
    public void Reveal()
    {
        if (!revealed&&character.CheckAlive())
        {
            revealed = true;
            charaSprite.enabled = true;
            charaSprite.sprite = character.CharaStatus().spriteForUI;
            frame.color = !character.PlayerPos() ? Definer.colorRef.enemy : (character.CharaStatus().player) ? Definer.colorRef.player : Definer.colorRef.abilityColors[5];
            //if (omenSet) { sign.sprite = signIcons[(int)omen.abilityType]; }
            if(tweener == null) { tweener = charaSprite.transform.DOLocalMoveX(0, 0.5f); }
        }
    }
    public void OnMouseDown()
    {
        if (revealed) { character.DisplayInfo(); }
        else { FindObjectOfType<InfoText>().SetText_Old("•s–¾", ""); }
    }

    public void RemoveTurnOrderIcon()
    {
        //if (!destroyed)
        //{
        //    destroyed = true;
        //    Destroy(gameObject);
        //}
        gameObject.SetActive(false);
        //if (gameObject != null) { Destroy(gameObject); }
    }

    public  Character GetCharacter() { return character; }
}
