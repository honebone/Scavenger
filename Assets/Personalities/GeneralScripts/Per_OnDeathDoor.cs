using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Per_OnDeathDoor : Per_Master
{
    public override void OnDecreasedHP(int value)
    {
        if (character.CharaStatus().HP == 0) { Activate(); }
    }
}
