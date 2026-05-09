using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Per_OnHPDecreased : Per_Master
{
    public override void OnDecreasedHP(int value)
    {
        Activate();
    }
}
