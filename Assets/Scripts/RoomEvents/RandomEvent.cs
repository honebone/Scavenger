using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomEvent : MonoBehaviour
{
    protected ExpeditionManager expeditionManager;
    protected CharactersManager characterManager;
    protected InfoText infoText;
    public void Init()
    {
        expeditionManager = FindObjectOfType<ExpeditionManager>();
        characterManager = FindObjectOfType<CharactersManager>();
        infoText = FindObjectOfType<InfoText>();
    }
}
