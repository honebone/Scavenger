using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript : MonoBehaviour
{
    [SerializeField] int chance;//Šm—¦[%]
    private void Start()
    {
        if (chance.Dice())
        {
            Debug.Log("true");
        }

        if (50.Dice())
        {
            Debug.Log("true");
        }
    }
}
