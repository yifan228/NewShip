using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class TheGlobal : MonoBehaviour
{
    private static TheGlobal instance;
    public static TheGlobal Instance => instance;
    public IServerAgent ServerAgent;
    public Language CurrentLanguage = Language.TW;
    [SerializeField] private TextAsset shipExcelDatabase;
    [SerializeField] private TextAsset weaponExcelDatabase;
    [SerializeField] private TextAsset keyStrImageExcelDatabase;
    [SerializeField] private TextAsset descExcelDatabase;
    public  Excel_KeyStr_Image_Datas keyStrImageDatas{get;private set;}
    public  Excel_ShipDatas ShipExcelDatabase{get;private set;}
    public Excel_WeaponDatas WeaponExcelDatabase{get;private set;}
    public Excel_DescDatas DescExcelDatabase{get;private set;}
    [Tooltip("測試用")]
    [SerializeField]private int testPlayerDataIndex = 0;
    [SerializeField]private List<PlayerData> testPlayerDatas;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        //ServerAgent = new FakeServerAgent();
        //todo not yet 6/3 (ServerAgent as FakeServerAgent).SetUpForTest(testPlayerDatas,testPlayerDataIndex);
        DontDestroyOnLoad(gameObject);
        ShipExcelDatabase = JsonUtility.FromJson<Excel_ShipDatas>(shipExcelDatabase.text);

        WeaponExcelDatabase = JsonUtility.FromJson<Excel_WeaponDatas>(weaponExcelDatabase.text);

        keyStrImageDatas = JsonUtility.FromJson<Excel_KeyStr_Image_Datas>(keyStrImageExcelDatabase.text);

        DescExcelDatabase = JsonUtility.FromJson<Excel_DescDatas>(descExcelDatabase.text);

        keyStrImageDatas.Init();
        DescExcelDatabase.Init();
    }
    
}
#if UNITY_EDITOR
[CustomEditor(typeof(TheGlobal))]
public class TheGlobalEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
    }
}
#endif

public enum Language
{
    TW,
    EN
}


