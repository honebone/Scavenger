using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P_Hunter_PA : PA_Personality
{
    [SerializeField, TextArea(3, 10)]
    string info;
    public override string GetPAInfo_Base()
    {
        return info;
    }
}
