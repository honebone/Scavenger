using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Extentions
{
    public static string ColorStr(this string str,Color color)
    {
        return "<color=#" + ColorUtility.ToHtmlStringRGB(color) + ">" + str + "</color>";
    }
    public static Color ToColor(this AbilityData.AbilityType abilityType)
    {
        return Definer.colorRef.abilityColors[(int)abilityType];
    }
    public static Color ToColor(this PA_Personality.PersonalityStatus.PersonalityType type)
    {
        return Definer.colorRef.personalityColors[(int)type];
    }
    public static Color ToColor(this PA_StatusEffect.StatusEffectStatus.StatusEffectType type)
    {
        return Definer.colorRef.statusEffectColors[(int)type];
    }
    public static Color ToColor(this ItemData.Rarity rarity)
    {
        return Definer.colorRef.rarityColors[(int)rarity];
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
    public static bool Probability(this int fPercent)
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
        //Debug.Log("確率：" + fPercent.ToString("N1") + "出目：" + fProbabilityRate.ToString("N1") + "結果：" + result); 
        return result;
    }
    public static int ChoiceWithWeight(this float[] weight)
    {
        float sum = 0;
        foreach (float c in weight)
        {
            sum += c;
        }
        float dice = Random.Range(0, sum);
        //Debug.Log(dice.ToString());
        for (int i = 0; i < weight.Length; i++)
        {
            if (dice < weight[i])
            {
                //Debug.Log(i.ToString());
                return i;
            }
            dice -= weight[i];
        }
        if (dice == sum) { return weight.Length - 1; }
        else
        {
            Debug.Log("error");
            return -1;
        }
    }
    public static int ChoiceWithWeight(this List<float> weight)
    {
        float sum = 0;
        foreach (float c in weight)
        {
            sum += c;
        }
        float dice = Random.Range(0, sum);
        //Debug.Log(dice.ToString());
        for (int i = 0; i < weight.Count; i++)
        {
            if (dice < weight[i])
            {
                //Debug.Log(i.ToString());
                return i;
            }
            dice -= weight[i];
        }
        if (dice == sum) { return weight.Count - 1; }
        else
        {
            Debug.Log("error");
            return -1;
        }
    }
    public static int ChoiceWithWeight(this List<int> weight)
    {
        float sum = 0;
        foreach (float c in weight)
        {
            sum += c;
        }
        float dice = Random.Range(0, sum);
        //Debug.Log(dice.ToString());
        for (int i = 0; i < weight.Count; i++)
        {
            if (dice < weight[i])
            {
                //Debug.Log(i.ToString());
                return i;
            }
            dice -= weight[i];
        }
        if (dice == sum) { return weight.Count - 1; }
        else
        {
            Debug.Log("error");
            return -1;
        }
    }

}
