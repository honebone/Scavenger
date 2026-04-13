using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaE_StatueOfGreatMage : RE_RandomEvents
{
    [SerializeField] Vector2Int expCount;
    [SerializeField] List<REOptionParams> options;

    public override void StartRandomEvent()
    {
        expeditionManager.SetREOptionButtons(options);
    }
    public override void SelectOption(int index)
    {
        StartCoroutine(GainExp());
    }

    IEnumerator GainExp()
    {
        int count = expCount.Range();
        float time=0.25f;

        yield return new WaitForSeconds(1f);

        for (int i=0; i < count; i++)
        {
            yield return new WaitForSeconds(time);
            time *= 1.3f;
            players.Choice().GainEXP(expeditionManager.GetExpAmount(1));
            SoundManager.instance.PlaySE(Definer.soundRef.expOrb);
        }
        EndRoomEvent();
    }
}
