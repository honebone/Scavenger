using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Definer : MonoBehaviour
{
    [System.Serializable]
   public class ColorRef
    {
        /// <summary>0:other 1:attack 2:heal 3:buff 4:debuff 5:summon</summary>
        public Color[] abilityColors;
    }

    public static ColorRef colorRef;
    [SerializeField]
    ColorRef colorRef_Inspector;

    private void Awake()
    {
        colorRef = colorRef_Inspector;
    }
}
