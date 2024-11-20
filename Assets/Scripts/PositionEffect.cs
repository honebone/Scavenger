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

        public bool DoT;

        [Header("以下は代入される")]
        public int stack;
        public int value;

        public int DMGPerTurn;
    }
    [SerializeField]
    protected PositionEffectStatus PEStatus;
    public PositionEffectStatus GetPositionEffectStatus() { return PEStatus; }

    [System.Serializable]
    public struct PositionEffectParams
    {
        public GameObject applyPE;
        public bool guaranteed;
        public float applyChance;

        public int stack;
        public int value;

        [Header("DoT")]
        public bool refATK;
        [Header("以下は代入される")]
        public int DMGPerTurn;
    }
    protected Character character;
    //protected Character.CharacterStatus charaStatus;
    protected CharactersManager charactersManager;
    protected PositionManager positionManager;

    bool destroyed;
    PEIcon PEIcon;
    public void Init(Character c, PositionManager pm,PositionEffectParams PEParams,Character.CharacterStatus ownerStatus,PEIcon icon)
    {
        character = c;
        positionManager = pm;

        PEStatus.stack = PEParams.stack;
        PEStatus.value = PEParams.value;
        if (PEStatus.DoT)
        {
            int baseDMG = (PEParams.refATK) ? ownerStatus.ATK : ownerStatus.INT;
            PEStatus.DMGPerTurn = (baseDMG * PEParams.value / 100f).ToInt();
        }
        
        PEIcon = icon;
        PEIcon.Init(PEStatus);
        if (PEStatus.merge && PEStatus.refValue) { FindObjectOfType<InfoText>().AddErrorText("mergeとrefValueが同時にtrueとなるPEは作ってはいけません!!"); }

        //charaStatus = character.GetCharacterStatus();
        charactersManager = FindObjectOfType<CharactersManager>();
        OnPEInit();
    }

    public virtual string GetPEName(bool forRef)
    {
        string s = PEStatus.PEName;
        if (PEStatus.refValue)
        {
            if (forRef) { s += "X"; }
            else { s += PEStatus.stack.ToString(); }
        }
        return s;
    }
    public string GetPEInfo(bool forRef)
    {       
        string info = string.Format("{0}：\n",GetPEName(forRef));
        if (PEStatus.PEInfo != "") { info += PEStatus.PEInfo + "\n"; }
        info += GetAdditionalInfo();
        if (PEStatus.DoT) { info += $"\n減少HP：{PEStatus.DMGPerTurn}/ターン\n".ColorStr(Definer.colorRef.decreaseHP); }
        return info;
    }
    public virtual string GetAdditionalInfo()
    {
        return "";
    }

    public void SetCharacter(Character c)
    {
        if (character != c)
        {
            if(character != null)
            {
                OnCharaLeave();
            }
            character = c;
            if(character != null)
            {
                OnCharaEnter();
            }
        }
    }

    public void AddStack(int stack)
    {
        if (PEStatus.maxStack == 0)//最大スタック数が無制限なら
        {
            PEStatus.stack += stack;
        }
        else { PEStatus.stack = Mathf.Clamp(PEStatus.stack + stack, 0, PEStatus.maxStack); }

        if (!destroyed)
        {
            if (PEStatus.stack <= 0)
            {
                PEStatus.stack = 0;
                Destroy(PEIcon.gameObject);
                Disable();
            }
            else { PEIcon.SetStackText(PEStatus.stack); }
        }      
    }


    public void Disable()
    {
        destroyed = true;
        AtTheEnd();
        positionManager.DisablePE(this);
        if (PEIcon != null) { Destroy(PEIcon.gameObject); }
        Destroy(gameObject);
    }

    public void Enqueue(Action.ActionStatus actionStatus, bool setTargets, List<Character> actionTargets,int targetCount=0, bool nullOwner = false)
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            actionStatus.sprite = sr.sprite;
        }
        else
        {
            //infoText.AddDebugText(string.Format("{0}にSpriteRendererなし", GetPAName()));
        }
        character.Enqueue(actionStatus, setTargets, actionTargets, targetCount, nullOwner);
    }


    public virtual void OnPEInit() { }
    public virtual void AtTheEnd() { }

    public virtual void OnCharaEnter() { }
    public virtual void OnCharaLeave() { }

    public virtual void OnBattleStart() { }
    public virtual void OnRoundStart() { }
   public virtual void OnTurnOrderDecide() { }
    public virtual void OnTurnStart(Character currentTurnChara, int turnCount) { }
    public virtual void OnTurnEnd() { }
    public virtual void OnRoundEnd() { }
    public virtual void OnBattleEnd() { Disable(); }
}
