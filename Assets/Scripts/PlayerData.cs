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
    public List<ShipData> Ship;
    public List<ShipWeaponData> Weapon;
    public List<ShipBulleteData> Bullete;

    private void OnEnable()
    {
        if (Ship == null) Ship = new List<ShipData>();
        if (Weapon == null) Weapon = new List<ShipWeaponData>();
        if (Bullete == null) Bullete = new List<ShipBulleteData>();
    }
}

[System.Serializable]
public class ShipData
{
    public string ID;// 每個人不一樣
    public int Index;// 頁面顯示要用
    public string KeStr;// 每個人一樣
    public HashSet<string> EquippedGunID;//1_id,2_id 1=up 2=right，clockwise，clockwise
    public HashSet<string> EquippedBulleteID;
    public List<string> AwakeLevel;//0_1_3,0_2_2

    public ShipData()
    {
        EquippedGunID = new HashSet<string>();
        EquippedBulleteID = new HashSet<string>();
        AwakeLevel = new List<string>();
    }
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
