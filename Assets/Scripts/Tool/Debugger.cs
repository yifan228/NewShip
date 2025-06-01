using UnityEngine;
using System.Runtime.CompilerServices;

public static class Debugger
{
    public static void Log(DebugCategory category, string message, [CallerFilePath] string file = "")
    {
        if (DebugSettings.CategoryToggles.HasFlag(category))
        {
            Debug.Log($"[{category}][{System.IO.Path.GetFileName(file)}] {message}");
        }
    }

    public static void LogWarning(DebugCategory category, string message, [CallerFilePath] string file = "")
    {
        if (DebugSettings.CategoryToggles.HasFlag(category))
        {
            Debug.LogWarning($"[{category}][{System.IO.Path.GetFileName(file)}] {message}");
        }
    }

    public static void LogError(DebugCategory category, string message, [CallerFilePath] string file = "")
    {
        if (DebugSettings.CategoryToggles.HasFlag(category))
        {
            Debug.LogError($"[{category}][{System.IO.Path.GetFileName(file)}] {message}");
        }
    }
} 