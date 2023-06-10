using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    [System.Serializable]
    public struct CharacterStatus
    {
        public string fileName;
        public CharacterData.CharacterTag[] characterTags;
        public string charaName;
        public int size;

        public bool player;
        public bool playable;
        /// <summary>0:idle 1:damaged </summary>
        public GameObject[] variableSprites; 
        public Sprite spriteForUI;
        //public GameObject[] abilities;
        public List<GameObject> actionMods;

        //public EquipmentType[] equipableTypes;
        //[Header("equipableTypesと要素数を合わせる")]
        //public Equipment[] equipments;

        public int maxHP;
        public int maxHP_base;
        public float maxHP_mul;
        public int maxSAN;
        public int maxSAN_base;
        public float maxSAN_mul;

        public int STR;
        public int STR_base;
        public float STR_mul;
        public int DEX;
        public int DEX_base;
        public float DEX_mul;
        public int INT;
        public float INT_base;
        public float INT_mul;

        public float CRITC;
        public float CRITD;
        
        public float EVD;
        public float ACC;

        public int ACT;
        public int turnPerRound;

        public float GHeal;
        public float RHeal;

        //public DropItem[] dropItems;
        public string leftBehind;//死亡時に変身するキャラクター名

        public float stunRes;
        public float bleedRes;
        public float poisonRes;
        public float burnRes;


        public int instanceID;
        public int positon;

        /// <summary>自身をかばっているキャラのinstanceID</summary>
        public int protectedBy;

        public int HP;
        public int shield;

        public int SAN;

        public int exATK;

        //以下バフ
        public int hide;

        //以下デバフ
        public int focused;
        public int stun;
        public int bleed;//被ダメージ時この値分HP減少
        public int poison;//行動時この値分HP減少
        public int burn;//ターン終了時にこの値分HPが減少

        public bool dead;
        //ここに状態異常入れれるといいね 

        public void Init(CharacterData data,int ID)
        {
            fileName = data.fileName;
            characterTags = data.characterTags;
            charaName = data.charaName;
            size = data.size;

            player = data.player;
            playable = data.playable;
            variableSprites = data.variableSprites;
            spriteForUI = data.spriteForUI;

            actionMods = data.actionMods;

            maxHP_base = data.maxHP;
            maxHP_mul = 100f;
            maxHP = data.maxHP;
            maxSAN_base = data.maxSAN;
            maxSAN_mul = 100f;
            maxSAN = data.maxSAN;

            STR_base = data.STR;
            STR_mul = 100f;
            STR = data.STR;
            DEX_base = data.DEX;
            DEX_mul = 100f;
            DEX = data.DEX;
            INT_base = data.INT;
            INT_mul = 100f;
            INT = data.INT;

            CRITC = data.CRITC;
            CRITD = data.CRITD;

            EVD = data.EVD;
            ACC = data.ACC;

            ACT = data.ACT;
            turnPerRound = data.turnPerRound;

            GHeal = data.GHeal;
            RHeal = data.RHeal;

            leftBehind = data.leftBehind;

            stunRes = data.stunRes;
            bleedRes = data.bleedRes;
            poisonRes = data.poisonRes;
            burnRes = data.burnRes;

            instanceID = ID;
        }
        public Vector2Int posIntToVector() { return new Vector2Int(positon % 3, Mathf.FloorToInt(positon / 3)); }
    }
    [SerializeField]
    CharacterStatus charaStatus;
    public CharacterStatus GetCharacterStatus() { return charaStatus; }

    Character_Object charaObj;

    public void Init(CharacterStatus status,Character_Object obj)
    {
        charaStatus = status;
        charaObj = obj;

        charaStatus.HP = charaStatus.maxHP;
        charaStatus.SAN = charaStatus.maxSAN;

        charaObj.SetCharaSprite(charaStatus.variableSprites[0]);
        if (!charaStatus.player) { charaObj.DisableSANBar(); }
        charaObj.SetHPandShieldBar(charaStatus);
        charaObj.SetSANBar(charaStatus);
        //TurnIconはラウンド開始時にセット
    }
    //ActionQueue actionQueue;
    //BattleManager battleManager;
    //private void Start()
    //{
    //    actionQueue = FindObjectOfType<ActionQueue>();
    //    battleManager = FindObjectOfType<BattleManager>();
    //}
    //public void Enqueue(GameObject action) { actionQueue.Enqueue(action); }

    //public void MyTurnStart()
    //{
    //    OnTurnStart();
    //    actionQueue.StartResolve(2);
    //}
    //public virtual void MainPhase()
    //{
    //    //行動可能か～
    //    OnActivateAbility();
    //    actionQueue.StartResolve(3);
    //}
    //public void EndPhase()
    //{
    //    OnTurnEnd();
    //    //Resolve開始
    //    EndMyTurn();
    //}
    //public void EndMyTurn()
    //{
    //    battleManager.TurnEnd();
    //}

    public virtual void OnBattleStart() { }
    public virtual void OnRoundStart() { }
    public virtual void OnTurnStart() { }
    public virtual void OnTurnEnd() { }
    public virtual void OnRoundEnd() { }
    public virtual void OnBattleEnd() { }


    public virtual void OnActivateAbility() { }
    /// <summary>攻撃命中時</summary>
    public virtual void OnDamage(int DMG) { }
    public virtual void OnCRIT() { }
    public virtual void OnMiss() { }
}

