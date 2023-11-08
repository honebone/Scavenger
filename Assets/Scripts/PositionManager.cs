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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
