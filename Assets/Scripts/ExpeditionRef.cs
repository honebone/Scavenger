using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpeditionRef : MonoBehaviour
{
   public static ActionQueueManager actionQueue;
    public static BattleManager battleManager;
    public static  InfoText infoText;
    public static CharactersManager charactersManager;
    public static SoundManager soundManager;
    public static LootPanel loot;
    public static Definer definer;
    public static CameraManager cameraManager;
    public static ExpeditionManager expeditionManager;
    public static TutorialManager tutorialManager;

    void Start()
    {
        actionQueue = FindObjectOfType<ActionQueueManager>();
        battleManager = FindObjectOfType<BattleManager>();
        infoText = FindObjectOfType<InfoText>();
        charactersManager = FindObjectOfType<CharactersManager>();
        soundManager = FindObjectOfType<SoundManager>();
        loot = FindObjectOfType<LootPanel>();
        definer = FindObjectOfType<Definer>();
        cameraManager = FindObjectOfType<CameraManager>();
        expeditionManager = FindObjectOfType<ExpeditionManager>();
        tutorialManager = FindObjectOfType<TutorialManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
