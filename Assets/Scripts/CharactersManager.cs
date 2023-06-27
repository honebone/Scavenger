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
    Character_TargetButton[] targetButtons_size3;


    [SerializeField]
    GameObject characterObject;

    [SerializeField]//test
    List<Character> generatedCharacters;
    List<Character> existingCharacters;
    public void AddCharacter(Character character)
    {
        generatedCharacters.Add(character);
        existingCharacters.Add(character);
        existingCharacters.Sort((a, b) => a.GetCharacterStatus().position - b.GetCharacterStatus().position);
        //if (existingCharacters.Count == 0) { existingCharacters.Add(character); }
        //else
        //{
        //    for (int i = 0; i < existingCharacters.Count; i++)
        //    {
        //        if (existingCharacters[i].GetCharacterStatus().positon > character.GetCharacterStatus().positon)
        //        {
        //            existingCharacters.Insert(i, character);
        //            break;
        //        }
        //    }
        //}         
    }
    public List<Character> GetExistingCharacters_All() { return existingCharacters; }
    public List<Character> GetExistingCharacters(List<int> positions)
    {
        List<Character> characters = new List<Character>();
        foreach (int pos in positions) { characters.Add(GetCharacterWithPos(pos)); }
        return characters;
    }
    public Character GetExistingCharacter(int index) { return existingCharacters[index]; }
    public List<Character.CharacterStatus> GetExistingCharactersStatus()
    {
        List<Character.CharacterStatus> charactersStatus = new List<Character.CharacterStatus>();
        foreach (Character existingCharacter in existingCharacters) { charactersStatus.Add(existingCharacter.GetCharacterStatus()); }
        return charactersStatus;
    }
    public Character.CharacterStatus GetExistingCharacterStatus(int index)
    {
        return existingCharacters[index].GetCharacterStatus();
    }

    public bool CheckCharaExist(int checkPos)
    {
        foreach (Character.CharacterStatus characterStatus in GetExistingCharactersStatus())
        {
            switch (characterStatus.size)
            {
                case 1:
                    if (characterStatus.position == checkPos) { return true; }
                    break;
                case 2:
                    if (characterStatus.position == checkPos) { return true; }
                    if (characterStatus.position + 1 == checkPos) { return true; }
                    if (characterStatus.position + 3 == checkPos) { return true; }
                    if (characterStatus.position + 4 == checkPos) { return true; }
                    break;
                case 3:
                    if (characterStatus.position < 9 == checkPos < 9) { return true; }
                    break;
            }
        }
        return false;
    }
    public Character GetCharacterWithPos(int pos)
    {
        foreach (Character character in GetExistingCharacters_All())
        {
            Character.CharacterStatus characterStatus = character.GetCharacterStatus();
            switch (characterStatus.size)
            {
                case 1:
                    if (characterStatus.position == pos) { return character; }
                    break;
                case 2:
                    if (characterStatus.position == pos) { return character; }
                    if (characterStatus.position + 1 == pos) { return character; }
                    if (characterStatus.position + 3 == pos) { return character; }
                    if (characterStatus.position + 4 == pos) { return character; }
                    break;
                case 3:
                    if (characterStatus.position < 9 == pos < 9) { return character; }
                    break;
            }
        }
        return null;
    }
    public void ResetAllTargetIcons()
    {
        foreach(Character_TargetButton targetButton in targetButtons_size1) { targetButton.ResetTargetIcon(); }
        foreach (Character_TargetButton targetButton in targetButtons_size2) { if (targetButton != null) { targetButton.ResetTargetIcon(); }; }
        foreach (Character_TargetButton targetButton in targetButtons_size3) { if (targetButton != null) { targetButton.ResetTargetIcon(); }; }
    }
    public void ResetAllActionInvolvedIcons()
    {
        foreach (Character_TargetButton targetButton in targetButtons_size1) { targetButton.ResetActionInvolvedIcon(); }
        foreach (Character_TargetButton targetButton in targetButtons_size2) { if (targetButton != null) { targetButton.ResetActionInvolvedIcon(); }; }
        foreach (Character_TargetButton targetButton in targetButtons_size3) { if (targetButton != null) { targetButton.ResetActionInvolvedIcon(); }; }
    }
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
        generatedCharaStatus.position = pos;

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
        generatedCharaStatus.position = pos;

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
