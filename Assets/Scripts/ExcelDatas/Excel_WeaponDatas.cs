using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public struct Excel_WeaponDatas 
{
    public List<Excel_WeaponData> WeaponDatas;
}

[Serializable]
public struct Excel_WeaponData
{
    public string KeyString;
    public string Attributes;
    public string ImageName;
}
