using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Extentions
{
    public static string ColorStr(this string str,Color color)
    {
        return "<color=#" + ColorUtility.ToHtmlStringRGB(color) + ">" + str + "</color>";
    }
    public static Color ATToColor(this AbilityData.AbilityType abilityType)
    {
        return Definer.colorRef.abilityColors[(int)abilityType];
    }
}
