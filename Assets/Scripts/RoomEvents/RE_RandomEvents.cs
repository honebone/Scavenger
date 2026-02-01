using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class RE_RandomEvents : RoomEvent
{
    //[SerializeField]
    //List<GameObject> events;

    public enum Rarity { negative,common,rare,epic,legendary}
    [SerializeField] Rarity rarity;
    [SerializeField,Header("キャラ選択があるもののみ関連")] CharaSelectInfo charaSelectInfo;

    protected int choice = 0;
    protected List<Character> players= new List<Character>();
    protected REOptionParams option_exit;
    //GameObject eventManager;
    public override void StartRoomEvent()
    {
        infoText.SwitchToLog();
    }
    public override void OnEndREInfo()
    {
        foreach (Character c in characterManager.GetExistingCharacters_All())
        {
            if (c.CharaStatus().playable) { players.Add(c); }
        }

        option_exit = new REOptionParams();
        option_exit.optionName = "立ち去る";
        option_exit.optionInfo = "イベントを修了する";

        StartRandomEvent();
    }
    public virtual void StartRandomEvent()
    {

    }


    /// <summary>
    /// プレイヤー選択の選択肢を作成
    /// </summary>
    /// <returns></returns>
    public List<REOptionParams> GenPlayerSelects()
    {
        List<REOptionParams > result = new List<REOptionParams>();
        players.ForEach(p =>
        {
            REOptionParams option = new REOptionParams(charaSelectInfo.option);
            option.optionName = p.CharaStatus().charaName;

            string s = "";
            if (charaSelectInfo.showExp) s += $"{Extentions.NL(s,2)}現在の{p.CharaStatus().GetExpInfo()}";
            if (charaSelectInfo.showBadPers)
            {
                string badPer = "";
                p.GetPers(PA_Personality.PersonalityStatus.PersonalityType.bad).ForEach(b => { badPer += $"\n{b.GetPAName()}"; });
                if (badPer == "") badPer = "無し";
                s += $"{Extentions.NL(s, 2)}所持中の<color=#C900FF>悪い特性</color>{badPer}";
            }

            if (s != "") option.optionInfo_suffix += $"\n\n{s}";

            result.Add(option);
        });

        return result;
    }

    [System.Serializable]
    public class CharaSelectInfo
    {
        public REOptionParams option;
        public bool showExp;
        public bool showBadPers;
    }

    public static Dictionary<Rarity, string> rarityName = new Dictionary<Rarity, string>(){
    {Rarity.negative,"ネガティブ"}, {Rarity.common,"コモン"},{Rarity.rare,"レア"},
    {Rarity.epic,"エピック"},{Rarity.legendary,"レジェンダリー"}
    };

    public Rarity GetRarity() { return rarity; }
    public string GetRarityStr() { return rarityName[rarity].ColorStr(Definer.colorRef.RaERarityColors[(int)rarity]); }
}
