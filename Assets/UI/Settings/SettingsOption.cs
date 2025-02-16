using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsOption : MonoBehaviour
{
    protected string key;
    public delegate void Del(float value,string key);
    protected Del valueChanged;

    public virtual void Init(string k, Del del)
    {
        key = k;
    }

    public virtual float GetValue() { return -99; }
}
