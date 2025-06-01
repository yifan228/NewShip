using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DebugSettings", menuName = "Tool/DebugSettings")]
[System.Serializable]
public class DebugSettings : ScriptableObject
{
    public static DebugCategory CategoryToggles;
    [SerializeField] public DebugCategory debugCategory;
    private void OnValidate()
    {
        CategoryToggles = debugCategory;
    }
}