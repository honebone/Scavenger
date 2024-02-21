using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PE_Narrow : PositionEffect
{
    [SerializeField]
    GameObject actionMod;

    public override void OnPEInit()
    {
        positionManager.AddActionMod(actionMod, true);
    }
  
    public override void AtTheEnd()
    {
        positionManager.AddActionMod(actionMod, false);
    }

}
