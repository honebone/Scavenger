using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Definer : MonoBehaviour
{
    [System.Serializable]
   public class ColorRef
    {
        public Color debug;
        /// <summary>0:other 1:attack 2:heal 3:buff 4:debuff 5:summon</summary>
        public Color[] abilityColors;
        public Color[] personalityColors;

        public Color decreaseHP;
        public Color damage;
        public Color CRIT;
        public Color evade;
        public Color heal;
        public Color shield;
        public Color SANHeal;
        public Color SANDecrease;


        public Color failed_unavailable;


    }
    [System.Serializable]
    public class SoundRef
    {
        public AudioClip miss;
        public AudioClip evade;
        public AudioClip damage;
        public AudioClip CRIT;
        public AudioClip dying;
        public AudioClip heal;
        public AudioClip shield;
        public AudioClip SANHeal;
        public AudioClip SANDecrease;

    }

    public static ColorRef colorRef;
    public static SoundRef soundRef;    
    public static GameObject abilityManager_General;
    public static GameObject actionManager_General;
    [SerializeField]
    ColorRef colorRef_Inspector;
    [SerializeField]
    SoundRef soundRef_Inspector;
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
        soundRef = soundRef_Inspector;
        abilityManager_General = abilityManager_General_Inspector;
        actionManager_General=actionManager_General_Inspector;
    }
}
