using EnemyNameSpace;
using System.Collections.Generic;
using System.Reflection;
using System;

public static class RoundDataManager
{
    static List<RoundAttributesData> RoundDatas;

    public static int FusilladeCount=1;
    public static int DroneCount=1;
    public static float RoundGameTime;  

    /// <summary>
    /// 局外成長
    /// </summary>
    public static OutRoundData Out_PlayerData;
    /// <summary>
    /// 局內成長
    /// </summary>
    public static InRoundData In_GameData;
    /// <summary>
    /// 擊破
    /// </summary>
    public static float PlayerCrashPara;
    /// <summary>
    /// 散射角度
    /// </summary>
    public static float BulleteSpreadAngle=15;

    #region this round enemy
    public static Dictionary<string, float> EnemyDamage;
    static List<EnemyDataStruct>  enemiesData;
    #endregion
    /// <summary>
    /// 這一關卡會出現那些敵人
    /// </summary>
    /// <param name="d"></param>
    public static void InitDatabase(List<EnemyDataStruct> d)
    {
        enemiesData = d;
        EnemyDamage = new Dictionary<string, float>();
        foreach(EnemyDataStruct e in enemiesData)
        {
            EnemyDamage.Add(e.KeyString+ "_hpDamage", e.HpDamage);
            EnemyDamage.Add(e.KeyString + "_armorDamage", e.ArmorDamage);
        }
    }

    public static void InitPlayerPartData(OutRoundData PlayerData)
    {
        Out_PlayerData = PlayerData;
    }

    public static void PlayerGet(RoundAttributesData d)
    {
        RoundDatas.Add(d);
    }

    static AllInRoundData CalculateTotalDamage(List<RoundAttributesData> dataList)
    {
        AllInRoundData totalData = new AllInRoundData();
        Type allDataType = typeof(AllInRoundData);

        foreach (RoundAttributesData d in dataList)
        {
            Type atkType = d.GetType();
            FieldInfo[] fields = atkType.GetFields();

            foreach (FieldInfo field in fields)
            {
                FieldInfo allDataField = allDataType.GetField(field.Name);
                if (allDataField != null)
                {
                    object atkValue = field.GetValue(d);
                    object totalValue = allDataField.GetValue(totalData);

                    if (atkValue is int)
                    {
                        allDataField.SetValue(totalData, (int)totalValue + (int)atkValue);
                    }
                    else if (atkValue is float)
                    {
                        allDataField.SetValue(totalData, (float)totalValue + (float)atkValue);
                    }
                }
            }
        }

        return totalData;
    }
}

public class AllInRoundData
{
    public float damage;
    public int penetrateCount;
    public int secondaryDamate;
    public int fusilladeCount;
}

public class RoundAttributesData
{

}
