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
    

    [SerializeField] GameObject SANBarObj;

    [SerializeField] Slider HPBar;
    public Image HPBarFill;
    [SerializeField] Slider DMGBar;
    [SerializeField] Image DMGBarFill;
    [SerializeField] Slider ShieldBar;
    public Image ShieldBarFill;
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
    Tweener moveAnim;

    Character character;
    CharactersManager charactersManager;

    Sequence sequence;
    GameObject charaSprite;


    //Character.CharacterStatus characterStatus, GameObject charaManager, Character_TargetButton tb, bool dropItem, Character.SummonCharaStatusParams summonCharaParams
    public Character Init(SpawnCharaParams spawnCharaParams)
    {
        if (spawnCharaParams.generatedCharaStatus.Obstacle()) { HPBarColor.color = Definer.colorRef.failed_unavailable; }

        charactersManager = FindObjectOfType<CharactersManager>();

        if (spawnCharaParams.generatedCharaStatus.position >= 9) { charaSpriteParent.Rotate(new Vector3(0, 180, 0)); }

        var c = Instantiate(spawnCharaParams.manager, characterManagerParent);
        character = c.GetComponent<Character>();
        character.Init(spawnCharaParams, this);

        foreach (Ability.AbilityStatus abilityStatus in spawnCharaParams.generatedCharaStatus.abilitiesStatus)
        {
            GameObject abilityManager;
            if (abilityStatus.abilityManager != null) { abilityManager = abilityStatus.abilityManager; }
            else { abilityManager = Definer.abilityManager_General; }
            var a = Instantiate(abilityManager, abilitiesP);
            a.GetComponent<Ability>().Init(character, abilityStatus);
            abilityStatus.SetManager(a.GetComponent<Ability>());
        }

        if (spawnCharaParams.madness)
        {
            HPBarColor.color = Definer.colorRef.affricted;
        }

        charactersManager.AddCharacter(character);
        return character;
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

    float barLength = 0;
    public Sprite spr_bar;
    public Sprite spr_bar2;
    public int sprSwapTH = 200;
    public void SetHPandShieldBar()
    {
        Character.CharacterStatus status = character.CharaStatus();
        if (barLength != status.maxHP + status.shield)
        {
            if(status.maxHP + status.shield >= sprSwapTH)
            {
                barLength = (status.maxHP + status.shield) / 5f;
                HPBarFill.sprite=spr_bar2;
                DMGBarFill.sprite=spr_bar2;
                HPBarFill.sprite=spr_bar2;
            }
            else
            {
                barLength= status.maxHP + status.shield;
                HPBarFill.sprite = spr_bar;
                DMGBarFill.sprite = spr_bar;
                HPBarFill.sprite = spr_bar;
            }
            Vector2 width = new Vector2(barLength * 0.8f, 10);
            Vector2 scale = new Vector2(5f / barLength, 0.08f);
            HPBar.GetComponent<RectTransform>().sizeDelta = width;
            HPBar.GetComponent<RectTransform>().localScale = scale;
            DMGBar.GetComponent<RectTransform>().sizeDelta = width;
            DMGBar.GetComponent<RectTransform>().localScale = scale;
            ShieldBar.GetComponent<RectTransform>().sizeDelta = width;
            ShieldBar.GetComponent<RectTransform>().localScale = scale;
        }
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
       //move= StartCoroutine(Move( pos));
        if (moveAnim != null) { moveAnim.Kill(true); }
        moveAnim = transform.DOMove(charactersManager.GetCharacterWorldPos(pos), 0.3f);
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
