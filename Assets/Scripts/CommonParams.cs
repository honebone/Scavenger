using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "commonParams")]
public class CommonParams : ScriptableObject
{
    public List<TextSpriteParams> textSpriteParamsList = new List<TextSpriteParams>();
    public List<GameObject> statusEffectDataBase;
    public Definer.ColorRef colorRef;
    public List<CharacterData> playerDataBase;
    public List<CharacterData> enemyDataBase;
    public List<CharacterData> obstacleDataBase;
    public List<CharacterData> summonDataBase;
}
