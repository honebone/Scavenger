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
        public Ability.AbilityStatus[] abilitiesStatus;
        public List<GameObject> actionMods;

        //public EquipmentType[] equipableTypes;
        //[Header("equipableTypesと要素数を合わせる")]
        //public Equipment[] equipments;

        public bool surviveFatalWounds;
        public int maxHP;
        public int maxHP_base;
        public float maxHP_mul;
        public int maxSAN;
        public int maxSAN_base;
        public float maxSAN_mul;

        public int ATK;
        public int ATK_base;
        public float ATK_mul;

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

        public float moveRes;
        public float debuffRes;


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

        public string GetInfo()
        {
            string s = "";
            if (player && !playable) { s += "操作不可\n"; }
            //種族
            s += string.Format("HP/maxHP：{0}/{1}\n", HP, maxHP);
            if (shield > 0) { s += string.Format("シールド：{0}\n", shield); }
            if (player) { s += string.Format("SAN/maxSAN：{0}/{1}\n\n", SAN, maxSAN); }
            else { s += "\n"; }

            s += string.Format("ATK：{0}\n", ATK);
            s += string.Format("CRIT：{0}％で{1}倍ダメージ\n\n", CRITC, CRITD);

            s += string.Format("EVD：{0}\n", EVD);
            s += string.Format("ACC：{0}\n\n", ACC);

            s += string.Format("ACT：{0}\n", ACT);
            s += string.Format("ラウンド毎ターン数：{0}\n\n", turnPerRound);

            if (GHeal != 100) { s += string.Format("与える回復量：{0}％\n", GHeal); }
            if (RHeal != 100) { s += string.Format("受ける回復量：{0}％\n", RHeal); }
            return s;
        }

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

            abilitiesStatus = new Ability.AbilityStatus[data.abilities.Length];
            for (int i = 0; i < abilitiesStatus.Length; i++) { abilitiesStatus[i].Init(data.abilities[i]); }

            actionMods = data.actionMods;

            surviveFatalWounds = data.surviveFatalWounds;
            maxHP_base = data.maxHP;
            maxHP_mul = 100f;
            maxHP = data.maxHP;
            maxSAN_base = data.maxSAN;
            maxSAN_mul = 100f;
            maxSAN = data.maxSAN;

            ATK_base = data.ATK;
            ATK_mul = 100f;
            ATK = data.ATK;

            CRITC = data.CRITC;
            CRITD = data.CRITD;

            EVD = data.EVD;
            ACC = data.ACC;

            ACT = data.ACT;
            turnPerRound = data.turnPerRound;

            GHeal = data.GHeal;
            RHeal = data.RHeal;

            leftBehind = data.leftBehind;

            debuffRes = data.debuffRes;

            stunRes = data.stunRes;
            bleedRes = data.bleedRes;
            poisonRes = data.poisonRes;
            burnRes = data.burnRes;

            moveRes = data.moveRes;
           

            instanceID = ID;
        }
        public Vector2Int posIntToVector() { return new Vector2Int(positon % 3, Mathf.FloorToInt(positon / 3)); }
    }
    [SerializeField]
    CharacterStatus charaStatus;
    public CharacterStatus GetCharacterStatus() { return charaStatus; }

    Character_Object charaObj;
    Character_TargetButton targetButton;
    public Character_Object GetCharacter_Object() { return charaObj; }

    ActionQueueManager actionQueue;
    BattleManager battleManager;
    Utility util;
    InfoText infoText;

    public void Init(CharacterStatus status,Character_Object obj,Character_TargetButton tb)
    {
        charaStatus = status;
        charaObj = obj;
        targetButton = tb;

        charaStatus.HP = charaStatus.maxHP;
        charaStatus.SAN = charaStatus.maxSAN;

        charaObj.SetCharaSprite(charaStatus.variableSprites[0]);
        if (!charaStatus.player) { charaObj.DisableSANBar(); }
        charaObj.SetHPandShieldBar();
        charaObj.SetSANBar();

        targetButton.SetCharacter(this);

        actionQueue = FindObjectOfType<ActionQueueManager>();
        battleManager = FindObjectOfType<BattleManager>();
        util = FindObjectOfType<Utility>();
        infoText = FindObjectOfType<InfoText>();

        //TurnIconはラウンド開始時にセット
    }

    public void DisplayInfo()
    {
        infoText.SetCharaInfo(charaStatus.charaName, charaStatus.GetInfo(), this);
        FindObjectOfType<AbilityButtonPanel>().SetAbilityButtons(charaStatus.abilitiesStatus,this);
        charaObj.SetSelectedIcon(true);
    }
    public void Enqueue(Action.ActionStatus actionStatus) { actionQueue.Enqueue(actionStatus); }

    public void SetTurnIcon() { charaObj.SetTurnIcons(charaStatus.turnPerRound); }
    public void SetTargetIcon(List<int> tg) { targetButton.SetTargetIcon(tg); }

    public void MyTurnStart()
    {
        charaObj.SetTurnIcon_CurentTurn();
        OnTurnStart();
        actionQueue.StartResolve(2);
    }
    public virtual void MainPhase()
    {
        //行動可能か～
        OnActivateAbility();
        if (charaStatus.playable) { 
            DisplayInfo();
            battleManager.SetSelectingAbility(true);
        }
        else { StartCoroutine(Test()); }   
    }
    IEnumerator Test()
    {
        print(charaStatus.charaName + "のターン");
        yield return new WaitForSeconds(0.5f);
        actionQueue.StartResolve(3);
    }
    public void EndPhase()
    {
        OnTurnEnd();
        charaObj.SetTurnIcon_End();
        //Resolve開始
        EndMyTurn();
    }
    public void EndMyTurn()
    {
        battleManager.TurnEnd();
    }



    public void DecreaseHP(int value)
    {
        charaStatus.HP -= value;
        charaObj.SetHPandShieldBar();
        charaObj.SetDamageText(value.ToString(), Definer.colorRef.decreaseHP);
        infoText.AddLogText(string.Format("{0}はHPを{1}失った", charaStatus.charaName, util.GetColoredText(Definer.colorRef.decreaseHP, value.ToString())));
        if (charaStatus.HP <= 0)
        {
            if (charaStatus.surviveFatalWounds)//瀕死で耐えるキャラは、HP減少によって死なない
            {
                charaStatus.HP = 0;
                charaObj.SetDamageText("瀕死!", Definer.colorRef.damage);
                infoText.AddLogText(string.Format("{0}は{1}だ...", charaStatus.charaName, util.GetColoredText(Definer.colorRef.damage, "瀕死")));
                charaObj.SetHPandShieldBar();
            }
            else
            {
                Die(0);
            }
        }
    }
    public void Damage(int DMG,bool CRIT,bool canCounter,Character attacker)
    {
        charaStatus.shield = 0;//シールドを0に

        if (CRIT)//テキストの表示
        {
            charaObj.SetDamageText("Critical!!", Definer.colorRef.CRIT);
            charaObj.SetDamageText(DMG.ToString(), Definer.colorRef.CRIT);
            infoText.AddLogText(string.Format("{0}\n{1}は{2}ダメージを受けた", util.GetColoredText(Definer.colorRef.CRIT, "Critical!!"), charaStatus.charaName, util.GetColoredText(Definer.colorRef.CRIT, DMG.ToString())));
        }
        else { 
            charaObj.SetDamageText(DMG.ToString(), Definer.colorRef.damage);
            infoText.AddLogText(string.Format("{0}は{1}ダメージを受けた",  charaStatus.charaName, util.GetColoredText(Definer.colorRef.damage, DMG.ToString())));
        }

        if (charaStatus.HP == 0)//瀕死の状態で1以上のダメージを受けたら死亡する
        {
            if (DMG > 0)
            {
                if (charaStatus.surviveFatalWounds)
                {
                    Die(0);
                }
                else { print("瀕死で耐えるキャラ出ないのにHP0で生き続けています"); }
            }
            else//0ダメージの時
            {
                charaStatus.HP = 0;
                charaObj.SetDamageText("瀕死!", Definer.colorRef.damage);
                infoText.AddLogText(string.Format("{0}は{1}だ...", charaStatus.charaName, util.GetColoredText(Definer.colorRef.damage, "瀕死")));
            }
        }
        else//瀕死でないなら
        {
            charaStatus.HP -= DMG;
            if (charaStatus.HP <= 0)
            {
                if (charaStatus.surviveFatalWounds)//瀕死で耐えるキャラは、瀕死でない状態で致命傷を受けても死なない
                {
                    charaStatus.HP = 0;
                    charaObj.SetDamageText("瀕死!", Definer.colorRef.damage);
                    infoText.AddLogText(string.Format("{0}は{1}だ...", charaStatus.charaName, util.GetColoredText(Definer.colorRef.damage, "瀕死")));
                }
                else
                {
                    Die(0);
                }
            }
        }

        if (!charaStatus.dead)//HPバーに反映
        {
            charaObj.SetHPandShieldBar();
        }
    }
    public void Heal(int value,Character healer)
    {
        charaStatus.HP = Mathf.Min(charaStatus.HP + value, charaStatus.maxHP);
        charaObj.SetDamageText(value.ToString(), Definer.colorRef.heal);
        infoText.AddLogText(string.Format("{0}はHPを{1}回復した", charaStatus.charaName, util.GetColoredText(Definer.colorRef.heal, value.ToString())));
        charaObj.SetHPandShieldBar();
    }
    /// <summary>0:HP0 1:SAN0</summary>
    void Die(int cause)
    {
        charaStatus.dead = true;
        if (cause == 0) { print("死亡"); }
        else if (cause == 1) { print("発狂"); }
    }

    public virtual void OnBattleStart() { }
    public virtual void OnRoundStart() { }
    public virtual void OnTurnStart() { }
    public virtual void OnTurnEnd() { }
    public virtual void OnRoundEnd() { }
    public virtual void OnBattleEnd() { }


    public virtual void OnActivateAbility() { }
    /// <summary>攻撃命中時</summary>
    public virtual void OnDamage(int DMG, int ID) { }
    public virtual void OnCRIT(int ID) { }
    public virtual void OnKill(int ID) { }
    public virtual void OnMiss(int ID) { }
    public virtual void OnHeal(int healValue, int ID) { }
    //public virtual void OnApplyStE() { }
    //public virtual void OnRemoveStE() { }

    public virtual void OnDamaged(int DMG, int ID) { }
    public virtual void OnCRITed(int ID) { }
    public virtual void OnEvade( int ID) { }
    public virtual void OnHealed(int healedValue, int ID) { }
    //public virtual void OnApplyedStE() { }
    //public virtual void OnRemoveedStE() { }
}

