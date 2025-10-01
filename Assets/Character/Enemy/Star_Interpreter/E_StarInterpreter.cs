using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class E_StarInterpreter : PA_Personality
{
    public GameObject stardust;
    public int TH1;
    public int TH2;
    public Action.ActionStatus onRS_self;
    public Action.ActionStatus omen;

    List<int> list = new List<int> { 0, 1, 2, 3, 4, 5, 6, 7, 8 };
    public override void OnRoundStart()
    {
        int center = list.Choice();
        int stack = character.GetStEStack_Sum(stardust);
        List<int> targets = new List<int>();
        if (stack >= TH2)
        {
            targets = list.Where(m => (m.GetColumn() == center.GetColumn()) || (m.GetRow() == center.GetRow())).ToList();
        }
        else if (stack >= TH1)
        {
            targets = list.Where(m => m.GetRow() == center.GetRow()).ToList();
        }
        else
        {
            targets = new List<int> { center };
        }

        Enqueue_Int(omen, true, targets);
        Enqueue_Self(onRS_self);
    }

    public override string GetPAInfo_Base()
    {
        string s = omen.GetInfo() + "\n";
        s += onRS_self.GetInfo();

        return s;
    }
}
