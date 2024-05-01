using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utility : MonoBehaviour
{
    [SerializeField]
    bool debug;
    

    public string GetColoredText(Color color, string text)
    {
        return "<color=#" + ColorUtility.ToHtmlStringRGB(color) + ">" + text + "</color>";
    }
    //for (int i = 0; i < Parent.childCount; i++) { Destroy(Parent.GetChild(i).gameObject); }

    /// <summary>
    /// dir: 0:right 1:upper 2:lower 3:left
    /// range: à⁄ìÆãóó£(1or2)
    /// </summary>
    /// <param name="dir">0:right 1:upper 2:lower 3:left</param>
    /// <param name="range">à⁄ìÆãóó£(1or2)</param>
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
                print(string.Format("error:åªç›ÇÃpos{0},dir{1},range{2}", currentPos, dir, range));
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
    public Vector2Int posIntToVector(int pos) { return new Vector2Int(Mathf.FloorToInt(pos / 3), pos % 3); }

}
