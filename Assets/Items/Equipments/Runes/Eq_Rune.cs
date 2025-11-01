using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eq_Rune : PA_Equipment
{
    // Start is called before the first frame update
    void Start()
    {
        if (!PATags.Contains(PATag.ルーン)) { infoText.AddErrorText("魔術装備品のタグに[ルーン]がありません"); }
    }
}
