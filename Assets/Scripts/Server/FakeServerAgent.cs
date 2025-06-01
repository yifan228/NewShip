using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class FakeServerAgent : MonoBehaviour, IServerAgent
{
    [SerializeField] private List<PlayerData> playerDatas;
    private PlayerData playerData;

    public IEnumerator Login(string userId, Action<PlayerData> callback)
    {
        playerData = playerDatas.FirstOrDefault(p => p.UserId == userId);
        if(playerData == null){
            playerData = new PlayerData();
            playerData.UserId = userId;
            playerDatas.Add(playerData);
        }
        // 給新的飛船
        if(playerData.Ship == null || playerData.Ship.Count == 0)
        {
            var initialShip = new ShipData
            {
                ID = "00_1",
                Index = 0,
                KeStr = "ship_001",
                AwakeLevel = new List<string>()
            };
            playerData.Ship.Add(initialShip);
        }
        
        callback?.Invoke(playerData);
        yield return null;
    }

    public IEnumerator UpgradeShipAwake(string data, Action<bool> callback)
    {
        // data ex:00_1,0_2_2
        bool success = false;

        string shipId = data.Split(',')[0];
        string awakedata = data.Split(',')[1];

        var shipData = playerData.Ship.FirstOrDefault(s => s.ID == shipId);
        if (shipData != null)
        {
            string awakepage = awakedata.Split('_')[0];
            string awakeIndex = awakedata.Split('_')[1];
            string awakeLevel = awakedata.Split('_')[2];
            
            // 尋找是否已存在相同 page_index 的資料
            string pageIndex = $"{awakepage}_{awakeIndex}";
            int existingIndex = shipData.AwakeLevel.FindIndex(x => x.StartsWith(pageIndex));
            
            if (existingIndex != -1)
            {
                // 如果找到，更新該筆資料
                shipData.AwakeLevel[existingIndex] = awakedata;
            }
            else
            {
                // 如果沒找到，新增這筆資料
                shipData.AwakeLevel.Add(awakedata);
            }
            success = true;
        }
        callback?.Invoke(success);
        yield return null;
    }

    public IEnumerator UpdateShipEquipped(HashSet<ShipData> shipData, Action<bool> callback)
    {   
        bool success = true;
        foreach (var ship in shipData)
        {
            var existingShip = playerData.Ship.FirstOrDefault(s => s.ID == ship.ID);
            if (existingShip == null)
            {
                success = false;
                Debugger.LogError(DebugCategory.Server, $"UpgradeShipEquipped: 找不到飛船 {ship.ID}");
                break;
            }
            // 更新裝備
            existingShip.EquippedGunID = ship.EquippedGunID;
            existingShip.EquippedBulleteID = ship.EquippedBulleteID;
        }
        callback?.Invoke(success);
        yield return null;
    }

    public IEnumerator UpgradeGunLevel(string data, Action<bool> callback)
    {
        // ex:id_scrollkestr
        bool success = false;
        string gunId = data.Split('_')[0];
        string scrollKeStr = data.Split('_')[1];
        var weaponData = playerData.Weapon.FirstOrDefault(w => w.ID == gunId);
        if (weaponData != null)
        {
            // 模擬升級武器
            weaponData.ScrollKeStr.Add(scrollKeStr);
            success = true;
        }
        callback?.Invoke(success);
        yield return null;
    }

/// <summary>
/// todo
/// </summary>
/// <param name="dropId"></param>
/// <param name="callback"></param>
/// <returns></returns>
    public IEnumerator RequestDrop(string dropId, Action<bool, string> callback)
    {
        bool success = true;
        string dropResult = "";
        callback?.Invoke(success, dropResult);
        yield return null;
    }
}
