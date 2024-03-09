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
    public static Color ToColor(this PositionEffect.PositionEffectStatus.PositionEffectType type)
    {
        return Definer.colorRef.positionEffectColors[(int)type];
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
    public static int RandIndex(this int length)
    {
        return Random.Range(0,length);
    }
    public static Vector2Int PosIntToVector(this int posInt)
    {
         return new Vector2Int(Mathf.FloorToInt(posInt / 3), posInt % 3);
    }
    public static string PosIntToStr(this int posInt)
    {
        Vector2Int posVec=posInt.PosIntToVector();
        return string.Format("({0},{1})", posVec.x + 1, posVec.y + 1);
    }
    /// <summary>
    /// 自身の位置から各方向にどれだけ移動可能かを返す (0:right 1:upper 2:lower 3:left)
    /// pos:position(0-17)
    /// </summary>
    public static List<int> GetMovableRanges(this int pos)
    {
        int[] range = new int[4];
        int p = pos % 9;
        Vector2Int vector = p.PosIntToVector();
        switch (vector.x)
        {
            case 0:
                range[0] = 2;
                range[3] = 0;
                break;
            case 1:
                range[0] = 1;
                range[3] = 1;
                break;
            case 2:
                range[0] = 0;
                range[3] = 2;
                break;
        }
        switch (vector.y)
        {
            case 0:
                range[1] = 2;
                range[2] = 0;
                break;
            case 1:
                range[1] = 1;
                range[2] = 1;
                break;
            case 2:
                range[1] = 0;
                range[2] = 2;
                break;
        }
        return new List<int>(range);
    }
    /// <summary>
    /// dir: 0:right 1:upper 2:lower 3:left
    /// range: 移動距離(1or2)
    /// </summary>
    /// <param name="dir">0:right 1:upper 2:lower 3:left</param>
    /// <param name="range">移動距離(1or2)</param>
    /// <returns></returns>
    public static int GetMoveToPos(this int currentPos, int dir, int range)
    {
        switch (dir)
        {
            case 0:
                return currentPos + (3 * range);
            case 1:
                return currentPos + range;
            case 2:
                return currentPos - range;
            case 3:
                return currentPos - (3 * range);
            default:
                return -1;
        }
    }
    /// <summary>
    /// どこの列にいるかを返す
    /// 0:front 1:mid 2:back
    /// </summary>
    public static int GetColumn(this int currentPos)
    {
        int x = currentPos.PosIntToVector().x;
        if (x == 2 || x == 3) { return 0; }
        if (x == 1 || x == 4) { return 1; }
        if (x == 0 || x == 5) { return 2; }
        return -1;
    }

    public static float GetPercent(this int value,int max)
    {
        return value * 100f / max;
    }
    /// <summary>重複なしで指定された個数の配列をランダムに取得　要素数<=指定個数の時はリスト全体を返す</summary>
    public static List<T> Sample<T>(this List<T> list, int amount)
    {
        if (list.Count <= amount) { return list; }

        List<T> pool = new List<T>(list);
        List<T> sample = new List<T>();
        int index;
        for(int i = 0; i < amount; i++)
        {
            index = RandIndex(pool.Count);
            sample.Add(pool[index]);
            pool.RemoveAt(index);
        }

        return sample;
    }public static T Choice<T>(this List<T> list)
    {
        return list[Random.Range(0, list.Count)];
    }
}
