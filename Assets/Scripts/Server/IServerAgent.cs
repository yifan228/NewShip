using System;
using System.Collections;
using System.Collections.Generic;

public interface IServerAgent
{
    IEnumerator Login(string userId, Action<PlayerData> callback);

    IEnumerator UpgradeShipAwake(string data,Action<bool> callback); // ex:00_1,0_2_2
    /// <summary>
    /// 換武器、換子彈 共鳴目前也會更新到，因為用整個shipdata
    /// </summary>
    /// <param name="data"></param>
    /// <param name="callback"></param>
    /// <returns></returns>
    IEnumerator UpdateShipEquipped(HashSet<ShipData> data,Action<bool> callback); // ex:0_1_3

    IEnumerator UpgradeGunLevel(string data,Action<bool> callback);//ex:id_scrollkestr

    /// <summary>
    /// todo: 掉落
    /// </summary>
    /// <param name="data"></param>
    /// <param name="callback"></param>
    /// <returns></returns>
    IEnumerator RequestDrop(string data,Action<bool,string> callback);
}
