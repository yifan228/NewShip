using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
/// <summary>
/// just for test now
/// </summary>
public struct InRoundData 
{
    public HpData hpdata;
    public HpDmgData hpdmgData;
    public ArmorDmgData armorDmgData;
    public DebuffAttribute debufData;
}

[Serializable]
/// <summary>
/// just for test now
/// </summary>
public struct OutRoundData
{
    public HpData hpdata;
    public HpDmgData hpdmgData;
    public ArmorDmgData armorDmgData;
    public DebuffAttribute debufData;
}