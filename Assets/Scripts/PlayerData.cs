using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 玩家資料
/// 玩家自己的飛船ID:00_1、00_2、00_3
/// 玩家自己的飛船等級:0_1_3、0_2_2
/// 玩家自己的飛船裝備:1_id,2_id 1=up 2=right，clockwise
/// 玩家自己的武器ID:01_1 
/// 玩家自己的子彈ID:02_1 
/// </summary>
[CreateAssetMenu(fileName = "PlayerData", menuName = "PlayerData")]
public class PlayerData : ScriptableObject
{
    public string UserId;
    public int Level;
    public int Gold;
    public List<ShipData> Ships;
    public List<ShipWeaponData> Weapons;
    public List<ShipBulleteData> Bulletes;

    private void OnEnable()
    {
        if (Ships == null) Ships = new List<ShipData>();
        if (Weapons == null) Weapons = new List<ShipWeaponData>();
        if (Bulletes == null) Bulletes = new List<ShipBulleteData>();
    }
}

[System.Serializable]
public class ShipData
{
    public string ID;// 每個人不一樣
    public int Index;// 頁面顯示要用
    public string KeStr;// 每個人一樣
    public List<PositionAndIDMap> EquippedWeaponID;//1_id,2_id 123456789 is position
    public List<PositionAndIDMap> EquippedBulleteID;
    public List<string> AwakeLevel;//0_1_3,0_2_2

    public ShipData()
    {
        EquippedWeaponID = new List<PositionAndIDMap>();
        EquippedBulleteID = new List<PositionAndIDMap>();
        AwakeLevel = new List<string>();
    }
}

[System.Serializable]
public class PositionAndIDMap{
    public int Position;
    public string ID;
}

/// <summary>
/// 武器 用 裝備ID
/// </summary>
[System.Serializable]
public class ShipWeaponData
{
    public string ID;
    public string KeStr;
    public HashSet<string> ScrollKeStr;

    public ShipWeaponData()
    {
        ScrollKeStr = new HashSet<string>();
    }
}

/// <summary>
/// 子彈 用 裝備ID
/// </summary>
[System.Serializable]
public class ShipBulleteData
{
    public string ID;
    public string KeStr;
}

public struct IdAndKeyString{   
    public string ID;
    public string KeyString;
}

