using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public struct Excel_ShipDatas
{
    public List<Excel_ShipData> ShipDatas;
}


[Serializable]
public struct Excel_ShipData 
{
    public string KeyString;
    public string Attributes;
    public string WeaponHole;
    public string UiPrefab;

}
