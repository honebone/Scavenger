using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PA_Personality : PassiveAbility
{
    [System.Serializable]
    public struct PersonalityStatus
    {
        public string personalityName;
        public enum PersonalityType { neutral, unique, soGood, good, bad, soBad }
        public PersonalityType personalityType;

        public string GetName()
        {
            string s = "";
            if (personalityType == PersonalityType.soGood) { s = string.Format("Åô{0}Åô", personalityName); }
            else if (personalityType == PersonalityType.soBad) { s = string.Format("Å~{0}Å~", personalityName); }
            else { s = personalityName; }
            return s.ColorStr(personalityType.ToColor());
        }
    }
    public override string GetPAName()
    {
        return personalityStatus.GetName();
    }
    [SerializeField]
    protected PersonalityStatus personalityStatus;
}
