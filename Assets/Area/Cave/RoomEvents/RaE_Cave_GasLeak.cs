using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaE_Cave_GasLeak : RE_RandomEvents
{
    [SerializeField]
    List<RoomEvent.REOptionParams> options;
    public override void StartRandomEvent()
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
        switch (choice)
        {
            case 0:
                infoText.AddLogText("物陰から松明を投げ、ガス燃え尽きるのを待った");
                infoText.SwitchToLog();
                for(int i = 0; i < 3; i++)
                {
                    yield return new WaitForSeconds(0.5f);
                    infoText.AddLogText("");
                }
                if (30.Dice())
                {
                    infoText.AddLogText("ガスが激しく爆発し、爆風がこちらまで飛んできた!!"); 
                    infoText.SwitchToLog();
                    foreach (Character chara in characterManager.GetExistingCharacters_All())
                    {
                        int maxHP = chara.GetCharacterStatus().maxHP;
                        float value = Random.Range(0.2f, 0.4f);
                        chara.DecreaseHP(Mathf.RoundToInt(maxHP * value));
                    }
                }
                else
                {
                    infoText.AddLogText("轟音とともにガスは消え失せ、再びガスが満ちる前に先に進んだ");
                    infoText.SwitchToLog();
                }
                break;
            case 1:
                infoText.AddLogText("ガスに引火しないように松明を消して進んだが、少しガスを吸ってしまった");
                infoText.SwitchToLog();
                yield return new WaitForSeconds(1.5f);
                foreach (Character chara in characterManager.GetExistingCharacters_All())
                {
                    int maxHP = chara.GetCharacterStatus().maxHP;
                    float value = Random.Range(0.1f, 0.2f);
                    chara.DecreaseHP(Mathf.RoundToInt(maxHP * value));
                }
                break;
            case 2:
                infoText.AddLogText("松明を消し、何も見えない闇の中を進んだ");
                infoText.SwitchToLog();
                yield return new WaitForSeconds(1.5f);
                foreach (Character chara in characterManager.GetExistingCharacters_All())
                {
                    chara.SANDamage(Random.Range(5, 20));
                }
                break;
        }
        //yield return new WaitForSeconds(1.0f);
        EndRoomEvent();
    }
}
