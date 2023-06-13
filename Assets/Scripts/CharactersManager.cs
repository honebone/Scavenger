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
    Character_TargetButton[] targetButtons_size1;
    [SerializeField]
    Character_TargetButton[] targetButtons_size2;


    [SerializeField]
    GameObject characterObject;

    [SerializeField]
    /// <summary>干渉するならこっち</summary>
    List<Character> generatedCharacters;
    /// <summary>干渉するならこっち</summary>
    List<Character> existingCharacters;
    public void AddCharacter(Character character)
    {
        generatedCharacters.Add(character);
        existingCharacters.Add(character);
    }
    public List<Character> GetExistingCharacters() { return existingCharacters; }

    private void Start()
    {
        generatedCharacters=new List<Character>();

        existingCharacters = new List<Character>();
    }

    public void SpawnPlayer(CharacterData characterData,int pos)
    {
        if (pos >= 9) { print("プレイヤーを召喚するのに、指定した位置がエネミー側です!"); }
        Character.CharacterStatus generatedCharaStatus = new Character.CharacterStatus();

        generatedCharaStatus.Init(characterData, generatedCharacters.Count);
        generatedCharaStatus.positon = pos;

        Vector2 worldPos = new Vector2Int();
        Character_TargetButton tb=GetComponent<Character_TargetButton>();
        if (generatedCharaStatus.size == 1)
        {
            worldPos = charactersWorldPos_Size1[pos];
            tb = targetButtons_size1[pos];
            //existCharaに追加
        }
        else if (generatedCharaStatus.size == 2)
        {
            worldPos = charactersWorldPos_Size2[pos];
            tb = targetButtons_size2[pos];
            //existCharaに追加
        }
        else { print("サイズ3に対する処理が出来てません"); }

        var co = Instantiate(characterObject, worldPos, Quaternion.identity);
        co.GetComponent<Character_Object>().Init(generatedCharaStatus, characterData.manager,tb,this);
    }

    public void SpawnEnemy(CharacterData characterData, int pos)
    {
        if (pos < 9) { print("エネミーを召喚するのに、指定した位置がプレイヤー側です!"); }
        Character.CharacterStatus generatedCharaStatus = new Character.CharacterStatus();

        generatedCharaStatus.Init(characterData, generatedCharacters.Count);
        generatedCharaStatus.positon = pos;

        Vector2 worldPos = new Vector2Int();
        Character_TargetButton tb = GetComponent<Character_TargetButton>();
        if (generatedCharaStatus.size == 1)
        {
            worldPos = charactersWorldPos_Size1[pos];
            tb = targetButtons_size1[pos];
            //existCharaに追加
        }
        else if (generatedCharaStatus.size == 2)
        {
            worldPos = charactersWorldPos_Size2[pos];
            tb = targetButtons_size2[pos];
            //existCharaに追加
        }
        else { print("サイズ3に対する処理が出来てません"); }

        var co = Instantiate(characterObject, worldPos, Quaternion.identity);
        co.GetComponent<Character_Object>().Init(generatedCharaStatus, characterData.manager, tb, this);
    }

}
