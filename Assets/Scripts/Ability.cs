using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability : MonoBehaviour
{
    [System.Serializable]
    public struct AbilityStatus
    {
        public string abilityName;

        public bool dontChangeSprite;
        public GameObject activateSprite;
        public int spriteIndex;

        public AbilityData.AbilityType abilityType;
        public int cooldownOnUse;
        public bool hasRemain;
        public int remainOnBattleStart;
        public int maxRemain;

        public bool availableFront;
        public bool availableMid;
        public bool availableBack;

        public Action.ActionStatus[] actionsStatus;

        public int cooldown;
        public int remain;


        public void Init(AbilityData data)
        {
            abilityName = data.abilityName;

            dontChangeSprite = data.dontChangeSprite;
            activateSprite = data.activateSprite;
            spriteIndex = data.spriteIndex;

            abilityType = data.abilityType;
            cooldownOnUse = data.cooldownOnUse;
            hasRemain = data.hasRemain;
            remainOnBattleStart = data.remainOnBattleStart;
            maxRemain = data.maxRemain;

            availableFront = data.availableFront;
            availableMid = data.availableMid;
            availableBack = data.availableBack;

            actionsStatus = new Action.ActionStatus[data.actions.Length];
            for (int i = 0; i < actionsStatus.Length; i++)
            {
                actionsStatus[i] = new Action.ActionStatus();
                actionsStatus[i].Init(data.actions[i]);
            }

        }
    }
}
