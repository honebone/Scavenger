using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PA_Personality : PassiveAbility
{
    [System.Serializable]
    public struct PersonalityStatus
    {
        public string personalityName;
        public enum PersonalityType { neutral, unique, awoken, good, bad, affricted, mutation }
        public PersonalityType personalityType;

        public string GetName()
        {
            string s = "";
            if (personalityType == PersonalityType.awoken) { s = string.Format("Åô{0}Åô", personalityName); }
            else if (personalityType == PersonalityType.affricted) { s = string.Format("Å~{0}Å~", personalityName); }
            else { s = personalityName; }
            return s.ColorStr(personalityType.ToColor());
        }
    }
    public override string GetPAName()
    {
        return personalityStatus.GetName();
    }
    [SerializeField]
    public PersonalityStatus personalityStatus;
    public PersonalityStatus GetPersonalityStatus() { return personalityStatus; }
    public PersonalityStatus.PersonalityType GetPerType() { return personalityStatus.personalityType; }
    public bool CheckPerType(PersonalityStatus.PersonalityType type) { return personalityStatus.personalityType == type; }
}
