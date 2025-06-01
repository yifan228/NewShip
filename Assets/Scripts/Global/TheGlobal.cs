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
    public static TheGlobal Instance;
    public static IServerAgent ServerAgent;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        ServerAgent = new FakeServerAgent();
        DontDestroyOnLoad(gameObject);
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
