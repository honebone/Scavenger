using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "commonParams")]
public class CommonParams : ScriptableObject
{
    public List<TextSpriteParams> textSpriteParamsList = new List<TextSpriteParams>();
    public List<GameObject> statusEffectDataBase;
    public Definer.ColorRef colorRef;
}
