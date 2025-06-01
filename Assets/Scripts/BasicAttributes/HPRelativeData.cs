using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct HpDmgData
{
    public float Dmg;
    public float CriticalDamage_Percentage;
    public float Critical_Percentage;
    public float IgnoreArmor;
}
[Serializable]
public struct ArmorDmgData
{
    public float Dmg;
    public float OnCrash;
    public float SuperCrashDmg_Percentage;
}