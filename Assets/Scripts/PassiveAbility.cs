using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassiveAbility : MonoBehaviour
{
   protected Character character;
    //protected Character.CharacterStatus charaStatus;
    protected CharactersManager charactersManager;
    /// <summary>0:StE 1:Personality 2:Equipment</summary>
    int PAType;
    /// <summary>0:StE 1:Personality 2:Equipment</summary>
    public int GetPAType() { return PAType; }
    public virtual string GetPAName() { return ""; }
    public virtual string GetPAInfo()
    {
        print("error:GetPAInfo‚Ìoverride‚ªİ’è‚³‚ê‚Ä‚¢‚Ü‚¹‚ñ");
        return "error:GetPAInfo‚Ìoverride‚ªİ’è‚³‚ê‚Ä‚¢‚Ü‚¹‚ñ";
    }
    public void Init(Character c,int type)
    {
        character = c;
        PAType = type;
        //charaStatus = character.GetCharacterStatus();
        charactersManager=FindObjectOfType<CharactersManager>();
        OnPAInit();
    }
    public void Disable()
    {
        AtTheEnd();
        character.RemovePA(this);
        if (PAType == 0) { GetComponent<PA_StatusEffect>().DestroyIcon(); }
        Destroy(gameObject);
    }
    public virtual void OnPAInit() { }
    public virtual void AtTheEnd() { }

    public virtual void OnBattleStart() { }
    public virtual void OnRoundStart() { }
    public virtual void OnTurnOrderDecide() { }

    public virtual void OnTurnStart(bool myTurn, int turnCount) { }
    public virtual void OnTurnEnd() { }
    public virtual void OnRoundEnd() { }
    public virtual void OnBattleEnd() { }


    public virtual void OnActivateAbility() { }
    /// <summary>UŒ‚A–½’†‚µ‚½‚©‚ÉŠÖ‚í‚ç‚¸—U”­</summary>
    public virtual void OnAttack(bool evaded, bool missed) { }
    /// <summary>UŒ‚–½’†</summary>
    public virtual void OnDamage(int DMG, Character target,Action.ActionStatus actionStatus) {  }
    public virtual void OnCRIT(int ID) { }
    public virtual void OnKill(int ID) { }
    public virtual void OnMiss(int ID) { }
    public virtual void OnHeal(int healValue, int ID) { }
    //public virtual void OnApplyStE() { }
    //public virtual void OnRemoveStE() { }

    public virtual void BecomeAbilityTarget(Character actor) { }
    public virtual void OnAttacked(Character attacker, bool evaded, bool missed) { }

    public virtual void OnDamaged(int DMG, Character attacker) { }
    
    public virtual void OnCRITed(int ID) { }
    public virtual void OnEvade(int ID) { }
    public virtual void OnHealed(int healedValue, int ID) { }
}
