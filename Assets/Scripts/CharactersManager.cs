using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

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

    InfoText infoText;
    Utility util;
    public void AddCharacter(Character character)
    {
        generatedCharacters.Add(character);
        existingCharacters.Add(character);
        SortExistingCharacters();
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
    public void SortExistingCharacters()
    {
        existingCharacters.Sort((a, b) => a.GetCharacterStatus().position - b.GetCharacterStatus().position);
    }
    public List<Character> GetExistingCharacters_All() { return existingCharacters; }
    public List<Character> GetExistingCharacters(List<int> positions,bool includeEmpty)
    {
        List<Character> characters = new List<Character>();
        foreach (int pos in positions) { if (CheckCharaExist(pos)||includeEmpty) { characters.Add(GetCharacterWithPos(pos)); } }
        return characters;
    }
    public Character GetExistingCharacter(int index) { return existingCharacters[index]; }
    public List<Character.CharacterStatus> GetExistingCharactersStatus()
    {
        List<Character.CharacterStatus> charactersStatus = new List<Character.CharacterStatus>();
        foreach (Character existingCharacter in existingCharacters) { charactersStatus.Add(existingCharacter.GetCharacterStatus()); }
        return charactersStatus;
    }
    public Character.CharacterStatus GetExistingCharacterStatus(int index) { return existingCharacters[index].GetCharacterStatus(); }


    /// <summary>移動処理に使用　進行方向にいるキャラを取得</summary>
    /// <param name="ranges">0:right 1:upper 2:lower 3:left</param>
    /// <returns></returns>
    public List<Character> GetTravelingDirCharas(int pos,int dir,int range)
    {
        HashSet<Character> characters = new HashSet<Character>();
        List<Character> c = new List<Character>();
        for(int i = 1; i <= range; i++)
        {
            c.Add(GetCharacterWithPos(util.GetMoveToPos(pos,dir,i)));
        }
        foreach(Character character in c)
        {
            characters.Add(character);
        }
        return characters.ToList();
    }
    

    public void RemoveExistingCharacter(Character chara)
    {
        if (existingCharacters.Contains(chara)) { existingCharacters.Remove(chara); }
        else { infoText.AddDebugText("error:今死亡したキャラは、そもそも存在していません"); }
    }

    public Character_TargetButton GetTargetButton(int size,int pos)
    {
        switch (size)
        {
            case 1:
                return targetButtons_size1[pos];
            case 2:
                if (targetButtons_size2[pos] == null) { infoText.AddDebugText(string.Format("error:{0}",pos)); }
                return targetButtons_size2[pos];
            case 3:
                if (targetButtons_size3[pos] == null) { infoText.AddDebugText(string.Format("error:{0}", pos)); }
                return targetButtons_size3[pos];
            default:
                infoText.AddDebugText(string.Format("error:{0}", size));
                return null;
        }
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="pos"></param>
    /// <param name="targetEmpty">空きスペースを対象とするか</param>
    /// <param name="checkOnlyCore">サイズ2以上のとき、左下のみチェック</param>
    /// <param name="size">空きスペースを対象とする場合、そのサイズ</param>
    /// <param name="targetGroup"></param>
    public void SetTargetIcon(int pos,bool targetEmpty,int size,List<int> targetGroup)
    {
        if (targetEmpty)
        {
            GetTargetButton(size, pos).SetTargetIcon(targetGroup);
        }
        else
        {
            if (CheckCharaExist(pos))
            {
                GetTargetButton(GetCharacterWithPos(pos).GetCharacterStatus().size, GetCharacterWithPos(pos).GetCharacterStatus().position).SetTargetIcon(targetGroup);
            }
        }
        
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="checkPos"></param>
    /// <param name="checkOnlyCore">サイズ2以上のとき、左下のみチェック</param>
    /// <returns></returns>
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
                    if (characterStatus.position + 3 == checkPos ) { return true; }
                    if (characterStatus.position + 4 == checkPos) { return true; }
                    break;
                case 3:
                    if (characterStatus.position < 9 == checkPos < 9 ) { return true; }
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
        infoText.AddDebugText(string.Format("error:ポジション{0}にキャラクターは存在していません", pos));
        return null;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="moveValue">0:right 1:upper 2:lower 3:left</param>
    /// <returns></returns>
    public List<int> GetMoveTargets(int pos,int size,List<int> moveValue)
    {
        int minPos = 0;
        int maxPos = 8;
        List<int> targets = new List<int>();

        switch (size)
        {
            case 1:
                if (pos >= 9)
                {
                    minPos = 9;
                    maxPos = 17;
                }
                if (moveValue[0] > 0)
                {
                    for (int i = 1; i <= moveValue[0]; i++)
                    {
                        if (pos + (3 * i) <= maxPos) { targets.Add(pos + (i * 3)); }
                        else { break; }
                    }
                }
                if (moveValue[1] > 0)
                {
                    for (int i = 1; i <= moveValue[1]; i++)
                    {
                        if (Mathf.FloorToInt(pos / 3f) == Mathf.FloorToInt((pos + i) / 3f)) { targets.Add(pos + i); }
                        else { break; }
                    }
                }
                if (moveValue[2] > 0)
                {
                    for (int i = 1; i <= moveValue[2]; i++)
                    {
                        if (Mathf.FloorToInt(pos / 3f) == Mathf.FloorToInt((pos - i) / 3f))
                        {
                            targets.Add(pos - i);
                        }
                        else { break; }
                    }
                }
                if (moveValue[3] > 0)
                {
                    for (int i = 1; i <= moveValue[3]; i++)
                    {
                        if (pos - (3 * i) >= minPos) { targets.Add(pos - (i * 3)); }
                        else { break; }
                    }
                }
                break;
            case 2:
                infoText.AddDebugText("size2の移動アビリティは実装しないはずでは");
                break;
            case 3:
                infoText.AddDebugText("size3のお前が動けるわけねえだろうが!!!");
                break;
            default:
                infoText.AddDebugText("error:sizeの値がおかしいです");
                break;
        }
       
        return targets;
    }
    public Vector2 GetCharacterWorldPos(int size, int pos)
    {
        Vector2 worldPos=new Vector2();
        switch (size)
        {
            case 1:
                worldPos = charactersWorldPos_Size1[pos];
                break; 
            case 2:
                worldPos = charactersWorldPos_Size2[pos];
                break;
            case 3:
                infoText.AddDebugText("size3の処理未実装");
                break;
            default:
                infoText.AddDebugText("error:sizeの値がおかしいです");
                break;
        }
        if (worldPos.x == -1) { infoText.AddDebugText(string.Format("存在しないworldPos:サイズ{0}の位置{1}", size, pos)); }
        return worldPos;
    }

    public bool CheckVictory()
    {
        foreach (Character chara in existingCharacters)
        {
            if(chara.GetCharacterStatus().position >= 9) { return false; }//敵側にキャラがいるなら勝利してない
        }
        return true;
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

        infoText = FindObjectOfType<InfoText>();
        util = FindObjectOfType<Utility>();
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
