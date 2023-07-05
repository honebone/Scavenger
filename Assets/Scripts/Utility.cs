using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utility : MonoBehaviour
{
    [SerializeField]
    bool debug;
    public bool Probability(float fPercent)
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
        if (debug) { Debug.Log("確率：" + fPercent.ToString("N1") + "出目：" + fProbabilityRate.ToString("N1") + "結果：" + result); }
        return result;
    }
    public int ChoiceWithWeight(float[] weight)
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

    public string GetColoredText(Color color, string text)
    {
        return "<color=#" + ColorUtility.ToHtmlStringRGB(color) + ">" + text + "</color>";
    }
    //for (int i = 0; i < Parent.childCount; i++) { Destroy(Parent.GetChild(i).gameObject); }

    /// <summary>
    /// dir: 0:right 1:upper 2:lower 3:left
    /// range: 移動距離(1or2)
    /// </summary>
    /// <param name="dir">0:right 1:upper 2:lower 3:left</param>
    /// <param name="range">移動距離(1or2)</param>
    /// <returns></returns>
    public int GetMoveToPos(int currentPos, int dir, int range)
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
                print(string.Format("error:現在のpos{0},dir{1},range{2}", currentPos, dir, range));
                return -1;
        }
    }
    /// <summary>
    /// 0:right 1:upper 2:lower 3:left
    /// </summary>
    /// <param name="currentPos"></param>
    /// <param name="moveToPos"></param>
    /// <returns></returns>
    public int GetMoveDir(int currentPos,int moveToPos)
    {
        if (currentPos + 3 == moveToPos || currentPos + 6 == moveToPos) { return 0; }
        if (currentPos + 1 == moveToPos || currentPos + 2 == moveToPos) { return 1; }
        if (currentPos - 1 == moveToPos || currentPos - 2 == moveToPos) { return 2; }
        if (currentPos - 3 == moveToPos || currentPos - 6 == moveToPos) { return 3; }
        print("error");
        return -1;
    }

}
