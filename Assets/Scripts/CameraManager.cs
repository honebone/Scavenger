using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CameraManager : MonoBehaviour
{


    public void ShakeCamera(float strength)
    {
        transform.DOShakePosition((1 + strength) * strength / 4f, strength, 30, 1);
        //FindObjectOfType<InfoText>().AddDebugText(string.Format("str:{0} dur:{1}", strength, (1 + strength) * strength / 4f));
    }
}
