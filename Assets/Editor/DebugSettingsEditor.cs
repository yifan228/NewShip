using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

[CustomEditor(typeof(DebugSettings))]
public class DebugSettingsEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DebugSettings debugSettings = (DebugSettings)target;
        EditorGUILayout.Space();

        // 全選/全不選按鈕
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("全選"))
        {
            debugSettings.debugCategory = DebugCategory.Enemy | DebugCategory.GameLogic | DebugCategory.DamageLogic | DebugCategory.UI | DebugCategory.Projectile;
        }
        if (GUILayout.Button("全不選"))
        {
            debugSettings.debugCategory = 0;
        }
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.Space();

        debugSettings.debugCategory = (DebugCategory)EditorGUILayout.MaskField(
            "要啟動的debug 類別",
        (int)debugSettings.debugCategory,
            System.Enum.GetNames(typeof(DebugCategory))
        );
        
        if (GUI.changed)
        {
            EditorUtility.SetDirty(target);
        }
    }

}