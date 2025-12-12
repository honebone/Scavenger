using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class RE_LastBoss : RoomEvent
{
    [SerializeField]
    List<RoomEvent.REOptionParams> options;
    [SerializeField]
    AreaManager.EnemySet boss;
    [SerializeField] ExpeditionManager.BattleParams battleParams;

    public AreaManager.EnemySet lastBoss;
    public BackgroundParams lastBack;
    public AudioClip lastBGM;
    public GameObject lastParticle;
    public List<AudioClip> SEOnSpawn;

    public float blackoutDelay;
    public float blackoutDur;
    public float spawnDelay;
    public float spawnDur;

    [Header("\nƒ‰ƒXƒ{ƒXŒ‚”jŒã")]
    public float fadeOutDur;
    //public float battleDelay;

    protected int choice = 0;
    //GameObject eventManager;

    bool last;

    public override void OnEndREInfo()
    {
        expeditionManager.SetREOptionButtons(options);
    }
    public override void SelectOption(int index)
    {
        choice = index;
        StartCoroutine(Consequence());
    }
    IEnumerator Consequence()
    {
        yield return new WaitForSeconds(1.5f);
        lootPanel.AddExp(5);
        expeditionManager.Battle(new List<AreaManager.EnemySet> { boss }, null, battleParams);
    }

    public override void OnEndBattle()
    {
        if (!last)
        {
            last = true;
            StartCoroutine(LastBattleDelay());
        }
        else
        {
            //supplyManager.AddSupply_Eq(partyStatus.supplyOptions, ItemData.Rarity.epic);
            //lootPanel.Loot();
            StartCoroutine(GameClearC());
        }
    }

    public override void OnEnterEndless()
    {
        supplyManager.AddSupply_Eq(partyStatus.supplyOptions, ItemData.Rarity.epic);
        lootPanel.Loot();
    }

    IEnumerator LastBattleDelay()
    {
        yield return new WaitForSeconds(blackoutDelay);
        var wait = new WaitForSeconds (blackoutDur/20f);
        float alpha = 0;
        for(int i = 0; i < 20; i++)
        {
            alpha += 0.05f;
            expeditionManager.blackBack.color=new Color(0,0,0,alpha);
            yield return wait;
        }

        yield return new WaitForSeconds(spawnDelay);
        expeditionManager.SetBackground(lastBack);
        BattleManager.inst.SetWave(new List<AreaManager.EnemySet> { lastBoss });
        Instantiate(lastParticle);
        SEOnSpawn.ForEach(x => SoundManager.instance.PlaySE(x));
        SoundManager.instance.StartBGM_Battle(lastBGM);

        yield return new WaitForSeconds(spawnDur);
        BattleManager.inst.BattleStart_WithoutSetEnemy(null, battleParams);
        alpha = 1;
        for (int i = 0; i < 20; i++)
        {
            alpha -= 0.05f;
            expeditionManager.blackBack.color = new Color(0, 0, 0, alpha);
            yield return wait;
        }
    }

    IEnumerator GameClearC()
    {
        FadeOutUI.inst.FadeOut();
        yield return new WaitForSeconds(fadeOutDur);
        GameResultManager.inst.SetResult(1);
    }
}
