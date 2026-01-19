using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "commonParams")]
public class CommonParams : ScriptableObject
{
    public List<TextSpriteParams> textSpriteParamsList = new List<TextSpriteParams>();
    public List<GameObject> statusEffectDataBase;
    public Definer.ColorRef colorRef;
    public Definer.SoundRef soundRef;
    public List<Sprite> perIcons;
    public List<CharacterData> playerDataBase;
    public List<CharacterData> enemyDataBase;
    public List<CharacterData> obstacleDataBase;
    public List<CharacterData> summonDataBase;
    public RoomRef roomRef;

    public Color StEColor( PA_StatusEffect.StatusEffectStatus status)
    {
        return status.overideColor ? status.colorOveride : colorRef.statusEffectColors[(int)status.StEType];
    }
}

[System.Serializable]
public class RoomRef
{
    public RoomEventData rest;
}