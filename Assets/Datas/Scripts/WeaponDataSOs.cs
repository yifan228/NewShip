using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "Weapons", menuName = "Data/Weapons")]
public class WeaponDataSOs : ScriptableObject
{
    public List<WeaponDataSO> weaponData;

    /// <summary>
    /// weapon_basic 是目前測試用的
    /// </summary>
    /// <param name="targetKeystring"></param>
    /// <returns></returns>
    public WeaponData GetData(string targetKeystring)
    {
        WeaponData data;
        data= weaponData.Find(x => x.weaponData.WeaponKeyString == targetKeystring).weaponData;
        return data;
    }
}

