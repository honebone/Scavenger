using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionManager : MonoBehaviour
{
    Character character;

    List<PositionEffect> positionEffects = new List<PositionEffect>();
    List<PositionEffect> deletePEs = new List<PositionEffect>();

    public void RemovePE(PositionEffect positionEffect)
    {
        deletePEs.Add(positionEffect);
    }
    void RemovePA_Execute()
    {
        foreach (PositionEffect deletePE in deletePEs) { positionEffects.Remove(deletePE); }
        deletePEs.Clear();
    }

    public void SetCharacter(Character chara)
    {
        bool f = false;
        if (character != chara)
        {
            f = true;
            if (character != null)
            {
                foreach (PositionEffect positionEffect in positionEffects) { positionEffect.OnCharaLeave(); }
                RemovePA_Execute();
            }   
        }
        character = chara;
        if (f)
        {
            foreach (PositionEffect positionEffect in positionEffects) { positionEffect.OnCharaEnter(); }
            RemovePA_Execute();
        }
    }
    public void ResetCharacter()
    {
        if (character != null)
        {
            foreach (PositionEffect positionEffect in positionEffects) { positionEffect.OnCharaLeave(); }
            RemovePA_Execute();
        }
        character = null;
    }
}
