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

}
