using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utils_VE : MonoBehaviour
{
    public static Utils_VE inst;
    private void Awake()
    {
        if (inst == null) { inst = this; }
    }
    public void SpawnVE_MousePos(GameObject VE)
    {
        if(VE != null)
        {
            Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            pos.z = 0;
            Instantiate(VE, pos, Quaternion.identity);
        }
    }
    public void SpawnVE_UISmoke()
    {
        Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        pos.z = 0;
        Instantiate(Definer.VERef.UI_smoke, pos, Quaternion.identity);
    }

    public void SpawnVE(GameObject VE,Vector3 pos)
    {
        if (VE != null)
        {
            Instantiate(VE, pos, Quaternion.identity);
        }
    }
}
