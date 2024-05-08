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
    Image HPBarColor;
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

    [SerializeField]
    Transform StEIconParent;
    

    Character character;
    CharactersManager charactersManager;
    public void Init(Character.CharacterStatus characterStatus, GameObject charaManager,Character_TargetButton tb, bool dropItem)
    {
        HPBar = HPBarObj.GetComponent<Slider>();
        if (characterStatus.obstacle) { HPBarColor.color = Definer.colorRef.failed_unavailable; }
        ShieldBar = ShieldBarObj.GetComponent<Slider>();
        SANBar = SANBarObj.GetComponent<Slider>();

        charactersManager = FindObjectOfType<CharactersManager>();

        if (characterStatus.position >= 9) { charaSpriteParent.Rotate(new Vector3(0, 180, 0)); }

        var c = Instantiate(charaManager, characterManagerParent);
        character=c.GetComponent<Character>();
        character.Init(characterStatus, this,tb,dropItem);
        charactersManager.AddCharacter(character);
    }
    /// <summary>Ž€–SŽž‚ÉŒÄ‚Î‚ê‚é </summary>
    public void HideCharacterObj()
    {
        FindObjectOfType<InfoText>().AddDebugText("Ok");
        if (charaSpriteParent.childCount > 0) { for (int i = 0; i < charaSpriteParent.childCount; i++) { Destroy(charaSpriteParent.GetChild(i).gameObject); } }
        HPBarObj.SetActive(false);
        ShieldBarObj.SetActive(false);
        SANBarObj.SetActive(false);
        for (int i = 0; i < TurnIconParent.childCount; i++) { Destroy(TurnIconParent.GetChild(i).gameObject); }
        selectedIcon.enabled = false;
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

    public GameObject SetStEIcon()
    {
        var s=Instantiate(Definer.statusEffectIcon,StEIconParent);
        return s;
    }

    public void MoveStart(int pos)
    {
        StartCoroutine(Move( pos));
    }
    public void StopMove(int pos)
    {
        StopCoroutine(Move(0));
        transform.position = charactersManager.GetCharacterWorldPos(pos);
    }
    IEnumerator Move(int pos)
    {
        int moveTime = 15;

        Vector3 moveToPos = charactersManager.GetCharacterWorldPos(pos);//–Ú“I’n
        Transform tf = this.transform;
        Vector3 delta = new Vector3(moveToPos.x - tf.position.x, moveToPos.y - tf.position.y, 0) / moveTime;
        var wait = new WaitForSeconds(0.02f);

        for (int i = 0; i < moveTime; i++)
        {
            tf.position += delta;
            yield return wait;
        }
    }
}
