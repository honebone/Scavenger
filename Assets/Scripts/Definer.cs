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
    public static GameObject abilityManager_General;
    public static GameObject actionManager_General;
    [SerializeField]
    ColorRef colorRef_Inspector;
    [SerializeField]
    GameObject abilityManager_General_Inspector;
    [SerializeField]
    GameObject actionManager_General_Inspector;

    public static Dictionary< AbilityData.AbilityType, string> AbiltyTypeName = new Dictionary<AbilityData.AbilityType, string>(){
    {AbilityData.AbilityType.other,"ì¡éÍ"}, {AbilityData.AbilityType.attack,"çUåÇ"},{AbilityData.AbilityType.heal,"âÒïú"},
    {AbilityData.AbilityType.buff,"ã≠âª"},{AbilityData.AbilityType.debuff,"é„ëÃâª"},{AbilityData.AbilityType.summon,"è¢ä´"}
};

    private void Awake()
    {
        colorRef = colorRef_Inspector;
        abilityManager_General = abilityManager_General_Inspector;
        actionManager_General=actionManager_General_Inspector;
    }
}
