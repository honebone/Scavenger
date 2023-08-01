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
        public bool immovable;

        public bool player;
        public bool playable;
        /// <summary>0:idle 1:damaged </summary>
        public GameObject[] variableSprites; 
        public Sprite spriteForUI;
        public Ability.AbilityStatus[] abilitiesStatus;

        public List<GameObject> passiveAbilities;
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
        public int position;

        /// <summary>自身をかばっているキャラのinstanceID</summary>
        public int protectedBy;

        public bool omenSet;
        public Ability.AbilityStatus omen;

        public int HP;
        public int shield;

        public int SAN;

        public int exATK;

        //以下バフ
        public int hide;

        //以下デバフ
        public int marked;
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
            if (immovable || size >= 2) { s += "移動不可"; }
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

            if (omenSet) {s += string.Format("<{0}>を準備中\n", omen.abilityName.ColorStr(omen.abilityType.ToColor())); }
            return s;
        }

        public void Init(CharacterData data,int ID)
        {
            fileName = data.fileName;
            characterTags = data.characterTags;
            charaName = data.charaName;
            size = data.size;
            immovable = data.immovable;

            player = data.player;
            playable = data.playable;
            variableSprites = data.variableSprites;
            spriteForUI = data.spriteForUI;

            abilitiesStatus = new Ability.AbilityStatus[data.abilities.Length];
            for (int i = 0; i < abilitiesStatus.Length; i++) { abilitiesStatus[i].Init(data.abilities[i]); }
            passiveAbilities = new List<GameObject>(data.passiveAbilities);

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
        public Vector2Int posIntToVector() { return new Vector2Int(position % 3, Mathf.FloorToInt(position / 3)); }
    }
    [SerializeField]
    CharacterStatus charaStatus;

    [SerializeField]
    protected Action.ActionStatus[] actionsStatusTest;
    public CharacterStatus GetCharacterStatus() { return charaStatus; }

    Character_Object charaObj;
    Character_TargetButton targetButton;
    public Character_Object GetCharacter_Object() { return charaObj; }
    public Character_TargetButton GetCharacter_TargetButton() { return targetButton; }

    protected List<PassiveAbility> passiveAbilities = new List<PassiveAbility>();

    ActionQueueManager actionQueue;
    BattleManager battleManager;
    Utility util;
    InfoText infoText;
    CharactersManager charactersManager;
    SoundManager soundManager;

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
        foreach(GameObject pa in charaStatus.passiveAbilities) { AddPA(pa); }

        targetButton.SetCharacter(this);

        actionQueue = FindObjectOfType<ActionQueueManager>();
        battleManager = FindObjectOfType<BattleManager>();
        util = FindObjectOfType<Utility>();
        infoText = FindObjectOfType<InfoText>();
        charactersManager=FindObjectOfType<CharactersManager>();
         soundManager=FindObjectOfType<SoundManager>();

        //TurnIconはラウンド開始時にセット
    }
    public void AddPA(GameObject paObj)
    {
        var p = Instantiate(paObj, transform);
        passiveAbilities.Add(p.GetComponent<PassiveAbility>());
        p.GetComponent<PassiveAbility>().Init(this);
    }

    public void DisplayInfo()
    {
        string info = charaStatus.GetInfo();
        info += "◇◇特性◇◇\n";
        foreach(PassiveAbility pa in passiveAbilities)
        {
            info += string.Format("<{0}>\n{1}\n", pa.GetPAName(),pa.GetPAInfo());
            
        }
        infoText.SetCharaInfo(charaStatus.charaName, info, this);
        FindObjectOfType<AbilityButtonPanel>().SetAbilityButtons(charaStatus.abilitiesStatus,this);
        charaObj.SetSelectedIcon(true);
    }
    public void ResetCharaSprite()
    {
        charaObj.SetCharaSprite(charaStatus.variableSprites[0]);
    }
    public void SetCharaSprite(GameObject sprite)
    {
        charaObj.SetCharaSprite(sprite);
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="actionStatus"></param>
    /// <param name="setTargets">actionTargetに第3引数を代入するか</param>
    /// <param name="actionTargets">setTargetsがtrueの時、これを対象群として改めてactionstatusに代入する</param>
    public void Enqueue(Action.ActionStatus actionStatus,bool setTargets,List<Character> actionTargets)
    {
        actionStatus.actionOwner = this;
        if (setTargets) { actionStatus.actionTargets = actionTargets; }    
        actionQueue.Enqueue(actionStatus);
    }

    public void SetTurnIcon() { charaObj.SetTurnIcons(charaStatus.turnPerRound); }
    public void SetActionInvolvedIcon(bool owner) { targetButton.SetActionInvolvedIcon(owner); }

    public void MyTurnStart()
    {
        charaObj.SetTurnIcon_CurentTurn();
        infoText.AddLogText(string.Format("=={0}のターン==", charaStatus.charaName));
        OnTurnStart();
        actionQueue.StartResolve(2);
    }
    public virtual void MainPhase()
    {
        if (CheckAlive())
        {
            //行動可能か～
            OnActivateAbility();
            if (charaStatus.playable)
            {
                DisplayInfo();
                battleManager.SetSelectingAbility(true);
            }
            else { StartCoroutine(Test()); }
        }
        else
        {
            infoText.AddDebugText("死亡につきターンスキップ");
            EndMyTurn();
        }
         
    }
    IEnumerator Test()
    {
        yield return new WaitForSeconds(0.5f);
        battleManager.SetSelectedAbility(charaStatus.omen, this);//test　本来はラウンド開始時に決定する
        charaStatus.omenSet = false;
        charaStatus.omen = new Ability.AbilityStatus();
        BattleManager.selectedAbility.StartSelectTarget();
    }
    public void EndPhase()
    {
        if (CheckAlive())
        {

            OnTurnEnd();
            charaObj.SetTurnIcon_End();
            //Resolve開始
        }

        EndMyTurn();
    }
    public void EndMyTurn()
    {
        battleManager.TurnEnd();
    }


    //ここからアクションによって呼ばれる関数
    public void DecreaseHP(int value)
    {
        charaStatus.HP -= value;
        charaObj.SetHPandShieldBar();
        charaObj.SetDamageText(value.ToString(), Definer.colorRef.decreaseHP);
        infoText.AddLogText(string.Format("{0}はHPを{1}失った", charaStatus.charaName, util.GetColoredText(Definer.colorRef.decreaseHP, value.ToString())));
        soundManager.PlaySE(Definer.soundRef.damage);
        if (charaStatus.HP <= 0)
        {
            if (charaStatus.surviveFatalWounds)//瀕死で耐えるキャラは、HP減少によって死なない
            {
                charaStatus.HP = 0;
                charaObj.SetDamageText("瀕死!", Definer.colorRef.damage);
                infoText.AddLogText(string.Format("{0}は{1}だ...", charaStatus.charaName, util.GetColoredText(Definer.colorRef.damage, "瀕死")));
                soundManager.PlaySE(Definer.soundRef.dying);
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
            soundManager.PlaySE(Definer.soundRef.CRIT);
        }
        else
        {
            charaObj.SetDamageText(DMG.ToString(), Definer.colorRef.damage);
            infoText.AddLogText(string.Format("{0}は{1}ダメージを受けた", charaStatus.charaName, util.GetColoredText(Definer.colorRef.damage, DMG.ToString())));
            soundManager.PlaySE(Definer.soundRef.damage);
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
                soundManager.PlaySE(Definer.soundRef.dying);

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
                    soundManager.PlaySE(Definer.soundRef.dying);
                }
                else
                {
                    Die(0);
                }
            }
        }

        charaObj.SetHPandShieldBar();//HPバーに反映
        if (CheckAlive())
        {
            OnDamaged(DMG, attacker);
            //カウンター
        }
    }
    public void Heal(int value,Character healer)
    {
        charaStatus.HP = Mathf.Min(charaStatus.HP + value, charaStatus.maxHP);
        charaObj.SetDamageText(value.ToString(), Definer.colorRef.heal);
        infoText.AddLogText(string.Format("{0}はHPを{1}回復した", charaStatus.charaName, util.GetColoredText(Definer.colorRef.heal, value.ToString())));
        soundManager.PlaySE(Definer.soundRef.heal);
        charaObj.SetHPandShieldBar();
    }
    public void SANHeal(int value)
    {
        charaStatus.SAN = Mathf.Min(charaStatus.SAN + value, charaStatus.maxSAN);
        charaObj.SetDamageText(value.ToString(), Definer.colorRef.SANHeal);
        infoText.AddLogText(string.Format("{0}は正気度を{1}回復した", charaStatus.charaName, util.GetColoredText(Definer.colorRef.SANHeal, value.ToString())));
        soundManager.PlaySE(Definer.soundRef.SANHeal);
        charaObj.SetSANBar();
    }
    public void SANDamage(int value)
    {
        charaStatus.SAN -= value;
        charaObj.SetDamageText(value.ToString(), Definer.colorRef.SANDecrease);
        infoText.AddLogText(string.Format("{0}は正気度を{1}失った", charaStatus.charaName, util.GetColoredText(Definer.colorRef.SANDecrease, value.ToString())));
        soundManager.PlaySE(Definer.soundRef.SANDecrease);
        charaObj.SetSANBar();
        if (charaStatus.SAN <= 0) { Die(1); }
    }

    public void AddShield(int value)
    {
        charaStatus.shield += value;
        charaObj.SetDamageText(value.ToString(), Definer.colorRef.shield);
        infoText.AddLogText(string.Format("{0}はシールドを{1}得た", charaStatus.charaName, util.GetColoredText(Definer.colorRef.shield, value.ToString())));
        soundManager.PlaySE(Definer.soundRef.shield);
        charaObj.SetHPandShieldBar();
    }

    public void ChangePos(int moveTo)
    {
        charaObj.StopMove(charaStatus.size, charaStatus.position);
        charaStatus.position = moveTo;
        targetButton = charactersManager.GetTargetButton(charaStatus.size, charaStatus.position);
        targetButton.SetCharacter(this);
        charactersManager.SortExistingCharacters();

        charaObj.MoveStart(charaStatus.size, charaStatus.position);
    }
    
    
    //ここまでアクションによって呼ばれる関数


    public bool CheckAlive() { return !charaStatus.dead; }
    /// <summary>0:HP0 1:SAN0</summary>
    void Die(int cause)
    {
        charaStatus.dead = true;
        if (cause == 0)
        {
            charaObj.SetDamageText("死亡", Definer.colorRef.damage);
            infoText.AddLogText(util.GetColoredText(Definer.colorRef.damage, string.Format("{0}は死亡した", charaStatus.charaName)));
        }
        else if (cause == 1)
        {
            charaObj.SetDamageText("発狂", Definer.colorRef.damage);
            infoText.AddLogText(util.GetColoredText(Definer.colorRef.damage, string.Format("{0}は発狂して死亡した", charaStatus.charaName)));
        }

        charactersManager.RemoveExistingCharacter(this);
        battleManager.RemoveTurn(this);

        targetButton.ResetCharacter();
        charaObj.HideCharacterObj();
    }
    public void Retreat()
    {

    }

    public virtual void SetOmen()
    {
        if (!charaStatus.playable && CheckAlive() && battleManager.CheckIfTurnRemain(this)&&!charaStatus.omenSet)
        {
            charaStatus.omen = charaStatus.abilitiesStatus[Random.Range(0, charaStatus.abilitiesStatus.Length)];
            charaStatus.omenSet = true;
            battleManager.SetOmenIcon(this, charaStatus.omen);
        }
    }
    public virtual void OnBattleStart() { }
    public virtual void OnRoundStart()
    {
        if (!charaStatus.playable)
        { 
           //予兆設定
        }
    }
    public virtual void OnTurnStart()
    {
        foreach (PassiveAbility passiveAbility in passiveAbilities) { passiveAbility.OnTurnStart(); }
    }
    public virtual void OnTurnEnd() { }
    public virtual void OnRoundEnd() { }
    public virtual void OnBattleEnd() { }


    public virtual void OnActivateAbility()
    {
        foreach (PassiveAbility passiveAbility in passiveAbilities) { passiveAbility.OnActivateAbility(); }
    }
    /// <summary>攻撃命中時</summary>
    public virtual void OnDamage(int DMG, Character target)
    {
        foreach (PassiveAbility passiveAbility in passiveAbilities) { passiveAbility.OnDamage(DMG, target); }
    }
    public virtual void OnCRIT(int ID) { }
    public virtual void OnKill(int ID) { }
    public virtual void OnMiss(int ID) { }
    public virtual void OnHeal(int healValue, int ID) { }
    //public virtual void OnApplyStE() { }
    //public virtual void OnRemoveStE() { }

    public virtual void OnDamaged(int DMG, Character attacker)
    {
        foreach (PassiveAbility passiveAbility in passiveAbilities) { passiveAbility.OnDamaged(DMG, attacker); }
    }
    public virtual void OnCRITed(int ID) { }
    public virtual void OnEvade( int ID) { }
    public virtual void OnHealed(int healedValue, int ID) { }
    //public virtual void OnApplyedStE() { }
    //public virtual void OnRemoveedStE() { }
}

