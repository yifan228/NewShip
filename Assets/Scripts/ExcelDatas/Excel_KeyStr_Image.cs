using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class Excel_KeyStr_Image_Datas
{
    public List<Excel_KeyStr_Image> Datas;
    private Dictionary<string,string> imageNameDict;
    public void Init(){
        imageNameDict = new Dictionary<string,string>();
        foreach(var data in Datas){
            imageNameDict[data.KeyString] = data.ImageName;
        }
    }
    public string GetImageName(string keyString){
        string result = imageNameDict[keyString];
        if(result !=""){
            return result;
        }
        return "empty";
    }
}

[Serializable]
public struct Excel_KeyStr_Image
{
    public string KeyString;
    public string ImageName;        
}
