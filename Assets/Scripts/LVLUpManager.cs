using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class LVLUpManager : MonoBehaviour
{
    [SerializeField] ExpeditionManager expeditionManager;
    [SerializeField] GameObject panel;

    [SerializeField] List<LVLUp_AbilityButton> buttons;
    [SerializeField] List<Transform> panelsTF;
    //[SerializeField] 
    List<Character> LVLUpQueue = new List<Character>();

    bool inLVLUp;

    public struct LVLUpParams
    {
        public Character character;
        public bool upgrade;
        public Ability.AbilityStatus abilityStatus;
    }
    
    public void LVLUp(Character chara)
    {
        LVLUpQueue.Add(chara);
        if (!inLVLUp)
        {
            inLVLUp = true;
            StartelectUpgrade();
        }
    }

    void StartelectUpgrade()
    {
        panel.SetActive(true);

        List<LVLUpParams> pool = new List<LVLUpParams>();
        foreach(Ability.AbilityStatus status in LVLUpQueue[0].GetCharacterStatus().abilitiesStatus)
        {
            if(status.locked|| status.abilityData.upgradeAbility != null)
            {
                LVLUpParams lvlUpParams = new LVLUpParams();
                lvlUpParams.character = LVLUpQueue[0];
                if (status.locked)
                {
                    lvlUpParams.abilityStatus = status;
                }
                else if (status.abilityData.upgradeAbility != null)
                {
                    Ability.AbilityStatus upgrade = new Ability.AbilityStatus(status.abilityData.upgradeAbility, 0);
                    lvlUpParams.abilityStatus = upgrade;
                }
                lvlUpParams.upgrade = !status.locked;
                pool.Add(lvlUpParams);
            }
        }

        List<LVLUpParams> upgrades = pool.Sample(2);
        buttons[0].SetUpgrade(upgrades[0]);
        if (upgrades.Count == 2) { buttons[1].SetUpgrade(upgrades[1]); }

        StartCoroutine(SelectUpgradeC());
    }
    IEnumerator SelectUpgradeC()
    {
        for(int i = 0; i < 3; i++)
        {
            panelsTF[i].DORotate(new Vector3(0, -90, 0), 0.5f,RotateMode.WorldAxisAdd);
            yield return new WaitForSeconds(0.15f);
        }
    }
}
