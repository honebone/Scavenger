using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionEffect : MonoBehaviour
{
    [System.Serializable]
    public struct PositionEffectStatus
    {
        public string PEName;
        [TextArea(3, 10)]
        public string PEInfo;
        public Sprite PEIcon;
        public enum PositionEffectType { neutral, buff, debuff, focus }
        public PositionEffectType PEType;
        [Header("0ならスタック制限なし")]
        public int maxStack;
        [Tooltip("同種の状態異常がすでにある場合、そのスタックを増加させるか")]
        public bool merge;
        public bool refValue;

        [Header("以下は代入される")]
        public int stack;
        public int value;

        public string GetName()
        {
            string s = PEName;
            if (refValue) { s += value.ToString(); }
            return s.ColorStr(PEType.ToColor());
        }
        public string GetPEInfo_forRef()
        {
            string s = PEName;
            if (refValue) { s += "X"; }
            s += "：";
            s += PEInfo + "\n";
            //if (merge) { s += string.Format("新たに{0}が付与された時は統合される\n",StEName); }

            return s.ColorStr(Color.gray);
        }
    }
    protected PositionEffectStatus PEStatus;
    public PositionEffectStatus GetPositionEffectStatus() { return PEStatus; }

    [System.Serializable]
    public struct PositionEffectParams
    {
        public GameObject applyPE;
        public float applyChance;

        public int stack;
        public int value;
    }
    protected Character character;
    protected Character.CharacterStatus charaStatus;
    protected CharactersManager charactersManager;
    protected PositionManager positionManager;
    public void Init(Character c, PositionManager pm,PositionEffectParams PEParams)
    {
        character = c;
        positionManager = pm;

        PEStatus.stack = PEParams.stack;
        PEStatus.value = PEParams.value;
        //StEIcon = icon;
        //StEIcon.Init(StEStatus);
        if (PEStatus.merge && PEStatus.refValue) { FindObjectOfType<InfoText>().AddErrorText("mergeとrefValueが同時にtrueとなるPEは作ってはいけません!!"); }

        charaStatus = character.GetCharacterStatus();
        charactersManager = FindObjectOfType<CharactersManager>();
        OnPEInit();
    }
    public void Disable()
    {
        AtTheEnd();
        positionManager.RemovePE(this);
        Destroy(gameObject);
    }
    public virtual void OnPEInit() { }
    public virtual void AtTheEnd() { }

    public virtual void OnCharaEnter() { }
    public virtual void OnCharaLeave() { }

    public virtual void OnBattleStart() { }
    public virtual void OnRoundStart() { }
    public virtual void OnTurnStart() { }
    public virtual void OnTurnEnd() { }
    public virtual void OnRoundEnd() { }
    public virtual void OnBattleEnd() { }
}
