using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public struct Excel_DescData
{
    public string KeyString;
    public string Desc_TW;
    public string Desc_EN;
}

/// <summary>
/// 需要被初始化，因為要用成字典
/// </summary>
[Serializable]
public class Excel_DescDatas
{
    public List<Excel_DescData> Datas;
    private Dictionary<string, Excel_DescData> descDict;

    public void Init()
    {
        descDict = new Dictionary<string, Excel_DescData>();
        foreach (var data in Datas)
        {
            descDict[data.KeyString] = data;
        }
    }

    public string GetDesc(string keyString)
    {
        if (descDict.TryGetValue(keyString, out var data))
        {
            switch (TheGlobal.Instance.CurrentLanguage)
            {
                case Language.TW:
                    return data.Desc_TW;
                case Language.EN:
                    return data.Desc_EN;
                default:
                    return data.Desc_TW;
            }
        }
        return $"{keyString} no desc";
    }
}

