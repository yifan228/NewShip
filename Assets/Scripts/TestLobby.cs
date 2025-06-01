using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class TestLobby : MonoBehaviour
{
    [SerializeField] Button startGame;
    [SerializeField] TMP_InputField HP;
    [SerializeField] TMP_InputField HpAtk;
    [SerializeField] TMP_InputField Armor;
    [SerializeField] TMP_InputField ArmorRec;
    [SerializeField] TMP_InputField ArmorRecT;
    [SerializeField] TMP_InputField ArmorAtk;
    [SerializeField] TMP_InputField OnCrash;

    private OutRoundData data;
    private string dataPath;

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        dataPath = Path.Combine(Application.dataPath, "outData.json");
        startGame.onClick.AddListener(GoToGameRound);

        if(File.Exists(dataPath)){
            string json = File.ReadAllText(dataPath);
            data = JsonUtility.FromJson<OutRoundData>(json);
            HP.text = data.hpdata.MaxHp.ToString();
            HpAtk.text = data.hpdmgData.Dmg.ToString();
            Armor.text = data.hpdata.Armormax.ToString();
            ArmorAtk.text = data.armorDmgData.Dmg.ToString();
            ArmorRec.text = data.hpdata.ArmorrecoverSpd.ToString();
            ArmorRecT.text = data.hpdata.ArmorstartRecoverTime.ToString();
            OnCrash.text = data.armorDmgData.OnCrash.ToString();
        }
    }

    public void GoToGameRound()
    {
        data = new OutRoundData
        {
            hpdata = new HpData
            {
                MaxHp = float.Parse(HP.text),
                Armormax = float.Parse(Armor.text),
                ArmorrecoverSpd = float.Parse(ArmorRec.text),
                ArmorstartRecoverTime = float.Parse(ArmorRecT.text)
            },
            hpdmgData = new HpDmgData
            {
                Dmg = float.Parse(HpAtk.text)
            },
            armorDmgData = new ArmorDmgData
            {
                OnCrash = float.Parse(OnCrash.text),
                Dmg = float.Parse(ArmorAtk.text)
            },
            debufData = new DebuffAttribute
            {
                type = DebufEnum.Curse,
                Velocity = 0,
                Position = 0,
                HitRate = 0
            }
        };

        string json = JsonUtility.ToJson(data);
        File.WriteAllText(dataPath, json);
        MySceneManager.Instance.LoadSceneWithLoading("Game");
    }  

}
