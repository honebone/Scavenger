using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PA_Personality : PassiveAbility
{
    [System.Serializable]
    public struct PersonalityStatus
    {
        public string personalityName;
        public enum PersonalityType { neutral, unique, good, bad }
        public PersonalityType personalityType;
    }
    [SerializeField]
    protected PersonalityStatus personalityStatus;
}
