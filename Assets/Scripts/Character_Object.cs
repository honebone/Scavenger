using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Character_Object : MonoBehaviour
{
    [SerializeField] CanvasGroup canvasGroup;
    [SerializeField]
    Transform characterManagerParent;
    [SerializeField]
    Transform abilitiesP;

    [SerializeField]
    Transform charaSpriteParent;

    
    [SerializeField]
    Image HPBarColor;    
    [SerializeField]
    Image SANFill;
    [SerializeField]
    Image DMGBarFill;

    [SerializeField] GameObject SANBarObj;

    [SerializeField] Slider HPBar;
    [SerializeField] Slider DMGBar;
    [SerializeField] Slider ShieldBar;
    [SerializeField] Slider SANBar;

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

    [SerializeField] float actAnim_move;
    [SerializeField] float actAnim_duration;

    Sequence DMGAnim;

    Character character;
    CharactersManager charactersManager;

    Sequence sequence;
    GameObject charaSprite;
    public void Init(Character.CharacterStatus characterStatus, GameObject charaManager, Character_TargetButton tb, bool dropItem, Character.SummonCharaStatusParams summonCharaParams)
    {
        if (characterStatus.obstacle) { HPBarColor.color = Definer.colorRef.failed_unavailable; }

        charactersManager = FindObjectOfType<CharactersManager>();

        if (characterStatus.position >= 9) { charaSpriteParent.Rotate(new Vector3(0, 180, 0)); }

        var c = Instantiate(charaManager, characterManagerParent);
        character = c.GetComponent<Character>();
        character.Init(characterStatus, this, tb, dropItem,summonCharaParams);

        foreach (Ability.AbilityStatus abilityStatus in characterStatus.abilitiesStatus)
        {
            GameObject abilityManager;
            if (abilityStatus.abilityManager != null) { abilityManager = abilityStatus.abilityManager; }
            else { abilityManager = Definer.abilityManager_General; }
            var a = Instantiate(abilityManager, abilitiesP);
            a.GetComponent<Ability>().Init(character, abilityStatus);
            abilityStatus.SetManager(a.GetComponent<Ability>());
        }

        charactersManager.AddCharacter(character);
    }
    /// <summary>Ž€–SŽž‚ÉŒÄ‚Î‚ê‚é </summary>
    public void HideCharacterObj()
    {
        if (charaSpriteParent.childCount > 0) { for (int i = 0; i < charaSpriteParent.childCount; i++) { Destroy(charaSpriteParent.GetChild(i).gameObject); } }
        canvasGroup.alpha = 0;
        for (int i = 0; i < TurnIconParent.childCount; i++) { Destroy(TurnIconParent.GetChild(i).gameObject); }
        selectedIcon.enabled = false;
    }

    public void SetCharaSprite(GameObject sprite)
    {
        Character.CharacterStatus status = character.CharaStatus();
        Vector2 pos = charactersManager.GetCharacterWorldPos(status.position);
        Vector2 offset = status.characterData.spriteOffset;
        if (status.position < 9) { offset.x *= -1; }

        if (charaSpriteParent.childCount > 0) { for (int i = 0; i < charaSpriteParent.childCount; i++) { Destroy(charaSpriteParent.GetChild(i).gameObject); } }
        charaSprite = Instantiate(sprite, pos+offset, Quaternion.identity, charaSpriteParent);
        if (status.position >= 9) { charaSprite.transform.Rotate(new Vector3(0, 180, 0)); }
        //if (charaSpriteParent.childCount > 0) { for (int i = 0; i < charaSpriteParent.childCount; i++) { Destroy(charaSpriteParent.GetChild(i).gameObject); } }
        //charaSprite= Instantiate(sprite, charaSpriteParent);
    }
    public void ActAnim()
    {
        if (charaSprite)
        {
            if (sequence != null) { sequence.Kill(true); }
            sequence = DOTween.Sequence();
            sequence.Append(charaSprite.transform.DOLocalMoveY(actAnim_move, actAnim_duration / 2f).SetRelative(true).SetEase(Ease.OutCubic));
            sequence.Append(charaSprite.transform.DOLocalMoveY(-actAnim_move, actAnim_duration / 2f).SetRelative(true).SetEase(Ease.InCubic));

            sequence.Play();
        }
    }

    public void DisableSANBar() { SANBarObj.SetActive(false); }

    //Coroutine barAnim;
    public void SetHPandShieldBar()
    {
        Character.CharacterStatus status = character.CharaStatus();
        ShieldBar.maxValue = status.maxHP + status.shield;
        ShieldBar.value = status.HP + status.shield;

        DMGBar.maxValue = status.maxHP + status.shield;
        HPBar.maxValue = status.maxHP + status.shield;
        HPBar.value = status.HP;

        if(DMGBar.value > HPBar.value)
        {
            DMGBarFill.color = Definer.colorRef.CRIT;

            if (DMGAnim != null) { DMGAnim.Kill(); }
            DMGAnim = DOTween.Sequence();
            DMGAnim.Append(DMGBar.DOValue(HPBar.value, 0.5f).SetEase(Ease.InExpo));
            DMGAnim.Join(DMGBarFill.DOColor(Color.red, 0.25f).SetEase(Ease.InExpo));

            DMGAnim.Play();
        }
        else { DMGBar.value = HPBar.value; }
    }

    IEnumerator DMGBarAnim()
    {
        yield return new WaitForSeconds(0.3f);
        int dec = Mathf.CeilToInt(character.CharaStatus().maxHP * 0.05f);
        while (DMGBar.value > HPBar.value)
        {
            yield return new WaitForSeconds(0.05f);
            DMGBar.value -= dec;
        }
        DMGBar.value = HPBar.value;
    }
    public void SetSANBar()
    {
        Character.CharacterStatus status = character.CharaStatus();
        SANBar.maxValue = status.maxSAN;
        SANBar.value = status.SAN;
    }
    public void Affrict()
    {
        SANFill.color = Definer.colorRef.affricted;
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
    public void AddTurnIcons(int turns)
    {
        for (int i = 0; i < turns; i++)
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

    //public void SetDamageText(string text, Color color)
    //{
    //    //var d = Instantiate(damagText, damageTextParent);
    //    //d.GetComponent<DamageText>().Init(text, color);
    //}

    public GameObject SetStEIcon()
    {
        var s=Instantiate(Definer.statusEffectIcon,StEIconParent);
        return s;
    }

    Coroutine move;
    public void MoveStart(int pos)
    {
       move= StartCoroutine(Move( pos));
    }
    public void StopMove(int pos)
    {
        if (move != null) { StopCoroutine(move); }
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

    public void SetAbilityManager(Ability.AbilityStatus abilityStatus)
    {
        GameObject abilityManager;
        if (abilityStatus.abilityManager != null) { abilityManager = abilityStatus.abilityManager; }
        else { abilityManager = Definer.abilityManager_General; }
        var a = Instantiate(abilityManager, abilitiesP);
        a.GetComponent<Ability>().Init(character, abilityStatus);
        abilityStatus.SetManager(a.GetComponent<Ability>());
    }
}
