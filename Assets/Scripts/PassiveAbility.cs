using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassiveAbility : MonoBehaviour
{
   protected Character character;
    protected Character.CharacterStatus charaStatus;
    /// <summary>0:StE 1:Personality 2:Equipment</summary>
    int PAType;
    /// <summary>0:StE 1:Personality 2:Equipment</summary>
    public int GetPAType() { return PAType; }
    public virtual string GetPAName() { return ""; }
    public virtual string GetPAInfo() {
        print("error:GetPAInfoÇÃoverrideÇ™ê›íËÇ≥ÇÍÇƒÇ¢Ç‹ÇπÇÒ");
        return ""; }
    public void Init(Character c,int type)
    {
        character = c;
        PAType = type;
        charaStatus = character.GetCharacterStatus();
        OnPAInit();
    }
    public void Disable()
    {
        AtTheEnd();
        character.RemovePA(this);
        Destroy(gameObject);
    }
    public virtual void OnPAInit() { }
    public virtual void AtTheEnd() { }

    public virtual void OnBattleStart() { }
    public virtual void OnRoundStart() { }
    public virtual void OnTurnStart() { }
    public virtual void OnTurnEnd() { }
    public virtual void OnRoundEnd() { }
    public virtual void OnBattleEnd() { }


    public virtual void OnActivateAbility() { }
    /// <summary>çUåÇñΩíÜéû</summary>
    public virtual void OnDamage(int DMG, Character target) {  }
    public virtual void OnCRIT(int ID) { }
    public virtual void OnKill(int ID) { }
    public virtual void OnMiss(int ID) { }
    public virtual void OnHeal(int healValue, int ID) { }
    //public virtual void OnApplyStE() { }
    //public virtual void OnRemoveStE() { }

    public virtual void BecomeAbilityTarget(Character actor) { }
    
    public virtual void OnDamaged(int DMG, Character attacker) { }
    
    public virtual void OnCRITed(int ID) { }
    public virtual void OnEvade(int ID) { }
    public virtual void OnHealed(int healedValue, int ID) { }
}
