using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Character_Object : MonoBehaviour
{
    [SerializeField]
    Transform characterManagerParent;

    [SerializeField]
    Transform charaSpriteParent;

    [SerializeField]
    GameObject HPBarObj;
    [SerializeField]
    GameObject ShieldBarObj;
    [SerializeField]
    GameObject SANBarObj;

    Slider HPBar;
    Slider ShieldBar;
    Slider SANBar;

    [SerializeField]
    Transform TurnIconParent;
    [SerializeField]
    GameObject TurnIcon;
    [SerializeField]
    Sprite currentTurn;
    [SerializeField]
    Sprite endedTurn;

    [SerializeField]
    Image selectedIcon;

    [SerializeField]
    Transform damageTextParent;
    [SerializeField]
    GameObject damagText;
    

    Character character;
    public void Init(Character.CharacterStatus characterStatus, GameObject charaManager,Character_TargetButton tb,CharactersManager cm)
    {
        HPBar = HPBarObj.GetComponent<Slider>();
        ShieldBar = ShieldBarObj.GetComponent<Slider>();
        SANBar = SANBarObj.GetComponent<Slider>();

        if (characterStatus.position >= 9) { charaSpriteParent.Rotate(new Vector3(0, 180, 0)); }

        var c = Instantiate(charaManager, characterManagerParent);
        character=c.GetComponent<Character>();
        character.Init(characterStatus, this,tb);
        cm.AddCharacter(character);
    }

    public void SetCharaSprite(GameObject sprite)
    {
        if (charaSpriteParent.childCount > 0) { for (int i = 0; i < charaSpriteParent.childCount; i++) { Destroy(charaSpriteParent.GetChild(i).gameObject); } }
        Instantiate(sprite, charaSpriteParent);
    }

    public void DisableSANBar() { SANBarObj.SetActive(false); }
    public void SetHPandShieldBar()
    {
        Character.CharacterStatus status = character.GetCharacterStatus();
        ShieldBar.maxValue = status.maxHP + status.shield;
        ShieldBar.value = status.HP + status.shield;

        HPBar.maxValue = status.maxHP + status.shield;
        HPBar.value = status.HP;
    }
    public void SetSANBar()
    {
        Character.CharacterStatus status = character.GetCharacterStatus();
        SANBar.maxValue = status.maxSAN;
        SANBar.value = status.SAN;
    }

    int turnCounter;
    public void SetTurnIcons(int turns)
    {
        turnCounter = 0;
        for (int i = 0; i < TurnIconParent.childCount; i++) { Destroy(TurnIconParent.GetChild(i).gameObject); }
        for (int i=0;i<turns; i++)
        {
            Instantiate(TurnIcon, TurnIconParent);
        }
    }
    public void SetTurnIcon_CurentTurn()
    {
        TurnIconParent.GetChild(turnCounter).GetComponent<Image>().sprite = currentTurn;
    }
    public void SetTurnIcon_End()
    {
        TurnIconParent.GetChild(turnCounter).GetComponent<Image>().sprite = endedTurn;
        turnCounter++;
    }

    public void SetSelectedIcon(bool set)
    {
        selectedIcon.enabled = set;
    }

    public void SetDamageText(string text,Color color)
    {
        var d = Instantiate(damagText, damageTextParent);
        d.GetComponent<DamageText>().Init(text, color);
    }
}
