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
    Image TurnIcon;
    [SerializeField]
    Sprite currentTurn;
    [SerializeField]
    Sprite endedTurn;

    public void Init(Character.CharacterStatus characterStatus, GameObject charaManager)
    {
        HPBar = HPBarObj.GetComponent<Slider>();
        ShieldBar = ShieldBarObj.GetComponent<Slider>();
        SANBar = SANBarObj.GetComponent<Slider>();
        var c = Instantiate(charaManager, characterManagerParent);
        c.GetComponent<Character>().Init(characterStatus, this);
    }

    public void SetCharaSprite(GameObject sprite)
    {
        if (charaSpriteParent.childCount > 0) { for (int i = 0; i < charaSpriteParent.childCount; i++) { Destroy(charaSpriteParent.GetChild(i).gameObject); } }
        Instantiate(sprite, charaSpriteParent);
    }

    public void DisableSANBar() { SANBarObj.SetActive(false); }
    public void SetHPandShieldBar(Character.CharacterStatus status)
    {
        ShieldBar.maxValue = status.maxHP + status.shield;
        ShieldBar.value = status.HP + status.shield;

        HPBar.maxValue = status.maxHP + status.shield;
        HPBar.value = status.HP;
    }
    public void SetSANBar(Character.CharacterStatus status)
    {
        SANBar.maxValue = status.maxSAN;
        SANBar.value = status.SAN;
    }
   
}
