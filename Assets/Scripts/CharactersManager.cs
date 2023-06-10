using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharactersManager : MonoBehaviour
{
    [SerializeField]
    Vector2Int[] charactersWorldPos_Size1;
    [SerializeField]
    Vector2[] charactersWorldPos_Size2;

    [SerializeField]
    GameObject characterObject;

    /// <summary>干渉するならこっち</summary>
    List<Character> generatedCharacters;
    /// <summary>読み込むだけならこっち</summary>
    List<Character.CharacterStatus> generatedCharactersStatus;
    private void Start()
    {
        generatedCharacters=new List<Character>();
        generatedCharactersStatus=new List<Character.CharacterStatus>();
    }

    public void GeneratePlayer(CharacterData characterData,int pos)
    {
        Character.CharacterStatus generatedCharaStatus = new Character.CharacterStatus();

        generatedCharaStatus.Init(characterData, generatedCharactersStatus.Count);
        generatedCharaStatus.positon = pos;

        generatedCharactersStatus.Add(generatedCharaStatus);
        generatedCharacters.Add(characterData.manager.GetComponent<Character>());

        Vector2 worldPos = new Vector2Int();
        if (generatedCharaStatus.size == 1)
        {
            worldPos = charactersWorldPos_Size1[pos];
            //existCharaに追加
        }
        else { print("サイズ2,3に対する処理が出来てません"); }

        var co = Instantiate(characterObject, worldPos, Quaternion.identity);
        co.GetComponent<Character_Object>().Init(generatedCharaStatus, characterData.manager);
    }

}
