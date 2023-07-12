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
    public static bool Probability(this float fPercent)
    {
        bool result;
        float fProbabilityRate = UnityEngine.Random.value * 100.0f;
        // Debug.Log(fProbabilityRate.ToString());
        if (fPercent == 100.0f && fProbabilityRate == fPercent)
        {
            result = true;
        }
        else if (fProbabilityRate < fPercent)
        {
            result = true;
        }
        else
        {
            result = false;
        }
        //if (debug) { Debug.Log("確率：" + fPercent.ToString("N1") + "出目：" + fProbabilityRate.ToString("N1") + "結果：" + result); }
        return result;
    }
}
