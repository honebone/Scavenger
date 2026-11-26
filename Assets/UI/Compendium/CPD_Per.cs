using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CPD_Per : CPDElement
{
    List<CPD_PerButton> perButtons = new List<CPD_PerButton>();
    List<PA_Personality> pers = new List<PA_Personality>();

    public override void Init()
    {
        var list = new List<PA_Personality>(Definer.inst.GetPersonalityDataBase().Select(o => o.GetComponent<PA_Personality>()));
        pers = list.OrderBy(x => x.GetPerType()).ToList();
        pers.ForEach(x =>
        {
            var b = Instantiate(buttonPrefab, buttonsP);
            int state = PlayerPrefs.GetFloat($"unlock_{x.fileName}", 0).ToInt();
            unlockStates.Add(state);
            perButtons.Add(b.GetComponent<CPD_PerButton>().Init(x, scroll, state != 0));
        });
    }

    public void UnlockPer(GameObject unlock)
    {
        PA_Personality pa = unlock.GetComponent<PA_Personality>();
        string fileName = pa.fileName;

        int index = -1;
        for (int i = 0; i < pers.Count; i++)
        {
            if( pers[i].fileName == pa.fileName)
            {
                if(index != -1)
                {
                    InfoText.inst.AddErrorText($"{pa.fileName}‚ðŠ¥‚·‚é“Á«‚ª•¡”‘¶Ý");
                    break;
                }
                index = i;
            }
        }

        if (index == -1) return;

        if (unlockStates[index] == 0)
        {
            GameManager.instance.SetSaveData($"unlock_{pa.fileName}", 1);
            unlockStates[index] = 1;
            perButtons[index].Unlock();
        }

    }
}
