using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CameraManager : MonoBehaviour
{

    bool shaking;
    public void ShakeCamera(float strength)
    {
        if (!shaking)
        {
            shaking = true;
            float duration = (1 + strength) * strength / 4f;
            transform.DOShakePosition(duration, strength, 30, 1);
            StartCoroutine(Shaking(duration));
        }
    }

    IEnumerator Shaking(float duration)
    {
        yield return new WaitForSeconds(duration);
        shaking = false;
    }
}
