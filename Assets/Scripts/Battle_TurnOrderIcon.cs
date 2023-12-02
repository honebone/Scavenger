using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Battle_TurnOrderIcon : MonoBehaviour
{
    [SerializeField]
    Image charaSprite;
    [SerializeField]
    Image sign;
    [SerializeField]
    Image frame;

    [SerializeField]
    Sprite unrevealedIcon;
    [SerializeField]
    Sprite[] signIcons;

    Character character;
    //bool omenSet;
    //Ability.AbilityStatus omen;
    bool revealed;
    public void Init(Character chara,bool reveal) {
        character = chara;
        revealed = reveal;
        if (revealed) { charaSprite.sprite = character.GetCharacterStatus().spriteForUI; }
        else
        {
            charaSprite.sprite = unrevealedIcon;
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
            charaSprite.sprite = character.GetCharacterStatus().spriteForUI;
            frame.color = Color.white;
            //if (omenSet) { sign.sprite = signIcons[(int)omen.abilityType]; }
        }
    }
    public void OnMouseDown()
    {
        if (revealed) { character.DisplayInfo(); }
        else { FindObjectOfType<InfoText>().SetText("•s–¾", ""); }
    }

    public void RemoveTurnOrderIcon(Character chara) { if (character == chara) {
            FindObjectOfType<InfoText>().AddDebugText(string.Format("”j‰ó:{0}at{1}", character.GetCharacterStatus().charaName, character.GetCharacterStatus().position));
            Destroy(gameObject); } }

    public  Character GetCharacter() { return character; }
}
